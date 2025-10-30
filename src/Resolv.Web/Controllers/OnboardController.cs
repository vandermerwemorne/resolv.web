using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Resolv.Domain.AssessmentSite;
using Resolv.Domain.Division;
using Resolv.Domain.Geographical;
using Resolv.Domain.HoldingCompany;
using Resolv.Domain.Onboarding;
using Resolv.Web.Models;

namespace Resolv.Web.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class OnboardController(
        ICustDivisionRepository custDivisionRepository,
        IHoldingCompanyRepository holdingCompanyRepository,
        IAssessmentSiteRepository assessmentSiteRepository,
        IProvinceRepository provinceRepository,
        ITownRepository townRepository,
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
        [ActionName("CreateHoldingCompany")]
        public async Task<IActionResult> CreateHoldingCompanyAsync(HoldingCompany model)
        {
            await SetViewBagHoldingCompanies();

            if (ModelState.IsValid)
            {
                if (model.Id.HasValue && model.Id.Value != Guid.Empty)
                {
                    // Update existing holding company
                    var existingCompany = await holdingCompanyRepository.GetAsync(model.Id.Value);
                    if (existingCompany.Id > 0)
                    {
                        // Check if another company already has this name (excluding current one)
                        var duplicateCheck = await holdingCompanyRepository.GetAsync(model.Name);
                        if (duplicateCheck.Id > 0 && duplicateCheck.Uid != model.Id.Value)
                        {
                            ModelState.AddModelError("Name", $"A holding company with the name '{model.Name}' already exists.");
                            return View(model);
                        }

                        existingCompany.Name = model.Name;
                        existingCompany.InsertDate = DateTime.UtcNow;
                        await holdingCompanyRepository.UpdateAsync(existingCompany);

                        return RedirectToAction("CreateHoldingCompany");
                    }
                }
                else
                {
                    // Create new holding company
                    var existing = await holdingCompanyRepository.GetAsync(model.Name);
                    if (existing.Id > 0)
                    {
                        ModelState.AddModelError("Name", $"A holding company with the name '{model.Name}' already exists.");
                        return View(model);
                    }

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var schema = System.Text.RegularExpressions.Regex.Replace(model.Name.Trim().ToLower(), @"[^a-z0-9]", "");

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
                    await onboardingRepository.AddTableUser(schema);
                    await onboardingRepository.AddTableRisk(schema);
                    await onboardingRepository.AddTableRiskLine(schema);
                    await onboardingRepository.AddTableRiskImages(schema);
                    await onboardingRepository.AddTableHazardCategory(schema);
                    await onboardingRepository.AddTableReEval(schema);

                    await onboardingRepository.CopyHazardCategory(schema);

                    return RedirectToAction("CreateDivision", new { holdingCompanyUid = uid });
                }
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
                if (model.Id.HasValue && model.Id.Value != Guid.Empty)
                {
                    // Update existing division
                    var existingDivision = await custDivisionRepository.GetAsync(holdingCompany.SchemaName, model.Id.Value);
                    if (existingDivision.Id > 0)
                    {
                        existingDivision.Name = model.Name;
                        existingDivision.InsertDate = DateTime.UtcNow;
                        await custDivisionRepository.UpdateAsync(existingDivision, holdingCompany.SchemaName);
                    }
                }
                else
                {
                    // Create new division
                    var division = new CustDivision
                    {
                        Name = model.Name,
                        AddedByUserId = int.Parse(userId ?? "0"),
                        HoldingCompanyId = holdingCompany.Id
                    };
                    await custDivisionRepository.AddAsync(division, holdingCompany.SchemaName);
                }
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var holdingCompany = await holdingCompanyRepository.GetAsync(model.HoldingCompanyUid);
            var division = await custDivisionRepository.GetAsync(holdingCompany.SchemaName, model.DivisionUid);

            if (ModelState.IsValid)
            {
                if (model.Id.HasValue && model.Id.Value != Guid.Empty)
                {
                    // Update existing assessment site
                    var existingAssessmentSite = await assessmentSiteRepository.GetByUidAsync(holdingCompany.SchemaName, model.Id.Value);
                    if (existingAssessmentSite.Id > 0)
                    {
                        existingAssessmentSite.Address = model.Address;
                        existingAssessmentSite.IdentityCode = model.IdentityCode;
                        existingAssessmentSite.ProvinceId = model.ProvinceId;
                        existingAssessmentSite.RefCode = model.RefCode;
                        existingAssessmentSite.SiteName = model.SiteName;
                        existingAssessmentSite.TownId = model.TownId;
                        existingAssessmentSite.InsertDate = DateTime.UtcNow;

                        await assessmentSiteRepository.UpdateAsync(existingAssessmentSite, holdingCompany.SchemaName);
                    }
                }
                else
                {
                    // Create new assessment site
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
                }

                return RedirectToAction("CreateAssessmentSite", new { divisionUid = model.DivisionUid, holdingCompanyUid = model.HoldingCompanyUid });
            }

            // Ensure ViewBag values are set for validation failure scenario
            ViewBag.DivisionUid = model.DivisionUid;
            ViewBag.HoldingCompanyUid = model.HoldingCompanyUid;
            await SetViewBagAssessmentSite(model.DivisionUid, model.HoldingCompanyUid, model.ProvinceId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetTown(int provinceId)
        {
            var towns = await townRepository.GetAsync(provinceId);
            var townList = towns.Select(t => new
            {
                t.Id,
                t.Name
            });
            return Json(townList);
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
                d.Uid,
                d.SiteName,
                d.IdentityCode,
                d.RefCode,
                d.Address,
                d.ProvinceId,
                d.TownId,
                d.InsertDate,
                DivisionIdUid = divisionId
            }).ToList();

            var provinces = await provinceRepository.GetAsync();
            ViewBag.Province = provinces.Prepend(new Province { Id = 0, Name = "-- Select Province --" })
                .Select(p => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();
        }

        private async Task SetViewBagAssessmentSite(Guid divisionId, Guid holdingCompanyId, int? provinceId = null)
        {
            await SetViewBagAssessmentSite(divisionId, holdingCompanyId);

            // If we have a provinceId, pre-populate the towns for validation error scenarios
            if (provinceId.HasValue && provinceId.Value > 0)
            {
                var towns = await townRepository.GetAsync(provinceId.Value);
                ViewBag.Towns = towns.Select(t => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                }).ToList();
            }
        }
    }
}
