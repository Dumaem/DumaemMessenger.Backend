using FluentMigrator;

namespace Messenger.Migrator;

public abstract class MessengerMigration : Migration
{
    public override void Down()
    {
        throw new NotSupportedException();
    }
}