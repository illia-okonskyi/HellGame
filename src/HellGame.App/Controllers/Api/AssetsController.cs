using HellEngine.Core.Exceptions;
using HellEngine.Core.Services.Assets;
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
        private readonly IAssetsManager assetsManager;

        public AssetsController(
            ILogger<AssetsController> logger,
            IAssetsManager assetsManager)
        {
            this.logger = logger;
            this.assetsManager = assetsManager;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<GetAsset>>> Text(
            string key,
            CancellationToken cancellationToken)
        {
            try
            {
                var asset = await assetsManager.GetTextAsset(
                    key,
                    cancellationToken: cancellationToken);
                var response = ApiResponse<GetAsset>.MakeSuccess(
                    new GetAsset
                    {
                        Key = asset.Descriptor.Key,
                        Type = asset.Descriptor.AssetType,
                        Data = asset.Data
                    });
                return Ok(response);
            }
            catch (AssetException ex)
            {
                return BadRequest(ApiResponse<GetAsset>.MakeError(ex));
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<GetAsset>.MakeError(ex));
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<GetAsset>>> Image(
            string key,
            CancellationToken cancellationToken)
        {
            try
            {
                var asset = await assetsManager.GetImageAsset(
                    key,
                    cancellationToken: cancellationToken);
                var response = ApiResponse<GetAsset>.MakeSuccess(
                    new GetAsset
                    {
                        Key = asset.Descriptor.Key,
                        Type = asset.Descriptor.AssetType,
                        Data = asset.Data
                    });
                return Ok(response);
            }
            catch (AssetException ex)
            {
                return BadRequest(ApiResponse<GetAsset>.MakeError(ex));
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<GetAsset>.MakeError(ex));
            }
        }
    }
}
