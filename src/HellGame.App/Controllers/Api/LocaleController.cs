using HellEngine.Core.Services.Assets;
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
        private readonly IAssetsManager assetsManager;

        public LocaleController(
            ILogger<LocaleController> logger,
            IAssetsManager assetsManager)
        {
            this.logger = logger;
            this.assetsManager = assetsManager;
        }

        [HttpGet]
        public ActionResult<ApiResponse<GetLocaleResponse>> Get([FromRoute]ApiRequest<GetLocaleRequest> request)
        {
            try
            {
                var currentLocale = assetsManager.GetLocale();
                var response = ApiResponse<GetLocaleResponse>.MakeSuccess(
                    new GetLocaleResponse { Locale = currentLocale });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<GetLocaleResponse>.MakeError(ex));
            }
        }

        [HttpPost]
        public ActionResult<ApiResponse<GetLocaleResponse>> Post(ApiRequest<SetLocaleRequest> request)
        {
            try
            {
                assetsManager.SetLocale(request.Payload.Locale);
                var currentLocale = assetsManager.GetLocale();
                var response = ApiResponse<GetLocaleResponse>.MakeSuccess(
                    new GetLocaleResponse { Locale = currentLocale });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<GetLocaleResponse>.MakeError(ex));
            }
        }
    }
}
