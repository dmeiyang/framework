using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amy.WebUI.Areas.Admin.Models
{
    /// <summary>
    /// 新闻评论
    /// </summary>
    public class Comment
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime DateTime { get; set; }
    }
}