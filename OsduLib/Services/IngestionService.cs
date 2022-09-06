using Newtonsoft.Json;
using OsduLib.Client;
using OsduLib.Models.ManifestIngestion;

namespace OsduLib.Services;

public class IngestionService : BaseOsduService
{
   
    private readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore
    };

    public IngestionService(BaseOsduClient client) : base(client, "workflow", "1")
    {
       
    }

    public async Task<string> CheckIngestionRunStatus(string runId)
    {
        var response = await _client.GetJson<RunStatusResponse>(
            $"{_servicePath}/workflow/Osdu_ingest/workflowRun/{runId}",
            _defaultHeaders, "");
        return response.Status;

    }
    private async Task<IngestionWorkflowResponse> IngestManifest(ManifestIngestionRequest request)
    {
        return await _client.PostJson<IngestionWorkflowResponse>(
            $"{_servicePath}/workflow/Osdu_ingest/workflowRun",
            _defaultHeaders,
            SerializeRequest(request)
        );
    }
    private string SerializeRequest(object request)
    {
        return JsonConvert.SerializeObject(request, Formatting.None, _jsonSerializerSettings);
    }
}

