namespace FileProcessingCore.IRepositories;

public interface IFileInfoProcessingRepository
{
  Task<System.IO.FileInfo> CreateAsync(System.IO.FileInfo fileInfo, CancellationToken cancellationToken = default);
  Task<System.IO.FileInfo?> GetAsync(int id, CancellationToken cancellationToken = default);
  Task<IEnumerable<System.IO.FileInfo>> GetAllAsync(CancellationToken cancellationToken = default);
}