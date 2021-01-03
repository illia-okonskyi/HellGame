using HellEngine.Core.Models.Vars;
using HellEngine.Core.Services.Sessions;
using HellGame.App.Constants;
using HellGame.App.ViewModels.Api;
using HellGame.App.ViewModels.Api.Payload.Vars;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HellGame.App.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class VarsController : ApiControllerBase
    {
        private readonly ILogger<VarsController> logger;
        private readonly ISessionManager sessionManager;

        public VarsController(
            ILogger<VarsController> logger,
            ISessionManager sessionManager)
        {
            this.logger = logger;
            this.sessionManager = sessionManager;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<GetVarsResponse>>> Get(
            [FromHeader(Name = Defaults.SessionIdHeader)] Guid sessionId,
            CancellationToken cancellationToken)
        {
            try
            {
                var session = sessionManager.GetSession(sessionId);
                var locale = session.LocaleManager.GetLocale();
                var vars = session.VarsManager.GetAllVars();

                var response = new GetVarsResponse();
                foreach (var aVar in vars)
                {
                    response.Vars.Add(new GameVar
                    {
                        Name = (await session.AssetsManager.GetTextAsset(
                            aVar.NameAssetKey,
                            locale,
                            cancellationToken)).Data,
                        Value = aVar.DisplayString
                    });
                }
                return Ok(ApiResponse<GetVarsResponse>.MakeSuccess(response));
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<GetVarsResponse>.MakeError(ex));
            }
        }
    }
}
