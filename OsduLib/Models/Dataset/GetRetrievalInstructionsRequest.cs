using Newtonsoft.Json;

namespace OsduLib.Models.Dataset;

public class GetRetrievalInstructionsRequest
{
    [JsonProperty("datasetRegistryIds")]
    public IEnumerable<string> DatasetRegistryIds;
}