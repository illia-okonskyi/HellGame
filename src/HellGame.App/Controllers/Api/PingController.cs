using HellEngine.Core.Services.Sessions;
using HellGame.App.Constants;
using HellGame.App.ViewModels.Api;
using HellGame.App.ViewModels.Api.Payload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace HellGame.App.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ApiControllerBase
    {
        private readonly ILogger<PingController> logger;
        private readonly ISessionManager sessionManager;

        public PingController(
            ILogger<PingController> logger,
            ISessionManager sessionManager)
        {
            this.logger = logger;
            this.sessionManager = sessionManager;
        }

        [HttpPost]
        public ActionResult<ApiResponse<EmptyPayload>> Post(
            [FromHeader(Name = Defaults.SessionIdHeader)] Guid sessionId)
        {
            try
            {
                sessionManager.PingSession(sessionId);
                return Ok(ApiResponse<EmptyPayload>.MakeSuccess());
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<EmptyPayload>.MakeError(ex));
            }
        }
    }
}
