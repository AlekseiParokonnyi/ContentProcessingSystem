using FileProcessingCore;

namespace FileProcessingApplication.Models;

public class FileUploadResult
{
  public FileInfoModel FileInfoModel { get; set; } = null!;
  public OperationStatus Status { get; set; }
}