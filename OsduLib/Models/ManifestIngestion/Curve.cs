

namespace OsduLib.Models.ManifestIngestion
{
    public class Curve
    {
        public string CurveID;
        public string? CurveUnit;
        public string Description;
        public Curve(string curveId, string description, string? curveUnit=null)
        {
            CurveID = curveId;
            CurveUnit = curveUnit;
            Description = description;
        }

    }
}

