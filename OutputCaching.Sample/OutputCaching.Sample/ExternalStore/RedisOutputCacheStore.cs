using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace OutputCaching.Sample.ExternalStore
{
    public interface IRedisOutputCacheStore : IOutputCacheStore
    {

    }

    internal class RedisOutputCacheStore : IRedisOutputCacheStore
    {
        private readonly IConnectionMultiplexer _connection; 

        private readonly Dictionary<string, HashSet<string>> _taggedEntries = new();

        public RedisOutputCacheStore(IConnectionMultiplexer connection)
        {
            _connection= connection;
        }

        public async ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(tag);
            var db = _connection.GetDatabase();
            //récup des membres avec le tag passé en paramètre
            var cachedKeys = await db.SetMembersAsync(tag);
            var keys = cachedKeys
                .Select(x => (RedisKey)x.ToString())
                .Concat(new[] { (RedisKey)tag})
                .ToArray();

            await db.KeyDeleteAsync(keys);
                      
        }

        public async ValueTask<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(key);
            var db = _connection.GetDatabase();
            return await db.StringGetAsync(key);

        }

        public async ValueTask SetAsync(string key, byte[] value, string[]? tags, TimeSpan validFor, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(value);
            var db = _connection.GetDatabase();

            foreach (var tag in tags ?? Array.Empty<string>())
            {
                //association de la clé avec un tag pour invalidation
                await db.SetAddAsync(tag, key);
            }
            await db.StringSetAsync(key, value, validFor);
        }
    }
}
