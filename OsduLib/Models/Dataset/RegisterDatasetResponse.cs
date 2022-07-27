using OsduLib.Models.Record;
using Newtonsoft.Json;

namespace OsduLib.Models.Dataset;

public class RegisterManifestFileResponse
{
    [JsonProperty("datasetRegistries")] public List<DatasetRegistry> DatasetRegistries;
}