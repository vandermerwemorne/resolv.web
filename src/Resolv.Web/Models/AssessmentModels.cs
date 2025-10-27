using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Resolv.Web.Models;

public class StepOneViewModel
{
    public Guid RiskUid { get; set; }
    public Guid RiskLineUid { get; set; }
    public Guid HoldingCompanyId { get; set; }

    [Display(Name = "Department/Division")]
    [StringLength(255, ErrorMessage = "Department/Division cannot exceed 255 characters")]
    [Required(ErrorMessage = "Department/Division is required")]
    public string? DeptDivision { get; set; }

    [Display(Name = "Reference No")]
    [StringLength(100, ErrorMessage = "Reference No cannot exceed 100 characters")]
    [Required(ErrorMessage = "Reference No is required")]
    public string? ReferenceNo { get; set; }

    [Display(Name = "Hazard Category")]
    public int? StepInOperationId { get; set; }

    [Display(Name = "Classification")]
    public int? ClassificationId { get; set; }

    [Display(Name = "Hazard")]
    [StringLength(500, ErrorMessage = "Hazard cannot exceed 500 characters")]
    public string? Hazard { get; set; }

    [Display(Name = "Risk")]
    [StringLength(500, ErrorMessage = "Risk cannot exceed 500 characters")]
    public string? Risk { get; set; }

    [Display(Name = "Picture Upload")]
    public IFormFile? PictureFile { get; set; }

    // Dropdown lists for the select options
    public List<SelectListItem> HazardCategories { get; set; } = [];
    public List<SelectListItem> Classifications { get; set; } = [];
}

public class StepTwoViewModel
{
    public Guid RiskUid { get; set; }
    public Guid RiskLineUid { get; set; }
    public Guid HoldingCompanyId { get; set; }

    [Display(Name = "Severity")]
    public int? SeverityId { get; set; }

    [Display(Name = "Frequency")]
    public int? FrequencyId { get; set; }

    [Display(Name = "Exposure")]
    public int? ExposureId { get; set; }

    [Display(Name = "Eliminate/Substitute")]
    public int? EliminateId { get; set; }

    [Display(Name = "Recommendations")]
    [StringLength(1000, ErrorMessage = "Eliminate Recommendations cannot exceed 1000 characters")]
    public string? EliminateRec { get; set; }

    [Display(Name = "Eng. Control")]
    public int? EngControlId { get; set; }

    [Display(Name = "Current Engineering Controls")]
    [StringLength(1000, ErrorMessage = "Current Engineering Controls cannot exceed 1000 characters")]
    public string? CurrentEngControls { get; set; }

    [Display(Name = "Recommended Engineering Controls")]
    [StringLength(1000, ErrorMessage = "Recommended Engineering Controls cannot exceed 1000 characters")]
    public string? RecEngControls { get; set; }

    [Display(Name = "Admin Control")]
    public int? AdminControlId { get; set; }

    [Display(Name = "Current Administrative Controls")]
    [StringLength(1000, ErrorMessage = "Current Administrative Controls cannot exceed 1000 characters")]
    public string? CurrentAdminControls { get; set; }

    [Display(Name = "Recommended Administrative Controls")]
    [StringLength(1000, ErrorMessage = "Recommended Administrative Controls cannot exceed 1000 characters")]
    public string? RecAdminControls { get; set; }

    [Display(Name = "MGNT/Sup")]
    public int? ManagementSuperId { get; set; }

    [Display(Name = "Current Management/Supervision Controls")]
    [StringLength(1000, ErrorMessage = "Current Management/Supervision Controls cannot exceed 1000 characters")]
    public string? CurrentManagementSuperControls { get; set; }

    [Display(Name = "Recommended Management/Supervision Controls")]
    [StringLength(1000, ErrorMessage = "Recommended Management/Supervision Controls cannot exceed 1000 characters")]
    public string? RecManagementSuperControls { get; set; }

    [Display(Name = "PPE Control")]
    public int? PPEControlId { get; set; }

    [Display(Name = "Current PPE Controls")]
    [StringLength(1000, ErrorMessage = "Current PPE Controls cannot exceed 1000 characters")]
    public string? CurrentPPEControls { get; set; }

    [Display(Name = "Recommended PPE Controls")]
    [StringLength(1000, ErrorMessage = "Recommended PPE Controls cannot exceed 1000 characters")]
    public string? RecPPEControls { get; set; }

    [Display(Name = "LGL/REG CONF")]
    public int? ConformLegalReqId { get; set; }

    [Display(Name = "Current Legal Requirements Controls")]
    [StringLength(1000, ErrorMessage = "Current Legal Requirements Controls cannot exceed 1000 characters")]
    public string? CurrentConformLegalReqControls { get; set; }

    [Display(Name = "Recommended Legal Requirements Controls")]
    [StringLength(1000, ErrorMessage = "Recommended Legal Requirements Controls cannot exceed 1000 characters")]
    public string? RecConformLegalReqControls { get; set; }

    [Display(Name = "Assigned To")]
    [StringLength(255, ErrorMessage = "Assigned To cannot exceed 255 characters")]
    public string? AssignedToCompositeId { get; set; }

    [Display(Name = "Assigned Date")]
    [DataType(DataType.Date)]
    public DateTime? AssignedDate { get; set; }

    [Display(Name = "Corrective Action Date")]
    [DataType(DataType.Date)]
    public DateTime? CorrectiveActionDate { get; set; }

    [Display(Name = "Raw Risk")]
    public int RawRisk { get; set; } = 125;

    [Display(Name = "Residual Risk")]
    public int ResidualRisk { get; set; } = 125;

    [Display(Name = "Priority")]
    public int Priority { get; set; } = 1;


    // Dropdown lists for the select options
    public List<SelectListItem> Severities { get; set; } = [];
    public List<SelectListItem> Frequencies { get; set; } = [];
    public List<SelectListItem> Exposures { get; set; } = [];
    public List<SelectListItem> Eliminates { get; set; } = [];
    public List<SelectListItem> EngControls { get; set; } = [];
    public List<SelectListItem> AdminControls { get; set; } = [];
    public List<SelectListItem> ManagementSupers { get; set; } = [];
    public List<SelectListItem> PPEControls { get; set; } = [];
    public List<SelectListItem> ConformLegalReqs { get; set; } = [];
    public List<SelectListItem> AssignedTo { get; set; } = [];
}

public class AssessmentViewModel
{
    public Guid SelectedHoldingCompanyUid { get; set; }
    public int? SelectedDivisionId { get; set; }
    public int? SelectedAssessmentSiteId { get; set; }

    public List<SelectListItem> HoldingCompanies { get; set; } = [];
    public List<SelectListItem> Divisions { get; set; } = [];
    public List<SelectListItem> AssessmentSites { get; set; } = [];
    public List<AssessmentViewModelRisk> Risks { get; set; } = [];
}

public class RiskLineViewModel
{
    public Guid SelectedHoldingCompanyUid { get; set; }
    public Guid RiskUid { get; set; }
    public AssessmentViewModelRisk Risk { get; set; } = new AssessmentViewModelRisk();
    public List<RiskLineViewModelItem> RiskLines { get; set; } = [];
    public string HoldingCompanyName { get; set; } = string.Empty;
    public string DivisionName { get; set; } = string.Empty;
    public string AssessmentSiteName { get; set; } = string.Empty;
}

public class RiskLinesTableViewModel
{
    public Guid RiskUid { get; set; }
    public Guid SelectedHoldingCompanyUid { get; set; }
    public List<RiskLineViewModelItem> RiskLines { get; set; } = [];
}

public class RiskLineViewModelItem
{
    public Guid RiskUid { get; set; }
    public Guid Uid { get; set; }
    public DateTime InsertDate { get; set; }
    public DateTime? HazardDate { get; set; }
    public DateTime? AssignedDate { get; set; }
    public DateTime? CorrectiveActionDate { get; set; }
    public string DeptDivision { get; set; } = string.Empty;
    public string ReferenceNo { get; set; } = string.Empty;
    public string Hazard { get; set; } = string.Empty;
    public string Risk { get; set; } = string.Empty;
    public int RawRisk { get; set; }
    public int ResidualRisk { get; set; }
    public string StatusDisplay { get; set; } = string.Empty;
    public string AssignedToCompositeId { get; set; } = string.Empty;
}

public class RisksTableViewModel
{
    public Guid SelectedHoldingCompanyUid { get; set; }
    public List<AssessmentViewModelRisk> Risks { get; set; } = [];
}

public class AssessmentViewModelRisk
{
    public string SiteName { get; set; } = string.Empty;
    public Guid Uid { get; set; }
    public DateTime InsertDate { get; set; }
    public DateTime? ReevaluationDate { get; set; }
    public string RiskStatus { get; set; } = string.Empty;
    public string EvaluationType { get; set; } = string.Empty;
    public int AnnualStatus { get; set; }
}