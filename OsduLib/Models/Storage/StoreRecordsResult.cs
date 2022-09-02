using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsduLib.Models.Storage
{
    public class StoreRecordsResult
    {
        [JsonProperty("recordCount")] public int RecordCount { get; set; }
        [JsonProperty("recordIds")] public List<string> RecordIds { get; set; }
        [JsonProperty("skippedRecordIds")] public List<string> SkippedRecordIds { get; set; }
        [JsonProperty("recordIdVersions")] public List<string> RecordIdVersions { get; set; }
    }
}
