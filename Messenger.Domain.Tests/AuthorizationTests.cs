using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Messenger.Domain.Services;
using Messenger.Domain.Services.Impl;
using Messenger.Domain.Settings;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.Domain.Tests;

public class AuthorizationTests
{
    private const string Email = "testEmail@mail.ru";
    private const string Password = "testEmail@mail.ru";
    
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<JwtSettings> _jwtSettingsMock;
    private readonly Mock<TokenValidationParameters> _tokenValidationParametersMock;

    private readonly IAuthorizationService _authorizationService;

    public AuthorizationTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _jwtSettingsMock = new Mock<JwtSettings>();
        _tokenValidationParametersMock = new Mock<TokenValidationParameters>();

        _authorizationService = new AuthorizationService(_userServiceMock.Object, _jwtSettingsMock.Object,
            _tokenValidationParametersMock.Object, _refreshTokenRepositoryMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ExistingUser_ShouldReturnUnsuccessfulResult()
    {
        _userServiceMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User());

        var res = await _authorizationService.RegisterAsync(It.IsAny<string>(), It.IsAny<string>());

        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeFalse();
        res.Message.Should().Be("User with this email already exists");
    }
    
    [Fact]
    public async Task RegisterAsync_UserHasNotBeenCreated_ShouldReturnUnsuccessfulResult()
    {
        _userServiceMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync((int?)null);

        var res = await _authorizationService.RegisterAsync(It.IsAny<string>(), It.IsAny<string>());
        
        res.Should().BeOfType<AuthenticationResult>();
        res.Success.Should().BeFalse();
        res.Message.Should().Be("Could not create user");
    }
    
    [Fact]
    public async Task RegisterAsync_SuccessPath_ShouldReturnSuccessfulResult()
    {
        _userServiceMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<User>(), Password)).ReturnsAsync(It.IsAny<int>());
        _jwtSettingsMock.SetupJwtSettingsMock();

        var res = await _authorizationService.RegisterAsync(Email, Password);
        
        res.Success.Should().BeTrue();
        res.Message.Should().BeNull();
        res.Token.Should().NotBeNull();
        res.Token!.RefreshToken.Should().NotBeNull();
        res.Token.AccessToken.Should().NotBeNull();
    }
}