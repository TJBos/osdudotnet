using Newtonsoft.Json;



namespace OsduLib.Models.Record
{
    public abstract class RecordBase

    {
        [JsonProperty("acl")] public Acl Acl => new Acl();

        [JsonProperty("legal")] public Legal Legal => new Legal();

        [JsonProperty("kind")] public string? Kind;

        [JsonProperty("id")] public string? Id;

    }
}
