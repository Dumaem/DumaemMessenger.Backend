using FluentMigrator;

namespace Messenger.Migrator.Migrations;

[Migration(2023_01_13_01)]
public class InitialMigration : Migration
{
    public override void Up()
    {
        Create.Table("message")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("date_of_dispatch").AsDateTime()
            .WithColumn("is_edited").AsBoolean()
            .WithColumn("is_deleted").AsBoolean()
            .WithColumn("sender_id").AsInt32().ForeignKey()
            .WithColumn("chat_id").AsInt32().ForeignKey()
            .WithColumn("replied_message_id").AsInt64().ForeignKey().Nullable()
            .WithColumn("forwarded_message_id").AsInt64().ForeignKey().Nullable();

        Create.Table("message_content")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("content").AsBinary()
            .WithColumn("message_id").AsInt64().ForeignKey()
            .WithColumn("type_id").AsInt32().ForeignKey();

        Create.Table("type_content")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString();

        Create.Table("chat")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString();

        Create.Table("user")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("username").AsString()
            .WithColumn("name").AsString();

        Create.Table("user_chat")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt32().ForeignKey()
            .WithColumn("chat_id").AsInt32().ForeignKey();

        Create.Table("deleted_message")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt32().ForeignKey()
            .WithColumn("message_id").AsInt64().ForeignKey();

        Create.Table("readed_message")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt32().ForeignKey()
            .WithColumn("message_id").AsInt64().ForeignKey();
    }

    public override void Down()
    {
    }
}