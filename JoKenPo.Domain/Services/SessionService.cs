using System;
using System.Threading.Tasks;
using JoKenPo.Domain.Interfaces;

namespace JoKenPo.Domain.Services
{
    public class SessionService : ISessionService
    {
        private ICache Cache { get; }

        public SessionService(ICache cache)
        {
            Cache = cache;
        }

        public Task<GameSession> InitializeSession()
        {
            return Task.Run(() =>
            {
                var session = new GameSession
                {
                    SessionId = System.Guid.NewGuid().ToString("N")
                };
                Cache.AddUpdateCache(session.SessionId, session, TimeSpan.FromHours(1));
                return session;
            });
        }

        public Task<GameSession> RetrieveSession(string sessionId)
        {
            return Task.Run(() => Cache.GetCache<GameSession>(sessionId));
        }

        public Task<GameSession> UpdateSession(GameSession session)
        {
            return Task.Run(() =>
            {
                Cache.AddUpdateCache(session.SessionId, session, TimeSpan.FromHours(1));
                return session;
            });
        }
    }
}