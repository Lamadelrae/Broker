using Broker.Abstractions;

namespace Broker.Configuration.Options;

public class BrokerOptions<TBroker> where TBroker : class, IBroker<TBroker>
{
    public string? Connection { get; set; }

    public static BrokerOptions<TBroker> FromAction(Action<BrokerOptions<TBroker>> configure)
    {
        var instance = new BrokerOptions<TBroker>();
        configure(instance);
        return instance;
    }
}
