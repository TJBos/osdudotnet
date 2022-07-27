namespace OsduLib.Models.FileReference;

public interface IFileReference
{
    public Task<Stream> GetContents(HttpClient client);
    public string GetFileId();
    public string GetFilename();
}