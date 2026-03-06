using FileProcessingApplication.Models;

namespace FileProcessingApplication;

public interface IFileProcessingService
{
  Task<FileJobResult> StoreFileAsync(string fileName, Stream fileStream, CancellationToken cancellationToken = default);
  Task<FileJobResult?> GetFileAsync(Guid id, CancellationToken cancellationToken = default);
  Task<IEnumerable<FileJobResult>> GetAllFilesAsync(CancellationToken cancellationToken = default);
}