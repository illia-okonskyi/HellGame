using HellEngine.Core.Exceptions;
using HellEngine.Core.Models;
using HellEngine.Core.Models.Assets;
using HellEngine.Core.Services.Assets;
using HellEngine.Core.Services.Locale;
using HellEngine.Core.Services.Sessions;
using HellGame.App.Controllers.Api;
using HellGame.App.ViewModels.Api;
using HellGame.App.ViewModels.Api.Payload.Assets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HellGame.App.Tests.Controllers.Api
{
    public class AssetsControllerTests
    {
        #region Context
        public enum Result
        {
            Success,
            BadRequest,
            ApiError
        }

        class TestCaseContext
        {
            #region Data
            public Session Session { get; } 
            public string Locale { get; }
            public string AssetKey { get; }
            public Asset Asset { get; }
            #endregion

            #region Services
            public ILogger<AssetsController> Logger { get; }
            public ISessionManager SessionManager { get; }
            #endregion

            #region Utils
            #endregion

            public TestCaseContext(
                AssetType assetType,
                Result result)
            {
                Session = new Session
                {
                    Id = Guid.NewGuid()
                };
                Locale = "ru-ru";
                AssetKey = "assetKey";
                Asset = new Asset(
                    new AssetDescriptor
                    {
                        Key = AssetKey,
                        AssetType = assetType,
                        MediaType = "mediaType",
                        SourceUrl = "http://www.example.com"
                    });
                Asset.SetData(AssetDataEncoding.String, "assetData");

                Logger = Mock.Of<ILogger<AssetsController>>();
                SessionManager = Mock.Of<ISessionManager>();
                Session.LocaleManager = Mock.Of<ILocaleManager>();
                Session.AssetsManager = Mock.Of<IAssetsManager>();

                Mock.Get(SessionManager).Setup(
                    m => m.GetSession(Session.Id))
                    .Returns(Session);

                Mock.Get(Session.LocaleManager).Setup(
                    m => m.GetLocale())
                    .Returns(Locale);

                Expression<Func<IAssetsManager, Task<Asset>>> getAssetExpr = assetType switch
                {
                    AssetType.Text => (IAssetsManager m) => m.GetTextAsset(
                        AssetKey,
                        Locale,
                        It.IsAny<CancellationToken>()),
                    AssetType.Image => (IAssetsManager m) => m.GetImageAsset(
                        AssetKey,
                        Locale,
                        It.IsAny<CancellationToken>()),
                    _ => throw new NotImplementedException()
                };
                var getAssetSetup = Mock.Get(Session.AssetsManager).Setup(getAssetExpr);

                switch (result)
                {
                    case Result.Success:
                        getAssetSetup.ReturnsAsync(Asset);
                        break;
                    case Result.BadRequest:
                        getAssetSetup.ThrowsAsync(new AssetException("message"));
                        break;
                    case Result.ApiError:
                        getAssetSetup.ThrowsAsync(new Exception("message"));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        #endregion

        [Fact]
        public async Task Text_Success()
        {
            // Arrange
            var context = new TestCaseContext(AssetType.Text, Result.Success);
            var sut = new AssetsController(
                context.Logger,
                context.SessionManager);

            // Act
            var actionResult = await sut.Text(
                    context.Session.Id,
                    ApiRequest<GetAssetRequest>.Make(new GetAssetRequest { Key = context.AssetKey }),
                    CancellationToken.None);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<TextAssetResponse>;
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            var payload = result.Payload;
            Assert.NotNull(payload);
            Assert.Equal(context.AssetKey, payload.Key);
        }

        [Fact]
        public async Task Text_AssetException_BadRequest()
        {
            // Arrange
            var context = new TestCaseContext(AssetType.Text, Result.BadRequest);
            var sut = new AssetsController(
                context.Logger,
                context.SessionManager);

            // Act
            var actionResult = await sut.Text(
                    context.Session.Id,
                    ApiRequest<GetAssetRequest>.Make(new GetAssetRequest { Key = context.AssetKey }),
                    CancellationToken.None);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(400, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<TextAssetResponse>;
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Null(result.Payload);
        }

        [Fact]
        public async Task Text_Exception_ApiError()
        {
            // Arrange
            var context = new TestCaseContext(AssetType.Text, Result.ApiError);
            var sut = new AssetsController(
                context.Logger,
                context.SessionManager);

            // Act
            var actionResult = await sut.Text(
                    context.Session.Id,
                    ApiRequest<GetAssetRequest>.Make(new GetAssetRequest { Key = context.AssetKey }),
                    CancellationToken.None);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<TextAssetResponse>;
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Null(result.Payload);
        }

        [Fact]
        public async Task Image_Success()
        {
            // Arrange
            var context = new TestCaseContext(AssetType.Image, Result.Success);
            var sut = new AssetsController(
                context.Logger,
                context.SessionManager);

            // Act
            var actionResult = await sut.Image(
                    context.Session.Id,
                    ApiRequest<GetAssetRequest>.Make(new GetAssetRequest { Key = context.AssetKey }),
                    CancellationToken.None);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<ImageAssetResponse>;
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            var payload = result.Payload;
            Assert.NotNull(payload);
            Assert.Equal(context.AssetKey, payload.Key);
            Assert.Equal(context.Asset.Descriptor.MediaType, payload.MediaType);
            Assert.Equal(context.Asset.Descriptor.SourceUrl, payload.SourceUrl);
        }

        [Fact]
        public async Task Image_AssetException_BadRequest()
        {
            // Arrange
            var context = new TestCaseContext(AssetType.Image, Result.BadRequest);
            var sut = new AssetsController(
                context.Logger,
                context.SessionManager);

            // Act
            var actionResult = await sut.Image(
                    context.Session.Id,
                    ApiRequest<GetAssetRequest>.Make(new GetAssetRequest { Key = context.AssetKey }),
                    CancellationToken.None);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(400, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<ImageAssetResponse>;
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Null(result.Payload);
        }

        [Fact]
        public async Task Image_Exception_ApiError()
        {
            // Arrange
            var context = new TestCaseContext(AssetType.Image, Result.ApiError);
            var sut = new AssetsController(
                context.Logger,
                context.SessionManager);

            // Act
            var actionResult = await sut.Image(
                    context.Session.Id,
                    ApiRequest<GetAssetRequest>.Make(new GetAssetRequest { Key = context.AssetKey }),
                    CancellationToken.None);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<ImageAssetResponse>;
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Null(result.Payload);
        }
    }
}
