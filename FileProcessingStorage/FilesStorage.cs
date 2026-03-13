using FileProcessingCore.IStorage;

namespace FileProcessingStorage
{
  public class FilesStorage : IFilesStorage
  {
    public Task<string> SaveFileAsync(Guid id, Stream fileStream, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }

    public Task<FileStream?> GetFileAsync(Guid id, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }
  }
}
