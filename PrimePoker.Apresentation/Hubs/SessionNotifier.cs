using Microsoft.AspNetCore.SignalR;
using PrimePoker.Application.Interfaces;
using PrimePoker.Domain.Session;

public class SessionNotifier : ISessionNotifier
{
    private readonly IHubContext<SessionHub> _hubContext;

    public SessionNotifier(IHubContext<SessionHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task SendHandAsync(Guid connectionId, Hand hand)
    {
        return _hubContext.Clients.Client(connectionId.ToString())
            .SendAsync("ReceiveHand", hand);
    }
}
