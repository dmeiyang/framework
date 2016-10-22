using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amy.Runtime.Caching;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace Amy.Runtime.Caching.Redis
{
    public class RedisCache : ICache
    {
        private static Configuration.Config config = (Configuration.Config)System.Configuration.ConfigurationManager.GetSection("RedisConfig");

        #region -- 连接信息 --
        public static PooledRedisClientManager prcm = new PooledRedisClientManager(config.ReadWriteHosts, config.ReadOnlyHosts, new RedisClientManagerConfig
        {
            MaxWritePoolSize = config.MaxWritePoolSize, // “写”链接池链接数  
            MaxReadPoolSize = config.MaxReadPoolSize, // “读”链接池链接数  
            AutoStart = true,
        });
        #endregion

        public TimeSpan DefaultExpireTime { get; set; }

        public RedisCache()
        {
            this.DefaultExpireTime = TimeSpan.FromHours(1);
        }

        #region redis：全部（all/bulk）
        /// <summary>
        /// 获取所有缓存key值集合
        /// </summary>
        /// <returns>key值集合</returns>
        public List<string> GetAll()
        {
            using (var client = prcm.GetReadOnlyClient())
            {
                return client.GetAllKeys();
            }
        }

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        /// <returns>true，移除成功；false，移除失败</returns>
        public bool FlushAll()
        {
            using (var client = prcm.GetClient())
            {
                client.FlushAll();

                return true;
            }
        }

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        /// <param name="dic">缓存集合</param>
        /// <returns>true，设置成功；false，设置失败</returns>
        public bool BulkSet<T>(Dictionary<string, T> dic)
        {
            using (var client = prcm.GetClient())
            {
                client.SetAll(dic);

                return true;
            }
        }

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        /// <param name="dic">缓存集合</param>
        /// <returns>true，设置成功；false，设置失败</returns>
        public bool BulkSet(Dictionary<string, object> dic)
        {
            using (var client = prcm.GetClient())
            {
                client.SetAll(dic);

                return true;
            }
        }

        /// <summary>
        /// 批量移除缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool BulkRemove(IEnumerable<string> keys)
        {
            using (var client = prcm.GetClient())
            {
                client.RemoveAll(keys);

                return true;
            }
        }
        #endregion

        #region redis：对象（item）
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>缓存内容</returns>
        public T Get<T>(string key)
        {
            using (var client = prcm.GetClient())
            {
                return client.Get<T>(key);
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存内容</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns>true，设置成功；false，设置失败</returns>
        public bool Set<T>(string key, T value, TimeSpan? expireTime = null)
        {
            using (var client = prcm.GetClient())
            {
                return client.Set<T>(key, value, expireTime == null ? this.DefaultExpireTime : (TimeSpan)expireTime);
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>true，移除成功；false，移除失败</returns>
        public bool Remove(string key)
        {
            using (var client = prcm.GetClient())
            {
                return client.Remove(key);
            }
        }
        #endregion
    }
}
