using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amy.WebUI.Areas.Admin.Models
{
    public class SearchResult<T> where T : class
    {
        public object Aggregations { get; set; }

        public PageList<T> DataList { get; set; }
    }
}