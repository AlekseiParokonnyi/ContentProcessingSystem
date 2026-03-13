using FileProcessingEventContracts;

namespace FileProcessingFunction.Services;

public class FileProcessingService : IFileProcessingService
{
  public async Task ProcessFileAsync(FileUploadedEvent fileUploadedEvent)
  {
    Console.WriteLine($"Processing file with Id: {fileUploadedEvent.Id} and Url: {fileUploadedEvent.Url}");
    await Task.Delay(1000);
  }
}