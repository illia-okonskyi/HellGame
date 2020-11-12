using HellEngine.Core.Exceptions;
using HellEngine.Core.Models.Assets;
using HellEngine.Core.Services.Assets;
using HellGame.Api.Controllers;
using HellGame.Api.ViewModels;
using HellGame.Api.ViewModels.ApiPayload.Assets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HellGame.Api.Tests.Controllers
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
            public string AssetKey { get; }
            public Asset Asset { get; }
            #endregion

            #region Services
            public ILogger<AssetsController> Logger { get; }
            public IAssetsManager AssetsManager { get; }
            #endregion

            #region Utils
            #endregion

            public TestCaseContext(
                AssetType assetType,
                Result result)
            {
                AssetKey = "assetKey";
                Asset = new Asset(
                    new AssetDescriptor
                    {
                        Key = AssetKey,
                        AssetType = assetType
                    });
                Asset.SetData(AssetDataEncoding.String, "assetData");

                Logger = Mock.Of<ILogger<AssetsController>>();
                AssetsManager = Mock.Of<IAssetsManager>();

                Expression<Func<IAssetsManager, Task<Asset>>> getAssetExpr = assetType switch
                {
                    AssetType.Text => (IAssetsManager m) => m.GetTextAsset(
                        AssetKey,
                        null,
                        It.IsAny<CancellationToken>()),
                    AssetType.Image => (IAssetsManager m) => m.GetImageAsset(
                        AssetKey,
                        null,
                        It.IsAny<CancellationToken>()),
                    _ => throw new NotImplementedException()
                };
                var getAssetSetup = Mock.Get(AssetsManager).Setup(getAssetExpr);

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

        [Theory]
        [InlineData(AssetType.Text)]
        [InlineData(AssetType.Image)]
        public async Task GetAsset_Success(AssetType assetType)
        {
            // Arrange
            var context = new TestCaseContext(assetType, Result.Success);
            var sut = new AssetsController(
                context.Logger,
                context.AssetsManager);

            // Act
            var actionResult = assetType switch
            {
                AssetType.Text => await sut.Text(context.AssetKey, CancellationToken.None),
                AssetType.Image => await sut.Image(context.AssetKey, CancellationToken.None),
                _ => throw new NotImplementedException(),
            };

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<GetAsset>;
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            var payload = result.Payload;
            Assert.NotNull(payload);
            Assert.Equal(context.AssetKey, payload.Key);
            Assert.Equal(assetType, payload.Type);
        }

        [Theory]
        [InlineData(AssetType.Text)]
        [InlineData(AssetType.Image)]
        public async Task GetAsset_AssetException_BadRequest(AssetType assetType)
        {
            // Arrange
            var context = new TestCaseContext(assetType, Result.BadRequest);
            var sut = new AssetsController(
                context.Logger,
                context.AssetsManager);

            // Act
            var actionResult = assetType switch
            {
                AssetType.Text => await sut.Text(context.AssetKey, CancellationToken.None),
                AssetType.Image => await sut.Image(context.AssetKey, CancellationToken.None),
                _ => throw new NotImplementedException(),
            };

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(400, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<GetAsset>;
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Null(result.Payload);
        }

        [Theory]
        [InlineData(AssetType.Text)]
        [InlineData(AssetType.Image)]
        public async Task GetAsset_Exception_ApiError(AssetType assetType)
        {
            // Arrange
            var context = new TestCaseContext(assetType, Result.ApiError);
            var sut = new AssetsController(
                context.Logger,
                context.AssetsManager);

            // Act
            var actionResult = assetType switch
            {
                AssetType.Text => await sut.Text(context.AssetKey, CancellationToken.None),
                AssetType.Image => await sut.Image(context.AssetKey, CancellationToken.None),
                _ => throw new NotImplementedException(),
            };

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<GetAsset>;
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Null(result.Payload);
        }
    }
}
