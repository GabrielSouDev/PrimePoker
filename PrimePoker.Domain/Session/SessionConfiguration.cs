
using PrimePoker.Domain.Session.Enums;

namespace PrimePoker.Domain.Session;

public class SessionConfiguration
{
    public int InitialTimeBank { get; set; }
    public int InitialChips {  get; set; }
    public int IntialBigBlind { get; set; }
    public float IncreseBigBlindPorcent { get; set; }
    public int SecondsToIncreseBigBlind { get; set; }
    public int ActionTime { get; set; }
    public string TimeType { get; set; } = string.Empty;
    public int MaxPlayer {  get; set; }
}
