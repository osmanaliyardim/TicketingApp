using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace TicketingApp.ApplicationCore.Services;

public class EventPublisher
{
    private readonly ServiceBusClient _client;
    private readonly string _queueOrTopicName;

    public EventPublisher(string connectionString, string queueOrTopicName)
    {
        _client = new ServiceBusClient(connectionString);
        _queueOrTopicName = queueOrTopicName;
    }

    public async Task PublishEventAsync<T>(T eventMessage)
    {
        ServiceBusSender sender = _client.CreateSender(_queueOrTopicName);

        try
        {
            // Serialize the event message to JSON
            string messageBody = JsonSerializer.Serialize(eventMessage);
            ServiceBusMessage message = new ServiceBusMessage(messageBody);

            // Send the message to the Service Bus queue or topic
            await sender.SendMessageAsync(message);
            Console.WriteLine($"Order has been sent to {_queueOrTopicName}: {messageBody}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending order to {_queueOrTopicName}: {ex.Message}");
        }
        finally
        {
            await sender.DisposeAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DisposeAsync();
    }
}

