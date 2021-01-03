using HellEngine.Core.Models;
using HellEngine.Core.Services.Locale;
using HellEngine.Core.Services.Sessions;
using HellGame.App.Controllers.Api;
using HellGame.App.ViewModels.Api;
using HellGame.App.ViewModels.Api.Payload.Locale;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace HellGame.App.Tests.Controllers.Api
{
    public class LocaleControllerTests
    {
        #region Context
        class TestCaseContext
        {
            #region Data
            public Session Session { get; }
            #endregion

            #region Services
            public ILogger<LocaleController> Logger { get; }
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

                Logger = Mock.Of<ILogger<LocaleController>>();
                SessionManager = Mock.Of<ISessionManager>();
                Session.LocaleManager = Mock.Of<ILocaleManager>();

                Mock.Get(SessionManager).Setup(
                    m => m.GetSession(Session.Id))
                    .Returns(Session);
            }
        }
        #endregion

        [Fact]
        public void Get_Success()
        {
            // Arrange
            var context = new TestCaseContext();
            var sut = new LocaleController(
                context.Logger,
                context.SessionManager);

            var locale = "ru-ru";

            Mock.Get(context.Session.LocaleManager).Setup(
                m => m.GetLocale())
                .Returns(locale);

            // Act
            var actionResult = sut.Get(context.Session.Id);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<GetLocaleResponse>;
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            var payload = result.Payload;
            Assert.NotNull(payload);
            Assert.Equal(locale, payload.Locale);
        }

        [Fact]
        public void Get_Error()
        {
            // Arrange
            var context = new TestCaseContext();
            var sut = new LocaleController(
                context.Logger,
                context.SessionManager);

            Mock.Get(context.Session.LocaleManager).Setup(
                m => m.GetLocale())
                .Throws(new Exception("message"));

            // Act
            var actionResult = sut.Get(context.Session.Id);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<GetLocaleResponse>;
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Null(result.Payload);
        }

        [Fact]
        public void Post_Success()
        {
            // Arrange
            var context = new TestCaseContext();
            var sut = new LocaleController(
                context.Logger,
                context.SessionManager);

            var locale = "ru-ru";

            Mock.Get(context.Session.LocaleManager).Setup(
                m => m.GetLocale())
                .Returns(locale);

            var request = ApiRequest<SetLocaleRequest>.Make(new SetLocaleRequest { Locale = locale });

            // Act
            var actionResult = sut.Post(context.Session.Id, request);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<GetLocaleResponse>;
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            var payload = result.Payload;
            Assert.NotNull(payload);
            Assert.Equal(locale, payload.Locale);
        }

        [Fact]
        public void Post_Error()
        {
            // Arrange
            var context = new TestCaseContext();
            var sut = new LocaleController(
                context.Logger,
                context.SessionManager);

            var locale = "ru-ru";

            Mock.Get(context.Session.LocaleManager).Setup(
                m => m.GetLocale())
                .Throws(new Exception("message"));

            var request = ApiRequest<SetLocaleRequest>.Make(new SetLocaleRequest { Locale = locale });

            // Act
            var actionResult = sut.Post(context.Session.Id, request);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<GetLocaleResponse>;
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Null(result.Payload);
        }
    }
}
