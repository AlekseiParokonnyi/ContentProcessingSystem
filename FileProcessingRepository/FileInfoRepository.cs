using FileProcessingCore;
using FileProcessingCore.IRepositories;
using Microsoft.EntityFrameworkCore;


namespace FileProcessingRepository
{
  public class FileInfoRepository : IFileInfoRepository
  {
    private readonly FilesDbContext _db;

    public FileInfoRepository(FilesDbContext db)
    {
      _db = db;
    }

    public async Task<FileInfoModel> CreateAsync(FileInfoModel fileInfo, CancellationToken cancellationToken = default)
    {
      _db.Files.Add(fileInfo);

      await _db.SaveChangesAsync(cancellationToken);

      return fileInfo;
    }

    public async Task<FileInfoModel?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
      return await _db.Files
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<FileInfoModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
      return await _db.Files
        .AsNoTracking()
        .ToListAsync(cancellationToken);
    }
  }
}
