
namespace OsduLib.Models.Record
{
     public interface IRecord
    {
        string Kind { get; set; }
        Acl Acl { get; set; }
        Legal Legal { get; set; }
    }
}
