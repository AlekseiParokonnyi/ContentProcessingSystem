using FileProcessingCore;
using FileInfo = FileProcessingCore.FileInfo;

namespace FileProcessingApplication.Models;

public class FileJobResult
{
  public FileInfo FileInfo { get; set; }
  public FileStatus Status { get; set; }
}