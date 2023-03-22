using Broker.Core.Interfaces;

public class PersonAddedEvent : IEvent<AddPersonCommand>
{
    public string Name {get; set;}
    public AddPersonCommand ToCommand()
    {
        return new AddPersonCommand() { Name = Name};
    }
}