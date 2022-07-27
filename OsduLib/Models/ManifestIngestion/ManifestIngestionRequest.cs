using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsduLib.Models.ManifestIngestion
{
    public class ManifestIngestionRequest
    {
        [JsonProperty("runId")] public Guid RunID { get; private set; }

        [JsonProperty("executionContext")] public ExecutionContext ExecutionContext;
        public ManifestIngestionRequest()
        {
            RunID = Guid.NewGuid();
            ExecutionContext = new ExecutionContext();
        }
    }
}
