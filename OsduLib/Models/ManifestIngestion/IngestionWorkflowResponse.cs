using Newtonsoft.Json;


namespace OsduLib.Models.ManifestIngestion;

public class IngestionWorkflowResponse
{
    [JsonProperty("runId")]
    public string RunId;
    [JsonProperty("startTimeStamp")]
    public long StartTimeStamp;
    [JsonProperty("status")]
    public string Status;
}
