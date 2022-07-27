using Newtonsoft.Json;
using OsduLib.Client;
using OsduLib.Models.Search;
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
        public Task<SearchResult> GetRecord(string id)
        {
            return _client.GetJson<SearchResult>(
                $"{_servicePath}/records/{id}",
                _defaultHeaders,
                ""
            );
        }

    }
}
