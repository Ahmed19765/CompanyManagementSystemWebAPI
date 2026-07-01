using CompanyManagementSystem.Application.Interfaces.Services.MemoryCache;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace CompanyManagementSystem.Infrastructure.Services
{
    public class MemoryCacheService<T> : IMemoryCache<T>
    {
        private readonly Microsoft.Extensions.Caching.Memory.IMemoryCache _memoryCache;

        public MemoryCacheService(Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Save(string key, T value, TypeOfValue TypeV, TimeSpan expiration)
        {
            var cacheKey = GetCacheKey(key, TypeV);
            _memoryCache.Set(cacheKey, value, expiration);
        }

        public bool Validate(string key, T value, TypeOfValue TypeV)
        {
            var cacheKey = GetCacheKey(key, TypeV);
            if (_memoryCache.TryGetValue(cacheKey, out T? cachedValue))
            {
                return cachedValue != null && cachedValue.Equals(value);
            }
            return false;
        }

        public void Remove(string key, TypeOfValue TypeV)
        {
            var cacheKey = GetCacheKey(key, TypeV);
            _memoryCache.Remove(cacheKey);
        }

        public void Cleaner()
        {
            // Note: Microsoft.Extensions.Caching.Memory handles self-cleanup of expired entries.
        }

        private string GetCacheKey(string key, TypeOfValue typeV)
        {
            return $"{typeV}:{key}";
        }
    }
}
