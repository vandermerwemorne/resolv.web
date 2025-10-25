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

        [HttpPost]
        public async Task<IActionResult> AddRisk(AssessmentViewModel model)
        {
            if (model.SelectedHoldingCompanyUid != Guid.Empty && model.SelectedAssessmentSiteId.HasValue)
            {
                // Get the holding company to obtain the schema name
                var holdingCompany = await holdingCompanyRepository.GetAsync(model.SelectedHoldingCompanyUid);
                if (holdingCompany != null)
                {
                    var newRisk = new CustRisk
                    {
                        Uid = Guid.NewGuid(),
                        InsertDate = DateTime.UtcNow,
                        ReevaluationDate = DateTime.UtcNow.AddYears(2),
                        RiskStatusId = 0, // TODO do we need this?
                        EvaluationTypeId = 1, // Default evaluation type
                        ClientId = model.SelectedAssessmentSiteId.Value,
                        UserId = 1, // TODO: Get current user ID
                        SectorId = 1, // TODO: Set appropriate sector
                        SubSectorId = 1, // TODO: Set appropriate sub-sector
                        AddedByUserId = 1, // TODO: Get current user ID
                        AnnualStatus = 0 // TODO do we need this?
                    };

                    await riskRepository.AddAsync(newRisk, holdingCompany.SchemaName);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddRiskLine(Guid riskUid, Guid selectedHoldingCompanyUid)
        {
            if (riskUid != Guid.Empty && selectedHoldingCompanyUid != Guid.Empty)
            {
                var holdingCompany = await holdingCompanyRepository.GetAsync(selectedHoldingCompanyUid);
                var risk = await riskRepository.GetAsync(holdingCompany.SchemaName, riskUid);
                var uid = Guid.Empty;

                if (holdingCompany != null)
                {
                    (_, uid) = await riskLineRepository.AddEmptyAsync(holdingCompany.SchemaName, risk.Id);
                }
                return RedirectToAction("StepOne", new { risklineid = uid, holdingCompanyId = selectedHoldingCompanyUid });
            }

            return BadRequest();
        }

        public async Task<IActionResult> StepOne(Guid riskId, Guid riskLineId, Guid holdingCompanyId)
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

            var viewModel = new StepOneViewModel
            {
                RiskUid = riskId,
                RiskLineUid = riskLine.Uid,
                HoldingCompanyId = holdingCompanyId,
                DeptDivision = riskLine.DeptDivision,
                ReferenceNo = riskLine.ReferenceNo,
                StepInOperationId = riskLine.StepInOperationId,
                ClassificationId = riskLine.ClassificationId,
                Hazard = riskLine.Hazard,
                Risk = riskLine.Risk,
                // TODO: Populate dropdown lists for HazardCategories and Classifications
                HazardCategories = [],
                Classifications = []
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> StepOne(StepOneViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // TODO: Repopulate dropdown lists
                model.HazardCategories = [];
                model.Classifications = [];
                return View(model);
            }

            if (model.HoldingCompanyId == Guid.Empty)
            {
                return BadRequest("Holding company ID is required");
            }

            var holdingCompany = await holdingCompanyRepository.GetAsync(model.HoldingCompanyId);
            if (holdingCompany == null)
            {
                return NotFound("Holding company not found");
            }

            var existingRiskLine = await riskLineRepository.GetByUidAsync(holdingCompany.SchemaName, model.RiskLineUid);
            if (existingRiskLine == null)
            {
                return NotFound("Risk line not found");
            }

            // Update the risk line with form data
            existingRiskLine.DeptDivision = model.DeptDivision;
            existingRiskLine.ReferenceNo = model.ReferenceNo;
            existingRiskLine.StepInOperationId = model.StepInOperationId;
            existingRiskLine.ClassificationId = model.ClassificationId;
            existingRiskLine.Hazard = model.Hazard;
            existingRiskLine.Risk = model.Risk;
            existingRiskLine.UpdatedBy = 1; // TODO: Get current user ID

            await riskLineRepository.UpdateAsync(holdingCompany.SchemaName, existingRiskLine);

            // TODO: Handle picture upload when implemented

            // Redirect to StepTwo after successfully saving StepOne
            return RedirectToAction("StepTwo", new { riskid = model.RiskUid, risklineid = model.RiskLineUid, holdingcompanyid = model.HoldingCompanyId });
        }

        public async Task<IActionResult> StepTwo(Guid riskId, Guid riskLineId, Guid holdingCompanyId)
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

            var viewModel = new StepTwoViewModel
            {
                RiskUid = riskId,
                RiskLineUid = riskLine.Uid,
                HoldingCompanyId = holdingCompanyId,
                SeverityId = riskLine.SeverityId,
                FrequencyId = riskLine.FrequencyId,
                ExposureId = riskLine.ExposureId,
                EliminateId = riskLine.EliminateId,
                EliminateRec = riskLine.EliminateRec,
                EngControlId = riskLine.EngControlId,
                CurrentEngControls = riskLine.CurrentEngControls,
                RecEngControls = riskLine.RecEngControls,
                AdminControlId = riskLine.AdminControlId,
                CurrentAdminControls = riskLine.CurrentAdminControls,
                RecAdminControls = riskLine.RecAdminControls,
                ManagementSuperId = riskLine.ManagementSuperId,
                CurrentManagementSuperControls = riskLine.CurrentManagementSuperControls,
                RecManagementSuperControls = riskLine.RecManagementSuperControls,
                PPEControlId = riskLine.PpeControlId,
                CurrentPPEControls = riskLine.CurrentPpeControls,
                RecPPEControls = riskLine.RecPpeControls,
                ConformLegalReqId = riskLine.ConformLegalReqId,
                CurrentConformLegalReqControls = riskLine.CurrentConformLegalReqControls,
                RecConformLegalReqControls = riskLine.RecConformLegalReqControls,
                AssignedToCompositeId = riskLine.AssignedToCompositeId,
                AssignedDate = riskLine.AssignedDate,
                CorrectiveActionDate = riskLine.CorrectiveActionDate,
                // TODO: Populate dropdown lists
                Severities = [],
                Frequencies = [],
                Exposures = [],
                Eliminates = [],
                EngControls = [],
                AdminControls = [],
                ManagementSupers = [],
                PPEControls = [],
                ConformLegalReqs = []
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> StepTwo(StepTwoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // TODO: Repopulate dropdown lists
                model.Severities = [];
                model.Frequencies = [];
                model.Exposures = [];
                model.Eliminates = [];
                model.EngControls = [];
                model.AdminControls = [];
                model.ManagementSupers = [];
                model.PPEControls = [];
                model.ConformLegalReqs = [];
                return View(model);
            }

            if (model.HoldingCompanyId == Guid.Empty)
            {
                return BadRequest("Holding company ID is required");
            }

            var holdingCompany = await holdingCompanyRepository.GetAsync(model.HoldingCompanyId);
            if (holdingCompany == null)
            {
                return NotFound("Holding company not found");
            }

            var existingRiskLine = await riskLineRepository.GetByUidAsync(holdingCompany.SchemaName, model.RiskLineUid);
            if (existingRiskLine == null)
            {
                return NotFound("Risk line not found");
            }

            // Update the risk line with StepTwo form data
            existingRiskLine.SeverityId = model.SeverityId;
            existingRiskLine.FrequencyId = model.FrequencyId;
            existingRiskLine.ExposureId = model.ExposureId;
            existingRiskLine.EliminateId = model.EliminateId ?? 0;
            existingRiskLine.EliminateRec = model.EliminateRec;
            existingRiskLine.EngControlId = model.EngControlId;
            existingRiskLine.CurrentEngControls = model.CurrentEngControls;
            existingRiskLine.RecEngControls = model.RecEngControls;
            existingRiskLine.AdminControlId = model.AdminControlId;
            existingRiskLine.CurrentAdminControls = model.CurrentAdminControls;
            existingRiskLine.RecAdminControls = model.RecAdminControls;
            existingRiskLine.ManagementSuperId = model.ManagementSuperId;
            existingRiskLine.CurrentManagementSuperControls = model.CurrentManagementSuperControls;
            existingRiskLine.RecManagementSuperControls = model.RecManagementSuperControls;
            existingRiskLine.PpeControlId = model.PPEControlId;
            existingRiskLine.CurrentPpeControls = model.CurrentPPEControls;
            existingRiskLine.RecPpeControls = model.RecPPEControls;
            existingRiskLine.ConformLegalReqId = model.ConformLegalReqId;
            existingRiskLine.CurrentConformLegalReqControls = model.CurrentConformLegalReqControls;
            existingRiskLine.RecConformLegalReqControls = model.RecConformLegalReqControls;
            existingRiskLine.AssignedToCompositeId = model.AssignedToCompositeId;
            existingRiskLine.AssignedDate = model.AssignedDate;
            existingRiskLine.CorrectiveActionDate = model.CorrectiveActionDate;
            existingRiskLine.UpdatedBy = 1; // TODO: Get current user ID

            await riskLineRepository.UpdateAsync(holdingCompany.SchemaName, existingRiskLine);

            // Redirect back to assessments list after successful save
            return RedirectToAction("Assessments", new { riskid = model.RiskUid, holdingcompanyid = model.HoldingCompanyId });
        }
    }
}