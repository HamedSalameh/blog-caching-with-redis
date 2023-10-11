using Microsoft.Extensions.ObjectPool;
using StackExchange.Redis;

namespace cache_example.webapi.Cache
{
    public class RedisCacheHandler : ICacheHandler
    {
        private readonly IRedisDbProvider _redisDbProvider;

        public RedisCacheHandler(IRedisDbProvider redisDbProvider)
        {
            _redisDbProvider = redisDbProvider ?? throw new ArgumentNullException(nameof(_redisDbProvider));

            if (_redisDbProvider.database == null)
            {
                throw new ArgumentNullException("The provided redisDbProvider or its database is null");
            }
        }

        private bool disposedValue;

        public Task StringDeleteAsync(string key)
        {
            var _  = _redisDbProvider.database.StringGetDeleteAsync(key).ConfigureAwait(false);
            return Task.CompletedTask;
        }

        public async Task<bool> StringExistsAsync(string key)
        {
            return await _redisDbProvider.database.KeyExistsAsync(key).ConfigureAwait(false);
        }

        public async Task<string?> StringGetAsync(string key)
        {
            return await _redisDbProvider.database.StringGetAsync(key).ConfigureAwait(false);
        }

        public async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null)
        {
            return await _redisDbProvider.database.StringSetAsync(key, value, expiry).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a hashset with the given key, then add a single entry to it
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task HashSetAsync(string key, string hashField, string value)
        {
            await _redisDbProvider.database.HashSetAsync(key, hashField, value).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a hashset with the given key, then add multiple entries to it
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task HashSetAsync(string key, IEnumerable<KeyValuePair<string, string?>> values)
        {
            // convert the values to HashEntry[] array
            var hashEntries = values.Select(v => new HashEntry(v.Key, v.Value)).ToArray();
            await _redisDbProvider.database.HashSetAsync(key, hashEntries).ConfigureAwait(false);
        }

        /// <summary>
        /// Attempts to get a value from a hashset with the given key and hashField
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns>The requested hash, null of none was found mathcing the provided key</returns>
        public async Task<string?> HashGetAsync(string key, string hashField)
        {
            return await _redisDbProvider.database.HashGetAsync(key, hashField).ConfigureAwait(false);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _redisDbProvider.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~RedisCacheHandler()
        {
            Dispose(disposing: false);
        }
    }
}
