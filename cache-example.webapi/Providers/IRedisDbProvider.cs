using StackExchange.Redis;

namespace cache_example.webapi.Cache
{
    public interface IRedisDbProvider : IDisposable
    {
        public IDatabase database { get; }
    }
}
