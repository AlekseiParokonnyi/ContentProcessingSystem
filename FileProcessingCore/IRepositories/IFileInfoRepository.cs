namespace FileProcessingCore.IRepositories;

public interface IFileInfoRepository
{
  Task<FileInfoModel> CreateAsync(FileInfoModel fileInfo, CancellationToken cancellationToken = default);
  Task<FileInfoModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
  Task<IEnumerable<FileInfoModel>> GetAllAsync(CancellationToken cancellationToken = default);
}