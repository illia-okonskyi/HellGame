using HellEngine.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HellGame.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : ControllerBase
    {
        private readonly ILogger<HelloWorldController> logger;
        private readonly IHelloWorlder helloWorlder;
        
        public HelloWorldController(
            ILogger<HelloWorldController> logger,
            IHelloWorlder helloWorlder)
        {
            this.logger = logger;
            this.helloWorlder = helloWorlder;
        }

        [HttpGet]
        public string Get()
        {
            logger.LogInformation("Hello world request received");
            return helloWorlder.GetHelloString();
        }
    }
}
