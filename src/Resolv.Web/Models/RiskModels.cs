using Microsoft.AspNetCore.Mvc.Rendering;

namespace Resolv.Web.Models;

public class RiskIndexViewModel
{
    public Guid? SelectedHoldingCompanyUid { get; set; }
    public int? SelectedDivisionId { get; set; }
    public int? SelectedAssessmentSiteId { get; set; }

    public List<SelectListItem> HoldingCompanies { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> Divisions { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> AssessmentSites { get; set; } = new List<SelectListItem>();
}