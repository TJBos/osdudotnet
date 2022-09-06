using Newtonsoft.Json;



namespace OsduLib.Models.Record
{
    public abstract class RecordBase : IRecord

    {
        [JsonProperty("acl")] public Acl Acl { get; set; }

        [JsonProperty("legal")] public Legal Legal { get; set; }

        [JsonProperty("kind")] public string? Kind { get; set; }

        [JsonProperty("id")] public string? Id;

    }
}
