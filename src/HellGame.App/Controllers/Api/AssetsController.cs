using HellEngine.Core.Exceptions;
using HellEngine.Core.Services.Sessions;
using HellGame.App.Constants;
using HellGame.App.ViewModels.Api;
using HellGame.App.ViewModels.Api.Payload.Assets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HellGame.App.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AssetsController : ApiControllerBase
    {
        private readonly ILogger<AssetsController> logger;
        private readonly ISessionManager sessionManager;

        public AssetsController(
            ILogger<AssetsController> logger,
            ISessionManager sessionManager)
        {
            this.logger = logger;
            this.sessionManager = sessionManager;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<TextAssetResponse>>> Text(
            [FromHeader(Name = Defaults.SessionIdHeader)] Guid sessionId,
            [FromQuery] ApiRequest<GetAssetRequest> request,
            CancellationToken cancellationToken)
        {
            try
            {
                var session = sessionManager.GetSession(sessionId);

                var locale = session.LocaleManager.GetLocale();
                var asset = await session.AssetsManager.GetTextAsset(
                    request.Payload.Key,
                    locale,
                    cancellationToken);

                var response = new TextAssetResponse
                {
                    Key = asset.Descriptor.Key,
                    Data = asset.Data
                };
                return Ok(ApiResponse<TextAssetResponse>.MakeSuccess(response));
            }
            catch (AssetException ex)
            {
                return BadRequest(ApiResponse<TextAssetResponse>.MakeError(ex));
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<TextAssetResponse>.MakeError(ex));
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<ImageAssetResponse>>> Image(
            [FromHeader(Name = Defaults.SessionIdHeader)] Guid sessionId,
            [FromQuery] ApiRequest<GetAssetRequest> request,
            CancellationToken cancellationToken)
        {
            try
            {
                var session = sessionManager.GetSession(sessionId);

                var locale = session.LocaleManager.GetLocale();
                var asset = await session.AssetsManager.GetImageAsset(
                    request.Payload.Key,
                    locale,
                    cancellationToken);

                var response = new ImageAssetResponse
                {
                    Key = asset.Descriptor.Key,
                    MediaType = asset.Descriptor.MediaType,
                    SourceUrl = asset.Descriptor.SourceUrl,
                    Data = asset.Data
                };
                return Ok(ApiResponse<ImageAssetResponse>.MakeSuccess(response));
            }
            catch (AssetException ex)
            {
                return BadRequest(ApiResponse<ImageAssetResponse>.MakeError(ex));
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<ImageAssetResponse>.MakeError(ex));
            }
        }
    }
}
