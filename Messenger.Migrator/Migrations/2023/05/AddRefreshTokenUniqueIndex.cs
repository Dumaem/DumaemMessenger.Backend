using Dapper;

namespace Messenger.Migrator.Migrations._2023._05;

[Migration(2023_05_18_01)]
public class AddRefreshTokenUniqueIndex : MessengerMigration
{
    public override void Up()
    {
        Connection = new(ConnectionString);
        Connection.Execute(
            @"CREATE UNIQUE INDEX 
                    ON refresh_token (is_used, is_revoked, user_id, device_id) 
                 WHERE NOT is_used 
                    AND NOT is_revoked;");
    }
}