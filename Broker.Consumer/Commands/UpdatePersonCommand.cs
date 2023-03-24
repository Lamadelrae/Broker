using MediatR;

namespace Broker.Consumer.Commands;

public class UpdatePersonCommand : IRequest
{
    public string? Name { get; set; }
}


public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand>
{
    public Task Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
