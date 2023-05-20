using Npgsql;

namespace Messenger.Database.Read;

public class MessengerReadonlyContext : IDisposable, IAsyncDisposable
{
    public NpgsqlConnection Connection { get; }

    public MessengerReadonlyContext(string connectionString)
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