using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace FlyMosquito.Common
{
    /// <summary>
    /// MemoryCache 的辅助类，用于全局缓存操作
    /// </summary>
    public class MemoryCacheHelper
    {
        private readonly IMemoryCache _cache;
        private readonly ConcurrentDictionary<string, byte> _keys;

        public MemoryCacheHelper(IMemoryCache cache)
        {
            _cache = cache;
            _keys = new ConcurrentDictionary<string, byte>();
        }

        /// <summary>
        /// 将对象添加到缓存
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存对象</param>
        /// <param name="intMinutes">缓存过期时间（分钟）</param>
        public void SetObject<T>(string key, T value, int intMinutes = 60)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var cacheKey = $"FlyMosquito_{key}";
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(intMinutes));

            _cache.Set(cacheKey, value, cacheEntryOptions);
            _keys.TryAdd(cacheKey, 0);
        }

        /// <summary>
        /// 从缓存中获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>如果找到对象，返回对象；否则，返回默认值</returns>
        public T GetObject<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var cacheKey = $"FlyMosquito_{key}";
            _cache.TryGetValue(cacheKey, out T value);
            return value;
        }

        /// <summary>
        /// 从缓存中移除对象
        /// </summary>
        /// <param name="key">缓存键</param>
        public void RemoveObject(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var cacheKey = $"FlyMosquito_{key}";
            _cache.Remove(cacheKey);
            _keys.TryRemove(cacheKey, out _);
        }

        /// <summary>
        /// 清空所有缓存项
        /// </summary>
        public void ClearAll()
        {
            foreach (var key in _keys.Keys)
            {
                _cache.Remove(key);
                _keys.TryRemove(key, out _);
            }
        }
    }
}
