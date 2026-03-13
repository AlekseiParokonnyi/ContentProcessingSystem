using FileProcessingCore.IPublisher;

namespace FileProcessingPublisher
{
  public class BusPublisher : IBusPublisher
  {
    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
    {
      // Implementation goes here
      return Task.CompletedTask;
    }
  }
}
