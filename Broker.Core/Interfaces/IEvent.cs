using MediatR;

namespace Broker.Core.Interfaces;

public interface IEvent<TCommand> where TCommand : IBaseRequest
{
    TCommand ToCommand();
}