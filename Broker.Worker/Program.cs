using Broker.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IBroker, Broker.Core.Broker>();
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.Configure((ctx, app) =>
        {
            var broker = app.ApplicationServices.GetRequiredService<IBroker>();
            broker.Receive<PersonAddedEvent, AddPersonCommand>("my-queue");
        });
    })
    .Build();

host.Run();
