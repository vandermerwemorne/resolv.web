using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Resolv.Web.Models;

namespace Resolv.Web.Controllers
{
    public class HomeController(IWebHostEnvironment env, IConfiguration config) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Temporary diagnostic endpoint for UAT debugging
        public IActionResult DebugInfo()
        {
            var info = new
            {
                Environment = env.EnvironmentName,
                IsDevelopment = env.IsDevelopment(),
                DetailedErrorsConfig = config.GetValue<bool>("DetailedErrors"),
                AspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                AspNetCoreDetailedErrors = Environment.GetEnvironmentVariable("ASPNETCORE_DETAILEDERRORS")
            };

            return Json(info);
        }

        // Test endpoint to force an exception for debugging
        public IActionResult TestError()
        {
            throw new InvalidOperationException("This is a test exception to verify error handling in UAT");
        }
    }
}
