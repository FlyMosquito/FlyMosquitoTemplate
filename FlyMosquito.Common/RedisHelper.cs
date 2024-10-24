#region using
using Newtonsoft.Json;
using StackExchange.Redis;
#endregion

namespace FlyMosquito.Common
{
    public class RedisHelper
    {
        private readonly ConnectionMultiplexer RedisConnection;
        private readonly object _redisConnectionLock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public RedisHelper()
        {
            RedisConnection = ConnectionMultiplexer.Connect(GetConfigurationOptions());
        }

        private ConfigurationOptions GetConfigurationOptions()
        {
            List<RedisModel> redisConfig = AppsettHelper.app<RedisModel>(new string[] { "Redis" }); // 读取Redis配置信息
            if (redisConfig == null || !redisConfig.Any())
            {
                LoggerHelper.Error("Redis数据库配置有误");
                throw new ApplicationException("Redis数据库配置有误");
            }

            var firstConfig = redisConfig.First();
            return new ConfigurationOptions
            {
                EndPoints =
                {
                    {
                        firstConfig.Ip,
                        firstConfig.Port
                    }
                },
                ClientName = firstConfig.Name,
                Password = firstConfig.Password,
                ConnectTimeout = firstConfig.Timeout,
                DefaultDatabase = firstConfig.Db,
            };
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return RedisConnection.GetDatabase().StringGet(key);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        public void Set(string key, object value, TimeSpan? expiry = null)
        {
            if (value != null)
            {
                RedisConnection.GetDatabase().StringSet(key, JsonConvert.SerializeObject(value), expiry);
            }
        }

        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            RedisConnection.GetDatabase().KeyDelete(key);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TEntity Get<TEntity>(string key)
        {
            var value = RedisConnection.GetDatabase().StringGet(key);
            return value.HasValue ? JsonConvert.DeserializeObject<TEntity>(value) : default;
        }
    }

    public class RedisModel
    {
        public string? Name { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }

        public string? Password { get; set; }

        public int Timeout { get; set; }

        public int Db { get; set; }
    }
}
