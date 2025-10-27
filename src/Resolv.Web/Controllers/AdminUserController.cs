using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Resolv.Domain.Users;
using Resolv.Domain.Services;
using Resolv.Web.Models;

namespace Resolv.Web.Controllers
{
    public class AdminUserController(
        IComUserRepository comUserRepository,
        IEncryptionService encryptionService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await comUserRepository.GetAsync();
            ViewBag.Users = users.Select(u => new
            {
                u.Uid,
                u.Email,
                u.FullName,
                u.KnownName,
                u.HasAccess,
                u.Roles,
                u.InsertDate
            }).ToList();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(Guid? userId = null)
        {
            var model = new AdminUser();

            if (userId.HasValue && userId.Value != Guid.Empty)
            {
                // Load existing user for editing
                var existingUser = await comUserRepository.GetAsync(userId.Value);
                if (existingUser.Id > 0)
                {
                    model = new AdminUser
                    {
                        Id = existingUser.Uid,
                        FullName = existingUser.FullName ?? "",
                        KnownName = existingUser.KnownName ?? "",
                        Email = existingUser.Email ?? "",
                        HasAccess = existingUser.HasAccess,
                        Roles = existingUser.Roles ?? ""
                        // Don't populate password for security
                    };
                }
            }
            else
            {
                // Creating new user
                model = new AdminUser
                {
                    HasAccess = true // Default to having access
                };
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(AdminUser model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                if (model.Id.HasValue && model.Id.Value != Guid.Empty)
                {
                    // Update existing user
                    var existingUser = await comUserRepository.GetAsync(model.Id.Value);
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

                        await comUserRepository.UpdateAsync(existingUser);
                    }
                }
                else
                {
                    // Create new user with default password "password1"
                    var hashedPassword = encryptionService.Hash("password1", model.Email);
                    var comUser = new ComUser
                    {
                        Email = model.Email,
                        FullName = model.FullName,
                        KnownName = model.KnownName,
                        HasAccess = model.HasAccess,
                        Roles = model.Roles,
                        Password = hashedPassword,
                        AddedByUserId = int.Parse(userId ?? "0")
                    };

                    await comUserRepository.AddAsync(comUser);
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}