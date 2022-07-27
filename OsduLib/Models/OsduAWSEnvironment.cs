using Amazon;

namespace OsduLib.Models;

public class OsduAWSEnvironment
{
    public readonly string DataPartitionId;
    public readonly string? Profile;
    public readonly string Region;

    public readonly string ResourcePrefix;
    public readonly string? UserPoolClientId;
    public readonly string? UserPoolClientSecret;
    public readonly string? UserPoolId;

    public OsduAWSEnvironment(string resourcePrefix, string region,
        string? dataPartitionId,
        string? profile,
        string? userPoolId = null, string? userPoolClientId = null,
        string? userPoolClientSecret = null,
        string? baseApiUrl = null)
    {
        ResourcePrefix = string.IsNullOrEmpty(resourcePrefix) ? DefaultResourcePrefix : resourcePrefix;
        Region = region;
        DataPartitionId = string.IsNullOrEmpty(dataPartitionId) ? DefaultDataPartitionId : dataPartitionId;
        Profile = profile;
        BaseApiUrl = baseApiUrl;
        UserPoolId = userPoolId;
        UserPoolClientId = userPoolClientId;
        UserPoolClientSecret = userPoolClientSecret;
    }

    public static string DefaultDataPartitionId => "osdu";
    public static string DefaultResourcePrefix => "osdur3m9";

    public string? BaseApiUrl { get; }
    public RegionEndpoint RegionEndpoint => RegionEndpoint.GetBySystemName(Region);
}