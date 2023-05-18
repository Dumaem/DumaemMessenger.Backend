namespace Messenger.Migrator.Migrations._2023._01;

[Migration(2023_01_14_04)]
public class Migration2023011404 : Migration
{
    public override void Down()
    {
        throw new NotImplementedException();
    }

    public override void Up()
    {
        Create.ForeignKey()
            .FromTable("message").ForeignColumn("forwarded_message_id")
            .ToTable("message").PrimaryColumn("id");

        Create.ForeignKey()
            .FromTable("message").ForeignColumn("replied_message_id")
            .ToTable("message").PrimaryColumn("id");

        Create.ForeignKey()
            .FromTable("message").ForeignColumn("chat_id")
            .ToTable("chat").PrimaryColumn("id");

        Create.ForeignKey()
            .FromTable("message").ForeignColumn("sender_id")
            .ToTable("user").PrimaryColumn("id");
            
        Create.ForeignKey()
            .FromTable("message_content").ForeignColumn("message_id")
            .ToTable("message").PrimaryColumn("id");
            
        Create.ForeignKey()
            .FromTable("message_content").ForeignColumn("type_id")
            .ToTable("content_type").PrimaryColumn("id");
            
        Create.ForeignKey()
            .FromTable("user_chat").ForeignColumn("user_id")
            .ToTable("user").PrimaryColumn("id");
            
        Create.ForeignKey()
            .FromTable("deleted_message").ForeignColumn("user_id")
            .ToTable("user").PrimaryColumn("id");
            
        Create.ForeignKey()
            .FromTable("deleted_message").ForeignColumn("message_id")
            .ToTable("message").PrimaryColumn("id");
            
        Create.ForeignKey()
            .FromTable("read_message").ForeignColumn("user_id")
            .ToTable("user").PrimaryColumn("id");
            
        Create.ForeignKey()
            .FromTable("read_message").ForeignColumn("message_id")
            .ToTable("message").PrimaryColumn("id");
    }
}