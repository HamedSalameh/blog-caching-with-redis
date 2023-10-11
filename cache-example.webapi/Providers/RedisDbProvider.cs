using StackExchange.Redis;

namespace cache_example.webapi.Cache
{
    public class RedisDbProvider : IRedisDbProvider
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
        private bool disposed = false;
        private readonly string _connectionString;

        public RedisDbProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(connectionString);
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_connectionString));
        }

        public IDatabase database => _lazyConnection.Value.GetDatabase();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return; 

            if (disposing)
            {
                if (_lazyConnection.IsValueCreated)
                {
                    _lazyConnection.Value.Dispose();
                }
            }

            disposed = true;
        }
    }
}
