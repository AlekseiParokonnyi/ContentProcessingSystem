using FileProcessingApplication.Models;

namespace FileProcessingApplication
{
  public class FileProcessingService : IFileProcessingService
  {
    public Task<FileJobResult> StoreFileAsync(string fileName, Stream fileStream, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }

    public Task<FileJobResult?> GetFileAsync(Guid id, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<FileJobResult>> GetAllFilesAsync(CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }
  }
}
