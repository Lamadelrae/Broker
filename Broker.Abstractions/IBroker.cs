using MediatR;

namespace Broker.Abstractions;

public interface IBroker<TBroker> where TBroker : class, IBroker<TBroker>
{
    Task Receive<TEvent, TCommand>(string topic, string subscription) where TEvent : IIntegrationEvent<TCommand> where TCommand : class, IBaseRequest;
    Task Receive<TEvent, TCommand>(string queue) where TEvent : IIntegrationEvent<TCommand> where TCommand : class, IBaseRequest;
    Task Publish<TEvent>(string queueOrTopic, TEvent @event) where TEvent : IDomainEvent;
}
