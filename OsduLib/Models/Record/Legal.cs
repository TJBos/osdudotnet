using Newtonsoft.Json;

namespace OsduLib.Models;

public class Legal
{
    [JsonProperty("legaltags")] public List<string> LegalTags { get; set; }

    [JsonProperty("otherRelevantDataCountries")]
    public List<string> OtherRelevantDataCountries { get; set; }

    [JsonProperty("status")] public string? Status { get; set; }

    public Legal()
    {
        LegalTags = new List<string>() { "osdu-public-usa-dataset" };
        OtherRelevantDataCountries = new List<string>() { "US" };
    }

}