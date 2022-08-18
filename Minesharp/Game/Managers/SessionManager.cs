using System.Collections.Concurrent;
using Minesharp.Network;

namespace Minesharp.Game.Managers;

public class SessionManager
{
    private readonly ConcurrentDictionary<Guid, NetworkSession> sessions = new();

    public void Add(NetworkSession session)
    {
        sessions[session.Id] = session;
    }

    public void Remove(NetworkSession session)
    {
        sessions.Remove(session.Id, out _);
    }

    public IEnumerable<NetworkSession> GetSessions()
    {
        return sessions.Values;
    }
}