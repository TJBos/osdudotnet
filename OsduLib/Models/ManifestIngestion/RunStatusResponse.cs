using Newtonsoft.Json;

namespace OsduLib.Models.ManifestIngestion
{
    public class RunStatusResponse
    {
        [JsonProperty("runId")]
        public string RunId;
        [JsonProperty("status")]
        public string Status;
    }
}
