using Newtonsoft.Json;
using OsduLib.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsduLib.Models.Storage
{
    public class GetRecordsResult
    {
        [JsonProperty("records")]
        public List<SearchResult> Records { get; set; }
    }
}
