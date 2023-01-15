namespace Messenger.Domain.Settings;

/// <summary>
/// Settings from appsettings.json
/// </summary>
public class JwtSettings
{
    public string Key { get; set; } = null!;
    public TimeSpan AccessTokenLifetime { get; set; }
    public int RefreshTokenMonthLifetime { get; set; }
}