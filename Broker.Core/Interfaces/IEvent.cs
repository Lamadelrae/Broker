namespace Broker.Core.Interfaces;

public interface IEvent<TCommand> where TCommand : class
{
    TCommand ToCommand();
}