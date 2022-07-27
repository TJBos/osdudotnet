using OsduLib.Client;
using OsduLib.Models;

namespace OsduLib.Services;

public abstract class BaseOsduService
{
    protected readonly BaseOsduClient _client;
    protected string _serviceName;
    protected string _serviceVersion;

    protected string _servicePath => $"api/{_serviceName}/v{_serviceVersion}";
    protected HttpHeaders _defaultHeaders;
    protected BaseOsduService(BaseOsduClient client, string serviceName, string serviceVersion)
    {
        _client = client;
        _serviceName = serviceName;
        _serviceVersion = serviceVersion;
        _defaultHeaders = new HttpHeaders() { 
            { "data-partition-id", _client.DataPartitionId },
            {"Authorization", "Bearer " + _client.AccessToken }
        };
    }
}