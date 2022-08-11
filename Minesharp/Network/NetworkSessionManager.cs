using System.Collections.Concurrent;

namespace Minesharp.Network;

public class NetworkSessionManager
{
    private readonly ConcurrentDictionary<Guid, NetworkSession> sessions = new();

    public void AddSession(NetworkSession session)
    {
        sessions[session.Id] = session;
    }

    public void RemoveSession(NetworkSession session)
    {
        sessions.Remove(session.Id, out _);
    }

    public IEnumerable<NetworkSession> GetSessions()
    {
        return sessions.Values;
    }
}