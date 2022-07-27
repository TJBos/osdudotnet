using OsduLib.Exceptions;
using OsduLib.Models.FileReference;
using OsduLib.Models.Record;
using System.Net;

namespace OsduLib.Services;

internal class FileReferenceManager
{
    public const string S3ProviderKey = "AWS_S3";
    private readonly IHttpClientFactory _clientFactory;

    public FileReferenceManager(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public IFileReference GetFile(string fileId, string signedUrl, string providerKey)
    {
        switch (providerKey)
        {
            case S3ProviderKey:
                return new AwsFileReference(fileId, signedUrl);
            default:
                throw new FileProviderException("Unsupported file provider - " + providerKey);
        }
    }

    public async IAsyncEnumerable<(string filename, string filepath)> SaveFile(string destination,
        params IFileReference[] fileReferences)
    {
        var httpClient = _clientFactory.CreateClient();
        foreach (var fileReference in fileReferences)
        {
            var filename = fileReference.GetFilename();
            var extension = Path.GetExtension(filename);
            var basename = Path.GetFileNameWithoutExtension(filename);
            filename = Utilities.Slugify(basename) + extension;
            //TO DO; if fileName = 'signedupload' => maybe like pass in file name from metdata and save here.
            filename = "Well_Core Analysis.csv";
            var filepath = Path.Join(destination, filename);
            using var stream = await fileReference.GetContents(httpClient);
            await using Stream writer = File.Create(filepath);
            await stream.CopyToAsync(writer);
            stream.Close();
            
            yield return (filename, filepath);
        }
    }
    // this method is from AWS SDK docs, TODO = change deprecated webrequest to httpclient
    public bool UploadObject(string filePath, string signedUrl)
    {
        HttpWebRequest httpRequest = WebRequest.Create(signedUrl) as HttpWebRequest;
        httpRequest.Method = "PUT";
        using (Stream dataStream = httpRequest.GetRequestStream())
        {
            var buffer = new byte[8000];
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    dataStream.Write(buffer, 0, bytesRead);
                }
            }
        }
        HttpWebResponse response = httpRequest.GetResponse() as HttpWebResponse;
        return response.StatusCode == HttpStatusCode.OK;
    }

    public static void DeleteFiles(string folderPath)
    {
        var dir = new DirectoryInfo(@folderPath);
        dir.Delete(true);
    }
}