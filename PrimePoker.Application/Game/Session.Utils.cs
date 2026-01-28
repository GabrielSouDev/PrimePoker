using PrimePoker.Application.Game.Extensions;
using PrimePoker.Domain.Session;
using PrimePoker.Domain.Session.Enums;

namespace PrimePoker.Application.Game;

public partial class Session
{
    private bool IsLastToAct(PlayerInGame player)
    {
        if (Table.HasRaiseOrAllWin)
            return player == Table.Players.OrderByDescending(p => p.Chips).First();

        var roundAlivePlayers = Table.Players.Where(p => p.IsRoundAlive == true).ToList();

        switch (Table.RoundState)
        {
            case RoundState.PreFlop:
                return player.Id == roundAlivePlayers.Last().Id;
            default:
                return player.Id == roundAlivePlayers[roundAlivePlayers.Count - 2].Id;
        }
    }

    private void DefineBigBlind()
    {
        var elapsed = DateTime.UtcNow - Table.CreateAt;
        var intervalCount = (int)(elapsed.TotalSeconds / SessionConfiguration.SecondsToIncreseBigBlind);

        var increaseRate = 1 + SessionConfiguration.IncreseBigBlindPorcent;
        var newBigBlind = SessionConfiguration.IntialBigBlind * Math.Pow(increaseRate, intervalCount);

        Table.BigBlind = (int)newBigBlind;
    }

    private bool RoundIsFinished()
    {
        var playersAlive = Table.Players.Where(p => p.IsRoundAlive == true).ToList();

        return playersAlive.Count == 1 || playersAlive.All(p => p.IsAllWin == true);
    }

    private void ResetCommunitaryCards() => Table.CommunityCards = new Card[5];

    private void ResetPlayersCard() => Table.Players.ForEach(p => p.SetHand(new Hand()));

    private void NewRound()
    {
        Console.WriteLine($"* NewRound - {Table.Timmer}");
        Table.RoundState = RoundState.None;
        DefineBigBlind();

        ResetCommunitaryCards();
        ResetPlayersCard();

        NewDeck();

        OrderRotate();
    }

    private void NewDeck()
    {
        var newDeck = DeckExtensions.NewDeck();
        newDeck.Shuffle();

        _deck.Cards = new Stack<Card>(newDeck);
    }

    private void OrderRotate()
    {
        var lastPlayer = Table.Players[Table.Players.Count - 1];
        for (int i = Table.Players.Count - 1; i > 0; i--)
        {
            Table.Players[i] = Table.Players[i - 1];
        }
        Table.Players[0] = lastPlayer;
    }
}