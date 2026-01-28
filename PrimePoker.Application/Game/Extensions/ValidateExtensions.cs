using PrimePoker.Domain.Session;
using PrimePoker.Domain.Session.Enums;
using System.Linq;
using System.Numerics;

namespace PrimePoker.Application.Game.Extensions;

public static class ValidateExtensions
{
    public static void DetermineRound(this IEnumerable<PlayerInGame> players, IEnumerable<Card> communityCards)
    {
        var playersAlive = players.Where(p => p.IsAlive).ToList();

        if (playersAlive.Count == 1)
        {
            // Apenas um jogador vivo → ele ganha o pote inteiro
            var winner = playersAlive.First();
            winner.Chips += players.Sum(p => p.ChipsInPot);
        }
        else
        {
            // Calcula poder de cada jogador
            Dictionary<Guid, (IEnumerable<Card>, HandLevel)> playersPower = new();
            foreach (var p in playersAlive)
            {
                var pPower = p.CalcPower(communityCards);
                playersPower.Add(p.GetConnectionId(), pPower);
            }

            Console.WriteLine("----------------------");
            Console.WriteLine($"Total de Mãos Finais: {playersPower.Count}");
            foreach (var PlayerPower in playersPower)
            {
                Console.WriteLine($"Player {PlayerPower.Key}:");
                foreach (var card in PlayerPower.Value.Item1)
                {
                    Console.Write($"| {card.Rank}/{card.Suit} | ");
                }
                Console.WriteLine();
                Console.WriteLine($" {PlayerPower.Value.Item2.ToString()}");
                
                Console.WriteLine("----------------------");
                Console.WriteLine();
            }

            // Ordena por nível da mão
            var bestHands = playersPower
                .OrderByDescending(p => p.Value.Item2)
                .ToList();

            // TODO: tratar empates corretamente (split pot)
            var winnerId = bestHands.First().Key;
            var winner = playersAlive.First(p => p.GetConnectionId() == winnerId);
            winner.Chips += players.Sum(p => p.ChipsInPot);
        }

        // Zera o pote
        foreach (var player in players)
        {
            player.ChipsInPot = 0;
        }
    }

    private static (IEnumerable<Card>, HandLevel) CalcPower(this PlayerInGame player, IEnumerable<Card> communityCards)
    {
        var allCards = player.GetHand().Cards.Concat(communityCards).ToList();

        var candidateHands = allCards.Combinations(5).Select(c => c.ToList()).ToList();

        var candidateHandsWPower = candidateHands
            .Select(c => c.GetPower())
            .ToList();

        // Retorna a melhor mão
        return candidateHandsWPower
            .OrderByDescending(h => h.Item2)
            .First();
    }

    private static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source, int length)
    {
        if (length == 0) return new[] { Enumerable.Empty<T>() };

        return source.SelectMany((item, index) =>
            Combinations(source.Skip(index + 1), length - 1)
                .Select(result => new[] { item }.Concat(result)));
    }

    private static (IEnumerable<Card>, HandLevel) GetPower(this IEnumerable<Card> candidateHands)
    {
        var rankGroups = candidateHands
            .GroupBy(c => c.Rank)
            .ToDictionary(g => g.Key, g => (g.Count(), g.AsEnumerable()));

        var suitGroups = candidateHands
            .GroupBy(c => c.Suit)
            .ToDictionary(g => g.Key, g => (g.Count(), g.AsEnumerable()));

        var orderedRanks = candidateHands
            .OrderByDescending(c => c.Rank)
            .Select(c => c.Rank)
            .ToList();

        bool straight = orderedRanks
            .Zip(orderedRanks.Skip(1), (a, b) => a - 1 == b)
            .All(x => x);

        var flush = suitGroups.Values.Any(v => v.Item1 == 5);

        // Royal Flush
        if (flush && straight && orderedRanks.First() == Rank.Ace)
            return (candidateHands.OrderByDescending(c => c.Rank), HandLevel.RoyalFlush);

        // Straight Flush
        if (flush && straight)
            return (candidateHands.OrderByDescending(c => c.Rank), HandLevel.StraightFlush);

        // Four of a Kind
        if (rankGroups.Values.Any(v => v.Item1 == 4))
            return (candidateHands.OrderByDescending(c => c.Rank), HandLevel.FourOfAKind);

        // Full House
        if (rankGroups.Values.Any(v => v.Item1 == 3) && rankGroups.Values.Any(v => v.Item1 == 2))
            return (candidateHands.OrderByDescending(c => c.Rank), HandLevel.FullHouse);

        // Flush
        if (flush)
            return (candidateHands.OrderByDescending(c => c.Rank), HandLevel.Flush);

        // Straight
        if (straight)
            return (candidateHands.OrderByDescending(c => c.Rank), HandLevel.Straight);

        // Three of a Kind
        if (rankGroups.Values.Any(v => v.Item1 == 3))
            return (candidateHands.OrderByDescending(c => c.Rank), HandLevel.ThreeOfAKind);

        // Two Pair
        if (rankGroups.Values.Count(v => v.Item1 == 2) == 2)
            return (candidateHands.OrderByDescending(c => c.Rank), HandLevel.TwoPair);

        // Pair
        if (rankGroups.Values.Any(v => v.Item1 == 2))
            return (candidateHands.OrderByDescending(c => c.Rank), HandLevel.Pair);

        // High Card
        return (candidateHands.OrderByDescending(c => c.Rank), HandLevel.HighCard);
    }
}
