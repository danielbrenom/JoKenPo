using System.Threading.Tasks;
using JoKenPo.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace JoKenPo.Api
{
    public class SessionHandler
    {
        public SessionHandler(ISessionService service)
        {
            Service = service;
        }

        private ISessionService Service { get; }

        [FunctionName("StartSession")]
        public async Task<IActionResult> Init([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "game")]
            HttpRequest req, ILogger log)
        {
            var session = await Service.InitializeSession();
            return new OkObjectResult(session);
        }
        
        [FunctionName("ListSession")]
        public async Task<IActionResult> List([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "game/{sessionId}")]
            HttpRequest req, string sessionId, ILogger log)
        {
            var session = await Service.RetrieveSession(sessionId);
            return new OkObjectResult(session);
        }
    }
}