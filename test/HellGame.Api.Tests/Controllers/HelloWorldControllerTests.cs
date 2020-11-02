using HellEngine.Core.Services;
using HellEngine.Core.Services.Scripting;
using HellGame.Api.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using Xunit;

namespace HellGame.Api.Tests.Controllers
{
    public class HelloWorldControllerTests
    {
        #region Context
        class TestCaseContext
        {
            #region Data
            #endregion

            #region Services
            public ILogger<HelloWorldController> Logger { get; }
            public IHelloWorlder HelloWorlder { get; }
            public IScriptHost ScriptHost { get; }
            #endregion

            #region Utils
            #endregion

            public TestCaseContext()
            {
                Logger = Mock.Of<ILogger<HelloWorldController>>();
                HelloWorlder = Mock.Of<IHelloWorlder>();
                ScriptHost = Mock.Of<IScriptHost>();
            }
        }
        #endregion

        [Fact]
        public void Get()
        {
            // Arrange
            var context = new TestCaseContext();
            var sut = new HelloWorldController(
                context.Logger,
                context.HelloWorlder,
                context.ScriptHost);

            // Act
            var result = sut.Get(CancellationToken.None);

            // Assert
            Mock.Get(context.HelloWorlder).Verify(
                m => m.GetHelloString(),
                Times.Once);
        }
    }
}
