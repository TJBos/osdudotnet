using OsduLib.Models.Record;
using Newtonsoft.Json;

namespace OsduLib.Models.Dataset;

public class RetrievalInstruction
{
    [JsonProperty("providerKey")]
    public string ProviderKey;

    [JsonProperty("datasetRegistryId")]
    public string DatasetRegistryId;

    [JsonProperty("retrievalProperties")]
    public GenericData RetrievalProperties;
}