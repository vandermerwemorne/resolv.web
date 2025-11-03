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
    public string Division { get; set; } = string.Empty;
    public string AssessmentSite { get; set; } = string.Empty;
    public int VerificationsCount { get; set; }
}