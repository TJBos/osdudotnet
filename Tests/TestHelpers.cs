using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Tests
{
    internal class TestHelpers
    {
            public static OsduConfiguration GetApplicationConfiguration()
        {
            IConfiguration iConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.tests.json")
                .Build();

            var configuration = new OsduConfiguration();

            iConfig.Bind(configuration);

            return configuration;
        }
    }

    public class OsduConfiguration
    {
        public string DataPartitionId { get; set; }
        public string ApiUrl { get; set; }
        public string Region { get; set; }
        public string Profile { get; set; }
        public string SvpClientId { get; set; }
        public string SvpClientSecret { get; set; }
        public string TokenUrl { get; set; }
        public string CognitoClientId { get; set; }
        public string CognitoClientSecret { get; set; }
        public string PoolId { get; set; }
        public AWSCredentials AWSCredentials { get; set; }

    }
    public class AWSCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
