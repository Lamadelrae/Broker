using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Broker.Abstractions;
using Broker.Configuration.Options;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Broker.ServiceBus;

public class ServiceBusBroker : IBroker<ServiceBusBroker>
{
    private readonly ILogger<ServiceBusBroker> _logger;
    private readonly IMediator _mediator;
    private readonly ServiceBusClient _client;


    public ServiceBusBroker(
        ILogger<ServiceBusBroker> logger,
        IMediator mediator,
        BrokerOptions<ServiceBusBroker> options)
    {
        _logger = logger;
        _mediator = mediator;
        _client = new ServiceBusClient(options.Connection);
    }

    public Task Receive<TEvent, TCommand>(string topic, string subscription)
        where TEvent : IIntegrationEvent<TCommand>
        where TCommand : class, IBaseRequest
    {
        if (string.IsNullOrWhiteSpace(topic)) throw new ArgumentException(nameof(topic));
        if (string.IsNullOrWhiteSpace(subscription)) throw new ArgumentException(nameof(subscription));

        var processor = _client.CreateProcessor(topic, subscription);
        processor.ProcessMessageAsync += ProcessMessageAsync<TEvent, TCommand>;
        processor.ProcessErrorAsync += ProcessErrorAsync;

        return processor.StartProcessingAsync();
    }


    public Task Receive<TEvent, TCommand>(string queue)
        where TEvent : IIntegrationEvent<TCommand>
        where TCommand : class, IBaseRequest
    {
        if (string.IsNullOrWhiteSpace(queue)) throw new ArgumentException(nameof(queue));

        var processor = _client.CreateProcessor(queue);
        processor.ProcessMessageAsync += ProcessMessageAsync<TEvent, TCommand>;
        processor.ProcessErrorAsync += ProcessErrorAsync;

        return processor.StartProcessingAsync();
    }

    public Task Publish<TEvent>(string queueOrTopic, TEvent @event) where TEvent : IDomainEvent
    {
        var payload = JsonSerializer.Serialize(@event);
        var sender = _client.CreateSender(queueOrTopic);
        return sender.SendMessageAsync(new ServiceBusMessage(payload)
        {
            MessageId = Guid.NewGuid().ToString(),
            ContentType = nameof(@event)
        });
    }

    private async Task ProcessMessageAsync<TEvent, TCommand>(ProcessMessageEventArgs messageEvent)
        where TEvent : IIntegrationEvent<TCommand>
        where TCommand : class, IBaseRequest
    {
        var json = messageEvent.Message.Body.ToString();
        var @event = JsonSerializer.Deserialize<TEvent>(json, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new Exception($"Event was not deserialized. Event: {nameof(TEvent)}");

        var command = @event.ToCommand();
        await _mediator.Send(command);
        await messageEvent.CompleteMessageAsync(messageEvent.Message);
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs messageEvent)
    {
        _logger.LogError(messageEvent.Exception.ToString());
        return Task.CompletedTask;
    }
}