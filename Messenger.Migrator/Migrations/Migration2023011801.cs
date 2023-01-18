namespace Messenger.Migrator.Migrations;

[Migration(2023011801)]
public class Migration2023011801 : Migration
{
    public override void Up()
    {
        Create
            .UniqueConstraint()
            .OnTable("user")
            .Column("email");
        Create
            .UniqueConstraint()
            .OnTable("user")
            .Column("username");
    }

    public override void Down()
    {
        throw new NotImplementedException();
    }
}