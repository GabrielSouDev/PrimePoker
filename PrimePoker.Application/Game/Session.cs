using PrimePoker.Application.Interfaces;
using PrimePoker.Application.Game.Extensions;
using PrimePoker.Domain.Player;
using PrimePoker.Domain.Session;
using PrimePoker.Domain.Session.Enums;

namespace PrimePoker.Application.Game;
public partial class Session
{
    private readonly ISessionNotifier _sessionNotifier;
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get => $"Room {Id.ToString().Substring(24)} - {SessionConfiguration.TimeType} - {SessionConfiguration.MaxPlayer} Players"; }
    public Table Table { get; init; } = new();
    public SessionConfiguration SessionConfiguration { get; init; }
    private Deck _deck = new();

    public Session(IEnumerable<SessionRequest> players, SessionConfiguration sessionConfiguration, ISessionNotifier sessionNotifier)
    {
        _sessionNotifier = sessionNotifier;
        SessionConfiguration = sessionConfiguration;

        foreach (var player in players)
        {
            var newPlayer = new PlayerInGame()
            {
                Id = player.Id,
                Name = player.Name,
                Chips = sessionConfiguration.InitialChips,
                TimeBank = sessionConfiguration.InitialTimeBank
            };

            Table.Players.Add(newPlayer);
        }
    }

    public async Task Run()
    {
        while (Table.Players.Where(p => p.IsAlive).Count() > 1)
        {
            NewRound();
            await ActionTurn();

            await DrawPlayersCards();
            if (!RoundIsFinished())
                await ActionTurn();

            DrawFlop();
            if (!RoundIsFinished())
                await ActionTurn();

            DrawTurn();
            if (!RoundIsFinished())
                await ActionTurn();

            DrawRiver();
            if (!RoundIsFinished())
                await ActionTurn();

            Table.Players.DetermineRound(Table.CommunityCards);
        }
    }
}