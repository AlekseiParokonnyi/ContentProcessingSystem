namespace FileProcessingEventContracts
{
  public class FileUploadedEvent 
  {
    public Guid Id { get; set; }
    public string Url { get; set; }
  }
}
