using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Elasticsearch.Net;


namespace Amy.Unit.Elasticsearch.NET
{
    public class ConnectionBase<T> : IConnection<T> where T : class
    {
        #region 初始化客户端
        private static ConnectionBase<T> instance;

        //进程辅助对象，多线程同时访问该类时，对该对象加锁，只有一个线程可以进入
        private static readonly object syncRoot = new object();

        private static ElasticLowLevelClient client;

        private static Configuration.Config config = (Configuration.Config)System.Configuration.ConfigurationManager.GetSection("ElasticsearchConfig");


        /// <summary>
        /// 定义个私有的构造方法，防止外界通过new创建该类的实例
        /// </summary>
        private ConnectionBase()
        {
            var nodes = new Uri[config.Nodes.Count()];

            for (int n = 0; n < nodes.Count(); n++)
            {
                nodes[n] = new Uri(config.Nodes[n]);
            }

            var pool = new SniffingConnectionPool(nodes);

            var settings = new ConnectionConfiguration(pool).DisableDirectStreaming();

            client = new ElasticLowLevelClient(settings);
        }

        /// <summary>
        /// 获取本类实例的唯一全局访问点（由于定义了静态的只读变量，因此不需要考虑线程安全问题）
        /// </summary>
        public static ConnectionBase<T> GetInstance()
        {
            //先判断实例是否存在，不存在再加锁，提高访问性能
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new ConnectionBase<T>();
                    }
                }
            }

            return instance;
        }
        #endregion

        #region 索引操作
        /// <summary>
        /// 判断索引是否存在
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <returns>true，存在；false，不存在</returns>
        public bool ExistsIndex(string index)
        {
            var result = client.IndicesExists<T>(index);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }

        /// <summary>
        /// 判断类型是否存在
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <returns>true，存在；false，不存在</returns>
        public bool ExistsIndex(string index, string type)
        {
            var result = client.IndicesExistsType<T>(index, type);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="shards">分片数</param>
        /// <param name="replicas">副本数</param>
        /// <returns>true，创建成功；false，创建失败</returns>
        public bool CreateIndex(string index, int shards = 5, int replicas = 1)
        {
            var data = new
            {
                settings = new
                {
                    index = new { number_of_shards = shards, number_of_replicas = replicas },
                    //使用IK分词器，可删除使用默认分词器
                    analysis = new { analyzer = new { ik = new { tokenizer = "ik_smart" } } }
                }
            };

            var result = client.IndicesCreate<T>(index, data);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }

        /// <summary>
        /// 修改副本数
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="replicas">副本数</param>
        /// <returns>true，修改成功；false，修改失败</returns>
        public bool UpdateSetting(string index, int replicas = 1)
        {
            var data = new { index = new { number_of_replicas = replicas } };

            var result = client.IndicesPutSettings<T>(index, data);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <returns>true，删除成功；false，删除失败</returns>
        public bool DeleteIndex(string index)
        {
            var result = client.IndicesDelete<T>(index);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }

        /// <summary>
        /// 开启索引
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <returns>true，开启成功；false，开启失败</returns>
        public bool OpenIndex(string index)
        {
            var result = client.IndicesOpen<T>(index);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }

        /// <summary>
        /// 关闭索引
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <returns>true，关闭成功；false，关闭失败</returns>
        public bool CloseIndex(string index)
        {
            var result = client.IndicesClose<T>(index);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }
        #endregion

        #region mapping操作
        /// <summary>
        /// 创建Mapping
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="data">映射属性对象</param>
        /// <returns>true，创建成功；false，创建失败</returns>
        public bool CreateMapping(string index, string type, object properties)
        {
            var data = new { properties = properties };

            var result = client.IndicesPutMapping<T>(index, type, data);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }

        /// <summary>
        /// 获取Mapping
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <returns>索引详情</returns>
        public string GetMapping(string index)
        {
            var result = client.IndicesGetMapping<T>(index);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.ResponseBodyInBytes.ByteToString();
        }
        #endregion

        #region document操作
        /// <summary>
        /// 添加/更新文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="id">文档Id</param>
        /// <param name="model">对象实体</param>
        /// <returns>true，put成功；false，put失败</returns>
        public bool PutDocument(string index, string type, string id, T model)
        {
            //AddQueryString给文档_id赋值，null则会被随机指定值
            var result = client.Index<T>(index, type, model, x => x.AddQueryString("id", id));

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 201 && result.HttpStatusCode != 200)
                throw result.OriginalException;

            return true;
        }

        /// <summary>
        /// 获取文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="id">文档Id</param>
        /// <returns>对象实体</returns>
        public T GetDocument(string index, string type, string id)
        {
            var result = client.Get<T>(index, type, id);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.ResponseBodyInBytes.ByteToString().GetJsonValue("_source").ToObjectByJsonNet<T>();
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="id">文档Id</param>
        /// <returns>true，删除成功；false，删除失败</returns>
        public bool DeleteDocument(string index, string type, string id)
        {
            var result = client.Delete<T>(index, type, id);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }

        /// <summary>
        /// 局部更新文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="id">文档Id</param>
        /// <param name="data">局部数据</param>
        /// <returns>true，局部更新成功；false，局部更新失败</returns>
        public bool UpdateDocument(string index, string type, string id, object data)
        {
            var result = client.Update<T>(index, type, id, data);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }

        /// <summary>
        /// 局部追加文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="id">文档Id</param>
        /// <param name="data">追加数据</param>
        /// <returns>true，局部追加成功；false，局部追加失败</returns>
        public bool AppendDocument(string index, string type, string id, object data)
        {
            var result = client.Update<T>(index, type, id, data);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }
        #endregion

        #region bulk操作
        /// <summary>
        /// 获取多文档
        /// </summary>
        /// <param name="data">请求参数</param>
        /// <returns>文档集合</returns>
        public IEnumerable<T> GetDocumentList(object data)
        {
            var result = client.Mget<T>(data);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.ResponseBodyInBytes.ByteToString().ToObjectByJsonNet<IEnumerable<T>>();
        }

        /// <summary>
        /// 获取多文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="ids">文档Id集合</param>
        /// <returns>文档集合</returns>
        public IEnumerable<T> GetDocumentList(string index, string type, IEnumerable<object> ids)
        {
            var list = ids.Select(x => new
            {
                _index = index,
                _type = type,
                _id = x
            });

            var data = new { docs = list };

            return GetDocumentList(data);
        }

        /// <summary>
        /// 批量添加/更新文档
        /// </summary>
        /// <param name="data">请求参数</param>
        /// <returns>true，批量操作成功；false，批量操作失败</returns>
        public bool BulkPutDocument(object data)
        {
            var result = client.Bulk<T>(data);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }

        /// <summary>
        /// 批量添加/更新文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="list">文档集合</param>
        /// <returns>true，批量操作成功；false，批量操作失败</returns>
        public bool BulkPutDocument(string index, string type, IEnumerable<T> list)
        {
            var data = new List<object>();

            foreach (var v in list)
            {
                string id = null;

                var t = typeof(T);

                var property = t.GetProperty("Id");

                if (property == null)
                    id = IDHelper.Id32;

                id = (string)property.GetValue(v, null);

                if (id == null)
                    id = IDHelper.Id32;

                data.Add(new { index = new { _index = index, _type = type, _id = id } });
                data.Add(v);
            }

            return BulkPutDocument(data);
        }

        /// <summary>
        /// 批量删除文档
        /// </summary>
        /// <param name="data">请求参数</param>
        /// <returns>true，批量操作成功；false，批量操作失败</returns>
        public bool BulkDeleteDocument(object data)
        {
            var result = client.Bulk<T>(data);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }

        /// <summary>
        /// 批量删除文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="ids">文档Id集合</param>
        /// <returns>true，批量操作成功；false，批量操作失败</returns>
        public bool BulkDeleteDocument(string index, string type, IEnumerable<object> ids)
        {
            var list = ids.Select(x => new
            {
                delete = new { _index = index, _type = type, _id = x }
            });

            return BulkDeleteDocument(list);
        }

        /// <summary>
        /// 批量局部更新文档
        /// </summary>
        /// <param name="data">请求参数</param>
        /// <returns>true，批量操作成功；false，批量操作失败</returns>
        public bool BulkUpdateDocument(object data)
        {
            var result = client.Bulk<T>(data);

            //TODO:记录操作日志/抛出异常
            if (result.HttpStatusCode != 200)
                throw result.OriginalException;

            return result.HttpStatusCode == 200;
        }

        /// <summary>
        /// 批量局部更新文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="ids">文档Id集合</param>
        /// <param name="data">局部更新数据</param>
        /// <returns>true，批量操作成功；false，批量操作失败</returns>
        public bool BulkUpdateDocument(string index, string type, IEnumerable<object> ids, object data)
        {
            var list = new List<object>();

            foreach (var v in ids)
            {
                list.Add(new
                {
                    update = new { _index = index, _type = type, _id = v }
                });

                list.Add(data);
            }

            return BulkUpdateDocument(list);
        }
        #endregion

        #region 检索文档
        /// <summary>
        /// 检索文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="data">请求参数</param>
        /// <returns>文档集合</returns>
        public Models.SearchResult<T> Search(string index, string type, object data)
        {
            var result = client.Search<T>(index, type, data);

            return result.ResponseBodyInBytes.ByteToString().ToObjectByJsonNet<Models.SearchResult<T>>();
        }
        #endregion
    }
}
