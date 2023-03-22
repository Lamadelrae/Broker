namespace Broker.Core.Interfaces;

public interface IBroker
{
    public Task Receive<TEvent, TCommand>(string queue) where TEvent : IEvent<TCommand> where TCommand : class;
}