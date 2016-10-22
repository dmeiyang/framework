using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amy.Runtime.Caching
{
    public interface ICache
    {
        /// <summary>
        /// 默认过期时间，默认60分钟
        /// </summary>
        TimeSpan DefaultExpireTime { get; set; }

        #region redis：全部（all/bulk）
        /// <summary>
        /// 获取所有缓存key值集合
        /// </summary>
        /// <returns>key值集合</returns>
        List<string> GetAll();

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        /// <returns>true，移除成功；false，移除失败</returns>
        bool FlushAll();

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        /// <param name="dic">缓存集合</param>
        /// <returns>true，设置成功；false，设置失败</returns>
        bool BulkSet<T>(Dictionary<string, T> dic);

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        /// <param name="dic">缓存集合</param>
        /// <returns>true，设置成功；false，设置失败</returns>
        bool BulkSet(Dictionary<string, object> dic);

        /// <summary>
        /// 批量移除缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        bool BulkRemove(IEnumerable<string> keys);
        #endregion

        #region redis：对象（item）
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>缓存内容</returns>
        T Get<T>(string key);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存内容</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns>true，设置成功；false，设置失败</returns>
        bool Set<T>(string key, T value, TimeSpan? expireTime = null);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>true，移除成功；false，移除失败</returns>
        bool Remove(string key);
        #endregion
    }
}
