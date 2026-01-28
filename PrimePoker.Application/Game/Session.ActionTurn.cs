using PrimePoker.Domain.Session;
using PrimePoker.Domain.Session.Enums;

namespace PrimePoker.Application.Game;
public partial class Session
{
    private async Task ActionTurn()
    {
        Table.HasRaiseOrAllWin = false;

        int startIndex = Table.RoundState == RoundState.PreFlop ? 0 : Table.Players.Count - 2;

        for (int i = 0; i < Table.Players.Count; i++)
        {
            int index = (startIndex + i) % Table.Players.Count;
            var currentPlayer = Table.Players[index];

            if (currentPlayer.IsRoundAlive)
            {
                await RequestPlayerAction(currentPlayer);
            }
        }

        Console.WriteLine();
    }

    private async Task RequestPlayerAction(PlayerInGame currentPlayer)
    {
        var timeout = Task.Delay(TimeSpan.FromSeconds(SessionConfiguration.ActionTime));
        var playerResponse = WaitForPlayerResponse(currentPlayer);

        var completedTask = await Task.WhenAny(timeout, playerResponse);

        if (completedTask == playerResponse)
        {
            ProcessPlayerAction(currentPlayer);
        }
        else
        {
            HandleTimeout(currentPlayer);
        }
    }

    private async Task WaitForPlayerResponse(PlayerInGame currentPlayer)
    {
        Console.WriteLine($"{currentPlayer.GetConnectionId()} - Esperando - {currentPlayer.Name} - {Table.Timmer}");
        await Task.Delay(TimeSpan.FromSeconds(1)); 
    }

    private void ProcessPlayerAction(PlayerInGame currentPlayer)
    {
        Console.WriteLine($"{currentPlayer.GetConnectionId()} - {currentPlayer.Name} - Response - {Table.Timmer}");
    }

    private void HandleTimeout(PlayerInGame currentPlayer)
    {
        Console.WriteLine($"{currentPlayer.GetConnectionId()} - {currentPlayer.Name} - Fold - {Table.Timmer}");
    }
}
