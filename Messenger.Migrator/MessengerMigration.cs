using FluentMigrator;
using Npgsql;

namespace Messenger.Migrator;

public abstract class MessengerMigration : Migration
{
    protected NpgsqlConnection Connection;

    protected MessengerMigration()
    {
        Connection = new(ConnectionString);
    }

    public override void Down()
    {
        throw new NotSupportedException();
    }
}