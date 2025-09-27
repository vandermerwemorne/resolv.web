using Microsoft.AspNetCore.Mvc;
using Resolv.Domain.HoldingCompany;
using Resolv.Domain.Users;
using Resolv.Domain.Services;
using Resolv.Web.Models;
using System.Security.Claims;

namespace Resolv.Web.Controllers
{
    public class UsersController(
        IHoldingCompanyRepository holdingCompanyRepository,
        ICustUserRepository custUserRepository,
        IEncryptionService encryptionService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            await SetViewBagHoldingCompanies();
            var model = new User();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Users(User model)
        {
            await SetViewBagHoldingCompanies();

            if (!model.SelectedHoldingCompanyUid.HasValue || model.SelectedHoldingCompanyUid == Guid.Empty)
            {
                ModelState.AddModelError("SelectedHoldingCompanyUid", "Please select a holding company.");
                return View(model);
            }

            // Redirect to UsersCreate page with the selected holding company
            return RedirectToAction("UsersCreate", new { holdingCompanyUid = model.SelectedHoldingCompanyUid });
        }

        [HttpGet]
        public async Task<IActionResult> UsersCreate(Guid holdingCompanyUid)
        {
            var holdingCompany = await holdingCompanyRepository.GetAsync(holdingCompanyUid);
            if (holdingCompany.Id == 0)
            {
                return RedirectToAction("Users");
            }

            await SetViewBagUsers(holdingCompany.SchemaName);
            ViewBag.SelectedHoldingCompanyName = holdingCompany.Name;

            var model = new User { SelectedHoldingCompanyUid = holdingCompanyUid };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UsersCreate(User model)
        {
            if (!model.SelectedHoldingCompanyUid.HasValue || model.SelectedHoldingCompanyUid == Guid.Empty)
            {
                return RedirectToAction("Users");
            }

            var holdingCompany = await holdingCompanyRepository.GetAsync(model.SelectedHoldingCompanyUid.Value);
            if (holdingCompany.Id == 0)
            {
                return RedirectToAction("Users");
            }

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (model.Id.HasValue && model.Id.Value != Guid.Empty)
                {
                    // Update existing user
                    var existingUser = await custUserRepository.GetUserAsync(holdingCompany.SchemaName, model.Id.Value);
                    if (existingUser.Id > 0)
                    {
                        // Only update password if ResetPassword is checked
                        if (model.ResetPassword)
                        {
                            var hashedPassword = encryptionService.Hash("Password1", existingUser.Email ?? string.Empty);
                            existingUser.Password = hashedPassword;
                        }

                        existingUser.FullName = model.FullName;
                        existingUser.Email = model.Email;
                        existingUser.HasAccess = model.HasAccess;
                        existingUser.Roles = model.Roles;
                        existingUser.KnownName = model.KnownName;

                        await custUserRepository.UpdateUserAsync(existingUser, holdingCompany.SchemaName);
                    }
                }
                else
                {
                    // Create new user - always set default password
                    var hashedPassword = encryptionService.Hash("Password1", model.Email);

                    var commonUser = new CustUser
                    {
                        Password = hashedPassword,
                        FullName = model.FullName,
                        Email = model.Email,
                        HasAccess = model.HasAccess,
                        AddedByUserId = int.Parse(userId ?? "0"),
                        Roles = model.Roles,
                        KnownName = model.KnownName
                    };

                    await custUserRepository.AddUserAsync(commonUser, holdingCompany.SchemaName);
                }

                // Reset form after successful save and redirect back to same page
                return RedirectToAction("UsersCreate", new { holdingCompanyUid = model.SelectedHoldingCompanyUid });
            }

            // If validation failed, reload the page with current data
            await SetViewBagUsers(holdingCompany.SchemaName);
            ViewBag.SelectedHoldingCompanyName = holdingCompany.Name;
            return View(model);
        }

        private async Task SetViewBagHoldingCompanies()
        {
            var holdingCompanys = await holdingCompanyRepository.GetAsync();
            ViewBag.HoldingCompanies = holdingCompanys.Select(d => new
            {
                d.Name,
                d.InsertDate,
                d.Uid,
            }).ToList();
        }

        private async Task SetViewBagUsers(string? schemaName)
        {
            if (string.IsNullOrEmpty(schemaName))
            {
                ViewBag.Users = new List<object>();
                return;
            }

            var users = await custUserRepository.GetUsersAsync(schemaName);
            ViewBag.Users = users.Select(u => new
            {
                u.Uid,
                u.FullName,
                u.Email,
                u.KnownName,
                u.Roles,
                u.HasAccess,
                u.InsertDate
            }).ToList();
        }
    }
}
