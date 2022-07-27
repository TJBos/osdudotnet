using OsduLib.Client;
using OsduLib.Models.Schema;

namespace OsduLib.Services;

public class SchemaService : BaseOsduService
{
    public SchemaService(BaseOsduClient client) : base(client, "schema-service", "1")
    {
    }

    public Task<SchemaResultSet> Kinds(int limit = 300)
    {
        return _client.GetJson<SchemaResultSet>(
            $"{_servicePath}/schema?limit={limit}",
            _defaultHeaders,
            ""
        );
    }
}