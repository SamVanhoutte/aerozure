namespace Aerozure.Caching
{
    internal class CachedItem<T>(T item, TimeSpan timeToCache, DateTime cacheEntry = default)
    {
        public DateTime CacheEntry { get; set; } = cacheEntry == default ? DateTime.UtcNow : cacheEntry;
        public DateTime CacheExpiration { get; set; } = DateTime.UtcNow.Add(timeToCache);
        public T Value { get; set; } = item;
        public bool IsValid => CacheExpiration > DateTime.UtcNow && Value != null;
    }
}