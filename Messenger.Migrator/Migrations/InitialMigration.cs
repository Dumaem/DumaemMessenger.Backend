using FluentMigrator;

namespace Messenger.Migrator.Migrations;

[Migration(2023_01_13_1)]
public class InitialMigration : Migration
{
    public override void Up()
    {
        Create.Table("test")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("name").AsString(50).NotNullable();
    }

    public override void Down()
    {
    }
}