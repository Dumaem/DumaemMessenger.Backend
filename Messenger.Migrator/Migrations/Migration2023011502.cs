using FluentMigrator;

namespace Messenger.Migrator.Migrations;

[Migration(2023011502)]
public class Migration2023011502 : Migration
{
    public override void Up()
    {
        Alter.Column("creation_date")
            .OnTable("refresh_token")
            .AsCustom("timestamp with time zone");

        Alter.Column("expiry_date")
            .OnTable("refresh_token")
            .AsCustom("timestamp with time zone");
    }

    public override void Down()
    {
        throw new NotImplementedException();
    }
}