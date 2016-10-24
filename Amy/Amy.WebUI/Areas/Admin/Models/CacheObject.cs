using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amy.WebUI.Areas.Admin.Models
{
    public class CacheObject
    {
        public string Id { get; set; }

        /// <summary>
        /// 键值
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 缓存类型
        /// </summary>
        public string CacheType { get; set; }

        /// <summary>
        /// 所属模块
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// 缓存数据
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string ValueType { get; set; }

        
    }
}