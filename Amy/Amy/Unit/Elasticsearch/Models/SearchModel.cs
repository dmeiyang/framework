using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amy.Unit.Elasticsearch.Models
{
    public class SearchModel<T> where T : class
    {
        public long total { get; set; }

        public List<SearchHits<T>> hits { get; set; }
    }
}
