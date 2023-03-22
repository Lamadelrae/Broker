using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Broker.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Broker.Core;

public class Broker : IBroker
{
    private readonly ServiceBusClient _client;

    public Broker(IConfiguration configuration)
    {
        _client = new ServiceBusClient(configuration.GetConnectionString("serviceBus"));
    }

    public Task Receive<TEvent, TCommand>(string queue) where TEvent : IEvent<TCommand> where TCommand : class
    {
        var processor = _client.CreateProcessor(queue);
        processor.ProcessMessageAsync += async (messageEvent) =>
        {
            var json = messageEvent.Message.Body.ToString();
            var @event = JsonSerializer.Deserialize<TEvent>(json);
            if(@event == null)
            {
                return;
            }

            var command = @event.ToCommand();
            Console.WriteLine("{@command}", command);
        };

        return processor.StartProcessingAsync();
    }
}