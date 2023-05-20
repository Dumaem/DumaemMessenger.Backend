namespace Messenger.Migrator.Migrations._2023._05;

[Migration(2023110501)]
public class Migration2023110501 : MessengerMigration
{
    public override void Up()
    {
        Create.UniqueConstraint().OnTable("user_chat")
            .Columns("user_id", "chat_id");
    }
}