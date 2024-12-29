using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using PM.CORE;

namespace PM.INFRASTRUCTURE.Cache
{
    public interface ICacheService
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getDataFunc, int? timeInMinutes = null);
        Task<T> GetOrSetLongTermAsync<T>(string key, Func<Task<T>> getDataFunc);
        void Remove(string key);
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getDataFunc, int? timeInMinutes = null)
        {
            if (_cache.TryGetValue(key, out T cachedData))
                return cachedData;

            var data = await getDataFunc();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(
                    timeInMinutes ?? Constants.Cache.DefaultCacheTimeInMinutes));

            _cache.Set(key, data, cacheOptions);

            return data;
        }

        public async Task<T> GetOrSetLongTermAsync<T>(string key, Func<Task<T>> getDataFunc)
        {
            if (_cache.TryGetValue(key, out T cachedData))
                return cachedData;

            var data = await getDataFunc();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.NeverRemove);

            _cache.Set(key, data, cacheOptions);

            return data;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}