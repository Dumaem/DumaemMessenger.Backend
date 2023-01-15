using Messenger.Domain.Settings;

namespace Messenger.Domain.Tests;

public static class MockExtensions
{
    public static void SetupJwtSettingsMock(this Mock<JwtSettings> mock)
    {
        mock.Object.Key = "abcdabcdabcdabcdabcdabcdabcdabcd";
        mock.Object.AccessTokenLifetime = TimeSpan.FromMinutes(15);
        mock.Object.RefreshTokenMonthLifetime = 6;
    }
}