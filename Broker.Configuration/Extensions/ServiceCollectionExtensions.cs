
using Broker.Abstractions;
using Broker.Configuration.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Broker.Configuration.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBroker<TBroker>(this IServiceCollection services, Action<BrokerOptions<TBroker>> configure) where TBroker : class, IBroker<TBroker>
    {
        services.AddSingleton<IBroker<TBroker>, TBroker>();
        services.AddSingleton(BrokerOptions<TBroker>.FromAction(configure));
    }
}