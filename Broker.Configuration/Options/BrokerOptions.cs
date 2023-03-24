namespace Broker.Configuration.Options;

public class BrokerOptions
{
    public string Connection { get; set; }

    public static BrokerOptions FromAction(Action<BrokerOptions> configure)
    {
        var instance = new BrokerOptions();
        configure(instance);
        return instance;
    }
}