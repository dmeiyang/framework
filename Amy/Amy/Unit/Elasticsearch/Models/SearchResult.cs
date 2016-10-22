using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amy.Unit.Elasticsearch.Models
{
    public class SearchResult<T> where T : class
    {
        public int took { get; set; }

        public SearchModel<T> hits { get; set; }

        public object aggregations { get; set; }
    }


}
