using PrimePoker.Domain.Session.Enums;

namespace PrimePoker.Domain.Player;

public class SessionRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public SessionTimeType SessionTimeType { get; set; }
    public SessionType SessionType { get; set; }
}