using Microsoft.AspNetCore.Mvc.Rendering;
using Resolv.Domain.Risk;

namespace Resolv.Web.Models;

public class AssessmentViewModel
{
    public Guid? SelectedHoldingCompanyUid { get; set; }
    public int? SelectedDivisionId { get; set; }
    public int? SelectedAssessmentSiteId { get; set; }

    public List<SelectListItem> HoldingCompanies { get; set; } = [];
    public List<SelectListItem> Divisions { get; set; } = [];
    public List<SelectListItem> AssessmentSites { get; set; } = [];
    public List<AssessmentViewModelRisk> Risks { get; set; } = [];
}

public class RiskLineViewModel
{
    public Guid RiskUid { get; set; }
    public AssessmentViewModelRisk Risk { get; set; } = new AssessmentViewModelRisk();
    public List<RiskLineViewModelItem> RiskLines { get; set; } = [];
    public string HoldingCompanyName { get; set; } = string.Empty;
    public string DivisionName { get; set; } = string.Empty;
    public string AssessmentSiteName { get; set; } = string.Empty;
}

public class RiskLinesTableViewModel
{
    public List<RiskLineViewModelItem> RiskLines { get; set; } = [];
}

public class RiskLineViewModelItem
{
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
    public string AnnualStatus { get; set; } = string.Empty;
}