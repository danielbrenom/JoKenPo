using System.Collections.Generic;
using JoKenPo.Domain.Models;

namespace JoKenPo.Domain
{
    public class GameSession
    {
        public string SessionId { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public List<PlayerTurn> PlayerMoves { get; set; } = new List<PlayerTurn>();
        public List<PlayerTurn> CurrentPlayerMoves { get; set; } = new List<PlayerTurn>();
        public int CurrentIteration { get; set; }
    }
}