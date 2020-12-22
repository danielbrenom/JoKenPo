using System.Collections.Generic;
using System.Threading.Tasks;
using JoKenPo.Domain.Models;

namespace JoKenPo.Domain.Interfaces
{
    public interface IPlayerService
    {
        public Task<IReadOnlyCollection<Player>> ListPlayers(string sessionId);
        public Task<Player> AddPlayer(string sessionId, Player player);
        public Task<Player> RemovePlayer(string sessionId, Player player);
    }
}