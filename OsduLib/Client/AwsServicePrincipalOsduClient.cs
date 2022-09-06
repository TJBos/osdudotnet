using Newtonsoft.Json;
using OsduLib.Models;
using OsduLib.Services;
using OsduLib.Services.Authentication;
using System.Net.Http.Headers;

namespace OsduLib.Client
{
    public class AwsServicePrincipalOsduClient : BaseOsduClient
    {
        private readonly ServicePrincipal servicePrincipal;

        public AwsServicePrincipalOsduClient(OsduAWSEnvironment osduAWSEnvironment) : base(osduAWSEnvironment.DataPartitionId, osduAWSEnvironment.BaseApiUrl)
        {

            if (osduAWSEnvironment.ServicePrincipalClientId != null && osduAWSEnvironment.ServicePrincipalClientSecret != null && osduAWSEnvironment.TokenUrl != null)
            {
                servicePrincipal = new ServicePrincipal(osduAWSEnvironment.ServicePrincipalClientId, osduAWSEnvironment.ServicePrincipalClientSecret, osduAWSEnvironment.TokenUrl, osduAWSEnvironment.CustomScope);
            }
            else if (osduAWSEnvironment.Profile != null && osduAWSEnvironment.Region != null && osduAWSEnvironment.ResourcePrefix != null)
            {
                servicePrincipal = new ServicePrincipal(osduAWSEnvironment.ResourcePrefix, osduAWSEnvironment.Region, osduAWSEnvironment.Profile);
            }
            else
            {
                throw new ArgumentException("You must provide either a Profile, Region and ResourcePrefix or a ServicePrincipalClientId, ServicePrincipalClientSecret and TokenUrl to create a ServicePrincipalClient.");
            }
            Task.Run(async () => await SetToken()).Wait();
        }
        private async Task SetToken()
        {
            TokenResponse response = await servicePrincipal.GetToken();
            AccessToken = response.AccessToken;
            TokenExpiration = DateTime.Now.AddSeconds(response.ExpiresIn);
        }

        protected override async Task UpdateToken()
        {
            await SetToken();
        }

    }
}
