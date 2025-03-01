using System.Text.Json;
using System.Text;
using WebhookAPI.Controllers;

public class PaymentService : IPaymentService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public PaymentService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    public async Task ProcessPaymentAsync(PaymentRequest paymentRequest)
    {
        await Task.Delay(10000);

        var client = _httpClientFactory.CreateClient();
        var statusUpdate = new { Status = "Completed", paymentRequest.Amount };

        var content = new StringContent(JsonSerializer.Serialize(statusUpdate), Encoding.UTF8, "application/json");

        await client.PostAsync(String.Concat(_config["Api:BaseUrl"], _config["Api:Routes:UpdateStatus"]), content);
    }
}