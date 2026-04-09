using FileProcessingEventContracts;
using FileProcessingFunction.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FileProcessingFunction
{
  public class FileProcessingFunction
  {
    private readonly IFileProcessingService _fileProcessingService;
    private readonly ILogger _logger;

    public FileProcessingFunction(
      IFileProcessingService fileProcessingService,
      ILogger<FileProcessingFunction> logger)
    {
      _fileProcessingService = fileProcessingService;
      _logger = logger;
    }

    [Function("FileProcessingFunction")]
    public async Task Run([ServiceBusTrigger(topicName: "cps-files-processing-topic", subscriptionName: "FileProcessingWorker", Connection = "ServiceBusConnection")] string message)
    {
      _logger.LogInformation("Received message: {message}", message);

      var fileInfo = JsonSerializer.Deserialize<FileUploadedEvent>(message);

      if (fileInfo == null)
      {
        _logger.LogError("Invalid message");
        return;
      }

      await _fileProcessingService.ProcessFileAsync(fileInfo);
    }
  }
}
