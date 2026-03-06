namespace FileProcessingCore.IRepositories;

public interface IFileInfoProcessingRepository
{
  Task<FileInfoModel> CreateAsync(FileInfoModel fileInfo, CancellationToken cancellationToken = default);
  Task<FileInfoModel?> GetAsync(int id, CancellationToken cancellationToken = default);
  Task<IEnumerable<FileInfoModel>> GetAllAsync(CancellationToken cancellationToken = default);
}