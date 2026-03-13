namespace FileProcessingCore.IPublisher;

public interface IBusPublisher
{
  Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;
}