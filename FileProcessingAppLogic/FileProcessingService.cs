using FileProcessingApplication.Models;
using FileProcessingCore;
using FileProcessingCore.IPublisher;
using FileProcessingCore.IRepositories;
using FileProcessingCore.IStorage;
using FileProcessingEventContracts;

namespace FileProcessingApplication
{
  public class FileProcessingService : IFileProcessingService
  {
    private readonly IFileInfoRepository _fileInfoProcessingRepository;
    private readonly IFilesStorage _fileStorage;
    private readonly IBusPublisher _busPublisher;

    public FileProcessingService(IFileInfoRepository fileInfoProcessingRepository, IFilesStorage fileStorage, IBusPublisher busPublisher)
    {
      _fileInfoProcessingRepository = fileInfoProcessingRepository;
      _fileStorage = fileStorage;
      _busPublisher = busPublisher;
    }

    public async Task<FileUploadResult> StoreFileAsync(string fileName, Stream fileStream, CancellationToken cancellationToken = default)
    {
      var fileGuid = Guid.NewGuid();

      var fileUrl = await _fileStorage.SaveFileAsync(fileGuid, fileStream, cancellationToken);

      var fileInfo = new FileInfoModel
      {
        Id = fileGuid,
        FileName = fileName,
        Url = fileUrl,
        CreatedAt = DateTime.UtcNow
      };

      await _fileInfoProcessingRepository.CreateAsync(fileInfo, cancellationToken);

      await _busPublisher.PublishAsync(new FileUploadedEvent
      {
        Id = fileGuid,
        Url = fileUrl
      }, cancellationToken);

      return new FileUploadResult
      {
        FileInfoModel = fileInfo,
        Status = OperationStatus.Completed
      };
    }

    public async Task<FileContentResult?> GetFileContentAsync(Guid id, CancellationToken cancellationToken = default)
    {
      var fileInfo = await _fileInfoProcessingRepository.GetAsync(id, cancellationToken);

      if (fileInfo is null) return null;

      var fileStream = await _fileStorage.GetFileAsync(fileInfo.Id, cancellationToken);

      if (fileStream is null) return null;

      return new FileContentResult
      {
        FileInfoModel = fileInfo,
        Content = fileStream
      };
    }

    public Task<IEnumerable<FileInfoModel>> GetAllFilesAsync(CancellationToken cancellationToken = default)
    {
      return _fileInfoProcessingRepository.GetAllAsync(cancellationToken);
    }
  }
}
