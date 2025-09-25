using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Resolv.Domain.AssessmentSite;
using Resolv.Domain.Division;
using Resolv.Domain.HoldingCompany;
using Resolv.Domain.Onboarding;
using Resolv.Web.Models;

namespace Resolv.Web.Controllers
{
    public class OnboardController(
        ICustDivisionRepository custDivisionRepository,
        IHoldingCompanyRepository holdingCompanyRepository,
        IAssessmentSiteRepository assessmentSiteRepository,
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
            await SetViewBagHoldingCompanies();

            if (existing.Id > 0)
            {
                ModelState.AddModelError("Name", $"A holding company with the name '{model.Name}' already exists.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var schema = model.Name.Trim().ToLower();

                var commonHoldingCompany = new ComHoldingCompany
                {
                    Name = model.Name,
                    AddedByUserId = int.Parse(userId ?? "0"),
                    SchemaName = schema
                };
                var (_, uid) = await holdingCompanyRepository.AddAsync(commonHoldingCompany);

                await onboardingRepository.AddCustomerSchema(schema);
                await onboardingRepository.AddTableDivision(schema);
                await onboardingRepository.AddTableAssessmentSite(schema);

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

            await SetViewBagDivisions(holdingCompany.SchemaName, holdingCompany.Uid);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDivision(Division model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var holdingCompany = await holdingCompanyRepository.GetAsync(model.HoldingCompanyId);
            await SetViewBagDivisions(holdingCompany.SchemaName, holdingCompany.Uid);

            if (ModelState.IsValid)
            {
                var division = new CustDivision
                {
                    Name = model.Name,
                    AddedByUserId = int.Parse(userId ?? "0"),
                    HoldingCompanyId = holdingCompany.Id
                };
                await custDivisionRepository.AddAsync(division, holdingCompany.SchemaName);

                await SetViewBagDivisions(holdingCompany.SchemaName, holdingCompany.Uid);
                return View(model);
            }
            ViewBag.HoldingCompanyId = model.HoldingCompanyId;
            return View(model);
        }

        // Step 3: Create Assessment Site
        [HttpGet]
        public async Task<IActionResult> CreateAssessmentSite(Guid divisionId, Guid holdingCompanyId)
        {
            await SetViewBagAssessmentSite(divisionId, holdingCompanyId);

            ViewBag.DivisionId = divisionId;
            ViewBag.HoldingCompanyUid = holdingCompanyId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssessmentSite(AssessmentSite model)
        {
            await SetViewBagAssessmentSite(model.DivisionId, model.HoldingCompanyId);

            if (ModelState.IsValid)
            {
                
            }
            ViewBag.DivisionId = model.DivisionId;
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

        private async Task SetViewBagDivisions(string schemaName, Guid holdingCompanyId)
        {
            var divisions = await custDivisionRepository.GetAsync(schemaName);
            ViewBag.Divisions = divisions.Select(d => new
            {
                d.Name,
                d.InsertDate,
                d.Uid,
                HoldingCompanyUid = holdingCompanyId
            }).ToList();
        }

        private async Task SetViewBagAssessmentSite(Guid divisionId, Guid holdingCompanyId)
        {
            var holdingCompany = await holdingCompanyRepository.GetAsync(holdingCompanyId);
            var division = await custDivisionRepository.GetAsync(holdingCompany.SchemaName, divisionId);
            var assessmentSites = await assessmentSiteRepository.GetByDivisionIdAsync(holdingCompany.SchemaName, division.Id);

            ViewBag.HoldingName = holdingCompany.Name;
            ViewBag.Division = division.Name;
            ViewBag.AssessmentSites = assessmentSites.Select(d => new
            {
                d.SiteName,
                d.InsertDate,
                d.Uid,
                DivisionIdUid = divisionId
            }).ToList();
        }
    }
}
