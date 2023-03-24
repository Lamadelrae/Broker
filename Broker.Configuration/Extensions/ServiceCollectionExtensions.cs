
using Broker.Abstractions;
using Broker.Configuration.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Broker.Configuration.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBroker<TBroker>(this IServiceCollection services, Action<BrokerOptions> configure) where TBroker : class, IBroker
    {
        services.AddSingleton<IBroker, TBroker>();
        services.AddSingleton(BrokerOptions.FromAction(configure));
    }
}