using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using JoKenPo.Domain;
using JoKenPo.Domain.Enum;
using JoKenPo.Domain.Interfaces;
using JoKenPo.Domain.Models;
using JoKenPo.Domain.Models.Requests;
using Newtonsoft.Json;

namespace JoKenPo.ViewModels
{
    public class GameViewModel
    {
        private IHttpService HttpService { get; }
        private IMessageService MessageService { get; }
        private string GameSessionId { get; set; }

        public GameViewModel(IHttpService httpService, IMessageService messageService)
        {
            HttpService = httpService;
            MessageService = messageService;
        }

        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        public ObservableCollection<PlayerTurn> CurrentMoves { get; set; } = new ObservableCollection<PlayerTurn>();
        public ObservableCollection<PlayerTurn> GameMoves { get; set; } = new ObservableCollection<PlayerTurn>();
        public ObservableCollection<TurnResult> TurnResults { get; set; } = new ObservableCollection<TurnResult>();
        public Player EditPlayer { get; set; } = new Player();
        public bool SessionBegun { get; set; }
        public bool AddingPlayer { get; set; }
        public int GameTurn { get; set; }

        public async Task InitGame()
        {
            try
            {
                var session = await HttpService.Get("/game").ExecuteAsync<GameSession>();
                GameSessionId = session.SessionId;
                GameTurn = session.CurrentIteration + 1;
                SessionBegun = true;
            }
            catch (System.Exception e)
            {
                await MessageService.ShowError(e.Message);
            }
        }

        public async Task AddPlayer()
        {
            try
            {
                if (!SessionBegun)
                    throw new System.ArgumentNullException(nameof(GameSessionId), "Session not initialized");
                var player = new PlayerRequest
                {
                    SessionId = GameSessionId,
                    Name = EditPlayer.Name
                };
                var result = await HttpService.Post("/player")
                                              .AddBody(string.Empty, JsonConvert.SerializeObject(player))
                                              .ExecuteAsync<Player>();
                if (result != null)
                    Players.Add(result);
                AddingPlayer = !AddingPlayer;
            }
            catch (System.Exception e)
            {
                await MessageService.ShowError(e.Message);
            }
        }

        public async Task RemovePlayer(Player player)
        {
            try
            {
                var remove = new PlayerRequest
                {
                    SessionId = GameSessionId,
                    Id = player.Id
                };
                var result = await HttpService.Delete($"/player/{player.Id}")
                                              .AddBody(string.Empty, JsonConvert.SerializeObject(remove))
                                              .ExecuteAsync<Player>();
                if (result != null)
                    Players.Remove(player);
            }
            catch (System.Exception e)
            {
                await MessageService.ShowError(e.Message);
            }
        }

        public async Task MakeMove(Player player, HandSymbol move)
        {
            try
            {
                var playerMove = new PlayerMove
                {
                    Move = move,
                    PlayerId = player.Id,
                    SessionId = GameSessionId
                };
                var result = await HttpService.Post("/move")
                                              .AddBody(string.Empty, JsonConvert.SerializeObject(playerMove))
                                              .ExecuteAsync<GameSession>();
                CurrentMoves.Clear();
                result.CurrentPlayerMoves.ForEach(CurrentMoves.Add);
            }
            catch (System.Exception e)
            {
                await MessageService.ShowError(e.Message);
            }
        }

        public async Task CheckTurn()
        {
            try
            {
                var result = await HttpService.Get($"/endTurn/{GameSessionId}")
                                              .ExecuteAsync<TurnResult>();
                GameTurn++;
                TurnResults.Add(result);
                CurrentMoves.ToList().ForEach(GameMoves.Add);
                CurrentMoves.Clear();
            }
            catch (System.Exception e)
            {
                await MessageService.ShowError(e.Message);
            }
        }

        public async Task RestartGame()
        {
            try
            {
                var session = await HttpService.Get("/game").ExecuteAsync<GameSession>();
                GameSessionId = session.SessionId;
                Players.Clear();
                CurrentMoves.Clear();
                GameMoves.Clear();
                TurnResults.Clear();
                SessionBegun = true;
                GameTurn = 1;
            }
            catch (System.Exception e)
            {
                await MessageService.ShowError(e.Message);
            }
        }
    }
}