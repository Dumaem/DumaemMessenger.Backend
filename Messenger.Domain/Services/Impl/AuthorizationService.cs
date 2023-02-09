using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DeviceDetectorNET;
using Messenger.Domain.ErrorMessages;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Messenger.Domain.Results;
using Messenger.Domain.Settings;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.Domain.Services.Impl;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserService _userService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IEncryptionService _encryptionService;
    private readonly IUserVerificationRepository _userVerificationRepository;

    public AuthorizationService(IUserService userService, JwtSettings jwtSettings,
        TokenValidationParameters tokenValidationParameters, IRefreshTokenRepository refreshTokenRepository,
        IEncryptionService encryptionService, IUserVerificationRepository userVerificationRepository)
    {
        _userService = userService;
        _jwtSettings = jwtSettings;
        _tokenValidationParameters = tokenValidationParameters;
        _refreshTokenRepository = refreshTokenRepository;
        _encryptionService = encryptionService;
        _userVerificationRepository = userVerificationRepository;
    }

    public async Task<AuthenticationResult> RegisterAsync(string name, string email, string password, string? username,
        string userAgent)
    {
        var existingUser = await _userService.GetUserByEmailAsync(email);

        if (existingUser is not null)
            return new AuthenticationResult {Success = false, Message = "User with this email already exists"};

        var user = new User
        {
            Email = email, Username = username, Password = password, Name = name
        };

        var createdUserId = await _userService.CreateUserAsync(user, password);
        user.Id = createdUserId;

        return await GenerateTokenForUserAsync(user, userAgent);
    }

    public async Task<AuthenticationResult> AuthorizeAsync(string email, string password, string userAgent)
    {
        var existingUser = await _userService.GetUserByEmailAsync(email);

        if (existingUser is null)
            return new AuthenticationResult {Success = false, Message = "User does not exist"};

        var isPasswordValid = await _userService.CheckUserPasswordAsync(existingUser.Id, password);

        if (!isPasswordValid)
            return new AuthenticationResult {Success = false, Message = "User has wrong password"};

        var deviceId = await GenerateDeviceId(userAgent);
        await RevokeActualTokenIfExists(existingUser.Id, deviceId);

        return await GenerateTokenForUserAsync(existingUser, deviceId);
    }

    public async Task<AuthenticationResult> RefreshAsync(string token, string refreshToken, string userAgent)
    {
        var validatedToken = GetPrincipalFromToken(token);

        if (validatedToken == null)
            return new AuthenticationResult {Success = false, Message = RefreshTokenErrorMessages.InvalidToken};

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        var storedRefreshToken = await _refreshTokenRepository.GetTokenAsync(refreshToken);

        if (storedRefreshToken is null || storedRefreshToken.JwtId != jti)
            return new AuthenticationResult {Success = false, Message = RefreshTokenErrorMessages.UnrecognizedToken};

        if (storedRefreshToken.ExpiryDate < DateTime.UtcNow)
            return new AuthenticationResult {Success = false, Message = RefreshTokenErrorMessages.ExpiredToken};


        int.TryParse(validatedToken.Claims.Single(x => x.Type == "id").Value, out var userId);
        var user = await _userService.GetUserByIdAsync(userId);

        var deviceId = await GenerateDeviceId(userAgent);
        if (storedRefreshToken.IsRevoked || storedRefreshToken.IsUsed)
        {
            await RevokeActualTokenIfExists(user!.Id, deviceId);
            return new AuthenticationResult {Success = false, Message = RefreshTokenErrorMessages.UsedToken};
        }

        await _refreshTokenRepository.UseTokenAsync(storedRefreshToken.Id);

        return await GenerateTokenForUserAsync(user!, deviceId);
    }

    private ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
            if (!IsJwtHasValidSecurityAlgorithm(validatedToken))
                return null;

            return principal;
        }
        catch
        {
            return null;
        }
    }

    private bool IsJwtHasValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return validatedToken is JwtSecurityToken token &&
               token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase);
    }

    private async Task<AuthenticationResult> GenerateTokenForUserAsync(User user, string deviceId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim("deviceId", deviceId)
            }),
            Expires = DateTime.UtcNow.Add(_jwtSettings.AccessTokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var accessToken = tokenHandler.CreateToken(tokenDescriptor);
        var refreshToken = new RefreshToken
        {
            JwtId = accessToken.Id,
            UserId = user.Id,
            DeviceId = deviceId,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(_jwtSettings.RefreshTokenMonthLifetime)
        };

        await _refreshTokenRepository.CreateTokenAsync(refreshToken);

        return new AuthenticationResult
        {
            Success = true,
            Token = new JwtToken
            {
                AccessToken = tokenHandler.WriteToken(accessToken),
                RefreshToken = refreshToken.Token
            }
        };
    }

    private async Task<string> GenerateDeviceId(string userAgent)
    {
        DeviceDetector dd = new(userAgent);
        dd.Parse();

        var deviceClient = dd.GetClient();

        var deviceId = await _encryptionService.EncryptStringAsync(deviceClient.ToString());

        return deviceId;
    }

    /// <summary>
    /// Finds actual(not used and not revoked) token by userId and deviceId
    /// Sets the IsRevoked property to true
    /// </summary>
    private async Task RevokeActualTokenIfExists(int userId, string deviceId)
    {
        await _refreshTokenRepository.RevokeTokenIfExistsAsync(userId, deviceId);
    }

    /// <summary>
    /// Creating a new user verify token if it doesn't actually exist
    /// </summary>
    public async Task<BaseResult> GenerateUserVerifyToken(string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        if (user is null)
            return new BaseResult {Message = "User does not exist", Success = false};

        if (user.IsVerified)
            return new BaseResult {Message = "User already verified", Success = false};

        var existingToken = await _userVerificationRepository.GetExistingVerifyToken(user.Id);

        if (existingToken.Item1 is not null)
            return new BaseResult {Message = "User already has actual token", Success = false};

        var token = Guid.NewGuid().ToString();
        var expiryDate = DateTime.UtcNow.AddHours(1);
        await _userVerificationRepository.CreateVerifyToken(token, expiryDate, user.Id);

        return new BaseResult
        {
            Message = token, 
            Success = true
        };
    }

    public async Task<BaseResult> VerifyUser(string token, string userEmail)
    {
        var user = await _userService.GetUserByEmailAsync(userEmail);
        if (user is null)
            return new BaseResult {Message = "User does not exist", Success = false};

        if (user.IsVerified)
            return new BaseResult {Message = "User already verified", Success = false};

        var existingToken = await _userVerificationRepository.GetExistingVerifyToken(user.Id);

        if (existingToken.Item1 is null)
            return new BaseResult {Message = "User don't have a verification token", Success = false};

        if (existingToken.Item2 <= DateTime.UtcNow)
        {
            await  _userVerificationRepository.RevokeExpiredToken(existingToken.Item1);
            return new BaseResult {Message = "Token is expired", Success = false};   
        }

        await _userVerificationRepository.VerifyUser(user.Id);

        return new BaseResult {Success = true};
    }
}