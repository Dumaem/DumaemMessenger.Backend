using FluentMigrator;

namespace Messenger.Migrator.Migrations;

[Migration(2023012301)]
public class Migration2023012301 : Migration
{
    public override void Up()
    {
        Alter.Table("refresh_token")
            .AddColumn("device_id")
            .AsString(250)
            .Nullable();
        
        Create.Index()
            .OnTable("refresh_token")
            .OnColumn("user_id").Ascending();

        Create.Index()
            .OnTable("refresh_token")
            .OnColumn("device_id").Ascending();
    }

    public override void Down()
    {
        throw new NotImplementedException();
    }
}