using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amy.Unit.Elasticsearch
{
    public interface IConnection<T> where T : class
    {
        #region 索引操作
        /// <summary>
        /// 判断索引是否存在
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <returns>true，存在；false，不存在</returns>
        bool ExistsIndex(string index);

        /// <summary>
        /// 判断类型是否存在
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <returns>true，存在；false，不存在</returns>
        bool ExistsIndex(string index, string type);

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="shards">分片数</param>
        /// <param name="replicas">副本数</param>
        /// <returns>true，创建成功；false，创建失败</returns>
        bool CreateIndex(string index, int shards = 5, int replicas = 1);

        /// <summary>
        /// 修改副本数
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="replicas">副本数</param>
        /// <returns>true，修改成功；false，修改失败</returns>
        bool UpdateSetting(string index, int replicas = 1);

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <returns>true，删除成功；false，删除失败</returns>
        bool DeleteIndex(string index);

        /// <summary>
        /// 开启索引
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <returns>true，开启成功；false，开启失败</returns>
        bool OpenIndex(string index);

        /// <summary>
        /// 关闭索引
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <returns>true，关闭成功；false，关闭失败</returns>
        bool CloseIndex(string index);
        #endregion

        #region mapping操作
        /// <summary>
        /// 创建Mapping
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="data">映射属性对象</param>
        /// <returns>true，创建成功；false，创建失败</returns>
        bool CreateMapping(string index, string type, object properties);

        /// <summary>
        /// 获取Mapping
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <returns>索引详情</returns>
        string GetMapping(string index);
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
        bool PutDocument(string index, string type, string id, T model);

        /// <summary>
        /// 获取文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="id">文档Id</param>
        /// <returns>对象实体</returns>
        T GetDocument(string index, string type, string id);

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="id">文档Id</param>
        /// <returns>true，删除成功；false，删除失败</returns>
        bool DeleteDocument(string index, string type, string id);

        /// <summary>
        /// 局部更新文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="id">文档Id</param>
        /// <param name="data">局部数据</param>
        /// <returns>true，局部更新成功；false，局部更新失败</returns>
        bool UpdateDocument(string index, string type, string id, object data);

        /// <summary>
        /// 局部追加文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="id">文档Id</param>
        /// <param name="data">追加数据</param>
        /// <returns>true，局部追加成功；false，局部追加失败</returns>
        bool AppendDocument(string index, string type, string id, object data);
        #endregion

        #region bulk操作
        /// <summary>
        /// 获取多文档
        /// </summary>
        /// <param name="data">请求参数</param>
        /// <returns>文档集合</returns>
        IEnumerable<T> GetDocumentList(object data);

        /// <summary>
        /// 获取多文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="ids">文档Id集合</param>
        /// <returns>文档集合</returns>
        IEnumerable<T> GetDocumentList(string index, string type, IEnumerable<object> ids);

        /// <summary>
        /// 批量添加/更新文档
        /// </summary>
        /// <param name="data">请求参数</param>
        /// <returns>true，批量操作成功；false，批量操作失败</returns>
        bool BulkPutDocument(object data);

        /// <summary>
        /// 批量添加/更新文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="list">文档集合</param>
        /// <returns>true，批量操作成功；false，批量操作失败</returns>
        bool BulkPutDocument(string index, string type, IEnumerable<T> list);

        /// <summary>
        /// 批量删除文档
        /// </summary>
        /// <param name="data">请求参数</param>
        /// <returns>true，批量操作成功；false，批量操作失败</returns>
        bool BulkDeleteDocument(object data);

        /// <summary>
        /// 批量删除文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="ids">文档Id集合</param>
        /// <returns>true，批量操作成功；false，批量操作失败</returns>
        bool BulkDeleteDocument(string index, string type, IEnumerable<object> ids);

        /// <summary>
        /// 批量局部更新文档
        /// </summary>
        /// <param name="data">请求参数</param>
        /// <returns>true，批量操作成功；false，批量操作失败</returns>
        bool BulkUpdateDocument(object data);

        /// <summary>
        /// 批量局部更新文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="ids">文档Id集合</param>
        /// <param name="data">局部更新数据</param>
        /// <returns>true，批量操作成功；false，批量操作失败</returns>
        bool BulkUpdateDocument(string index, string type, IEnumerable<object> ids, object data);
        #endregion

        /// <summary>
        /// 检索文档
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <param name="type">索引类型</param>
        /// <param name="data">请求参数</param>
        /// <returns>文档集合</returns>
        Models.SearchResult<T> Search(string index, string type, object data);
    }
}
