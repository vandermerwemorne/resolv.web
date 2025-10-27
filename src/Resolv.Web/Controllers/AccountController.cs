using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Resolv.Domain.Users;
using Resolv.Domain.Services;
using Resolv.Web.Models;

namespace Resolv.Web.Controllers
{
    public class AccountController(IComUserRepository commonUserRepository, ICustUserRepository custUserRepository, IEncryptionService encryptionService) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var hash = encryptionService.Hash(password, username);

            // Common software admin users
            var user = await commonUserRepository.GetByCredentialsAsync(username, hash);
            if (user.Id > 0)
            {
                var passwordResetResult = CheckNeedsPasswordReset(hash, username, user.Uid);
                if (passwordResetResult != null)
                {
                    return passwordResetResult;
                }

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

        [HttpGet]
        public async Task<IActionResult> ResetPassword(Guid id)
        {
            var model = new ResetPasswordViewModel { UserId = id };
            ViewBag.Schema = TempData["Schema"] as string ?? "";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var schema = TempData["Schema"] as string ?? "";

            if (!ModelState.IsValid)
            {
                ViewBag.Schema = schema;
                TempData.Keep("Schema"); // Keep TempData for the next request
                return View(model);
            }

            try
            {
                if (string.IsNullOrEmpty(schema))
                {
                    // Use IComUserRepository
                    var user = await commonUserRepository.GetAsync(model.UserId);
                    if (user != null)
                    {
                        var hashedPassword = encryptionService.Hash(model.Password, user.Email ?? "");
                        user.Password = hashedPassword;
                        await commonUserRepository.UpdateAsync(user);
                    }
                }
                else
                {
                    // Use ICustUserRepository
                    var user = await custUserRepository.GetUserAsync(schema, model.UserId);
                    if (user != null)
                    {
                        var hashedPassword = encryptionService.Hash(model.Password, user.Email ?? "");
                        user.Password = hashedPassword;
                        await custUserRepository.UpdateUserAsync(user, schema);
                    }
                }

                TempData["LoginError"] = "Login with new credentials.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                ViewBag.Schema = schema;
                TempData.Keep("Schema"); // Keep TempData for the next request
                ModelState.AddModelError("", "An error occurred while updating the password. Please try again.");
                return View(model);
            }
        }

        private IActionResult? CheckNeedsPasswordReset(string hash, string username, Guid userId, string schemaName = "")
        {
            var needsPasswordReset = encryptionService.Hash("password1", username);

            if (hash == needsPasswordReset)
            {
                TempData["Schema"] = schemaName;
                return RedirectToAction("ResetPassword", "Account", new { id = userId });
            }

            return null;
        }
    }
}
