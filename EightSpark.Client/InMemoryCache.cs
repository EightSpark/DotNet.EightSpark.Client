using System;
using System.Runtime.Caching;

namespace EightSpark.Client
{
    public class InMemoryCache : ICache
    {
        private bool _useCache;
        private int _cacheTimeMinutes;

        public InMemoryCache(bool useCache, int cacheTimeMinutes)
        {
            _useCache = useCache;
            _cacheTimeMinutes = cacheTimeMinutes;
        }

        public void Set(string url, RuleValue deserializedResult)
        {
            if (_useCache)
            {
                CacheItemPolicy cip = new CacheItemPolicy()
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddMinutes(_cacheTimeMinutes))
                };
                MemoryCache.Default.Add(url, deserializedResult.Result, cip);
            }
        }

        public string Get(string url)
        {
            if (!_useCache)
                return null;

            var cacheValue = MemoryCache.Default.Get(url);

            return (string) cacheValue;
        }

        public void SetCacheTime(int minutes)
        {
            _useCache = minutes != 0;
            _cacheTimeMinutes = minutes;
        }
    }
}