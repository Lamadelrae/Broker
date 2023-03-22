using Azure.Messaging.ServiceBus;

public interface IBroker
{
    public Task Receive<TEvent>(string topic, string subscription);
}

public class Broker : IBroker
{
    private readonly ServiceBusClient _client;

    public Broker(string connectionString)
    {
        _client = new ServiceBusClient(connectionString);
    }

    public async Task Receive<TEvent>(string topic, string subscription)
    {
        var processor = _client.CreateProcessor(topic, subscription);
        processor.ProcessMessageAsync += async (messageArgs) =>
        {
            Console.WriteLine(messageArgs.Message?.Body.ToString());
            await messageArgs.CompleteMessageAsync(messageArgs.Message);
        };
        
        processor.ProcessErrorAsync += async (messageArgs) =>
        {
            Console.WriteLine(messageArgs.Exception.Message);
        };

        await processor.StartProcessingAsync();
    }
}