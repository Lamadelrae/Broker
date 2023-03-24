using Broker.Abstractions;
using Broker.Configuration.Extensions;
using Broker.Consumer.Commands;
using Broker.Consumer.Events;
using Broker.ServiceBus;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddBroker<ServiceBusBroker>(config =>
        {
            config.Connection = context.Configuration.GetConnectionString("ServiceBus");
        });
    });

var app = builder.Build();
var broker = app.Services.GetRequiredService<IBroker<ServiceBusBroker>>();
broker.Receive<PersonUpdatedEvent, UpdatePersonCommand>("person-topic", "broker-consumer-subscription");

app.Run();
