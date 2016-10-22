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

namespace Amy.Unit.Elasticsearch.NET.Configuration
{
    public class Config
    {
        /// <summary>
        /// 节点组
        /// </summary>
        public string[] Nodes { get; set; }

        /// <summary>
        /// 索引名称
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// 索引名称
        /// </summary>
        public string IndexType { get; set; }
    }
}
