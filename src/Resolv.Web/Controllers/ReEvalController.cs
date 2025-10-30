using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Resolv.Domain.AssessmentSite;
using Resolv.Domain.Classification;
using Resolv.Domain.Division;
using Resolv.Domain.HazardCategory;
using Resolv.Domain.HoldingCompany;
using Resolv.Domain.Risk;
using Resolv.Domain.RiskControl;
using Resolv.Domain.Users;
using Resolv.Web.Models;

namespace Resolv.Web.Controllers
{
    public class ReEvalController(
        IHoldingCompanyRepository holdingCompanyRepository,
        ICustDivisionRepository divisionRepository,
        IAssessmentSiteRepository assessmentSiteRepository,
        IRiskRepository riskRepository,
        IRiskLineRepository riskLineRepository,
        IHazardCategoryRepository hazardCategoryRepository,
        IClassificationRepository classificationRepository,
        IExposureRepository exposureRepository,
        IFrequencyRepository frequencyRepository,
        ISeverityRepository severityRepository,

        IAdminControlRepository adminControlRepository,
        IEliminateControlRepository eliminateControlRepository,
        IEngineeringControlRepository engineeringControlRepository,
        ILegalRequirementControlRepository legalRequirementControlRepository,
        IManagementSuperControlRepository managementSuperControlRepository,
        IPPEControlRepository ppeControlRepository,
        ICustUserRepository custUserRepository) : Controller
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
            if (model.SelectedHoldingCompanyUid != Guid.Empty)
            {
                var holdingCompany = await holdingCompanyRepository.GetAsync(model.SelectedHoldingCompanyUid);
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
                                AnnualStatus = risk.AnnualStatus
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

        public async Task<IActionResult> Assessments(Guid riskId, Guid holdingCompanyId)
        {
            var holdingCompany = await holdingCompanyRepository.GetAsync(holdingCompanyId);

            var risk = await riskRepository.GetAsync(holdingCompany.SchemaName, riskId);
            if (risk != null)
            {
                // Found the risk, now get the risk lines
                var riskLines = await riskLineRepository.GetByRiskIdAsync(holdingCompany.SchemaName, risk.Id);

                // Get breadcrumb information - we need to find the assessment site from the risk's client ID
                var assessmentSites = await assessmentSiteRepository.GetAsync(holdingCompany.SchemaName);
                var assessmentSite = assessmentSites.FirstOrDefault(site => site.Id == risk.ClientId);

                string divisionName = string.Empty;
                string assessmentSiteName = assessmentSite?.SiteName ?? "Unknown Site";

                if (assessmentSite != null)
                {
                    var divisions = await divisionRepository.GetAsync(holdingCompany.SchemaName);
                    var division = divisions.FirstOrDefault(d => d.Id == assessmentSite.DivisionId);
                    divisionName = division?.Name ?? "Unknown Division";
                }

                var viewModel = new RiskLineViewModel
                {
                    SelectedHoldingCompanyUid = holdingCompany.Uid,
                    RiskUid = riskId,
                    HoldingCompanyName = holdingCompany.Name ?? "Unknown Company",
                    DivisionName = divisionName,
                    AssessmentSiteName = assessmentSiteName,
                    Risk = new AssessmentViewModelRisk
                    {
                        SiteName = assessmentSiteName,
                        Uid = risk.Uid,
                        InsertDate = risk.InsertDate,
                        ReevaluationDate = risk.ReevaluationDate,
                        RiskStatus = risk.RiskStatusId == 1 ? "Complete" : "In progress",
                        EvaluationType = risk.EvaluationTypeId.ToString(),
                        AnnualStatus = risk.AnnualStatus
                    },
                    RiskLines = [.. riskLines.Select(rl => new RiskLineViewModelItem
                {
                    Uid = rl.Uid,
                    InsertDate = rl.InsertDate,
                    HazardDate = rl.HazardDate,
                    AssignedDate = rl.AssignedDate,
                    CorrectiveActionDate = rl.CorrectiveActionDate,
                    DeptDivision = rl.DeptDivision ?? string.Empty,
                    ReferenceNo = rl.ReferenceNo ?? string.Empty,
                    Hazard = rl.Hazard ?? string.Empty,
                    Risk = rl.Risk ?? string.Empty,
                    RawRisk = rl.RawRisk,
                    ResidualRisk = rl.ResidualRisk,
                    StatusDisplay = rl.StatusId == 1 ? "Active" : "Inactive",
                    AssignedToCompositeId = rl.AssignedToCompositeId ?? string.Empty
                })]
                };

                return View(viewModel);
            }

            return BadRequest();
        }

        public async Task<IActionResult> CorrectiveActions(Guid riskId, Guid riskLineId, Guid holdingCompanyId)
        {
            if (riskLineId == Guid.Empty || holdingCompanyId == Guid.Empty)
            {
                return BadRequest("Risk line ID and holding company ID are required");
            }

            var holdingCompany = await holdingCompanyRepository.GetAsync(holdingCompanyId);
            if (holdingCompany == null)
            {
                return NotFound("Holding company not found");
            }

            var riskLine = await riskLineRepository.GetByUidAsync(holdingCompany.SchemaName, riskLineId);
            if (riskLine == null)
            {
                return NotFound("Risk line not found");
            }

            var viewModel = new CorrectiveActionsViewModel
            {
                RiskUid = riskId,
                RiskLineUid = riskLine.Uid,
                HoldingCompanyId = holdingCompanyId,
                RecEngControls = riskLine.RecEngControls,
                RecAdminControls = riskLine.RecAdminControls,
                RecManagementSuperControls = riskLine.RecManagementSuperControls,
                RecConformLegalReqControls = riskLine.RecConformLegalReqControls,
                RecPPEControls = riskLine.RecPpeControls,

                DeptDivision = riskLine.DeptDivision ?? string.Empty,
                Hazard = riskLine.Hazard ?? string.Empty,
                Risk = riskLine.Risk ?? string.Empty,
                ReferenceNo = riskLine.ReferenceNo ?? string.Empty,
                ClassificationId = riskLine.ClassificationId,
                StepInOperationId = riskLine.StepInOperationId,

                CorrectEngControls = "",
                CorrectAdminControls = "",
                CorrectManagementSuperControls = "",
                CorrectPPEControls = "",
                CorrectLegalReqControls = "",

                HazardCategories = await SetHazardCategories(holdingCompany.SchemaName),
                Classifications = await SetClassifications(),
            };

            return View(viewModel);
        }

        private async Task<List<SelectListItem>> SetHazardCategories(string schemaName)
        {
            var data = await hazardCategoryRepository.GetCustAsync(schemaName);
            return [.. data.Prepend(new CustHazardCategory { Id = 0, Description = "-- Select Hazard Category --" })
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Description
                })];
        }

        private async Task<List<SelectListItem>> SetClassifications()
        {
            var data = await classificationRepository.GetComAsync();
            return [.. data.Prepend(new ComClassification { Id = 0, Description = "-- Select Classification --" })
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Description
                })];
        }        
    }
}