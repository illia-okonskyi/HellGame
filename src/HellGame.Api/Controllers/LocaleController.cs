using HellEngine.Core.Services.Assets;
using HellGame.Api.ViewModels;
using HellGame.Api.ViewModels.ApiPayload.Locale;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace HellGame.Api.Controllers
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
        public ActionResult<ApiResponse<GetLocale>> Get()
        {
            try
            {
                var currentLocale = assetsManager.GetLocale();
                var response = ApiResponse<GetLocale>.MakeSuccess(
                    new GetLocale { Locale = currentLocale });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<GetLocale>.MakeError(ex));
            }
        }

        [HttpPost]
        public ActionResult<ApiResponse<GetLocale>> Post(ApiRequest<SetLocale> request)
        {
            try
            {
                assetsManager.SetLocale(request.Payload.Locale);
                var currentLocale = assetsManager.GetLocale();
                var response = ApiResponse<GetLocale>.MakeSuccess(
                    new GetLocale { Locale = currentLocale });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ApiError(ApiResponse<GetLocale>.MakeError(ex));
            }
        }
    }
}
