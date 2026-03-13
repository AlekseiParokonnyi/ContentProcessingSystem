using FileProcessingCore;

namespace FileProcessingApplication.Models;

public class FileContentResult : IDisposable
{
  public FileInfoModel FileInfoModel { get; set; }
  public FileStream Content { get; set; } = null!;

  public void Dispose()
  {
    Content.Dispose();
  }
}