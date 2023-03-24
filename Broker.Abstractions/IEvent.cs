using MediatR;

namespace Broker.Abstractions;

public interface IEvent { }

public interface IDomainEvent : IEvent { }

public interface IIntegrationEvent<TCommand> : IEvent where TCommand : class, IBaseRequest
{
    TCommand ToCommand();
}
