using System.Security.Claims;
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

        [HttpGet]
        public async Task<IActionResult> CreateHoldingCompany()
        {
            await SetViewBagHoldingCompanies();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHoldingCompanyAsync(HoldingCompany model)
        {
            await SetViewBagHoldingCompanies();

            var existing = await holdingCompanyRepository.GetAsync(model.Name);
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

                return RedirectToAction("CreateDivision", new { holdingCompanyUid = uid });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateDivision(Guid holdingCompanyUid)
        {
            var holdingCompany = await holdingCompanyRepository.GetAsync(holdingCompanyUid);
            ViewBag.HoldingCompanyUid = holdingCompanyUid;
            ViewBag.HoldingName = holdingCompany.Name;

            await SetViewBagDivisions(holdingCompany.SchemaName, holdingCompany.Uid);

            var model = new Division
            {
                HoldingCompanyUid = holdingCompanyUid
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDivision(Division model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var holdingCompany = await holdingCompanyRepository.GetAsync(model.HoldingCompanyUid);

            if (ModelState.IsValid)
            {
                var division = new CustDivision
                {
                    Name = model.Name,
                    AddedByUserId = int.Parse(userId ?? "0"),
                    HoldingCompanyId = holdingCompany.Id
                };
                await custDivisionRepository.AddAsync(division, holdingCompany.SchemaName);
                return RedirectToAction("CreateDivision", new { holdingCompanyUid = model.HoldingCompanyUid });
            }

            // Ensure ViewBag values are set for validation failure scenario
            await SetViewBagDivisions(holdingCompany.SchemaName, holdingCompany.Uid);
            ViewBag.HoldingCompanyUid = model.HoldingCompanyUid;
            ViewBag.HoldingName = holdingCompany.Name;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAssessmentSite(Guid divisionUid, Guid holdingCompanyUid)
        {
            await SetViewBagAssessmentSite(divisionUid, holdingCompanyUid);
            ViewBag.DivisionUid = divisionUid;
            ViewBag.HoldingCompanyUid = holdingCompanyUid;

            var model = new AssessmentSite
            {
                DivisionUid = divisionUid,
                HoldingCompanyUid = holdingCompanyUid
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssessmentSite(AssessmentSite model)
        {
            ViewBag.DivisionUid = model.DivisionUid;
            ViewBag.HoldingCompanyUid = model.HoldingCompanyUid;
            await SetViewBagAssessmentSite(model.DivisionUid, model.HoldingCompanyUid);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var holdingCompany = await holdingCompanyRepository.GetAsync(model.HoldingCompanyUid);
            var division = await custDivisionRepository.GetAsync(holdingCompany.SchemaName, model.DivisionUid);

            if (ModelState.IsValid)
            {
                var assessmentSite = new CustAssessmentSite
                {
                    AddedByUserId = int.Parse(userId ?? "0"),
                    Address = model.Address,
                    DivisionId = division.Id,
                    IdentityCode = model.IdentityCode,
                    ProvinceId = model.ProvinceId,
                    RefCode = model.RefCode,
                    SiteName = model.SiteName,
                    TownId = model.TownId
                };

                await assessmentSiteRepository.AddAsync(assessmentSite, holdingCompany.SchemaName);
                return View();
            }

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
