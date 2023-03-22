using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Broker.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Broker.Core;

public class Broker : IBroker
{
    private readonly IMediator _mediator;
    private readonly ServiceBusClient _client;

    public Broker(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _client = new ServiceBusClient(configuration.GetConnectionString("sql"));
    }

    public Task Receive<TEvent, TCommand>(string topic, string subscription) where TEvent : IEvent<TCommand> where TCommand : IBaseRequest
    {
        var processor = _client.CreateProcessor(topic, subscription);
        processor.ProcessMessageAsync += async (messageEvent) =>
        {
            var json = messageEvent.Message.Body.ToString();
            var @event = JsonSerializer.Deserialize<TEvent>(json);
            if(@event == null)
            {
                return;
            }

            var command = @event.ToCommand();
            await _mediator.Send(command);
        };

        return processor.StartProcessingAsync();
    }
}