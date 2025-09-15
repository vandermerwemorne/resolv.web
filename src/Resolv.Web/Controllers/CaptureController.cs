using Microsoft.AspNetCore.Mvc;

namespace Resolv.Web.Controllers
{
    public class CaptureController(ILogger<CaptureController> logger) : Controller
    {
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
