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

namespace Amy.Runtime.Caching.Redis.Configuration
{
    public class Config
    {
        /// <summary>
        /// “写”服务器地址
        /// </summary>
        public string[] ReadWriteHosts { get; set; }

        /// <summary>
        /// “读”服务器地址
        /// </summary>
        public string[] ReadOnlyHosts { get; set; }

        /// <summary>
        /// “写”链接池链接数  
        /// </summary>
        public int MaxWritePoolSize { get; set; }

        /// <summary>
        /// “读”链接池链接数
        /// </summary>
        public int MaxReadPoolSize { get; set; }
    }
}
