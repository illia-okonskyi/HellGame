using HellEngine.Core.Sdk.Models;
using HellEngine.Core.Services;
using HellEngine.Core.Services.Scripting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace HellGame.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : ControllerBase
    {
        private readonly ILogger<HelloWorldController> logger;
        private readonly IHelloWorlder helloWorlder;
        private readonly IScriptHost scriptHost;

        public HelloWorldController(
            ILogger<HelloWorldController> logger,
            IHelloWorlder helloWorlder,
            IScriptHost scriptHost)
        {
            this.logger = logger;
            this.helloWorlder = helloWorlder;
            this.scriptHost = scriptHost;
        }

        [HttpGet]
        public async Task<string> Get(CancellationToken cancellationToken)
        {
            var code1 = System.IO.File.ReadAllText("assets\\TestScript1.txt");
            var code2 = System.IO.File.ReadAllText("assets\\TestScript2.txt");
            var code3 = System.IO.File.ReadAllText("assets\\TestScript3.txt");

            var script1 = scriptHost.CreateScript("MyScript1", code1);
            var script2 = scriptHost.CreateScript<SumInput>("MyScript2", code2);
            var script3 = scriptHost.CreateScript<CombinedInput, CombinedOutput>("MyScript3", code3);

            await scriptHost.RunScript(script1, cancellationToken);
            await scriptHost.RunScript(script2, new SumInput { IntVal1 = 3, IntVal2 = 5 }, cancellationToken);
            var output = await scriptHost.RunScript(
                script3,
                new CombinedInput
                {
                    SumInput = new SumInput { IntVal1 = 77, IntVal2 = 88 },
                    ConcatInput = new ConcatInput { StrVal1 = "jjjj", StrVal2 = "mmmmm" }
                },
                cancellationToken);

            var logMessage =
                $"Sum = {output?.Sum}; Concat = {output?.Concat}; HelloWorld = {output?.HelloWorld}";

            logger.LogInformation(logMessage);
            return helloWorlder.GetHelloString();
        }
    }
}
