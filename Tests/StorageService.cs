namespace Tests
{
    [TestClass]
    public class StorageService
    {
        private readonly AwsServicePrincipalOsduClient _client;
        public StorageService()
        {
            OsduConfiguration config = TestHelpers.GetApplicationConfiguration();
            OsduAWSEnvironment AwsEnv = TestHelpers.ConfigureAwsEnv();
            _client = new AwsServicePrincipalOsduClient(AwsEnv);
        }
        [TestMethod]
        public async Task storeRecord()
        {
            var record = new OsduRecord();
            var recordsToStore = new List<OsduRecord> { record };
            var response = await _client.Storage.StoreRecords(recordsToStore);
            Assert.AreEqual(response.RecordCount, recordsToStore.Count);
            await _client.Storage.DeleteRecord(response.RecordIds.FirstOrDefault());
        }
        [TestMethod]
        public async Task getRecord()
        {
            var record = new OsduRecord();
            var response = await _client.Storage.StoreRecords(new List<OsduRecord> { record });
            var recordId = response.RecordIds.FirstOrDefault();
            var result = await _client.Storage.GetRecord(recordId);
            Assert.AreEqual(recordId, result.Id);
            await _client.Storage.DeleteRecord(recordId);
        }

        [TestMethod]
        public async Task deleteRecord()
        {
            var record = new OsduRecord();
            var response = await _client.Storage.StoreRecords(new List<OsduRecord> { record });
            var recordId = response.RecordIds.FirstOrDefault();
            var result1 = await _client.Storage.GetRecord(recordId);
            Assert.IsNotNull(result1);
            await _client.Storage.DeleteRecord(recordId);
            await Task.Delay(5000);
            OsduLib.Models.Search.SearchResult result2;
            try
            {
            result2 = await _client.Storage.GetRecord(recordId);
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.InnerException.Message, "404");
            }
        }
    }
}
