using FileProcessingApplication.Models;
using FileProcessingCore;

namespace FileProcessingApplication;

public interface IFileProcessingService
{
  Task<FileJobResult> StoreFileAsync(string fileName, Stream fileStream, CancellationToken cancellationToken = default);
  Task<FileContentResult?> GetFileContentAsync(Guid id, CancellationToken cancellationToken = default);
  Task<IEnumerable<FileInfoModel>> GetAllFilesAsync(CancellationToken cancellationToken = default);
}