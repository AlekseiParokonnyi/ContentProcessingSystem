using Azure.Messaging.ServiceBus;
using FileProcessingCore.IPublisher;
using System.Text.Json;

namespace FileProcessingPublisher
{
  public class BusPublisher : IBusPublisher
  {
    private readonly ServiceBusSender _sender;

    public BusPublisher(ServiceBusSender sender)
    {
      _sender = sender;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
    {
      var json = JsonSerializer.Serialize(@event);

      var message = new ServiceBusMessage(json)
      {
        ContentType = "application/json",
        MessageId = Guid.NewGuid().ToString()
      };

      message.ApplicationProperties["varsion"] = "1.0";
      message.ApplicationProperties["message-type"] = typeof(TEvent).Name;

      await _sender.SendMessageAsync(message);
    }
  }
}
