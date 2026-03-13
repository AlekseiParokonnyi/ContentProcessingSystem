namespace FileProcessingCore;

public class FileInfoModel
{
  public Guid Id { get; set; }
  public string FileName { get; set; } = "";
  public string? Url { get; set; }
  public FileStatus Status { get; set; }
  public DateTime CreatedAt { get; set; }
}