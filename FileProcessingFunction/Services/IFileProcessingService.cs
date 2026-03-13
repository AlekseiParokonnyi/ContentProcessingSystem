using FileProcessingEventContracts;

namespace FileProcessingFunction.Services;

public interface IFileProcessingService
{
  Task ProcessFileAsync(FileUploadedEvent fileUploadedEvent);
}