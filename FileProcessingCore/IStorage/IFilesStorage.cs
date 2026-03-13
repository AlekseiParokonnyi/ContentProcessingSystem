namespace FileProcessingCore.IStorage;

public interface IFilesStorage
{
  Task<string> SaveFileAsync(Guid id, Stream fileStream, CancellationToken cancellationToken = default);
  Task<FileStream?> GetFileAsync(Guid id, CancellationToken cancellationToken = default);
}