namespace Messenger.Migrator.Migrations;

[Migration(2023100501)]
public class Migration2023100501 : MessengerMigration
{
    public override void Up()
    {
        Alter
            .Table("message_content")
            .InSchema("public")
            .AlterColumn("content")
            .AsString(1000)
            .NotNullable();
    }
}