using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Resolv.Domain.AssessmentSite;
using Resolv.Domain.Division;
using Resolv.Domain.HoldingCompany;
using Resolv.Domain.Risk;
using Resolv.Web.Models;

namespace Resolv.Web.Controllers
{
    public class AssessmentController(
        IHoldingCompanyRepository holdingCompanyRepository,
        ICustDivisionRepository divisionRepository,
        IAssessmentSiteRepository assessmentSiteRepository,
        IRiskRepository riskRepository,
        IRiskLineRepository riskLineRepository) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var viewModel = new AssessmentViewModel();

            // Load all holding companies
            var holdingCompanies = await holdingCompanyRepository.GetAsync();
            viewModel.HoldingCompanies = [.. holdingCompanies.Select(hc => new SelectListItem
            {
                Value = hc.Uid.ToString(),
                Text = hc.Name
            })];

            // Add default option
            viewModel.HoldingCompanies.Insert(0, new SelectListItem { Value = "", Text = "-- Select Holding Company --" });
            viewModel.Divisions.Insert(0, new SelectListItem { Value = "", Text = "-- Select Division --" });
            viewModel.AssessmentSites.Insert(0, new SelectListItem { Value = "", Text = "-- Select Assessment Site --" });

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AssessmentViewModel model)
        {
            // Reload all holding companies
            var holdingCompanies = await holdingCompanyRepository.GetAsync();
            model.HoldingCompanies = [.. holdingCompanies.Select(hc => new SelectListItem
            {
                Value = hc.Uid.ToString(),
                Text = hc.Name,
                Selected = hc.Uid == model.SelectedHoldingCompanyUid
            })];

            // Add default option
            model.HoldingCompanies.Insert(0, new SelectListItem { Value = "", Text = "-- Select Holding Company --" });

            // Load divisions if holding company is selected
            if (model.SelectedHoldingCompanyUid.HasValue)
            {
                var holdingCompany = await holdingCompanyRepository.GetAsync(model.SelectedHoldingCompanyUid.Value);
                if (holdingCompany != null)
                {
                    var divisions = await divisionRepository.GetAsync(holdingCompany.SchemaName);
                    model.Divisions = [.. divisions.Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name,
                        Selected = d.Id == model.SelectedDivisionId
                    })];

                    // Load assessment sites if division is selected
                    if (model.SelectedDivisionId.HasValue)
                    {
                        var assessmentSites = await assessmentSiteRepository.GetByDivisionIdAsync(holdingCompany.SchemaName, model.SelectedDivisionId.Value);
                        model.AssessmentSites = [.. assessmentSites.Select(site => new SelectListItem
                        {
                            Value = site.Id.ToString(),
                            Text = site.SiteName,
                            Selected = site.Id == model.SelectedAssessmentSiteId
                        })];

                        model.Risks = [];

                        // Load risks if assessment site is selected
                        if (model.SelectedAssessmentSiteId.HasValue)
                        {
                            var risks = await riskRepository.GetByAssessmentSiteAsync(holdingCompany.SchemaName, model.SelectedAssessmentSiteId.Value);
                            var selectedSiteName = model.AssessmentSites.FirstOrDefault(s => s.Value == model.SelectedAssessmentSiteId.Value.ToString())?.Text ?? string.Empty;

                            model.Risks = [.. risks.Select(risk => new AssessmentViewModelRisk
                            {
                                SiteName = selectedSiteName,
                                Uid = risk.Uid,
                                InsertDate = risk.InsertDate,
                                ReevaluationDate = risk.ReevaluationDate,
                                RiskStatus = risk.RiskStatusId == 1 ? "Complete" : "In progress",
                                EvaluationType = risk.EvaluationTypeId.ToString(), // TODO: do we still need this for resolv?
                                AnnualStatus = risk.AnnualStatus ?? string.Empty
                            })];
                        }
                    }
                }
            }

            // Add default options
            model.Divisions.Insert(0, new SelectListItem { Value = "", Text = "-- Select Division --" });
            model.AssessmentSites.Insert(0, new SelectListItem { Value = "", Text = "-- Select Assessment Site --" });

            return View(model);
        }
    }
}