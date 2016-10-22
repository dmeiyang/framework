using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amy.WebUI.Areas.Admin.Models
{
    /// <summary>
    /// 站点新闻
    /// </summary>
    public class Article
    {
        /// <summary>
        /// 新闻标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 新闻标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 新闻描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 新闻作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 新闻作者拼音
        /// </summary>
        public string AuthorSpelling { get; set; }

        /// <summary>
        /// 新闻来源（拥有该新闻的门户站点）
        /// </summary>
        public List<string> Sources { get; set; }

        /// <summary>
        /// 新闻标签
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// 点击数
        /// </summary>
        public int Hits { get; set; }

        /// <summary>
        /// 小费（用户打赏总额）
        /// </summary>
        public decimal Tips { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsDisplay { get; set; }

        /// <summary>
        /// 新闻评论
        /// </summary>
        public List<Comment> Comments { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}