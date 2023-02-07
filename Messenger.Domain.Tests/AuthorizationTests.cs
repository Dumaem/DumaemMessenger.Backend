using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Messenger.Domain.Services;
using Messenger.Domain.Services.Impl;
using Messenger.Domain.Settings;
using Microsoft.IdentityModel.Tokens;
using Messenger.Domain.ErrorMessages;

namespace Messenger.Domain.Tests;

public class AuthorizationTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<JwtSettings> _jwtSettingsMock;
    private readonly Mock<IEncryptionService> _encryptionService;
    private readonly IAuthorizationService _authorizationService;
    private const string DefaultDeviceId = "defaultDeviceId";
    private readonly TokenValidationParameters _tokenValidationParameters;

    public AuthorizationTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _jwtSettingsMock = new Mock<JwtSettings>();
        _encryptionService = new Mock<IEncryptionService>();
        _tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(MockExtensions.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = false,
            ClockSkew = TimeSpan.Zero
        };

        _authorizationService = new AuthorizationService(_userServiceMock.Object, _jwtSettingsMock.Object,
            _tokenValidationParameters, _refreshTokenRepositoryMock.Object, _encryptionService.Object);
    }

    #region RegisterAsync

    [Fact]
    public async Task RegisterAsync_ExistingUser_ShouldReturnUnsuccessfulResult()
    {
        _userServiceMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User());

        var res = await _authorizationService.RegisterAsync(It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeFalse();
        res.Message.Should().Be("User with this email already exists");
    }

    [Fact]
    public async Task RegisterAsync_SuccessPath_ShouldReturnSuccessfulResult()
    {
        _encryptionService.Setup(x => x.EncryptStringAsync(It.IsAny<string>()))
            .ReturnsAsync(DefaultDeviceId);
        _userServiceMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);
        _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(It.IsAny<int>());
        _jwtSettingsMock.SetupJwtSettingsMock();

        var res = await _authorizationService.RegisterAsync(string.Empty, string.Empty,
            string.Empty, It.IsAny<string>(), String.Empty);

        res.Success.Should().BeTrue();
        res.Message.Should().BeNull();
        res.Token.Should().NotBeNull();
        res.Token!.RefreshToken.Should().NotBeNull();
        res.Token.AccessToken.Should().NotBeNull();
    }

    #endregion

    #region AuthorizeAsync

    [Fact]
    public async Task AuthorizeAsync_NotExistUser_ShouldReturnUnsuccessfulResult()
    {
        _userServiceMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var res = await _authorizationService.AuthorizeAsync(It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>());

        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeFalse();
        res.Message.Should().Be("User does not exist");
    }

    [Fact]
    public async Task AuthorizeAsync_WrongPassword_ShouldReturnUnsuccessfulResult()
    {
        _userServiceMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User());
        _userServiceMock.Setup(x => x.CheckUserPasswordAsync(It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(false);

        var res = await _authorizationService.AuthorizeAsync(It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>());

        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeFalse();
        res.Message.Should().Be("User has wrong password");
    }

    [Fact]
    public async Task AuthorizeAsync_SuccessPath_ShouldReturnSuccessfulResult()
    {
        _encryptionService.Setup(x => x.EncryptStringAsync(It.IsAny<string>()))
            .ReturnsAsync(DefaultDeviceId);
        _refreshTokenRepositoryMock.Setup(x =>
                x.GetTokenByUserAndDeviceIdAsync(It.IsAny<int>(), DefaultDeviceId))
            .ReturnsAsync(new RefreshToken
            {
                UserId = 1,
                IsUsed = false,
                Id = 1,
                IsRevoked = false
            });
        _userServiceMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User
        {
            Name = It.IsAny<string>(),
            Username = It.IsAny<string>(),
            Email = "1",
            Id = 1,
            Password = It.IsAny<string>()
        });
        _userServiceMock.Setup(x => x.CheckUserPasswordAsync(It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(true);
        _jwtSettingsMock.SetupJwtSettingsMock();
        _refreshTokenRepositoryMock.Setup(x => x.RevokeTokenIfExistsAsync(It.IsAny<int>(),
            It.IsAny<string>())).Returns(Task.CompletedTask);
        var res = await _authorizationService.AuthorizeAsync(It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>());

        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeTrue();
        res.Message.Should().BeNull();
        res.Token.Should().NotBeNull();
        res.Token!.RefreshToken.Should().NotBeNull();
        res.Token.AccessToken.Should().NotBeNull();
    }

    #endregion

    #region RefreshAsync

    [Fact]
    public async Task RefreshAsync_InvalidAccessToken_ShouldReturnUnsuccessfulResult()
    {
        var res = await _authorizationService.RefreshAsync(It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>());

        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeFalse();
        res.Message.Should().Be(RefreshTokenErrorMessages.InvalidToken);
    }

    [Fact]
    public async Task RefreshAsync_InvalidRefreshToken_ShouldReturnUnsuccessfulResult()
    {
        _jwtSettingsMock.SetupJwtSettingsMock();
        var testUser = new User
        {
            Email = "test@gmail.com",
            Id = 1
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettingsMock.Object.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, testUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, testUser.Email),
                new Claim("id", testUser.Id.ToString()),
                new Claim("deviceId", DefaultDeviceId)
            }),
            Expires = DateTime.UtcNow.Add(_jwtSettingsMock.Object.AccessTokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var accessToken = tokenHandler.CreateToken(tokenDescriptor);

        _refreshTokenRepositoryMock.Setup(x => x.GetTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((RefreshToken?)null);

        var res = await _authorizationService.RefreshAsync(tokenHandler.WriteToken(accessToken),
            It.IsAny<string>(), It.IsAny<string>());

        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeFalse();
        res.Message.Should().Be(RefreshTokenErrorMessages.UnrecognizedToken);
    }

    [Fact]
    public async Task RefreshAsync_NotContainsJtiToken_ShouldReturnUnsuccessfulResult()
    {
        _jwtSettingsMock.SetupJwtSettingsMock();
        var testUser = new User
        {
            Email = "test@gmail.com",
            Id = 1
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettingsMock.Object.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, testUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, testUser.Email),
                new Claim("id", testUser.Id.ToString()),
                new Claim("deviceId", DefaultDeviceId)
            }),
            Expires = DateTime.UtcNow.Add(_jwtSettingsMock.Object.AccessTokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var accessToken = tokenHandler.CreateToken(tokenDescriptor);

        _refreshTokenRepositoryMock.Setup(x => x.GetTokenAsync(It.IsAny<string>()))
            .ReturnsAsync(new RefreshToken
            {
                Id = 1,
                JwtId = "232323232"
            });

        var res = await _authorizationService.RefreshAsync(tokenHandler.WriteToken(accessToken),
            It.IsAny<string>(), It.IsAny<string>());

        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeFalse();
        res.Message.Should().Be(RefreshTokenErrorMessages.UnrecognizedToken);
    }

    [Fact]
    public async Task RefreshAsync_WrongSecurityAlgorithm_ShouldReturnUnsuccessfulResult()
    {
        _jwtSettingsMock.SetupJwtSettingsMock();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettingsMock.Object.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "test@gmail.com"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, "test@gmail.com"),
                new Claim("id", 1.ToString()),
                new Claim("deviceId", DefaultDeviceId)
            }),
            Expires = DateTime.UtcNow.Add(_jwtSettingsMock.Object.AccessTokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
        };
        var accessToken = tokenHandler.CreateToken(tokenDescriptor);

        var res = await _authorizationService.RefreshAsync(tokenHandler.WriteToken(accessToken),
            It.IsAny<string>(), It.IsAny<string>());

        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeFalse();
        res.Message.Should().Be(RefreshTokenErrorMessages.InvalidToken);
    }

    [Fact]
    public async Task RefreshAsync_ExpiredToken_ShouldReturnUnsuccessfulResult()
    {
        _jwtSettingsMock.SetupJwtSettingsMock();
        var testUser = new User
        {
            Email = "test@gmail.com",
            Id = 1
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettingsMock.Object.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, testUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, testUser.Email),
                new Claim("id", testUser.Id.ToString()),
                new Claim("deviceId", DefaultDeviceId)
            }),
            Expires = DateTime.UtcNow.Add(_jwtSettingsMock.Object.AccessTokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var accessToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(accessToken);

        var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);
        var jti = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        _refreshTokenRepositoryMock.Setup(x => x.GetTokenAsync(It.IsAny<string>()))
            .ReturnsAsync(new RefreshToken
            {
                Id = 1,
                JwtId = jti
            });

        var res = await _authorizationService.RefreshAsync(token,
            It.IsAny<string>(), It.IsAny<string>());

        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeFalse();
        res.Message.Should().Be(RefreshTokenErrorMessages.ExpiredToken);
    }

    [Fact]
    public async Task RefreshAsync_UsedToken_ShouldReturnUnsuccessfulResult()
    {
        var testUser = new User
        {
            Email = "test@gmail.com",
            Id = 1
        };
        _jwtSettingsMock.SetupJwtSettingsMock();
        _userServiceMock.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(testUser);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettingsMock.Object.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, testUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, testUser.Email),
                new Claim("id", testUser.Id.ToString()),
                new Claim("deviceId", DefaultDeviceId)
            }),
            Expires = DateTime.UtcNow.Add(_jwtSettingsMock.Object.AccessTokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var accessToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(accessToken);

        var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);
        var jti = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        _refreshTokenRepositoryMock.Setup(x => x.GetTokenAsync(It.IsAny<string>()))
            .ReturnsAsync(new RefreshToken
            {
                Id = 1,
                JwtId = jti,
                ExpiryDate = DateTime.UtcNow.AddDays(10),
                IsUsed = true,
                IsRevoked = false
            });
        _refreshTokenRepositoryMock.Setup(x => x.RevokeTokenIfExistsAsync(It.IsAny<int>(),
            It.IsAny<string>())).Returns(Task.CompletedTask);

        var res = await _authorizationService.RefreshAsync(token,
            It.IsAny<string>(), It.IsAny<string>());

        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeFalse();
        res.Message.Should().Be(RefreshTokenErrorMessages.UsedToken);
    }

    [Fact]
    public async Task RefreshAsync_RevokedToken_ShouldReturnUnsuccessfulResult()
    {
        var testUser = new User
        {
            Email = "test@gmail.com",
            Id = 1
        };
        _jwtSettingsMock.SetupJwtSettingsMock();
        _userServiceMock.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(testUser);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettingsMock.Object.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, testUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, testUser.Email),
                new Claim("id", testUser.Id.ToString()),
                new Claim("deviceId", DefaultDeviceId)
            }),
            Expires = DateTime.UtcNow.Add(_jwtSettingsMock.Object.AccessTokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var accessToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(accessToken);

        var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);
        var jti = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        _refreshTokenRepositoryMock.Setup(x => x.GetTokenAsync(It.IsAny<string>()))
            .ReturnsAsync(new RefreshToken
            {
                Id = 1,
                JwtId = jti,
                ExpiryDate = DateTime.UtcNow.AddDays(10),
                IsUsed = false,
                IsRevoked = true
            });
        _refreshTokenRepositoryMock.Setup(x => x.RevokeTokenIfExistsAsync(It.IsAny<int>(),
            It.IsAny<string>())).Returns(Task.CompletedTask);

        var res = await _authorizationService.RefreshAsync(token,
            It.IsAny<string>(), It.IsAny<string>());

        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeFalse();
        res.Message.Should().Be(RefreshTokenErrorMessages.UsedToken);
    }

    [Fact]
    public async Task RefreshAsync_SuccessPath_ShouldReturnSuccessfulResult()
    {
        var testUser = new User
        {
            Email = "test@gmail.com",
            Id = 1
        };
        _jwtSettingsMock.SetupJwtSettingsMock();
        _userServiceMock.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(testUser);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettingsMock.Object.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, testUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, testUser.Email),
                new Claim("id", testUser.Id.ToString()),
                new Claim("deviceId", DefaultDeviceId)
            }),
            Expires = DateTime.UtcNow.Add(_jwtSettingsMock.Object.AccessTokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var accessToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(accessToken);

        var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);
        var jti = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        _refreshTokenRepositoryMock.Setup(x => x.GetTokenAsync(It.IsAny<string>()))
            .ReturnsAsync(new RefreshToken
            {
                Id = 1,
                JwtId = jti,
                ExpiryDate = DateTime.UtcNow.AddDays(10),
                IsUsed = false,
                IsRevoked = false
            });
        _encryptionService.Setup(x => x.EncryptStringAsync(It.IsAny<string>()))
            .ReturnsAsync(DefaultDeviceId);

        var res = await _authorizationService.RefreshAsync(token,
            It.IsAny<string>(), It.IsAny<string>());

        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeTrue();
    }

    #endregion
}