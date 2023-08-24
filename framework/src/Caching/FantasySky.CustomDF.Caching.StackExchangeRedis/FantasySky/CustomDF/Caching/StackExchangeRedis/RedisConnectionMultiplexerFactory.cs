using FantasySky.CustomDF.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.Caching.StackExchangeRedis;

[Dependency(typeof(IRedisConnectionMultiplexerFactory), ServiceLifetime.Singleton)]
internal class RedisConnectionMultiplexerFactory : IRedisConnectionMultiplexerFactory, IDisposable
{
    private readonly SemaphoreSlim _connectionLock = new(initialCount: 1, maxCount: 1);
    private readonly IDictionary<string, ConnectionMultiplexer> _connections = new Dictionary<string, ConnectionMultiplexer>(5);
    private readonly ILogger<RedisConnectionMultiplexerFactory> _logger;
    private bool _disposed = false;

    public RedisConnectionMultiplexerFactory(
        ILogger<RedisConnectionMultiplexerFactory> logger)
    {
        _logger = logger;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        foreach (var (_, connection) in _connections)
        {
            connection?.Close();
        }
    }

    public ConnectionMultiplexer GetConnection(string? connectionString)
    {
        this.CheckDisposed();

        if (String.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"{nameof(connectionString)} cannot be null or empty.");
        }

        if (_connections.ContainsKey(connectionString))
        {
            return _connections[connectionString];
        }
        else
        {
            _connectionLock.Wait();

            try
            {
                if (_connections.ContainsKey(connectionString))
                {
                    return _connections[connectionString];
                }

                var connection = ConnectionMultiplexer.Connect(connectionString);

                return connection;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Reids connect failed,Connection string:{connectionString}");
                throw;
            }
            finally
            {
                _connectionLock.Release();
            }
        }
    }

    public async Task<ConnectionMultiplexer> GetConnectionAsync(string? connectionString, CancellationToken cancellationToken = default)
    {
        this.CheckDisposed();

        cancellationToken.ThrowIfCancellationRequested();

        if (String.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"{nameof(connectionString)} cannot be null or empty.");
        }

        if (_connections.ContainsKey(connectionString))
        {
            return _connections[connectionString];
        }
        else
        {
            await _connectionLock.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                if (_connections.ContainsKey(connectionString))
                {
                    return _connections[connectionString];
                }

                var connection = await ConnectionMultiplexer.ConnectAsync(connectionString).ConfigureAwait(false);

                return connection;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Reids connect failed,Connection string:{connectionString}");
                throw;
            }
            finally
            {
                _connectionLock.Release();
            }
        }
    }

    private void CheckDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(this.GetType().FullName);
        }
    }
}
