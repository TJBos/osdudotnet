using OsduLib.Models.Record;

namespace OsduLib.Models.FileReference;

public class AwsFileReference : IFileReference
{
    private readonly string _fileId;
    private readonly string _filename;
    private readonly string _signedUrl;

    public AwsFileReference(string fileId, string signedUrl)
    {
        _fileId = fileId;
        _signedUrl = signedUrl;
        var uri = new Uri(_signedUrl);
        _filename = Path.GetFileName(uri.LocalPath);
    }

    public async Task<Stream> GetContents(HttpClient client)
    {
        var stream = await client.GetStreamAsync(_signedUrl);
        return stream;
    }

    public string GetFileId()
    {
        return _fileId;
    }

    public string GetFilename()
    {
        return _filename;
    }
}