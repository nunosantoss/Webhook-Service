using Microsoft.AspNetCore.Mvc;

namespace WebhookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public WebhookController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("receive")]
        public IActionResult ReceivePaymentData([FromBody] PaymentRequest paymentRequest)
        {
            _ = Task.Run(() => _paymentService.ProcessPaymentAsync(paymentRequest));  // ✅ Fire and forget

            return Accepted("Payment is being processed.");  // ✅ Returns instantly
        }
    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; }
    }

}
