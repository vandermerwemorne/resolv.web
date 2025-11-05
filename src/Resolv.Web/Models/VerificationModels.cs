using Microsoft.AspNetCore.Mvc.Rendering;

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
}