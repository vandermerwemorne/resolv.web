using Microsoft.AspNetCore.Mvc.Rendering;

namespace Resolv.Web.Infrastructure;

public interface ISetSelectList
{
    Task<List<SelectListItem>> ReEvalStatus();

    Task<List<SelectListItem>> SetSeverity();
    Task<List<SelectListItem>> SetExposure();
    Task<List<SelectListItem>> SetFrequency();

    Task<List<SelectListItem>> SetEngineeringControl();
    Task<List<SelectListItem>> SetAdminControl();
    Task<List<SelectListItem>> SetManagementSuperControl();
    Task<List<SelectListItem>> SetPPEControl();
    Task<List<SelectListItem>> SetLegalRequirementControl();

    /// <summary>
    /// Example: 123*O
    /// 
    /// 123 links to the customer user table
    /// * is the delimiter
    /// O means OHAS (the the customers staff responsable for occupational health and safety)
    /// </summary>
    /// <param name="schema"></param>
    /// <returns></returns>
    Task<List<SelectListItem>> SetAssignedTo(string schema);
}