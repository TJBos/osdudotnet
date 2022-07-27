using OsduLib.Models.Record;
using Newtonsoft.Json;

namespace OsduLib.Models.Schema;

public class SchemaInfo
{
    [JsonProperty("schemaIdentity")] public SchemaIdentity SchemaIdentity { get; set; }
    [JsonProperty("createdBy")] public string CreatedBy { get; set; }
    [JsonProperty("dateCreated")] public string DateCreated { get; set; }
    [JsonProperty("status")] public string Status { get; set; }
    [JsonProperty("scope")] public string Scope { get; set; }
}