using HellEngine.Core.Services.Sessions;
using HellGame.App.Constants;
using HellGame.App.ViewModels.Api;
using HellGame.App.ViewModels.Api.Payload.Locale;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace HellGame.App.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocaleController : ApiControllerBase
    {
        private readonly ILogger<LocaleController> logger;
        private readonly ISessionManager sessionManager;

        public LocaleController(
            ILogger<LocaleController> logger,
            ISessionManager sessionManager)
        {
            this.logger = logger;
            this.sessionManager = sessionManager;
        }

        [HttpGet]
        public ActionResult<ApiResponse<GetLocaleResponse>> Get(
            [FromHeader(Name = Defaults.SessionIdHeader)] Guid sessionId)
        {
            try
            {
                var session = sessionManager.GetSession(sessionId);

                var locale = session.LocaleManager.GetLocale();

                var response = new GetLocaleResponse { Locale = locale };
                return Ok(ApiResponse<GetLocaleResponse>.MakeSuccess(response));
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<GetLocaleResponse>.MakeError(ex));
            }
        }

        [HttpPost]
        public ActionResult<ApiResponse<GetLocaleResponse>> Post(
            [FromHeader(Name = Defaults.SessionIdHeader)] Guid sessionId,
            ApiRequest<SetLocaleRequest> request)
        {
            try
            {
                var session = sessionManager.GetSession(sessionId);

                session.LocaleManager.SetLocale(request.Payload.Locale);
                var locale = session.LocaleManager.GetLocale();
                
                var response = new GetLocaleResponse { Locale = locale };
                return Ok(ApiResponse<GetLocaleResponse>.MakeSuccess(response));
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<GetLocaleResponse>.MakeError(ex));
            }
        }
    }
}
