using FileProcessingCore.IRepositories;

namespace FileProcessingRepository
{
  public class FileInfoProcessingRepository : IFileInfoProcessingRepository
  {
    public Task<FileInfo> CreateAsync(FileInfo fileInfo, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }

    public Task<FileInfo?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<FileInfo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }
  }
}
