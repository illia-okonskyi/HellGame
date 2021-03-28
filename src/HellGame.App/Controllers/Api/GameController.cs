using HellEngine.Core.Exceptions;
using HellEngine.Core.Services.GameControl;
using HellEngine.Core.Services.Sessions;
using HellGame.App.Constants;
using HellGame.App.ViewModels.Api;
using HellGame.App.ViewModels.Api.Payload;
using HellGame.App.ViewModels.Api.Payload.Assets;
using HellGame.App.ViewModels.Api.Payload.Game;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace HellGame.App.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GameController : ApiControllerBase
    {
        private readonly ILogger<GameController> logger;
        private readonly IGameControlService gameControlService;

        public GameController(
            ILogger<GameController> logger,
            IGameControlService gameControlService)
        {
            this.logger = logger;
            this.gameControlService = gameControlService;
        }

        [HttpGet]
        public ActionResult<ApiResponse<GameStateResponse>> GameState(
            [FromHeader(Name = Defaults.SessionIdHeader)] Guid sessionId)
        {
            try
            {
                var state = gameControlService.GetCurrentGameState(sessionId);

                var transitions = state.Transitions
                    .Select(t => new Transition
                    {
                        Key = t.Key,
                        TextAssetKey = t.TextAssetKey,
                        IsEnabled = t.IsEnabled,
                        IsVisible = t.IsVisible
                    })
                    .ToList();
                var response = new GameStateResponse
                {
                    TextAssetKey = state.TextAssetKey,
                    ImageAssetKey = state.ImageAssetKey,
                    Transitions = transitions
                };

                return Ok(ApiResponse<GameStateResponse>.MakeSuccess(response));
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<GameStateResponse>.MakeError(ex));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EmptyPayload>>> StartGame(
            [FromHeader(Name = Defaults.SessionIdHeader)] Guid sessionId,
            ApiRequest<StartGameRequest> request,
            CancellationToken cancellationToken)
        {
            try
            {
                await gameControlService.StartGame(sessionId, request.Payload.UserName, cancellationToken);
                return Ok(ApiResponse<EmptyPayload>.MakeSuccess());
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<EmptyPayload>.MakeError(ex));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EmptyPayload>>> ExitGame(
            [FromHeader(Name = Defaults.SessionIdHeader)] Guid sessionId,
            CancellationToken cancellationToken)
        {
            try
            {
                await gameControlService.ExitGame(sessionId, cancellationToken);
                return Ok(ApiResponse<EmptyPayload>.MakeSuccess());
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<EmptyPayload>.MakeError(ex));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EmptyPayload>>> Transition(
            [FromHeader(Name = Defaults.SessionIdHeader)] Guid sessionId,
            ApiRequest<TransitionRequest> request,
            CancellationToken cancellationToken)
        {
            try
            {
                await gameControlService.Transition(sessionId, request.Payload.Key, cancellationToken);
                return Ok(ApiResponse<EmptyPayload>.MakeSuccess());
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<EmptyPayload>.MakeError(ex));
            }
        }

        [HttpPost]
        public ActionResult<ApiResponse<SaveGameResponse>> SaveGame(
            [FromHeader(Name = Defaults.SessionIdHeader)] Guid sessionId)
        {
            try
            {
                var fileData = gameControlService.SaveGame(sessionId);
                var response = new SaveGameResponse
                {
                    FileData = fileData
                };
                return Ok(ApiResponse<SaveGameResponse>.MakeSuccess(response));
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<SaveGameResponse>.MakeError(ex));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EmptyPayload>>> LoadGame(
            [FromHeader(Name = Defaults.SessionIdHeader)] Guid sessionId,
            ApiRequest<LoadGameRequest> request,
            CancellationToken cancellationToken)
        {
            try
            {
                await gameControlService.LoadGame(sessionId, request.Payload.FileData, cancellationToken);
                return Ok(ApiResponse<EmptyPayload>.MakeSuccess());
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<EmptyPayload>.MakeError(ex));
            }
        }
    }
}
