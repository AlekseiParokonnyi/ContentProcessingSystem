using FileProcessingCore.IPublisher;

namespace FileProcessingEventContracts
{
  public class FileUploadedEvent : IEvent
  {
    public Guid Id { get; set; }
    public string Url { get; set; }
  }
}
