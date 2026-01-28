using Microsoft.AspNetCore.SignalR;
using PrimePoker.Domain.Session;

public class SessionHub : Hub, ISessionHub
{
    public async Task ReceiveHand(Hand hand)
    {
        await Clients.All.SendAsync("ReceiveHand", hand);
    }
}
