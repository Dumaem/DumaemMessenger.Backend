using System.Text;
using Messenger.Domain.Settings;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.Domain.Tests;

public static class MockExtensions
{
    private const string Key = "abcdabcdabcdabcdabcdabcdabcdabcd"; 
    public static void SetupJwtSettingsMock(this Mock<JwtSettings> mock)
    {
        mock.Object.Key = Key;
        mock.Object.AccessTokenLifetime = TimeSpan.FromMinutes(15);
        mock.Object.RefreshTokenMonthLifetime = 6;
    }
    public static void SetupTokenValidationParameters(this Mock<TokenValidationParameters> mock)
    {
        mock.Object.ValidateIssuerSigningKey = true;
        mock.Object.IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        mock.Object.ValidateIssuer = false;
        mock.Object.ValidateAudience = false;
        mock.Object.ValidateLifetime = true;
        mock.Object.RequireExpirationTime = false;
        mock.Object.ClockSkew = TimeSpan.Zero;
    }
}