using OsduLib.Models.Dataset;
using Newtonsoft.Json;
using OsduLib.Client;

namespace OsduLib.Services;

public class DatasetService : BaseOsduService
{
    private readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore
    };

    public DatasetService(BaseOsduClient client) : base(client, "dataset", "1")
    {

    }

    public Task<RegisterDatasetFileResponse> RegisterDatasetFile(RegisterDatasetFileRequest registerDatasetFileRequest)
    {
        return _client.PutJson<RegisterDatasetFileResponse>(
       $"{_servicePath}/registerDataset",
       _defaultHeaders,
       SerializeRequest(registerDatasetFileRequest)
   );
    }

    public async Task<GetStorageInstructionsResponse> GetStorageInstructions(string kind)
    {
        var response = await _client.GetJson<GetStorageInstructionsResponse>(
            $"{_servicePath}/getStorageInstructions?kindSubType={kind}",
            _defaultHeaders,
            ""
            );
        return response;
    }

    public async Task<GetRetrievalInstructionsResponse> GetRetrievalInstructions(IEnumerable<string> datasetRegistryIds)
    {
        var result = await _client.PostJson<GetRetrievalInstructionsResponse>(
            $"{_servicePath}/getRetrievalInstructions",
            _defaultHeaders,
            SerializeRequest(new GetRetrievalInstructionsRequest { DatasetRegistryIds = datasetRegistryIds }));
        return result;
    }

    private string SerializeRequest(object request)
    {
        return JsonConvert.SerializeObject(request, Formatting.None, _jsonSerializerSettings);
    }

}