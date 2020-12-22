using System.Linq;
using System.Threading.Tasks;
using JoKenPo.Domain.Helper;
using JoKenPo.Domain.Interfaces;
using JoKenPo.Domain.Models;
using JoKenPo.Domain.Models.Requests;

namespace JoKenPo.Domain.Services
{
    public class TurnService : ITurnService
    {
        public TurnService(ISessionService sessionService)
        {
            SessionService = sessionService;
        }

        private ISessionService SessionService { get; }

        public async Task<GameSession> AddMove(PlayerMove move)
        {
            var session = await SessionService.RetrieveSession(move.SessionId);
            if (session is null)
                throw new System.NullReferenceException("Session doesn't exist");
            if (session.Players.All(player => player.Id != move.PlayerId))
                throw new System.NullReferenceException("Player doesn't exist");
            if (session.CurrentPlayerMoves.Any(playerMove => playerMove.PlayerId == move.PlayerId))
            {
                var moveToRemove = session.CurrentPlayerMoves.First(playerMove => move.PlayerId == playerMove.PlayerId);
                session.CurrentPlayerMoves.Remove(moveToRemove);
            }

            session.CurrentPlayerMoves.Add(new PlayerTurn {PlayerId = move.PlayerId, PlayerMove = move.Move});
            return session;
        }

        public async Task<TurnResult> EndTurn(string sessionId)
        {
            var session = await SessionService.RetrieveSession(sessionId);
            if (session is null)
                throw new System.NullReferenceException("Session doesn't exist");
            var playerHasWon = new bool[session.Players.Count];
            var playerOrder = new string[session.Players.Count];
            var currentPlayer = 0;
            if(!session.CurrentPlayerMoves.Any())
                throw new System.Exception("Turn has no moves");
            session.CurrentPlayerMoves.ForEach(attackerMove =>
            {
                var defenders = session.CurrentPlayerMoves.ToList();
                defenders.RemoveAt(currentPlayer);
                if (defenders.Count < 1)
                {
                    playerHasWon[currentPlayer] = true;
                    playerOrder[currentPlayer] = attackerMove.PlayerId;
                    return;
                }

                var player = currentPlayer;
                defenders.ForEach(defenderMove =>
                {
                    if (playerHasWon[player])
                        return;
                    playerHasWon[player] = GameLogic.CheckWin(attackerMove.PlayerMove, defenderMove.PlayerMove);
                    playerOrder[player] = attackerMove.PlayerId;
                });
                currentPlayer++;
            });
            var result = new TurnResult {TurnResults = playerHasWon};
            var winner = string.Empty;
            var draw = false;
            for (var j = 0; j < playerHasWon.Length; j++)
            {
                if (playerHasWon[j] && winner != string.Empty)
                {
                    winner += $", {session.Players.First(pl=> pl.Id == playerOrder[j]).Name}";
                    draw = true;
                }

                if (playerHasWon[j] && winner == string.Empty)
                    winner += session.Players.First(pl=> pl.Id == playerOrder[j]).Name;
            }


            await ClearTurn(session);
            if (winner == string.Empty)
                result.Annoucement = "No winners/draw";
            else
                result.Annoucement = draw ? $"Turn was a draw between players {winner}" : $"Player {winner} has won";
            return result;
        }

        private async Task ClearTurn(GameSession session)
        {
            session.PlayerMoves.AddRange(session.CurrentPlayerMoves);
            session.CurrentPlayerMoves.Clear();
            session.CurrentIteration++;
            await SessionService.UpdateSession(session);
        }
    }
}