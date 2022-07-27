using OsduLib.Models.Record;
using Newtonsoft.Json;

namespace OsduLib.Models.Dataset;

public class RegisterManifestFileRequest
{
    [JsonProperty("datasetRegistries")] public List<DatasetRegistry> DatasetRegistries;
}