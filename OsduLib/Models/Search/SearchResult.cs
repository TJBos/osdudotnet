using OsduLib.Models.Record;
using Newtonsoft.Json;

namespace OsduLib.Models.Search;

public class SearchResult
{
    [JsonProperty("id")] public string Id { get; set; }

    [JsonProperty("kind")] public string Kind { get; set; }

    [JsonProperty("source")] public string Source { get; set; }

    [JsonProperty("acl")] public Acl Acl { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("version")] public string Version { get; set; }

    [JsonProperty("tags")] public Dictionary<string, object> Tags { get; set; }

    [JsonProperty("data")] public GenericData Data { get; set; }

    [JsonProperty("modifyUser")] public string ModifyUser { get; set; }

    [JsonProperty("modifyTime")] public string ModifyTime { get; set; }

    [JsonProperty("createTime")] public string CreateTime { get; set; }

    [JsonProperty("authority")] public string Authority { get; set; }

    [JsonProperty("namespace")] public string Namespace { get; set; }

    [JsonProperty("legal")] public Legal Legal { get; set; }

    [JsonProperty("createUser")] public string CreateUser { get; set; }
}