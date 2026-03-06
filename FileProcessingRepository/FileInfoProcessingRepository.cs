using FileProcessingCore;
using FileProcessingCore.IRepositories;


namespace FileProcessingRepository
{
  public class FileInfoProcessingRepository : IFileInfoProcessingRepository
  {
    public Task<FileInfoModel> CreateAsync(FileInfoModel fileInfo, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }

    public Task<FileInfoModel?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<FileInfoModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }
  }
}
