using System;
using JoKenPo.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace JoKenPo.Domain.Services
{
    public class Cache : ICache
    {
        private static readonly IMemoryCache MemoryCache = new MemoryCache(new MemoryDistributedCacheOptions {SizeLimit = null});
        public void AddUpdateCache(string key, object value, TimeSpan duration)
        {
            if (value is null)
                return;
            MemoryCache.Set(key, value, duration);
        }

        public object GetCache(string key)
        {
            return MemoryCache.Get(key);
        }

        public T GetCache<T>(string key)
        {
            return MemoryCache.Get<T>(key);
        }

        public void DeleteCache(string key)
        {
            MemoryCache.Remove(key);
        }
    }
}