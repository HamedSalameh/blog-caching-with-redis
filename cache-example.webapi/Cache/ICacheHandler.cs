using StackExchange.Redis;
using System.Collections;

namespace cache_example.webapi.Cache
{
    public interface ICacheHandler : IDisposable
    {
        
        // Cache operations interface, include all possible operation supported by Redis 
        // https://stackexchange.github.io/StackExchange.Redis/Basics
        
        // String
        Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null);
        Task<string?> StringGetAsync(string key);
        Task StringDeleteAsync(string key);
        Task<bool> StringExistsAsync(string key);  
        
        // Hashes
        Task HashSetAsync(string key, string hashField, string value);
        Task HashSetAsync(string key, IEnumerable<KeyValuePair<string, string?>> values);
        Task<string?> HashGetAsync(string key, string hashField);
    }
}
