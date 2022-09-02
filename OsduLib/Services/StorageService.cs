using Newtonsoft.Json;
using OsduLib.Client;
using OsduLib.Models.Record;
using OsduLib.Models.Search;
using OsduLib.Models.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsduLib.Services
{
    public class StorageService : BaseOsduService
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public StorageService(BaseOsduClient client) : base(client, "storage", "2")
        {

        }
        public Task<SearchResult> GetRecord(string recordId)
            // Returns the latest version of the given record
        {
            return _client.GetJson<SearchResult>(
                $"{_servicePath}/records/{recordId}",
                _defaultHeaders,
                ""
            );
        }

        public Task<GetRecordsResult> GetRecords(IEnumerable<string> recordIds)
        // Returns the latest version of the given record
        {
            var request = new Dictionary<string, IEnumerable<string>> { { "records", recordIds } };
            return _client.PostJson<GetRecordsResult>(
                $"{_servicePath}/query/records",
                _defaultHeaders,
                SerializeRequest(request)
            );
        }

        public Task<StoreRecordsResult> StoreRecords(IEnumerable<IRecord> records)

        // Create and/or update records. When no record id is provided or when the provided id is not already present 
        // in the Data Ecosystem, then a new record is created.If the id is related to an existing record in the Data
        // Ecosystemthen an update operation takes place and a new version of the record is created.
        // Objects passed in have to be an implementation of IRecord and follow the correct OSDU schema for the record kind
        {
            return _client.PutJson<StoreRecordsResult>(
               $"{_servicePath}/records",
               _defaultHeaders,
               SerializeRequest(records)
           );
        }

        public Task<HttpResponseMessage> DeleteRecord(string recordId)
        
        // Delete record based on record Id. Status Code 204 returned upon successful deletion. Record and all its versions will be
        // permanently deleted!
        {
            return _client.Delete(
               $"{_servicePath}/records/{recordId}",
                _defaultHeaders,
                ""
                );
        }

        public Task<SearchResult> GetRecordVersion(string recordId, string version)

        // Get a specific version of a record.
        {
            return _client.GetJson<SearchResult>(
             $"{_servicePath}/records/{recordId}/{version}",
             _defaultHeaders,
             ""
         );
        }

        public Task<RecordVersionsResponse> GetAllRecordVersions(string recordId)
        {

        // Get a list of all versions for a record id

            return _client.GetJson<RecordVersionsResponse>(
             $"{_servicePath}/records/versions/{recordId}",
             _defaultHeaders,
             ""
         );
        }

        private string SerializeRequest(Object request)
        {
            return JsonConvert.SerializeObject(request, Formatting.None, _jsonSerializerSettings);
        }

    }
}
