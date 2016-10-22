using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amy.Runtime.Caching
{
    public interface IListCache<T> where T : class
    {
        /// <summary>
        /// 获取list缓存数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>缓存内容</returns>
        IEnumerable<T> GetList(string key);

        /// <summary>
        /// 获取list缓存数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="page">page</param>
        /// <param name="size">size</param>
        /// <returns>缓存内容</returns>
        PageList<T> GetList(string key, int page, int size);

        /// <summary>
        /// 获取list缓存总数
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>缓存总数</returns>
        long GetCountOfList(string key);

        /// <summary>
        /// 获取item详情
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="id">item对象id</param>
        /// <returns>item对象</returns>
        T GetItemFromList(string key, string id);

        /// <summary>
        /// 添加item到list
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">item对象</param>
        /// <returns>true，设置成功；false，设置失败</returns>
        bool AddItemToList(string key, T value);

        /// <summary>
        /// 压入item到list，与PopItemFromList组成栈（后进先出）
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">item对象</param>
        /// <returns>true，压入成功；false，压入失败</returns>
        bool PushItemToList(string key, T value);

        /// <summary>
        /// 取出list头部数据，与PushItemToList组成栈（后进先出）
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>item对象</returns>
        T PopItemFromList(string key);

        /// <summary>
        /// 压入item到list头部，与DequeueItemFromList组成队列（先进先出）
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">item对象</param>
        /// <returns>true，压入成功；false，压入失败</returns>
        bool PrependItemToList(string key, T value);

        /// <summary>
        /// 从list中取出item，PrependItemToList组成队列（先进先出）
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>item对象</returns>
        T DequeueItemFromList(string key);

        /// <summary>
        /// 设置list到期时间
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="expireTime">到期时间</param>
        /// <returns>true，设置成功；false，设置失败</returns>
        bool ExpireEntryAt(string key, DateTime expireTime);

        /// <summary>
        /// 移除list缓存数据（仅支持"Id"为标识的实体类缓存对象）
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="id">缓存对象Id</param>
        /// <returns>true，移除成功；false，移除失败</returns>
        bool RemoveItemFromList(string key, string id);

        /// <summary>
        /// 移除所有list缓存数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>true，移除成功；false，移除失败</returns>
        bool RemoveAllFromList(string key);
    }
}
