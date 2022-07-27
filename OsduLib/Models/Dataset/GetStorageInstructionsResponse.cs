using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsduLib.Models.Dataset
{
    public class GetStorageInstructionsResponse
    {
        [JsonProperty("storageLocation")]
        public StorageLocation StorageLocation;
    }

    public class StorageLocation
    {
        [JsonProperty("unsignedUrl")]
        public string UnsignedUrl;

        [JsonProperty("signedUrl")]
        public string SignedUrl;

        [JsonProperty("signedUploadFileName")]
        public string SignedUploadFileName;
    }
}
