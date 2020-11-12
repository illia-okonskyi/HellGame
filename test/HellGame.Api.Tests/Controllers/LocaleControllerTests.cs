using HellEngine.Core.Services.Assets;
using HellGame.Api.Controllers;
using HellGame.Api.ViewModels;
using HellGame.Api.ViewModels.ApiPayload.Locale;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace HellGame.Api.Tests.Controllers
{
    public class LocaleControllerTests
    {
        #region Context
        class TestCaseContext
        {
            #region Data
            #endregion

            #region Services
            public ILogger<LocaleController> Logger { get; }
            public IAssetsManager AssetsManager { get; }
            #endregion

            #region Utils
            #endregion

            public TestCaseContext()
            {
                Logger = Mock.Of<ILogger<LocaleController>>();
                AssetsManager = Mock.Of<IAssetsManager>();
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
                context.AssetsManager);

            var locale = "en-us";

            Mock.Get(context.AssetsManager).Setup(
                m => m.GetLocale())
                .Returns(locale);

            // Act
            var actionResult = sut.Get();

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<GetLocale>;
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
                context.AssetsManager);

            Mock.Get(context.AssetsManager).Setup(
                m => m.GetLocale())
                .Throws(new Exception("message"));

            // Act
            var actionResult = sut.Get();

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<GetLocale>;
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
                context.AssetsManager);

            var locale = "en-us";

            Mock.Get(context.AssetsManager).Setup(
                m => m.GetLocale())
                .Returns(locale);

            var request = ApiRequest<SetLocale>.Make(new SetLocale { Locale = locale });
        
            // Act
            var actionResult = sut.Post(request);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<GetLocale>;
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
                context.AssetsManager);

            var locale = "en-us";

            Mock.Get(context.AssetsManager).Setup(
                m => m.GetLocale())
                .Throws(new Exception("message"));

            var request = ApiRequest<SetLocale>.Make(new SetLocale { Locale = locale });

            // Act
            var actionResult = sut.Post(request);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<GetLocale>;
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Null(result.Payload);
        }
    }
}
