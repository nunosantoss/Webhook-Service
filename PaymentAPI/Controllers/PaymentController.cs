using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PaymentAPI.Interfaces;
using PaymentAPI.Models;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IHubContext<PaymentHub> _hubContext;

        public PaymentController(IPaymentService paymentService, IHubContext<PaymentHub> hubContext)
        {
            _paymentService = paymentService;
            _hubContext = hubContext;
        }

        [HttpPost("send-to-webhook")]
        public IActionResult SendToWebhook([FromBody] PaymentRequest paymentRequest)
        {
            _ = Task.Run(() => _paymentService.SendToWebhook(paymentRequest));
            _hubContext.Clients.All.SendAsync("ReceivePaymentUpdate", "Processing Payment");
            return Accepted("Payment is being processed.");
        }


        [HttpPost("update-status")]
        public IActionResult UpdatePaymentStatus([FromBody] StatusUpdate statusUpdate)
        {
            _hubContext.Clients.All.SendAsync("ReceivePaymentUpdate", "Payment Completed");
            return Ok();
        }
    }
}
