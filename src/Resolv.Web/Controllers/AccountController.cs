using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Resolv.Web;

namespace Resolv.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Simple authentication logic for demonstration
            // In production, use a user store/database
            string? role = null;
            if (username == "admin" && password == "password")
            {
                role = "Admin";
            }
            else if (username == "user" && password == "password")
            {
                role = "User";
            }

            if (role != null)
            {
                var claims = new List<System.Security.Claims.Claim>
                {
                    new(System.Security.Claims.ClaimTypes.Name, username),
                    new(System.Security.Claims.ClaimTypes.Role, role)
                };
                var identity = new System.Security.Claims.ClaimsIdentity(claims, AuthConstants.CookieAuthScheme);
                var principal = new System.Security.Claims.ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(AuthConstants.CookieAuthScheme, principal);
                return RedirectToAction("Index", "Home");
            }

            TempData["LoginError"] = "Invalid username or password.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(AuthConstants.CookieAuthScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
