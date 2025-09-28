using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Resolv.Domain.AssessmentSite;
using Resolv.Domain.Division;
using Resolv.Domain.HoldingCompany;
using Resolv.Domain.Risk;
using Resolv.Web.Models;

namespace Resolv.Web.Controllers
{
    public class RiskController : Controller
    {
        private readonly IHoldingCompanyRepository _holdingCompanyRepository;
        private readonly ICustDivisionRepository _divisionRepository;
        private readonly IAssessmentSiteRepository _assessmentSiteRepository;
        private readonly IRiskRepository _riskRepository;

        public RiskController(
            IHoldingCompanyRepository holdingCompanyRepository,
            ICustDivisionRepository divisionRepository,
            IAssessmentSiteRepository assessmentSiteRepository,
            IRiskRepository riskRepository)
        {
            _holdingCompanyRepository = holdingCompanyRepository;
            _divisionRepository = divisionRepository;
            _assessmentSiteRepository = assessmentSiteRepository;
            _riskRepository = riskRepository;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new RiskIndexViewModel();

            // Load all holding companies
            var holdingCompanies = await _holdingCompanyRepository.GetAsync();
            viewModel.HoldingCompanies = holdingCompanies.Select(hc => new SelectListItem
            {
                Value = hc.Uid.ToString(),
                Text = hc.Name
            }).ToList();

            // Add default option
            viewModel.HoldingCompanies.Insert(0, new SelectListItem { Value = "", Text = "-- Select Holding Company --" });
            viewModel.Divisions.Insert(0, new SelectListItem { Value = "", Text = "-- Select Division --" });
            viewModel.AssessmentSites.Insert(0, new SelectListItem { Value = "", Text = "-- Select Assessment Site --" });

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetDivisions(Guid holdingCompanyUid)
        {
            // First get the holding company to get its schema and ID
            var holdingCompany = await _holdingCompanyRepository.GetAsync(holdingCompanyUid);
            if (holdingCompany == null)
            {
                return Json(new List<SelectListItem>());
            }

            // Get divisions for this holding company
            var divisions = await _divisionRepository.GetAsync(holdingCompany.SchemaName);

            var divisionList = divisions.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }).ToList();

            return Json(divisionList);
        }

        [HttpGet]
        public async Task<IActionResult> GetAssessmentSites(Guid holdingCompanyUid, int divisionId)
        {
            // First get the holding company to get its schema
            var holdingCompany = await _holdingCompanyRepository.GetAsync(holdingCompanyUid);
            if (holdingCompany == null)
            {
                return Json(new List<SelectListItem>());
            }

            // Get assessment sites for this division
            var assessmentSites = await _assessmentSiteRepository.GetByDivisionIdAsync(holdingCompany.SchemaName, divisionId);

            var siteList = assessmentSites.Select(site => new SelectListItem
            {
                Value = site.Id.ToString(),
                Text = site.SiteName
            }).ToList();

            return Json(siteList);
        }

        [HttpGet]
        public async Task<IActionResult> GetRisks(Guid holdingCompanyUid, int assessmentSiteId)
        {
            // First get the holding company to get its schema
            var holdingCompany = await _holdingCompanyRepository.GetAsync(holdingCompanyUid);
            if (holdingCompany == null)
            {
                return Json(new List<CustRisk>());
            }

            // Get risks for this assessment site
            var risks = await _riskRepository.GetByAssessmentSiteAsync(holdingCompany.SchemaName, assessmentSiteId);

            return Json(risks);
        }
    }
}