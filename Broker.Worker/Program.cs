using Broker.Worker;
using Broker.Core;
using Broker.Core.Interfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => 
    {
        services.AddSingleton<IBroker, Broker>();
    })
    .Build();

host.Run();
