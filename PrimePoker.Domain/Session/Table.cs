using PrimePoker.Domain.Session.Enums;
using System;
using System.Data;

namespace PrimePoker.Domain.Session;

public class Table
{
    public List<PlayerInGame> Players { get; init; } = new();
    public Card[] CommunityCards { get; set; } = new Card[5];
    public int BigBlind { get; set; }
    public int Pot { get => Players.Sum(p => p.ChipsInPot); }
    public RoundState RoundState { get; set; } = RoundState.None;
    public bool HasRaiseOrAllWin { get; set; } = false;
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public TimeSpan Timmer
    {
        get => DateTime.UtcNow - CreateAt;
    }
}
