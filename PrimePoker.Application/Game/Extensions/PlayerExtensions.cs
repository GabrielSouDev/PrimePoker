using PrimePoker.Domain.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimePoker.Application.Game.Extensions;

public static class PlayerExtensions
{
    public static int AllWin(this PlayerInGame player)
    {
        var actualChips = player.Chips;

        player.Chips = 0;
        player.ChipsInPot += actualChips;

        return actualChips;
    }

    public static int Raise(this PlayerInGame player, int amount)
    {
        if (player.Chips < amount)
            return player.AllWin();

        player.Chips -= amount;
        player.ChipsInPot += amount;

        return amount;
    }
}
