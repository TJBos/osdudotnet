using Newtonsoft.Json;

namespace OsduLib.Models.Record;

public class DatasetRegistry : RecordData
{
    [JsonProperty("data")] public DatasetRegistryRecord Data;

    [JsonProperty("meta")] public List<object> Meta;

    [JsonProperty("tags")] public Dictionary<string, string> Tags;

    [JsonProperty("version")] public string Version;
}