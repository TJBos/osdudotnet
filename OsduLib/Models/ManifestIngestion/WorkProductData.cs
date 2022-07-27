using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsduLib.Models.ManifestIngestion
{
    public class WorkProductData
    {
        public string? Name;
        public string? Description;
        public List<string> Components;

        public WorkProductData()
        {
            Components = new List<string>();
        }

    }
}
