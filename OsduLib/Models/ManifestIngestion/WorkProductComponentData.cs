using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsduLib.Models.ManifestIngestion
{
    // This wpc follows the schema for a wellog-wpc, since it's the only wpc we'll use. In the case there will be multiple wpc schema's: extend this class amd pull out properties unique to schema.
    public class WorkProductComponentData
    {
        public List<string> Datasets;
        public string WellboreID;
        public double TopMeasuredDepth;
        public double BottomMeasuredDepth;
        public string ServiceCompanyId;
        public string LogVersion;
        public Dictionary<string, string> LogServiceDateInterval;
        public List<Curve> Curves;

        public WorkProductComponentData()
        {
            Datasets = new List<string>();
        }
    }
}
