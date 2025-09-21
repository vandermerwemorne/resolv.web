using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Resolv.Domain.HoldingCompany;
using Resolv.Domain.Onboarding;
using Resolv.Web.Models;

namespace Resolv.Web.Controllers
{
    public class OnboardController(
        IHoldingCompanyRepository holdingCompanyRepository,
        ICommonOnboardingRepository onboardingRepository) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("CreateHoldingCompany");
        }

        // Step 1: Create Holding Company
        [HttpGet]
        public async Task<IActionResult> CreateHoldingCompany()
        {
            await SetViewBagHoldingCompanies();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHoldingCompanyAsync(HoldingCompany model)
        {
            var existing = await holdingCompanyRepository.GetAsync(model.Name);
            if (existing.Id > 0)
            {
                ModelState.AddModelError("Name", $"A holding company with the name '{model.Name}' already exists.");
                await SetViewBagHoldingCompanies();

                return View(model);
            }

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var commonHoldingCompany = new ComHoldingCompany
                {
                    Name = model.Name,
                    AddedByUserId = int.Parse(userId ?? "0")
                };
                var (_, uid) = await holdingCompanyRepository.AddAsync(commonHoldingCompany);

                var schema = model.Name.Trim().ToLower();
                await onboardingRepository.AddCustomerSchema(schema);
                await onboardingRepository.AddTableDivision(schema);

                return RedirectToAction("CreateDivision", new { holdingCompanyId = uid });
            }
            return View(model);
        }

        // Step 2: Create Division
        [HttpGet]
        public async Task<IActionResult> CreateDivision(Guid holdingCompanyId)
        {
            var holdingCompany = await holdingCompanyRepository.GetAsync(holdingCompanyId);
            ViewBag.HoldingCompanyId = holdingCompanyId;
            ViewBag.HoldingName = holdingCompany.Name;

            return View();
        }

        [HttpPost]
        public IActionResult CreateDivision(Division model)
        {
            if (ModelState.IsValid)
            {
                // Normally save to DB, but out of scope
                // Redirect to assessment site creation, passing Division Id
                return RedirectToAction("CreateAssessmentSite", new { divisionId = model.Id });
            }
            ViewBag.HoldingCompanyId = model.HoldingCompanyId;
            return View(model);
        }

        // Step 3: Create Assessment Site
        [HttpGet]
        public IActionResult CreateAssessmentSite(Guid divisionId)
        {
            ViewBag.DivisionId = divisionId;
            return View();
        }

        [HttpPost]
        public IActionResult CreateAssessmentSite(AssessmentSite model)
        {
            if (ModelState.IsValid)
            {
                // Normally save to DB, but out of scope
                // End of onboarding flow
                return RedirectToAction("OnboardComplete");
            }
            ViewBag.DivisionId = model.DivisionId;
            return View(model);
        }

        public IActionResult OnboardComplete()
        {
            return View();
        }

        private async Task SetViewBagHoldingCompanies()
        {
            var holdingCompanys = await holdingCompanyRepository.GetAsync();
            ViewBag.HoldingCompanies = holdingCompanys;
        }        
    }
}
