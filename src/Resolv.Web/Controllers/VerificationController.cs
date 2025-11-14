using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Resolv.Domain.AssessmentSite;
using Resolv.Domain.Division;
using Resolv.Domain.HoldingCompany;
using Resolv.Domain.Risk;
using Resolv.Web.Infrastructure;
using Resolv.Web.Models;

namespace Resolv.Web.Controllers
{
    public class VerificationController(
        IHoldingCompanyRepository holdingCompanyRepository,
        ICustDivisionRepository divisionRepository,
        IAssessmentSiteRepository assessmentSiteRepository,
        ICustReEvalRepository custReEvalRepository,
        IRiskRepository riskRepository,
        IRiskLineRepository riskLineRepository,
        ISetSelectList setSelectList) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var viewModel = new VerificationViewModel();

            // Load all holding companies
            var holdingCompanies = await holdingCompanyRepository.GetAsync();
            viewModel.HoldingCompanies = [.. holdingCompanies.Select(hc => new SelectListItem
            {
                Value = hc.Uid.ToString(),
                Text = hc.Name
            })];

            // Add default option
            viewModel.HoldingCompanies.Insert(0, new SelectListItem { Value = "", Text = "-- Select Holding Company --" });

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(VerificationViewModel model)
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
                var divisions = await divisionRepository.GetAsync(holdingCompany.SchemaName);
                var reEvalsWithVerifications = new List<ReEvalsWithVerifications>();

                foreach (var division in divisions)
                {
                    var assessmentSites = await assessmentSiteRepository.GetByDivisionIdAsync(holdingCompany.SchemaName, division.Id);
                    foreach (var assessmentSite in assessmentSites)
                    {
                        var risks = await riskRepository.GetByAssessmentSiteAsync(holdingCompany.SchemaName, assessmentSite.Id);
                        var riskIds = risks.Select(r => r.Id).ToList();
                        var reEvals = await custReEvalRepository.GetByRiskIdsAsync(holdingCompany.SchemaName, riskIds);

                        if (reEvals.Count > 0)
                        {
                            reEvalsWithVerifications.Add(new ReEvalsWithVerifications
                            {
                                Uid = assessmentSite.Uid,
                                DivisionUid = division.Uid,
                                Division = division.Name ?? "Division",
                                AssessmentSite = assessmentSite.SiteName ?? "AssessmentSite",
                                VerificationsCount = reEvals.Count
                            });
                        }
                    }
                }

                model.Verifications = [.. reEvalsWithVerifications.OrderByDescending(x => x.VerificationsCount)];
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Assessments(Guid holdingCompanyUid, Guid assessmentSiteUid, Guid divisionUid)
        {
            var holdingCompany = await holdingCompanyRepository.GetAsync(holdingCompanyUid);
            var assessmentSite = await assessmentSiteRepository.GetByUidAsync(holdingCompany.SchemaName, assessmentSiteUid);
            var division = await divisionRepository.GetAsync(holdingCompany.SchemaName, divisionUid);

            var viewModel = new VerificationAssessmentsViewModel
            {
                HoldingCompanyUid = holdingCompanyUid,
                AssessmentSiteUid = assessmentSiteUid,
                DivisionUid = divisionUid,
                HoldingCompanyName = holdingCompany.Name ?? "Holding Company",
                AssessmentSiteName = assessmentSite.SiteName ?? "Assessment Site",
                DivisionName = division.Name ?? "Division"
            };

            var assessment = await assessmentSiteRepository.GetByUidAsync(holdingCompany.SchemaName, assessmentSiteUid);
            var risks = await riskRepository.GetByAssessmentSiteAsync(holdingCompany.SchemaName, assessment.Id);
            var reEvals = await custReEvalRepository.GetByRiskIdsAsync(holdingCompany.SchemaName, [.. risks.Select(r => r.Id)]);

            foreach (var reEval in reEvals)
            {
                var riskLine = await riskLineRepository.GetByIdAsync(holdingCompany.SchemaName, reEval.RiskLineId);
                viewModel.ReEvals.Add(new ReEvals()
                {
                    Uid = reEval.Uid,
                    ReferenceNo = riskLine?.ReferenceNo ?? "ReferenceNo",
                    InsertDate = reEval.InsertDate,
                    StepInOperationId = riskLine?.StepInOperationId,
                    ClassificationId = riskLine?.ClassificationId,
                    Hazard = riskLine?.Hazard,
                    Risk = riskLine?.Risk,
                    StatusId = reEval.ReEvalStatusId
                });
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Verify(Guid reEvalUid, Guid holdingCompanyUid, Guid assessmentSiteUid, Guid divisionUid)
        {
            var holdingCompany = await holdingCompanyRepository.GetAsync(holdingCompanyUid);
            var assessmentSite = await assessmentSiteRepository.GetByUidAsync(holdingCompany.SchemaName, assessmentSiteUid);
            var division = await divisionRepository.GetAsync(holdingCompany.SchemaName, divisionUid);
            
            var reEval = await custReEvalRepository.GetByUidAsync(holdingCompany.SchemaName, reEvalUid);
            var riskLine = await riskLineRepository.GetByIdAsync(holdingCompany.SchemaName, reEval.RiskLineId);

            var viewModel = new VerifyViewModel
            {
                ReEvalUid = reEvalUid,
                HoldingCompanyUid = holdingCompanyUid,
                AssessmentSiteUid = assessmentSiteUid,
                DivisionUid = divisionUid,
                HoldingCompanyName = holdingCompany.Name ?? "Holding Company",
                AssessmentSiteName = assessmentSite.SiteName ?? "Assessment Site",
                DivisionName = division.Name ?? "Division",

                NewHazard = riskLine.Hazard,
                NewRisk = riskLine.Risk,

                NewSeverityId = riskLine.SeverityId,
                NewFrequencyId = riskLine.FrequencyId,
                NewExposureId = riskLine.ExposureId,
                
                NewEngControlId = riskLine.EngControlId,
                NewAdminControlId = riskLine.AdminControlId,
                NewManagementSuperId = riskLine.ManagementSuperId,
                NewPPEControlId = riskLine.PpeControlId,
                NewConformLegalReqId = riskLine.ConformLegalReqId,

                ReEvalStatusId = reEval.ReEvalStatusId,
                AssignedToCompositeId = riskLine.AssignedToCompositeId,
                AssignedDate = riskLine.AssignedDate,
                CorrectiveActionDate = riskLine.CorrectiveActionDate,

                CorrectEngControls = reEval.CoractEngControls,
                CorrectAdminControls = reEval.CoractAdminControls,
                CorrectConformLegalReqControls = reEval.CoractLegalReqControls,
                CorrectManagementSuperControls = reEval.CoractSuperControls,
                CorrectPPEControls = reEval.CoractPpeControls,

                Severities = await setSelectList.SetSeverity(),
                Frequencies = await setSelectList.SetFrequency(),
                Exposures = await setSelectList.SetExposure(),

                EngControls = await setSelectList.SetEngineeringControl(),
                AdminControls = await setSelectList.SetAdminControl(),
                PPEControls = await setSelectList.SetPPEControl(),
                ManagementSupers = await setSelectList.SetManagementSuperControl(),
                ConformLegalReqs = await setSelectList.SetLegalRequirementControl(),

                ReEvalStatus = await setSelectList.ReEvalStatus(),
                AssignedTo = await setSelectList.SetAssignedTo(holdingCompany.SchemaName)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Verify(VerifyViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Assessments", new { 
                holdingCompanyUid = model.HoldingCompanyUid,
                assessmentSiteUid = model.AssessmentSiteUid,
                divisionUid = model.DivisionUid
            });
        }
    }
}