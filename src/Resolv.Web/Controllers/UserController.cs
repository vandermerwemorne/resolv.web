using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Resolv.Domain.HoldingCompany;
using Resolv.Domain.Users;
using Resolv.Domain.Services;
using Resolv.Web.Models;

namespace Resolv.Web.Controllers
{
    public class UserController(
        IHoldingCompanyRepository holdingCompanyRepository,
        ICustUserRepository custUserRepository,
        IEncryptionService encryptionService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(Guid? selectedHoldingCompanyUid = null)
        {
            await SetViewBagHoldingCompanies();
            var model = new UserManagement();

            if (selectedHoldingCompanyUid.HasValue)
            {
                model.SelectedHoldingCompanyUid = selectedHoldingCompanyUid.Value;
                var holdingCompany = await holdingCompanyRepository.GetAsync(selectedHoldingCompanyUid.Value);
                if (holdingCompany.Id > 0)
                {
                    await SetViewBagUsers(holdingCompany.SchemaName, holdingCompany.Uid);
                    ViewBag.SelectedHoldingCompanyName = holdingCompany.Name;
                    ViewBag.SelectedHoldingCompanyUid = holdingCompany.Uid;
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserManagement model)
        {
            await SetViewBagHoldingCompanies();

            if (model.SelectedHoldingCompanyUid.HasValue)
            {
                var holdingCompany = await holdingCompanyRepository.GetAsync(model.SelectedHoldingCompanyUid.Value);
                if (holdingCompany.Id > 0)
                {
                    await SetViewBagUsers(holdingCompany.SchemaName, holdingCompany.Uid);
                    ViewBag.SelectedHoldingCompanyName = holdingCompany.Name;
                    ViewBag.SelectedHoldingCompanyUid = holdingCompany.Uid;
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser(Guid holdingCompanyUid, Guid? userId = null)
        {
            var holdingCompany = await holdingCompanyRepository.GetAsync(holdingCompanyUid);
            ViewBag.HoldingCompanyUid = holdingCompanyUid;
            ViewBag.HoldingName = holdingCompany.Name;

            var model = new User();

            if (userId.HasValue && userId.Value != Guid.Empty)
            {
                // Load existing user for editing
                var existingUser = await custUserRepository.GetUserAsync(holdingCompany.SchemaName, userId.Value);
                if (existingUser.Id > 0)
                {
                    model = new User
                    {
                        Id = existingUser.Uid,
                        HoldingCompanyUid = holdingCompanyUid,
                        FullName = existingUser.FullName ?? "",
                        KnownName = existingUser.KnownName ?? "",
                        Email = existingUser.Email ?? "",
                        HasAccess = existingUser.HasAccess,
                        Roles = existingUser.Roles ?? "",
                        // Don't populate password for security
                        Password = ""
                    };
                }
            }
            else
            {
                // Creating new user
                model = new User
                {
                    HoldingCompanyUid = holdingCompanyUid,
                    HasAccess = true // Default to having access
                };
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var holdingCompany = await holdingCompanyRepository.GetAsync(model.HoldingCompanyUid);

            if (ModelState.IsValid)
            {
                if (model.Id.HasValue && model.Id.Value != Guid.Empty)
                {
                    // Update existing user
                    var existingUser = await custUserRepository.GetUserAsync(holdingCompany.SchemaName, model.Id.Value);
                    if (existingUser.Id > 0)
                    {
                        existingUser.Email = model.Email;
                        existingUser.FullName = model.FullName;
                        existingUser.KnownName = model.KnownName;
                        existingUser.HasAccess = model.HasAccess;
                        existingUser.Roles = model.Roles;

                        // Reset password if checkbox is checked
                        if (model.ResetPassword)
                        {
                            var hashedPassword = encryptionService.Hash("password1", model.Email);
                            existingUser.Password = hashedPassword;
                        }

                        await custUserRepository.UpdateUserAsync(existingUser, holdingCompany.SchemaName);
                    }
                }
                else
                {
                    // Create new user with default password "password1"
                    var hashedPassword = encryptionService.Hash("password1", model.Email);
                    var custUser = new CustUser
                    {
                        Email = model.Email,
                        FullName = model.FullName,
                        KnownName = model.KnownName,
                        HasAccess = model.HasAccess,
                        Roles = model.Roles,
                        Password = hashedPassword,
                        AddedByUserId = int.Parse(userId ?? "0")
                    };

                    await custUserRepository.AddUserAsync(custUser, holdingCompany.SchemaName);
                }

                return RedirectToAction("Index", new { selectedHoldingCompanyUid = model.HoldingCompanyUid });
            }

            // Ensure ViewBag values are set for validation failure scenario
            await SetViewBagUsers(holdingCompany.SchemaName, holdingCompany.Uid);
            ViewBag.HoldingCompanyUid = model.HoldingCompanyUid;
            ViewBag.HoldingName = holdingCompany.Name;
            return View(model);
        }

        private async Task SetViewBagHoldingCompanies()
        {
            var holdingCompanies = await holdingCompanyRepository.GetAsync();
            ViewBag.HoldingCompanies = holdingCompanies.Select(h => new SelectListItem
            {
                Value = h.Uid.ToString(),
                Text = h.Name
            }).Prepend(new SelectListItem
            {
                Value = "",
                Text = "Please select a holding company"
            }).ToList();
        }

        private async Task SetViewBagUsers(string schemaName, Guid holdingCompanyId)
        {
            var users = await custUserRepository.GetUsersAsync(schemaName);
            ViewBag.Users = users.Select(u => new
            {
                u.Uid,
                u.Email,
                u.FullName,
                u.KnownName,
                u.HasAccess,
                u.Roles,
                u.InsertDate,
                HoldingCompanyUid = holdingCompanyId
            }).ToList();
        }
    }
}