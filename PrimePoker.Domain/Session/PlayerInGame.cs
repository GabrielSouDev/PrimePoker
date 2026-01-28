using PrimePoker.Domain.Session.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePoker.Domain.Session;

public class PlayerInGame
{
    public Guid Id { get; init; }
    private Guid _connectionId { get; init; } = Guid.NewGuid();
    public string Name { get; init; }
    private Hand _hand { get; set; } = new(); 
    public int Chips { get; set; }
    public int TimeBank { get; set; }
    public int ChipsInPot { get; set; }
    public bool IsRoundAlive { get; set; } = true;
    public bool IsAlive => Chips > 0;
    public bool IsAllWin => Chips == 0 && ChipsInPot > 0;
    public Hand GetHand() => _hand;
    public void SetHand(Hand newHand) => _hand = newHand; 
    public Guid GetConnectionId() => _connectionId;
}
