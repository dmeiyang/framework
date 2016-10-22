using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amy.WebUI.Areas.Admin.Models
{
    public class ArticleCondition
    {
        public string Key { get; set; }

        /// <summary>
        /// 来源（当前登录门户站点）
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 最小点击数
        /// </summary>
        public int? MinHits { get; set; }

        /// <summary>
        /// 最大点击数
        /// </summary>
        public int? MaxHits { get; set; }

        /// <summary>
        /// 最小小费
        /// </summary>
        public int? MinTips { get; set; }

        /// <summary>
        /// 最大小费
        /// </summary>
        public int? MaxTips { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool? IsDisplay { get; set; }

        /// <summary>
        /// 最小时间
        /// </summary>
        public DateTime? MinTime { get; set; }

        /// <summary>
        /// 最大时间
        /// </summary>
        public DateTime? MaxTime { get; set; }

        /// <summary>
        /// 评论关键字
        /// </summary>
        public string CommentKey { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }
}