using PrimePoker.Domain.Session;

namespace PrimePoker.Application.Interfaces{
    public interface ISessionNotifier
    {
        Task SendHandAsync(Guid connectionId, Hand hand);
    }
}
