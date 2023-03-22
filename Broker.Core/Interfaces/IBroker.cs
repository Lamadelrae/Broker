using MediatR;

namespace Broker.Core.Interfaces;

public interface IBroker
{
    public Task Receive<TEvent, TCommand>(string topic, string subscription) where TEvent : IEvent<TCommand> where TCommand : IBaseRequest;
}