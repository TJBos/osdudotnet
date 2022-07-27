using OsduLib.Models;
using OsduLib.Services.Authentication;

namespace OsduLib.Client
{
    // This client can take in either a local CLI profile with right permissions.
    // Or the cognito service principal client_id and client_secret directly, also pass in the authtokenurl and the scope.
    public class AwsServicePrincipalOsduClient : BaseOsduClient
    {
        public string ResourcePrefix;
        private ServicePrincipal servicePrincipal;

        public AwsServicePrincipalOsduClient(string dataPartitionId, string apiBaseUrl, string resourcePrefix, string profile, string region) : base(dataPartitionId, apiBaseUrl)
        {
            ResourcePrefix = resourcePrefix;
            servicePrincipal = new ServicePrincipal(resourcePrefix, region, profile);
            Task.Run(async () => await SetToken()).Wait();
        }

        public AwsServicePrincipalOsduClient(string dataPartitionId, string apiBaseUrl, string clientId, string clientSecret, string authTokenUrl, string awsOAuthScope ) : base(dataPartitionId, apiBaseUrl)
        {
            servicePrincipal = new ServicePrincipal(clientId, clientSecret, authTokenUrl, awsOAuthScope);
            Task.Run(async () => await SetToken()).Wait();
        }
        private async Task SetToken()
        {
            TokenResponse response = await servicePrincipal.GetToken();
            AccessToken = response.AccessToken;
            TokenExpiration = response.ExpiresIn;
        }
    }
}
