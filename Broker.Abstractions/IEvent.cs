public interface IEvent { }

public interface IDomainEvent : IEvent { }

public interface IIntegrationEvent<TCommand> : IEvent where TCommand : class
{
    TCommand ToCommand();
}
