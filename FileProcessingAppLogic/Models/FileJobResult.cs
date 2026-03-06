using FileProcessingCore;

namespace FileProcessingApplication.Models;

public class FileJobResult
{
  public FileInfoModel FileInfoModel { get; set; }
  public FileStatus Status { get; set; }
}