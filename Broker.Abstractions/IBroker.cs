public interface IBroker { }

public interface IBroker<TBroker> where TBroker : class, IBroker
{
    Task Receive<TEvent, TCommand>(string topic, string subscription) where TEvent : IIntegrationEvent<TCommand> where TCommand : class;
    Task Receive<TEvent, TCommand>(string queue) where TEvent : IIntegrationEvent<TCommand> where TCommand : class;
    Task Publish<TEvent>(string topic) where TEvent : IDomainEvent;
}