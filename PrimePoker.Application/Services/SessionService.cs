using PrimePoker.Application.Game;
using PrimePoker.Application.Interfaces;
using PrimePoker.Domain.Player;
using PrimePoker.Domain.Session.Enums;
using System.Collections.Concurrent;

namespace PrimePoker.Application.Services;

public class SessionService
{
    private Dictionary<(SessionType,SessionTimeType),ConcurrentQueue<SessionRequest>> _playersQueues = new();
    private List<Session> _sessions = new();
    private readonly ISessionNotifier _sessionNotifier;

    public SessionService(ISessionNotifier sessionNotifier)
    {
        _sessionNotifier = sessionNotifier;
    }

    public async Task AddToQueueAsync(SessionRequest sessionRequest)
    {
        var key = (sessionRequest.SessionType, sessionRequest.SessionTimeType);

        if (!_playersQueues.TryGetValue(key, out var queue))
        {
            queue = new ConcurrentQueue<SessionRequest>();
            _playersQueues[key] = queue;
        }

        _playersQueues[key].Enqueue(sessionRequest);

        var sessionConfiguration = sessionRequest.CreateConfiguration();

        if (_playersQueues[key].Count >= sessionConfiguration.MaxPlayer)
        {
            List<SessionRequest> players = new();
            for (int i = 0; i < sessionConfiguration.MaxPlayer; i++)
            {
                if (_playersQueues[key].TryDequeue(out SessionRequest? p))
                    players.Add(p);
            }

            var newSession = new Session(players, sessionConfiguration, _sessionNotifier);
            _ = Task.Run(()=>newSession.Run());

            _sessions.Add(newSession);
        }
    }

    public List<SessionRequest> GetPlayersQueue() => _playersQueues.Values
                                                        .SelectMany(queue => queue)
                                                        .ToList();

    public List<Session> GetSessions() => _sessions;

    public Session? GetSession(Guid id) => _sessions.FirstOrDefault(s => s.Id == id);
}