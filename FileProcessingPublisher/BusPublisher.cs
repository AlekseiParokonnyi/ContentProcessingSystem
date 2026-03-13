using FileProcessingCore.IPublisher;
using FileProcessingEventContracts;
using System.Text.Json;

namespace FileProcessingPublisher
{
  public class BusPublisher : IBusPublisher
  {
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
    {
      //var client = new ServiceBusClient(connectionString);
      //var sender = client.CreateSender("orders-queue");

      //var message = new ServiceBusMessage(JsonSerializer.Serialize(@event));

      //await sender.SendMessageAsync(message);
    }
  }
}
