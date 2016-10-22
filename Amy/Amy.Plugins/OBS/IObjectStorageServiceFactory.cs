using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amy.Plugins.OBS
{
    public interface IObjectStorageServcieFactory
    {
        /// <summary>
        /// 推送资源
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="id">文件标识（即文件名称，非空则覆盖上传）</param>
        /// <returns>远程地址</returns>
        string Put(Stream stream, string id = null);

        /// <summary>
        /// 推送资源
        /// </summary>
        /// <param name="bytes">文件字节</param>
        /// <param name="id">文件标识（即文件名称，非空则覆盖上传）</param>
        /// <returns>远程地址</returns>
        string Put(byte[] bytes, string id = null);

        /// <summary>
        /// 移除资源
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns>true，操作成功；false，操作失败</returns>
        bool Remove(string id);
    }
}
