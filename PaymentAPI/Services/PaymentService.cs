using Microsoft.AspNetCore.SignalR;
using PaymentAPI.Interfaces;
using PaymentAPI.Models;
using System.Text;
using System.Text.Json;

public class PaymentService : IPaymentService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHubContext<PaymentHub> _hubContext;
    private readonly IConfiguration _config;

    public PaymentService(IHttpClientFactory httpClientFactory, IHubContext<PaymentHub> hubContext, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _hubContext = hubContext;
        _config = config;
    }

    public async Task<bool> SendToWebhook(PaymentRequest paymentRequest)
    {
        var client = _httpClientFactory.CreateClient();
        var url = String.Concat(_config["Api:WebhookUrl"], _config["Api:Routes:SendPayment"]);

        var jsonContent = JsonSerializer.Serialize(paymentRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            // Fire and forget
            _ = client.PostAsync(url, content);  // The underscore (_) means "do not await"

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
