using HellEngine.Core.Models;
using HellEngine.Core.Models.Assets;
using HellEngine.Core.Models.Vars;
using HellEngine.Core.Services.Assets;
using HellEngine.Core.Services.Locale;
using HellEngine.Core.Services.Sessions;
using HellEngine.Core.Services.Vars;
using HellGame.App.Controllers.Api;
using HellGame.App.ViewModels.Api;
using HellGame.App.ViewModels.Api.Payload.Vars;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HellGame.App.Tests.Controllers.Api
{
    public class VarsControllerTests
    {
        #region Context
        class TestCaseContext
        {
            #region Data
            public Session Session { get; }
            public string Var1Name { get; }
            public string Var2Name { get; }
            public string Locale { get; }
            public Asset Var1NameAsset { get; }
            public Asset Var2NameAsset { get; }
            public List<IVar> Vars { get; }
            #endregion

            #region Services
            public ILogger<VarsController> Logger { get; }
            public ISessionManager SessionManager { get; }
            #endregion

            #region Utils
            #endregion

            public TestCaseContext()
            {
                Session = new Session
                {
                    Id = Guid.NewGuid(),
                    VarsManager = Mock.Of<IVarsManager>(),
                    LocaleManager = Mock.Of<ILocaleManager>(),
                    AssetsManager = Mock.Of<IAssetsManager>()
                };
                Var1Name = "var1-name";
                Var2Name = "var2-name";
                Locale = "ru-ru";
                Var1NameAsset = new Asset(new AssetDescriptor { Key = Var1Name }, Locale);
                Var1NameAsset.SetData(AssetDataEncoding.Base64, "111");
                Var2NameAsset = new Asset(new AssetDescriptor { Key = Var2Name }, Locale);
                Var2NameAsset.SetData(AssetDataEncoding.Base64, "222");
                Vars = new List<IVar>
                {
                    new IntVar("var1", Var1Name, 0, 15),
                    new StringVar("var2", Var2Name, 1, "hello"),
                };

                Logger = Mock.Of<ILogger<VarsController>>();
                SessionManager = Mock.Of<ISessionManager>();

                Mock.Get(SessionManager).Setup(
                    m => m.GetSession(Session.Id))
                    .Returns(Session);

                Mock.Get(Session.LocaleManager).Setup(
                    m => m.GetLocale())
                    .Returns(Locale);

                Mock.Get(Session.AssetsManager).Setup(
                    m => m.GetTextAsset(Var1Name, Locale, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Var1NameAsset);

                Mock.Get(Session.AssetsManager).Setup(
                    m => m.GetTextAsset(Var2Name, Locale, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Var2NameAsset);

                Mock.Get(Session.VarsManager).Setup(
                    m => m.GetAllVars())
                    .Returns(Vars);
            }
        }
        #endregion

        [Fact]
        public async Task Get_Success()
        {
            // Arrange
            var context = new TestCaseContext();
            var sut = new VarsController(
                context.Logger,
                context.SessionManager);

            // Act
            var actionResult = await sut.Get(
                context.Session.Id,
                CancellationToken.None);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<GetVarsResponse>;
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            var payload = result.Payload;
            Assert.NotNull(payload);

            Assert.Equal(2, payload.Vars.Count); // hardcode, not a bug

            Assert.Equal(context.Var1NameAsset.Data, payload.Vars[0].Name);
            Assert.Equal(context.Vars[0].DisplayString, payload.Vars[0].Value);

            Assert.Equal(context.Var2NameAsset.Data, payload.Vars[1].Name);
            Assert.Equal(context.Vars[1].DisplayString, payload.Vars[1].Value);
        }

        [Fact]
        public async Task Get_Error()
        {
            // Arrange
            var context = new TestCaseContext();
            var sut = new VarsController(
                context.Logger,
                context.SessionManager);

            Mock.Get(context.Session.VarsManager).Setup(
                m => m.GetAllVars())
                .Throws(new Exception("message"));

            // Act
            var actionResult = await sut.Get(
                context.Session.Id,
                CancellationToken.None);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = actionResult.Result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            var result = objectResult.Value as ApiResponse<GetVarsResponse>;
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Null(result.Payload);
        }
    }
}
