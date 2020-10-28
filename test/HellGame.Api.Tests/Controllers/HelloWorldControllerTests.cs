using HellEngine.Core.Services;
using HellGame.Api.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
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
            #endregion

            #region Utils
            #endregion

            public TestCaseContext()
            {
                Logger = Mock.Of<ILogger<HelloWorldController>>();
                HelloWorlder = Mock.Of<IHelloWorlder>();
            }
        }
        #endregion

        [Fact]
        public void Get()
        {
            // Arrange
            var context = new TestCaseContext();
            var sut = new HelloWorldController(context.Logger, context.HelloWorlder);

            // Act
            var result = sut.Get();

            // Assert
            Mock.Get(context.HelloWorlder).Verify(
                m => m.GetHelloString(),
                Times.Once);
        }
    }
}
