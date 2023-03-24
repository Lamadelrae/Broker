using MediatR;

namespace Broker.Consumer.Commands;

public class UpdatePersonCommand : IRequest
{
    public string? Name { get; set; }
}