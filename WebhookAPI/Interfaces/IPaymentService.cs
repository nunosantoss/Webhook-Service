using WebhookAPI.Controllers;

public interface IPaymentService
{
    Task ProcessPaymentAsync(PaymentRequest paymentRequest);
}