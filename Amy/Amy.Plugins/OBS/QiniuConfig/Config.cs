/* ==================================================================
* 类 名 称：Config
* 版 本 号：v1.0.0
* 作 者: Administrator
* 创建时间：2016/5/24 11:53:04
* 类 说 明：
* 备 注：
*
* ==================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amy.Plugins.OBS.QiniuConfig
{
    public class Config
    {
        /// <summary>
        /// 七牛公钥
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// 七牛密钥
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 资源空间
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// 空间域名
        /// </summary>
        public string Domain { get; set; }
    }
}
