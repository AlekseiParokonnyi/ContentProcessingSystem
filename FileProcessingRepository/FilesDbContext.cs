using FileProcessingCore;
using Microsoft.EntityFrameworkCore;

namespace FileProcessingRepository;

public class FilesDbContext : DbContext
{
  public FilesDbContext(DbContextOptions<FilesDbContext> options)
    : base(options)
  {
  }

  public DbSet<FileInfoModel> Files => Set<FileInfoModel>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("files");
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(FilesDbContext).Assembly);
  }
}