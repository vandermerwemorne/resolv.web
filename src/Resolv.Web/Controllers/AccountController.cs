using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Resolv.Domain.Users;
using Resolv.Domain.Services;
using Resolv.Web.Models;
using Resolv.Domain.HoldingCompany;

namespace Resolv.Web.Controllers
{
    public class AccountController(
        IComUserRepository commonUserRepository, 
        ICustUserRepository custUserRepository, 
        IEncryptionService encryptionService,
        IHoldingCompanyRepository holdingCompanyRepository) : Controller
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
                    new(System.Security.Claims.ClaimTypes.Name, user.KnownName ?? username),
                    new(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(System.Security.Claims.ClaimTypes.Role, Roles.Admin)
                };
                var identity = new System.Security.Claims.ClaimsIdentity(claims, AuthConstants.CookieAuthScheme);
                var principal = new System.Security.Claims.ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(AuthConstants.CookieAuthScheme, principal);
                return RedirectToAction("Index", "Assessment");
            }

            var holdingCompanys = await holdingCompanyRepository.GetAsync();
            foreach (var holdingCompany in holdingCompanys)
            {
                var customerUser = await custUserRepository.GetByCredentialsAsync(holdingCompany.SchemaName, username, hash);
                if (customerUser.Id > 0) 
                {
                    var passwordResetResult = CheckNeedsPasswordReset(hash, username, customerUser.Uid, holdingCompany.SchemaName);
                    if (passwordResetResult != null)
                    {
                        return passwordResetResult;
                    }

                    var claims = new List<System.Security.Claims.Claim>
                    {
                        new(System.Security.Claims.ClaimTypes.Name, customerUser.KnownName ?? username),
                        new(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new(System.Security.Claims.ClaimTypes.Role, Roles.Capture)
                    };
                    var identity = new System.Security.Claims.ClaimsIdentity(claims, AuthConstants.CookieAuthScheme);
                    var principal = new System.Security.Claims.ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(AuthConstants.CookieAuthScheme, principal);
                    return RedirectToAction("Index", "Assessment");
                }
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

        [HttpGet]
        public IActionResult ResetPassword(Guid id)
        {
            var model = new ResetPasswordViewModel
            {
                UserId = id,
                Schema = TempData["Schema"] as string ?? ""
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (string.IsNullOrEmpty(model.Schema))
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
                    var user = await custUserRepository.GetUserAsync(model.Schema, model.UserId);
                    if (user != null)
                    {
                        var hashedPassword = encryptionService.Hash(model.Password, user.Email ?? "");
                        user.Password = hashedPassword;
                        await custUserRepository.UpdateUserAsync(user, model.Schema);
                    }
                }

                TempData["LoginError"] = "Login with new credentials.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
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
