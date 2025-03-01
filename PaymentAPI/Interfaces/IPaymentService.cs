using PaymentAPI.Controllers;
using PaymentAPI.Models;

namespace PaymentAPI.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> SendToWebhook(PaymentRequest paymentRequest);
    }
}
