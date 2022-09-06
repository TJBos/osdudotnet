using OsduLib.Models.Record;
using Newtonsoft.Json;

namespace OsduLib.Models.Dataset;

public class RegisterDatasetFileRequest
{
    [JsonProperty("datasetRegistries")] public IEnumerable<DatasetRegistry> DatasetRegistries;
}