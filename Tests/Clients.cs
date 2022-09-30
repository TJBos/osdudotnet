namespace Tests
{
    [TestClass]
    public class Clients
    {
        private readonly OsduAWSEnvironment AwsEnv;
        private readonly AWSCredentials AWSCredentials;

        public Clients()
        {
            OsduConfiguration config = TestHelpers.GetApplicationConfiguration();
            AwsEnv = TestHelpers.ConfigureAwsEnv();
            AWSCredentials = config.AWSCredentials;
        }
        [TestMethod]
        public void SVPClient()
        {
            
            var client = new AwsServicePrincipalOsduClient(AwsEnv);

            Assert.IsNotNull(client);
            Assert.IsTrue(client.TokenExpiration > DateTime.Now);
           
        }
        [TestMethod] 
        public void AWSClient()
        {
            var client = new AwsOsduClient(AwsEnv, AWSCredentials.Username, AWSCredentials.Password);
            Assert.IsNotNull(client);
            Assert.IsTrue(client.TokenExpiration > DateTime.Now);
        }

        [TestMethod]
        public void SimpleClientWithAccessToken()
        {
            var awsClient = new AwsOsduClient(AwsEnv, AWSCredentials.Username, AWSCredentials.Password);
            var token = awsClient.AccessToken;
            var simpleClient = new SimpleOsduClient(AwsEnv.DataPartitionId, AwsEnv.BaseApiUrl, token);
            Assert.IsNotNull(simpleClient);
            
        }
        [TestMethod]
        public void SimpleClientWithRefreshToken()
        {
            var awsClient = new AwsOsduClient(AwsEnv, AWSCredentials.Username, AWSCredentials.Password);
            var refreshToken = awsClient.RefreshToken;
            var simpleClient = new SimpleOsduClient(AwsEnv.DataPartitionId, AwsEnv.BaseApiUrl, refreshToken, AwsEnv.TokenUrl, AwsEnv.UserPoolClientId, AwsEnv.UserPoolClientSecret);
            Assert.IsNotNull(simpleClient);
            Assert.IsNotNull(simpleClient.AccessToken);
            Assert.IsTrue(simpleClient.TokenExpiration > DateTime.Now);
        }

    }
}