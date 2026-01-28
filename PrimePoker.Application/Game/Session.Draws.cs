using PrimePoker.Application.Interfaces;
using PrimePoker.Domain.Session;
using PrimePoker.Domain.Session.Enums;

namespace PrimePoker.Application.Game;
public partial class Session
{
    private async Task DrawPlayersCards()
    {
        Console.WriteLine($"* PreFlop - {Table.Timmer}");
        Table.RoundState = RoundState.PreFlop;
        foreach (var player in Table.Players.Where(p => p.IsRoundAlive == true).ToList())
        {
        Console.WriteLine($"{player.Name} - DrawPlayersCards - {Table.Timmer}");
            var newHand = new Hand();
            newHand.Cards[0] = _deck.Cards.Pop();
            newHand.Cards[1] = _deck.Cards.Pop();

            player.SetHand(newHand);
            await _sessionNotifier.SendHandAsync(player.GetConnectionId(), player.GetHand());
        }
    }

    private void DrawFlop()
    {
        Console.WriteLine($"* DrawFlop - {Table.Timmer}");
        if (Table.Players.Where(p => p.IsRoundAlive).Count() == 1) return;

        Table.RoundState = RoundState.Flop;
        Table.CommunityCards[0] = _deck.Cards.Pop();
        Table.CommunityCards[1] = _deck.Cards.Pop();
        Table.CommunityCards[2] = _deck.Cards.Pop();
    }

    private void DrawTurn()
    {
        Console.WriteLine($"* DrawTurn - {Table.Timmer}");
        if (Table.Players.Where(p => p.IsRoundAlive).Count() == 1) return;

        Table.RoundState = RoundState.Turn;
        Table.CommunityCards[3] = _deck.Cards.Pop();
    }

    private void DrawRiver()
    {
        Console.WriteLine($"* DrawRiver - {Table.Timmer}");
        if (Table.Players.Where(p => p.IsRoundAlive).Count() == 1) return;

        Table.RoundState = RoundState.River;
        Table.CommunityCards[4] = _deck.Cards.Pop();
    }
}