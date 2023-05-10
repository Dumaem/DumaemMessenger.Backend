using Messenger.Domain.Settings;

namespace Messenger.Domain.Tests;

public static class MockExtensions
{
    public const string Key = "abcdabcdabcdabcdabcdabcdabcdabcd"; 
    public static void SetupJwtSettingsMock(this Mock<JwtSettings> mock)
    {
        mock.Object.Key = Key;
        mock.Object.AccessTokenLifetime = TimeSpan.FromMinutes(15);
        mock.Object.RefreshTokenMonthLifetime = 6;
    }
}