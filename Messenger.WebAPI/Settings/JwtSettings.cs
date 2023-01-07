namespace Messenger.WebAPI.Settings;

/// <summary>
/// Settings from appsettings.json
/// </summary>
public class JwtSettings
{
    public string Key { get; set; }
    public TimeSpan AccessTokenLifetime { get; set; }
    public int RefreshTokenMonthLifetime { get; set; }
}