using System.IO;
using System.Threading.Tasks;
using JoKenPo.Domain.Interfaces;
using JoKenPo.Domain.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JoKenPo.Api
{
    public class TurnHandler
    {
        public TurnHandler(ITurnService service)
        {
            Service = service;
        }

        private ITurnService Service { get; }

        [FunctionName("AddMove")]
        public async Task<IActionResult> AddMove([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "move")]
            HttpRequest req, ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<PlayerMove>(requestBody);
                if (data is null || data.SessionId == string.Empty)
                    return new BadRequestObjectResult("Session ID must be provided");
                var move = await Service.AddMove(data);
                return new CreatedResult("move",move);
            }
            catch (System.Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        [FunctionName("EndTurn")]
        public async Task<IActionResult> EndTurn([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "endTurn/{sessionId}")]
            HttpRequest request, string sessionId, ILogger log)
        {
            try
            {
                var result = await Service.EndTurn(sessionId);
                return new OkObjectResult(result);
            }
            catch (System.Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}