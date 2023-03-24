
using Broker.Abstractions;
using Broker.Consumer.Commands;

namespace Broker.Consumer.Events;

public class PersonUpdatedEvent : IIntegrationEvent<UpdatePersonCommand>
{
    public string? Name { get; set; }
    
    public UpdatePersonCommand ToCommand()
    {
        return new UpdatePersonCommand()
        {
            Name = Name
        };
    }
}
