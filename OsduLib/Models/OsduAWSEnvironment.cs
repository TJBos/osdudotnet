using Amazon;

namespace OsduLib.Models;

public class OsduAWSEnvironment
{
    public string DataPartitionId;
    public string BaseApiUrl;
    public string? Profile;
    public string? Region;
    public string? ResourcePrefix;

    public string? ServicePrincipalClientId;
    public string? ServicePrincipalClientSecret;
    public string? TokenUrl;
    public string? CustomScope => "osduOnAws/osduOnAWSService";

    public string? UserPoolClientId;
    public string? UserPoolClientSecret;
    public string? UserPoolId;
   public RegionEndpoint RegionEndpoint => RegionEndpoint.GetBySystemName(Region);
}