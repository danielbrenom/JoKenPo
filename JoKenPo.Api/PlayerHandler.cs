using System.IO;
using System.Threading.Tasks;
using JoKenPo.Domain.Interfaces;
using JoKenPo.Domain.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JoKenPo.Api
{
    public class PlayerHandler
    {
        private IPlayerService Service { get; }

        public PlayerHandler(IPlayerService service)
        {
            Service = service;
        }

        [FunctionName("ListPlayers")]
        public async Task<IActionResult> List(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "player/{sessionId}")]
            HttpRequest request, string sessionId, ILogger log)
        {
            if (sessionId is null || sessionId == string.Empty)
                return new BadRequestObjectResult("Session ID must be provided");
            var players = await Service.ListPlayers(sessionId);
            return new OkObjectResult(players);
        }


        [FunctionName("AddPlayer")]
        public async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "player")]
            HttpRequest request, ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<PlayerRequest>(requestBody);
                if (data is null || data.SessionId == string.Empty)
                    return new BadRequestObjectResult("Session ID must be provided");
                var player = await Service.AddPlayer(data.SessionId, data);
                return new CreatedResult("player", player);
            }
            catch (System.Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        [FunctionName("RemovePlayer")]
        public async Task<IActionResult> Remove(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "player/{playerId}")]
            HttpRequest request, string playerId, ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<PlayerRequest>(requestBody);
                if (data is null || data.SessionId == string.Empty)
                    return new BadRequestObjectResult("Session ID must be provided");
                var player = await Service.RemovePlayer(data.SessionId, data);
                return new OkObjectResult(player);
            }
            catch (System.Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}