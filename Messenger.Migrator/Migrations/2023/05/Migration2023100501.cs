namespace Messenger.Migrator.Migrations._2023._05;

[Migration(2023100501)]
public class Migration2023100501 : MessengerMigration
{
    public override void Up()
    {
        Alter.Table("chat")
            .AddColumn("isPersonal")
            .AsBoolean()
            .WithDefaultValue(false);
    }
}