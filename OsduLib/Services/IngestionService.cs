//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using OsduLib.Models.Dataset;
//using OsduLib.Models.ManifestIngestion;

//namespace OsduLib.Services;

//public class IngestionService : BaseOsduService
//{
//    private readonly FileReferenceManager _fileReferenceManager;
//    private readonly ILogger _logger;

//    private readonly JsonSerializerSettings _jsonSerializerSettings = new()
//    {
//        NullValueHandling = NullValueHandling.Ignore
//    };

//    public IngestionService(OsduClient client, FileReferenceManager fileReferenceManager, ILogger logger) : base(client, "workflow", "1")
//    {
//        _fileReferenceManager = fileReferenceManager;
//        _logger = logger;
//    }

//    public async Task UploadToOsdu(string fileName, string masterdata, string typeData="core")
//    {

//        GetStorageInstructionsResponse resp = await _client.Dataset.GetStorageInstructions("dataset--File.Generic");
//        string unsignedUrl = resp.StorageLocation.UnsignedUrl;
//        string signedUrl = resp.StorageLocation.SignedUrl;
//    }

//    public async Task<string> CheckIngestionRunStatus(string runId)
//    {
//        var response = await _client.GetJson<RunStatusResponse>(
//            $"{_servicePath}/workflow/Osdu_ingest/workflowRun/{runId}",
//            _defaultHeaders, "");
//        return response.Status;
     
//    }
//    private async Task<IngestionWorkflowResponse> IngestManifest(ManifestIngestionRequest request)
//    {
//        return await _client.PostJson<IngestionWorkflowResponse>(
//            $"{_servicePath}/workflow/Osdu_ingest/workflowRun",
//            _defaultHeaders,
//            SerializeRequest(request)
//        );
//    }
//    private ManifestIngestionRequest CreateManifest(string osduFilePath, Guid datasetId, string masterdata, string typeData, string fileName)
//    {
//        // For now, we're using masterdata for wellbore only since Curate doesn't differentiate, so for the POC our masterdata Well:WellA01 has same info as masterdata Wellbore:WellA01
//        // For now, assume 1 WP = 1 WPC = 1 Dataset and so we'll use 1 GUID that's same for all 3

//        string masterDataId = $"osdu:master-data--Wellbore:{masterdata.Replace(" ", "")}";
       

//        var manifestRequest = new ManifestIngestionRequest();

//        var workProduct = new WorkProduct(datasetId);
//        workProduct.WorkProductData.Name = $"{masterDataId}-WP";
//        workProduct.WorkProductData.Components.Add($"osdu:work-product-component--WellLog:{datasetId}:");
//        manifestRequest.ExecutionContext.Manifest.Data.WorkProduct = workProduct;

//        var wpc = new WorkProductComponent(datasetId);
//        wpc.WorkProductComponentData.Datasets.Add($"osdu:dataset--File.Generic:{datasetId}:");
//        wpc.WorkProductComponentData.WellboreID = masterDataId+":";
//        wpc.Tags = new Dictionary<string, string>() { { "DataType", typeData } };

//        ///Add metadata from CSV file
     

//        manifestRequest.ExecutionContext.Manifest.Data.WorkProductComponents = new List<WorkProductComponent>() { wpc };

//        var dataset = new Dataset(datasetId);
//        dataset.Data.DatasetProperties.FileSourceInfo.FileSource = osduFilePath + "signedUpload";
//        dataset.Data.DatasetProperties.FileSourceInfo.PreloadFilePath = osduFilePath + "signedUpload";
//        dataset.Data.Name = fileName;
//        manifestRequest.ExecutionContext.Manifest.Data.Datasets = new List<Dataset>() { dataset };

//        return manifestRequest;
//    }

//    private bool UploadFileToOsdu(string filePath, string signedUrl)
//    {

//        return _fileReferenceManager.UploadObject(filePath, signedUrl);
//    }
//    private string SerializeRequest(object request)
//    {
//        return JsonConvert.SerializeObject(request, Formatting.None, _jsonSerializerSettings);
//    }
//}

