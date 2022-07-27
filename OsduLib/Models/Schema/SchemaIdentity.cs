using OsduLib.Models.Record;
using Newtonsoft.Json;

namespace OsduLib.Models.Schema;

public class SchemaIdentity
{
    [JsonProperty("authority")] public string Authority { get; set; }
    [JsonProperty("source")] public string Source { get; set; }
    [JsonProperty("entityType")] public string EntityType { get; set; }
    [JsonProperty("schemaVersionMajor")] public int SchemaVersionMajor { get; set; }
    [JsonProperty("schemaVersionMinor")] public int SchemaVersionMinor { get; set; }
    [JsonProperty("schemaVersionPatch")] public int SchemaVersionPatch { get; set; }
    [JsonProperty("id")] public string Id { get; set; }
}