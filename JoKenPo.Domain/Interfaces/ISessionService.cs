using System.Threading.Tasks;

namespace JoKenPo.Domain.Interfaces
{
    public interface ISessionService
    {
        public Task<GameSession> InitializeSession();
        public Task<GameSession> RetrieveSession(string sessionId);
        public Task<GameSession> UpdateSession(GameSession session);

    }
}