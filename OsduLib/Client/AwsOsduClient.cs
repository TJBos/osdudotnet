using OsduLib.Models;
using OsduLib.Services.Authentication;
using static OsduLib.Services.Authentication.UserCredentialService;

namespace OsduLib.Client
{
    public class AwsOsduClient : BaseOsduClient
    {
        public UserCredentialService userCredentialService;
        public AwsOsduClient(OsduAWSEnvironment osduAWSEnvironment, string username, string password) : base(osduAWSEnvironment.DataPartitionId, osduAWSEnvironment.BaseApiUrl)
        {
            userCredentialService = new UserCredentialService(osduAWSEnvironment);
            Task.Run(async () => await SetToken(username, password)).Wait();
        }
        private async Task SetToken(string username, string password)
        {
            UserCredentials userCredentials = await userCredentialService.GetUserCredentials(username, password);
            TokenResponse response = await userCredentials.GetToken();
            AccessToken = response.AccessToken;
            RefreshToken = response.RefreshToken;
            TokenExpiration = DateTime.Now.AddSeconds(response.ExpiresIn);
        }

    }

}
