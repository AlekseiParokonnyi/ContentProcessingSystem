namespace FileProcessingCore.IStorage;

public interface IFilesStorage
{
  Task<string> SaveFileAsync(Guid id, Stream fileStream, CancellationToken cancellationToken = default);
  Task<Stream?> GetFileAsync(Guid id, CancellationToken cancellationToken = default);
}