using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Polly;
using System.Text;

namespace TicketingApp.OrderItemsReserverFunc;

public class OrderItemsReserverFunc
{
    private static readonly string BlobContainerName = Environment.GetEnvironmentVariable("BlobContainerName");
    private static readonly string BlobStorageConnectionString = Environment.GetEnvironmentVariable("BlobStorageConnectionString");
    private readonly BlobServiceClient blobServiceClient = new BlobServiceClient(BlobStorageConnectionString);

    [FunctionName("orderitemsreserverfunc")]
    public async Task Run(
        [ServiceBusTrigger("ordersqueue", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage orderEvent,
        ILogger log)
    {
        log.LogInformation("Processing order event.");

        var orderObj = JsonConvert.DeserializeObject(orderEvent.Body.ToString());

        // Create unique blob name
        string blobName = $"order-{Guid.NewGuid()}.json";

        // Define the retry policy with Polly
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // Exponential back-off
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    log.LogWarning($"Retry {retryCount} encountered an error: {exception.Message}. Waiting {timeSpan} before next retry.");
                });

        try
        {
            var orderJsonStr = JsonConvert.SerializeObject(orderObj);

            BlobClient blobClient = blobServiceClient.GetBlobContainerClient(BlobContainerName).GetBlobClient(blobName);

            await retryPolicy.ExecuteAsync(async () =>
            {
                using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(orderJsonStr)))
                {
                    await blobClient.UploadAsync(memoryStream, overwrite: true);
                }
            });

            // Send email for successfull order
            await SendEmailAsync(orderObj, log);

            log.LogInformation($"Order created for ID: {orderObj.ToString}");
        }
        catch (Exception ex)
        {
            log.LogError($"Failed to proccess order after retries: {ex.Message}");

            // Send email for error
            await SendEmailAsync(orderObj, log);
        }
    }

    private static async Task SendEmailAsync(object orderData, ILogger log)
    {
        // Use Logic Apps to send a fallback email
        string logicAppUrl = Environment.GetEnvironmentVariable("LogicAppUrl");

        using (var client = new HttpClient())
        {
            var content = new StringContent(JsonConvert.SerializeObject(orderData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(logicAppUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                log.LogError($"Failed to send fallback email: {response.ReasonPhrase}");
            }
        }
    }
}
