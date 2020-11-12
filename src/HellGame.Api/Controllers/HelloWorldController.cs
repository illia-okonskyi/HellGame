using HellEngine.Core.Sdk.Models;
using HellEngine.Core.Services.Assets;
using HellEngine.Core.Services.Scripting;
using HellGame.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace HellGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloWorldController : ApiControllerBase
    {
        private readonly IScriptHost scriptHost;
        private readonly IAssetsManager assetsManager;

        public HelloWorldController(
            IScriptHost scriptHost,
            IAssetsManager assetsManager)
        {
            this.scriptHost = scriptHost;
            this.assetsManager = assetsManager;
        }

        [HttpGet]
        public async Task<HelloWorldViewModel> Get(CancellationToken cancellationToken)
        {
            assetsManager.SetLocale("en-us");

            var textAsset = await assetsManager.GetTextAsset(
                "_test.text",
                cancellationToken: cancellationToken);
            var imageAsset = await assetsManager.GetImageAsset(
                "_test.image",
                cancellationToken: cancellationToken);
            var scriptAsset = await assetsManager.GetScriptAsset(
                "_test.script",
                cancellationToken: cancellationToken);

            var code = scriptAsset.Data;
            var script = scriptHost.CreateScript<CombinedInput, CombinedOutput>(
                scriptAsset.Descriptor.Key, code);
            var output = await scriptHost.RunScript(
                script,
                new CombinedInput
                {
                    SumInput = new SumInput { IntVal1 = 77, IntVal2 = 88 },
                    ConcatInput = new ConcatInput { StrVal1 = "jjjj", StrVal2 = "mmmmm" }
                },
                cancellationToken);

            var scriptOutput =
                $"Sum = {output?.Sum}; Concat = {output?.Concat}; HelloWorld = {output?.HelloWorld}";

            return new HelloWorldViewModel
            {
                TextAssetData = textAsset.Data,
                ImageAssetData = imageAsset.Data,
                ScriptOutput = scriptOutput
            };
        }
    }
}
