using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Resolv.Web.Models;

public class VerificationViewModel
{
    public Guid SelectedHoldingCompanyUid { get; set; }

    public List<SelectListItem> HoldingCompanies { get; set; } = [];
    public List<ReEvalsWithVerifications> Verifications { get; set; } = [];
}

public class VerificationsTableViewModel
{
    public List<ReEvalsWithVerifications> Verifications { get; set; } = [];
    public Guid SelectedHoldingCompanyUid { get; set; }
}

public class ReEvalsWithVerifications
{
    public Guid Uid { get; set; }
    public Guid DivisionUid { get; set; }
    public string Division { get; set; } = string.Empty;
    public string AssessmentSite { get; set; } = string.Empty;
    public int VerificationsCount { get; set; }
}

public class VerificationAssessmentsViewModel
{
    public Guid HoldingCompanyUid { get; set; }
    public Guid AssessmentSiteUid { get; set; }
    public Guid DivisionUid { get; set; }
    public string HoldingCompanyName { get; set; } = string.Empty;
    public string AssessmentSiteName { get; set; } = string.Empty;
    public string DivisionName { get; set; } = string.Empty;
    public List<ReEvals> ReEvals { get; set; } = [];
}

public class ReEvals
{
    public Guid Uid { get; set; }
    public string ReferenceNo { get; set; } = string.Empty;
    public DateTime InsertDate { get; set; }
    public int? StepInOperationId { get; set; }
    public int? ClassificationId { get; set; }
    public string? Hazard { get; set; }
    public string? Risk { get; set; }
    public int StatusId { get; set; }
}

public class ReEvalsViewModel
{
    public List<ReEvals> ReEvals { get; set; } = [];
    public string HoldingCompanyName { get; set; } = string.Empty;
    public string AssessmentSiteName { get; set; } = string.Empty;
    public string DivisionName { get; set; } = string.Empty;
}

public class VerifyViewModel
{
    public Guid ReEvalUid { get; set; }
    public Guid HoldingCompanyUid { get; set; }
    public Guid AssessmentSiteUid { get; set; }
    public Guid DivisionUid { get; set; }
    public string HoldingCompanyName { get; set; } = string.Empty;
    public string AssessmentSiteName { get; set; } = string.Empty;
    public string DivisionName { get; set; } = string.Empty;

    [Display(Name = "Hazard")]
    [StringLength(500, ErrorMessage = "Hazard cannot exceed 500 characters")]
    public string? NewHazard { get; set; }

    [Display(Name = "Risk")]
    [StringLength(500, ErrorMessage = "Risk cannot exceed 500 characters")]
    public string? NewRisk { get; set; }

    [Display(Name = "Severity")]
    public int NewSeverityId { get; set; }

    [Display(Name = "Frequency")]
    public int NewFrequencyId { get; set; }

    [Display(Name = "Exposure")]
    public int NewExposureId { get; set; }

    [Display(Name = "Raw Risk")]
    public int NewRawRisk { get; set; } = 0;

    [Display(Name = "Eng. Control")]
    public int NewEngControlId { get; set; }
    [Display(Name = "Admin Control")]
    public int NewAdminControlId { get; set; }
    [Display(Name = "MGNT/Sup")]
    public int NewManagementSuperId { get; set; }
    [Display(Name = "PPE Control")]
    public int NewPPEControlId { get; set; }
    [Display(Name = "LGL/REG CONF")]
    public int NewConformLegalReqId { get; set; }

    [Display(Name = "Status")]
    public int ReEvalStatusId { get; set; }

    [Display(Name = "Correct Engineering Controls")]
    [StringLength(1000, ErrorMessage = "Correct Engineering Controls cannot exceed 500 characters")]
    public string? CorrectEngControls { get; set; }
    [Display(Name = "Correct Administrative Controls")]
    [StringLength(1000, ErrorMessage = "Correct Administrative Controls cannot exceed 500 characters")]
    public string? CorrectAdminControls { get; set; }
    [Display(Name = "Correct Management/Supervision Controls")]
    [StringLength(1000, ErrorMessage = "Correct Management/Supervision Controls cannot exceed 1000 characters")]
    public string? CorrectManagementSuperControls { get; set; }
    [Display(Name = "Correct PPE Controls")]
    [StringLength(1000, ErrorMessage = "Current PPE Controls cannot exceed 1000 characters")]
    public string? CorrectPPEControls { get; set; }
    [Display(Name = "Correct Legal Requirements Controls")]
    [StringLength(1000, ErrorMessage = "Correct Legal Requirements Controls cannot exceed 1000 characters")]
    public string? CorrectConformLegalReqControls { get; set; }

    [Display(Name = "Recommended Engineering Controls")]
    [StringLength(1000, ErrorMessage = "Recommended Engineering Controls cannot exceed 1000 characters")]
    public string? RecEngControls { get; set; }
    [Display(Name = "Recommended Administrative Controls")]
    [StringLength(1000, ErrorMessage = "Recommended Administrative Controls cannot exceed 1000 characters")]
    public string? RecAdminControls { get; set; }
    [Display(Name = "Recommended Management/Supervision Controls")]
    [StringLength(1000, ErrorMessage = "Recommended Management/Supervision Controls cannot exceed 1000 characters")]
    public string? RecManagementSuperControls { get; set; }
    [Display(Name = "Recommended PPE Controls")]
    [StringLength(1000, ErrorMessage = "Recommended PPE Controls cannot exceed 1000 characters")]
    public string? RecPPEControls { get; set; }
    [Display(Name = "Recommended Legal Requirements Controls")]
    [StringLength(1000, ErrorMessage = "Recommended Legal Requirements Controls cannot exceed 1000 characters")]
    public string? RecConformLegalReqControls { get; set; }

    [Display(Name = "Residual Risk")]
    public int ResidualRisk { get; set; } = 125;
    [Display(Name = "Priority")]
    public int Priority { get; set; } = 1;

    [Display(Name = "Assigned Date")]
    [DataType(DataType.Date)]
    public DateTime? AssignedDate { get; set; }
    [Display(Name = "Corrective Action Date")]
    [DataType(DataType.Date)]
    public DateTime? CorrectiveActionDate { get; set; }
    [Display(Name = "Assigned To")]
    public string? AssignedToCompositeId { get; set; }

    public List<SelectListItem> Severities { get; set; } = [];
    public List<SelectListItem> Frequencies { get; set; } = [];
    public List<SelectListItem> Exposures { get; set; } = [];

    public List<SelectListItem> EngControls { get; set; } = [];
    public List<SelectListItem> AdminControls { get; set; } = [];
    public List<SelectListItem> PPEControls { get; set; } = [];
    public List<SelectListItem> ManagementSupers { get; set; } = [];
    public List<SelectListItem> ConformLegalReqs { get; set; } = [];

    public List<SelectListItem> AssignedTo { get; set; } = [];
    public List<SelectListItem> ReEvalStatus { get; set; } = [];
}