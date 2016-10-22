/* ==================================================================
* 类 名 称：Qiniu
* 版 本 号：v1.0.0
* 作 者: Administrator
* 创建时间：2016/5/24 11:53:39
* 类 说 明：
* 备 注：
*
* ==================================================================*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;
using Qiniu.IO;
using Qiniu.RS;
using Qiniu.RPC;

namespace Amy.Plugins.OBS
{
    public class QiniuFactory : IObjectStorageServcieFactory
    {
        public string Put(System.IO.Stream stream, string id = null)
        {
            var config = (QiniuConfig.Config)ConfigurationManager.GetSection("QiniuOBSConfig");

            //设置账号的AK和SK
            Qiniu.Conf.Config.ACCESS_KEY = config.AccessKey;
            Qiniu.Conf.Config.SECRET_KEY = config.SecretKey;

            var target = new IOClient();
            var extra = new PutExtra();

            //设置上传的空间
            var bucket = config.Bucket;

            //设置上传的文件的key值（及文件名称）
            var key = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString("N") : id;

            //普通上传,只需要设置上传的空间名就可以了,第二个参数可以设定token过期时间，注意：name非空则为覆盖上传
            var put = string.IsNullOrEmpty(id) ? new PutPolicy(bucket, 3600) : new PutPolicy(string.Format("{0}:{1}", bucket, key), 3600);

            //调用Token()方法生成上传的Token
            var upToken = put.Token();

            //上传文件的路径
            //string filePath = @"F:\130326213588cbde762953a234.jpg";

            //调用PutFile()方法上传
            //PutRet ret = target.PutFile(upToken, key, filePath, extra);
            var ret = target.Put(upToken, key, stream, extra);


            return string.IsNullOrEmpty(ret.key) ? string.Empty : string.Format("{0}/{1}", config.Domain, ret.key);
        }

        public string Put(byte[] bytes, string id = null)
        {
            return Put(new System.IO.MemoryStream(bytes), id);
        }

        public bool Remove(string id)
        {
            var config = (QiniuConfig.Config)ConfigurationManager.GetSection("QiniuOBSConfig");

            var ret = new RSClient().Delete(new EntryPath(config.Bucket, id));

            return ret.OK;
        }
    }
}
