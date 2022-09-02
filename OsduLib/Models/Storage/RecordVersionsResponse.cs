
using Newtonsoft.Json;

namespace OsduLib.Models.Storage
{
    public class RecordVersionsResponse
    {
        [JsonProperty("recordId")] public string RecordId { get; set; }
        [JsonProperty("versions")] public List<string> Versions { get; set; }
    }
}
