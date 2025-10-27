using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Resolv.Domain.Users;
using Resolv.Domain.Services;

namespace Resolv.Web.Controllers
{
    public class AccountController(IComUserRepository commonUserRepository, IEncryptionService encryptionService) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var hash = encryptionService.Hash(password, username);

            // Common software admin users
            var user = await commonUserRepository.GetByCredentialsAsync(username, hash);
            if (user.Id > 0)
            {
                var claims = new List<System.Security.Claims.Claim>
                {
                    new(System.Security.Claims.ClaimTypes.Name, username),
                    new(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(System.Security.Claims.ClaimTypes.Role, Roles.Admin)
                };
                var identity = new System.Security.Claims.ClaimsIdentity(claims, AuthConstants.CookieAuthScheme);
                var principal = new System.Security.Claims.ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(AuthConstants.CookieAuthScheme, principal);
                return RedirectToAction("Index", "Assessment");
            }

            // TODO customer users

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
