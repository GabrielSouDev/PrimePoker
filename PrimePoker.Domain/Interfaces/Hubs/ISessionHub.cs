using PrimePoker.Domain.Session;

public interface ISessionHub
{
    Task ReceiveHand(Hand hand);
}
