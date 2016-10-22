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
    public class RedisListCache<T> : Amy.Runtime.Caching.IListCache<T> where T : class
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

        public RedisListCache()
        {
            this.DefaultExpireTime = TimeSpan.FromHours(1);
        }

        /// <summary>
        /// 获取list缓存数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>缓存内容</returns>
        public IEnumerable<T> GetList(string key)
        {
            using (var client = prcm.GetReadOnlyClient())
            {
                var tclient = client.GetTypedClient<T>();

                return tclient.Lists[key];
            }
        }

        /// <summary>
        /// 获取list缓存数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="page">page</param>
        /// <param name="size">size</param>
        /// <returns>缓存内容</returns>
        public PageList<T> GetList(string key, int page, int size)
        {
            using (var client = prcm.GetReadOnlyClient())
            {
                var tclient = client.GetTypedClient<T>();

                return tclient.Lists[key].AsQueryable().ToPageList(page, size);
            }
        }

        /// <summary>
        /// 获取list缓存总数
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>缓存总数</returns>
        public long GetCountOfList(string key)
        {
            using (var client = prcm.GetReadOnlyClient())
            {
                var tclient = client.GetTypedClient<T>();

                return tclient.Lists[key].Count;
            }
        }

        /// <summary>
        /// 获取item详情
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="id">item对象id</param>
        /// <returns>item对象</returns>
        public T GetItemFromList(string key, string id)
        {
            using (var client = prcm.GetClient())
            {
                var tclient = client.GetTypedClient<T>();

                var list = tclient.Lists[key];

                //动态生成lambda，示例：x=>x.Id=="1"
                var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");

                var constant = System.Linq.Expressions.Expression.Constant(id);

                System.Linq.Expressions.MemberExpression member = System.Linq.Expressions.Expression.PropertyOrField(parameter, "Id");

                var query = System.Linq.Expressions.Expression.Equal(member, constant);

                var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(query, parameter);

                return list.FirstOrDefault<T>(lambda.Compile());
            }
        }

        /// <summary>
        /// 添加item到list
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">item对象</param>
        /// <returns>true，设置成功；false，设置失败</returns>
        public bool AddItemToList(string key, T value)
        {
            using (var client = prcm.GetClient())
            {
                var tclient = client.GetTypedClient<T>();

                var list = tclient.Lists[key];

                //动态生成lambda，示例：x=>x.Id=="1"
                var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");

                var constant = System.Linq.Expressions.Expression.Constant(value.GetType().GetProperty("Id").GetValue(value));

                System.Linq.Expressions.MemberExpression member = System.Linq.Expressions.Expression.PropertyOrField(parameter, "Id");

                var query = System.Linq.Expressions.Expression.Equal(member, constant);

                var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(query, parameter);

                var entity = list.FirstOrDefault<T>(lambda.Compile());

                //判断list中是否存在指定Id缓存，若不存在则添加，存在则替换
                if (entity != null)
                    tclient.RemoveItemFromList(list, entity);

                tclient.AddItemToList(list, value);

                return true;
            }
        }

        /// <summary>
        /// 压入item到list，与PopItemFromList组成栈（后进先出）
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">item对象</param>
        /// <returns>true，压入成功；false，压入失败</returns>
        public bool PushItemToList(string key, T value)
        {
            using (var client = prcm.GetClient())
            {
                var tclient = client.GetTypedClient<T>();

                var list = tclient.Lists[key];

                //动态生成lambda，示例：x=>x.Id=="1"
                var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");

                var constant = System.Linq.Expressions.Expression.Constant(value.GetType().GetProperty("Id").GetValue(value));

                System.Linq.Expressions.MemberExpression member = System.Linq.Expressions.Expression.PropertyOrField(parameter, "Id");

                var query = System.Linq.Expressions.Expression.Equal(member, constant);

                var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(query, parameter);

                var entity = list.FirstOrDefault<T>(lambda.Compile());

                //判断list中是否存在指定Id缓存，若不存在则添加，存在则替换
                if (entity != null)
                    tclient.RemoveItemFromList(list, entity);

                tclient.PushItemToList(list, value);

                return true;
            }
        }

        /// <summary>
        /// 从list中取出item，与PushItemToList组成栈（后进先出）
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>item对象</returns>
        public T PopItemFromList(string key)
        {
            using (var client = prcm.GetClient())
            {
                var tclient = client.GetTypedClient<T>();

                var list = tclient.Lists[key];

                return tclient.PopItemFromList(list);
            }
        }

        /// <summary>
        /// 压入item到list，与DequeueItemFromList组成队列（先进先出）
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">item对象</param>
        /// <returns>true，压入成功；false，压入失败</returns>
        public bool PrependItemToList(string key, T value)
        {
            using (var client = prcm.GetClient())
            {
                var tclient = client.GetTypedClient<T>();

                var list = tclient.Lists[key];

                //动态生成lambda，示例：x=>x.Id=="1"
                var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");

                var constant = System.Linq.Expressions.Expression.Constant(value.GetType().GetProperty("Id").GetValue(value));

                System.Linq.Expressions.MemberExpression member = System.Linq.Expressions.Expression.PropertyOrField(parameter, "Id");

                var query = System.Linq.Expressions.Expression.Equal(member, constant);

                var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(query, parameter);

                var entity = list.FirstOrDefault<T>(lambda.Compile());

                //判断list中是否存在指定Id缓存，若不存在则添加，存在则替换
                if (entity != null)
                    tclient.RemoveItemFromList(list, entity);

                tclient.PrependItemToList(list, value);

                return true;
            }
        }

        /// <summary>
        /// 从list中取出item，PrependItemToList组成队列（先进先出）
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>item对象</returns>
        public T DequeueItemFromList(string key)
        {
            using (var client = prcm.GetClient())
            {
                var tclient = client.GetTypedClient<T>();

                var list = tclient.Lists[key];

                return tclient.DequeueItemFromList(list);
            }
        }

        /// <summary>
        /// 设置list到期时间
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="expireTime">到期时间</param>
        /// <returns>true，设置成功；false，设置失败</returns>
        public bool ExpireEntryAt(string key, DateTime expireTime)
        {
            using (var client = prcm.GetClient())
            {
                var tclient = client.GetTypedClient<T>();

                return tclient.ExpireEntryAt(key, expireTime);
            }
        }

        /// <summary>
        /// 移除list缓存数据（仅支持"Id"为标识的实体类缓存对象）
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="id">缓存对象Id</param>
        /// <returns>true，移除成功；false，移除失败</returns>
        public bool RemoveItemFromList(string key, string id)
        {
            using (var client = prcm.GetClient())
            {
                var tclient = client.GetTypedClient<T>();

                var list = tclient.Lists[key];

                //动态生成lambda，示例：x=>x.Id=="1"
                var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");

                var constant = System.Linq.Expressions.Expression.Constant(id);

                System.Linq.Expressions.MemberExpression member = System.Linq.Expressions.Expression.PropertyOrField(parameter, "Id");

                var query = System.Linq.Expressions.Expression.Equal(member, constant);

                var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(query, parameter);

                var entity = list.FirstOrDefault<T>(lambda.Compile());

                //判断list中是否存在指定Id缓存，若不存在则添加，存在则替换
                if (entity != null)
                    tclient.RemoveItemFromList(list, entity);

                return true;
            }
        }

        /// <summary>
        /// 移除所有list缓存数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>true，移除成功；false，移除失败</returns>
        public bool RemoveAllFromList(string key)
        {
            using (var client = prcm.GetClient())
            {
                var tclient = client.GetTypedClient<T>();

                var list = tclient.Lists[key];

                tclient.RemoveAllFromList(list);

                return true;
            }
        }
    }
}
