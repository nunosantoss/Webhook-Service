using Microsoft.AspNetCore.SignalR;

public class PaymentHub : Hub
{
    public async Task SendPaymentUpdate(string message)
    {
        await Clients.All.SendAsync("ReceivePaymentUpdate", message);
    }
}
