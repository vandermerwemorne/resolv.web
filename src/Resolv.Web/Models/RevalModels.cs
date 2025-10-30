using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Resolv.Web.Models;

public class CorrectiveActionsViewModel
{
    public Guid RiskUid { get; set; }
    public Guid RiskLineUid { get; set; }
    public Guid HoldingCompanyId { get; set; }

    [Display(Name = "Recommended Engineering Controls")]
    public string? RecEngControls { get; set; }

    [Display(Name = "Recommended Administrative Controls")]
    public string? RecAdminControls { get; set; }

    [Display(Name = "Recommended Management/Supervision Controls")]
    public string? RecManagementSuperControls { get; set; }

    [Display(Name = "Recommended PPE Controls")]
    public string? RecPPEControls { get; set; }

    [Display(Name = "Recommended Legal Requirements Controls")]
    public string? RecConformLegalReqControls { get; set; }

    [Display(Name = "Correct Engineering Controls")]
    [StringLength(1000, ErrorMessage = "Correct Engineering Controls cannot exceed 500 characters")]
    public string? CorrectEngControls { get; set; }

    [Display(Name = "Correct Administrative Controls")]
    [StringLength(1000, ErrorMessage = "Correct Administrative Controls cannot exceed 500 characters")]
    public string? CorrectAdminControls { get; set; }

    [Display(Name = "Correct Management/Supervision Controls")]
    [StringLength(1000, ErrorMessage = "Correct Management/Supervision Controls cannot exceed 500 characters")]
    public string? CorrectManagementSuperControls { get; set; }

    [Display(Name = "Correct PPE Controls")]
    [StringLength(1000, ErrorMessage = "Correct PPE Controls cannot exceed 500 characters")]
    public string? CorrectPPEControls { get; set; }

    [Display(Name = "Correct Legal Requirements Controls")]
    [StringLength(1000, ErrorMessage = "Correct Legal Requirements Controls cannot exceed 500 characters")]
    public string? CorrectLegalReqControls { get; set; }
    
    [Display(Name = "Department/Division")]
    public string DeptDivision { get; set; } = string.Empty;

    [Display(Name = "Hazard")]
    public string Hazard { get; set; } = string.Empty;

    [Display(Name = "Risk")]
    public string Risk { get; set; } = string.Empty;
    
    [Display(Name = "Reference No")]
    public string? ReferenceNo { get; set; }

    [Display(Name = "Classification")]
    public int? ClassificationId { get; set; }

    [Display(Name = "Hazard Category")]
    public int? StepInOperationId { get; set; }

    // Dropdown lists for the select options
    public List<SelectListItem> HazardCategories { get; set; } = [];
    public List<SelectListItem> Classifications { get; set; } = [];
}