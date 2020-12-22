using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JoKenPo.Domain.Interfaces;
using JoKenPo.Domain.Models;

namespace JoKenPo.Domain.Services
{
    public class PlayerService : IPlayerService
    {
        public PlayerService(ISessionService sessionService)
        {
            SessionService = sessionService;
        }

        private ISessionService SessionService { get; }
        public async Task<IReadOnlyCollection<Player>> ListPlayers(string sessionId)
        {
            var session = await SessionService.RetrieveSession(sessionId);
            return session?.Players;
        }

        public async Task<Player> AddPlayer(string sessionId, Player player)
        {
            var session = await SessionService.RetrieveSession(sessionId);
            if (session is null)
                throw new System.NullReferenceException("Session doesn't exist");
            if (session.Players.Any(players => players.Id == player.Id || players.Name.Equals(player.Name)))
                return null;
            if(player.Id is null || player.Id == string.Empty)
                player.Id = System.Guid.NewGuid().ToString("N");
            session.Players.Add(player);
            await SessionService.UpdateSession(session);
            return player;
        }

        public async Task<Player> RemovePlayer(string sessionId ,Player player)
        {
            var session = await SessionService.RetrieveSession(sessionId);
            if (session is null)
                throw new System.NullReferenceException("Session doesn't exist");
            if (session.Players.All(players => players.Id != player.Id))
                return null;
            var playerToRemove = session.Players.First(players => players.Id == player.Id);
            session.Players.Remove(playerToRemove);
            await SessionService.UpdateSession(session);
            return playerToRemove;
        }
    }
}