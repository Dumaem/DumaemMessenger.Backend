using Npgsql;

namespace Messenger.Database;

public class NgpsqlContext : IDisposable, IAsyncDisposable
{
    public NpgsqlConnection Connection { get; }

    public NgpsqlContext(string connectionString)
    {
        Connection = new NpgsqlConnection(connectionString);
    }

    public void Dispose()
    {
        Connection.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await Connection.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}