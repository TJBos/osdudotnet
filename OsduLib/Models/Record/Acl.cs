using Newtonsoft.Json;

namespace OsduLib.Models.Record;

public class Acl
{
    [JsonProperty("owners")] public List<string> Owners { get; set; }

    [JsonProperty("viewers")] public List<string> Viewers { get; set; }

    public Acl()
    {
        Owners = new List<string>() { "data.default.owners@osdu.example.com" };
        Viewers = new List<string>() { "data.default.viewers@osdu.example.com" };
    }
}