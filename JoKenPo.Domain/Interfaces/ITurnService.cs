using System.Threading.Tasks;
using JoKenPo.Domain.Models;
using JoKenPo.Domain.Models.Requests;

namespace JoKenPo.Domain.Interfaces
{
    public interface ITurnService
    {
        public Task<GameSession> AddMove(PlayerMove move);
        public Task<TurnResult> EndTurn(string sessionId);
    }
}