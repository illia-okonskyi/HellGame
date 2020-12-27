using HellGame.App.ViewModels.Home;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;

namespace HellGame.App.Controllers
{
    public class HomeController : Controller
    {
        private static string DevConfig = "dev";
        private static string ReleaseConfig = "release";

        private readonly IWebHostEnvironment hostEnvironment;

        public HomeController(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult RawJs()
        {
            var config = hostEnvironment.IsDevelopment()
                ? DevConfig
                : ReleaseConfig;

            var sessionId = Guid.NewGuid().ToString("D");

            return View(
                new RawJsImplementationViewModel
                {
                    Config = config,
                    SessionId = sessionId
                });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
