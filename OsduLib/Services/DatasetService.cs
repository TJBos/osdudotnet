using OsduLib.Exceptions;
using OsduLib.Models.Dataset;
using OsduLib.Models.FileReference;
using Newtonsoft.Json;
using OsduLib.Client;

namespace OsduLib.Services;

public class DatasetService : BaseOsduService
{
    private readonly FileReferenceManager _fileReferenceManager;

    private readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore
    };

    public DatasetService(BaseOsduClient client) : base(client, "dataset", "1")
    {
        
    }

    public Task<RegisterManifestFileResponse> RegisterManifestFile(RegisterManifestFileRequest registerManifestFileRequest)
    {

        if (registerManifestFileRequest?.DatasetRegistries?.Count < 1)
            throw new Exception("Must provide at least 1 dataset registry");

        return _client.PutJson<RegisterManifestFileResponse>(
            $"{_servicePath}/registerDataset",
            _defaultHeaders,
            SerializeRequest(registerManifestFileRequest)
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

    //public async IAsyncEnumerable<(string fileId, string filename, string filepath)> GetFileReferences(GetRetrievalInstructionsRequest request, string destination)
    //{
    //    var fileIds = request.DatasetRegistryIds;
    //    if (fileIds == null || fileIds.Count < 1)
    //    {
    //        throw new Exception("Must provide at least 1 file ID");
    //    }

    //    var invalidDatasetRegistryFileReferences = new List<RetrievalInstructionError>();
    //    int temp = 1;
    //    foreach (string fileId in fileIds)
    //    {
    //        var validDatasetRegistryFileReferences = new List<IFileReference>();

    //        string[] fileIdArray = new string[] { fileId };
    //        GetRetrievalInstructionsResponse result;
    //        try
    //        {
    //            result = await _client.PostJson<GetRetrievalInstructionsResponse>(
    //                $"{_servicePath}/getRetrievalInstructions",
    //                _defaultHeaders,
    //                SerializeRequest(new GetRetrievalInstructionsRequest() { DatasetRegistryIds = fileIdArray })
    //            );
    //        }
    //        catch(Exception ex)
    //        {
    //            invalidDatasetRegistryFileReferences.Add(new RetrievalInstructionError() { Error = ex, FileId = fileId });
    //            continue;
    //        }
    //        if (result.Delivery.Length == 0)
    //        {
    //            invalidDatasetRegistryFileReferences.Add(new RetrievalInstructionError() { Error = new Exception($"No data registry found for file {fileId}"), FileId = fileId });
    //            continue;
    //        }
    //        foreach (var datasetRegistry in result.Delivery)
    //        {
    //            try
    //            {
    //                var x = _fileReferenceManager.GetFile(
    //                    fileId,
    //                    (string)datasetRegistry.RetrievalProperties["signedUrl"],
    //                    datasetRegistry.ProviderKey
    //                );
    //                validDatasetRegistryFileReferences.Add(x);
    //            }
    //            catch (Exception ex)
    //            {
    //                invalidDatasetRegistryFileReferences.Add(new RetrievalInstructionError()
    //                    { FileId = fileId, RetrievalInstruction = datasetRegistry, Error = ex });
    //            }
    //            temp++;
    //        }
    //        await foreach (var (filename, filepath) in _fileReferenceManager.SaveFile(destination, validDatasetRegistryFileReferences.ToArray()))
    //        {
    //            yield return (fileId, filename, filepath);
    //        }
    //    }

    //    if (invalidDatasetRegistryFileReferences.Count > 0)
    //    {
    //        throw new InvalidFileReferenceException($"{invalidDatasetRegistryFileReferences.Count} download errors.", invalidDatasetRegistryFileReferences);
    //    }
    //}

    private string SerializeRequest(object request)
    {
        return JsonConvert.SerializeObject(request, Formatting.None, _jsonSerializerSettings);
    }

}