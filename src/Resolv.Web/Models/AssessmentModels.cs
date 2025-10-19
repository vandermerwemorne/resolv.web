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