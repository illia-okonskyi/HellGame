using HellEngine.Core.Models;
using HellEngine.Core.Services.Sessions;
using HellGame.App.Controllers.Api;
using HellGame.App.ViewModels.Api;
using HellGame.App.ViewModels.Api.Payload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace HellGame.App.Tests.Controllers.Api
{
    public class PingControllerTests
    {
        #region Context
        class TestCaseContext
        {
            #region Data
            public Session Session { get; }
            #endregion

            #region Services
            public ILogger<PingController> Logger { get; }
            public ISessionManager SessionManager { get; }
            #endregion

            #region Utils
            #endregion

            public TestCaseContext()
            {
                Session = new Session
                {
                    Id = Guid.NewGuid()
                };

                Logger = Mock.Of<ILogger<PingController>>();
                SessionManager = Mock.Of<ISessionManager>();
            }
        }
        #endregion

        [Fact]
        public void Post_Success()
        {
            // Arrange
            var context = new TestCaseContext();
            var sut = new PingController(
                context.Logger,
                context.SessionManager);

            // Act
            var actionResult = sut.Post(context.Session.Id);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<EmptyPayload>;
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            var payload = result.Payload;
            Assert.Null(payload);

            Mock.Get(context.SessionManager).Verify(
                m => m.PingSession(context.Session.Id),
                Times.Once);
        }

        [Fact]
        public void Post_Error()
        {
            // Arrange
            var context = new TestCaseContext();
            var sut = new PingController(
                context.Logger,
                context.SessionManager);

            Mock.Get(context.SessionManager).Setup(
                m => m.PingSession(context.Session.Id))
                .Throws(new Exception("message"));

            // Act
            var actionResult = sut.Post(context.Session.Id);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<EmptyPayload>;
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Null(result.Payload);
        }
    }
}
