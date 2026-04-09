using Azure.Storage.Blobs;
using FileProcessingCore.IStorage;

namespace FileProcessingStorage
{
  public class FilesStorage : IFilesStorage
  {
    private readonly BlobContainerClient _container;

    public FilesStorage(BlobContainerClient container)
    {
      _container = container;
    }

    public async Task<string> SaveFileAsync(Guid id, Stream fileStream, CancellationToken cancellationToken = default)
    {
      var blobClient = _container.GetBlobClient(id.ToString());
      await blobClient.UploadAsync(fileStream, overwrite: true, cancellationToken);
      return blobClient.Uri.ToString();
    }

    public async Task<Stream?> GetFileAsync(Guid id, CancellationToken cancellationToken = default)
    {
      var blobClient = _container.GetBlobClient(id.ToString());

      if (!await blobClient.ExistsAsync(cancellationToken))
        return null;

      var response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);

      return response.Value.Content;
    }
  }
}
