using FluentMigrator;

namespace Messenger.Migrator.Migrations;

[Migration(2023_02_04_01)]
public class Migration2023020401 : Migration
{
    public override void Up()
    {
        Create.ForeignKey()
            .FromTable("user_chat")
            .ForeignColumn("chat_id")
            .ToTable("chat")
            .PrimaryColumn("id");
    }

    public override void Down()
    {
        throw new NotImplementedException();
    }
}