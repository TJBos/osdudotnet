using Newtonsoft.Json;
using OsduLib.Models.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsduLib.Models.ManifestIngestion
{
    public class Dataset : RecordBase
    {
        [JsonProperty("data")] public DatasetRegistryRecord Data;

        [JsonProperty("meta")] public List<object>? Meta;

        [JsonProperty("tags")] public Dictionary<string, string>? Tags;

        [JsonProperty("version")] public string? Version;
        public Dataset(Guid id) : base()
        {
            Kind = "osdu:wks:dataset--File.Generic:1.0.0";
            Data = new DatasetRegistryRecord();
            Id = $"osdu:dataset--File.Generic:{id}";
        }

    }
}
