using Aerozure.Extensions;

namespace Aerozure.Caching
{
    public class InMemoryCache<TValue> : InMemoryCache<string, TValue>
    {
        public InMemoryCache(TimeSpan cacheDuration) : base(cacheDuration)
        {
            
        }
    }

    public class InMemoryCache<TKey, TValue>
    {
        private IDictionary<string, CachedItem<TValue>> _memoryCache = new Dictionary<string, CachedItem<TValue>>();
        private TimeSpan _defaultCacheDuration;

        public InMemoryCache(TimeSpan cacheDuration)
        {
            _defaultCacheDuration = cacheDuration;
        }


        public async Task<TValue> ReadThroughAsync(TKey cacheKey, Func<TKey, Task<TValue>> readAction)
        {
            lock (_memoryCache)
            {
                if (_memoryCache.TryGetValue(cacheKey.ToString(), out var cacheEntry))
                {
                    if (cacheEntry.IsValid)
                    {
                        // Value found that is not expired, so returning that
                        return cacheEntry.Value;
                    }
                }
            }

            var newEntry = await readAction(cacheKey);
            lock (_memoryCache)
            {
                if (newEntry != null)
                {
                    var newCacheItem = new CachedItem<TValue>(newEntry, _defaultCacheDuration);
                    _memoryCache.Upsert(cacheKey.ToString(), newCacheItem);
                }

                return newEntry;
            }
        }


        public async Task<TValue> WriteBehindAsync(TKey cacheKey, TValue updatedValue, Func<TKey, TValue, Task<TValue>> persistAction)
        {
            // Execute persistence first
            var result = await persistAction(cacheKey, updatedValue);
            lock (_memoryCache)
            {
                _memoryCache.Upsert(cacheKey.ToString(), new CachedItem<TValue>(updatedValue, _defaultCacheDuration));
            }

            return result;
        }


        public async Task FullDeleteAsync(TKey cacheKey, Func<TKey, Task> deleteAction)
        {
            if (_memoryCache.ContainsKey(cacheKey.ToString()))
            {
                _memoryCache.Remove(cacheKey.ToString());
            }

            await deleteAction(cacheKey);
        }
    }
}