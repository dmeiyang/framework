using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amy.Unit.Elasticsearch.Models
{
    public class SearchHits<T> where T : class
    {
        public string _id { get; set; }

        public T _source { get; set; }

        public Newtonsoft.Json.Linq.JObject highlight { get; set; }
    }
}
