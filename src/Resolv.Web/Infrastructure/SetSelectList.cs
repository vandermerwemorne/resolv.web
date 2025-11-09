using Microsoft.AspNetCore.Mvc.Rendering;
using Resolv.Domain.Classification;
using Resolv.Domain.Risk;
using Resolv.Domain.RiskControl;
using Resolv.Domain.Users;

namespace Resolv.Web.Infrastructure;

public class SetSelectList(
    ISeverityRepository severityRepository,
    IExposureRepository exposureRepository,
    IFrequencyRepository frequencyRepository,
    IEngineeringControlRepository engineeringControlRepository,
    IAdminControlRepository adminControlRepository,
    IManagementSuperControlRepository managementSuperControlRepository,
    IPPEControlRepository ppeControlRepository,
    ILegalRequirementControlRepository legalRequirementControlRepository,
    ICustUserRepository custUserRepository,
    IReEvalStatusRepository reEvalStatusRepository) : ISetSelectList
{
    public async Task<List<SelectListItem>> ReEvalStatus()
    {
        var data = await reEvalStatusRepository.GetAsync();
        return [.. data.Prepend(new ReEvalStatus { Id = 0, Description = "-- Select Status -"})
        .Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Description
        })];
    }

    public async Task<List<SelectListItem>> SetAssignedTo(string schema)
    {
        var data = await custUserRepository.GetUsersAsync(schema);
        return [.. data.Prepend(new CustUser { Id = 0, KnownName = null, Email = "-- Select User -"})
        .Select(p => new SelectListItem
        {
            Value = $"{p.Id}*O",
            Text = p.KnownName == null ? p.Email : $"{p.Email} ({p.KnownName})"
        })];
    }

    public async Task<List<SelectListItem>> SetAdminControl()
    {
        var data = await adminControlRepository.GetAsync();
        return [.. data.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Description
        })];
    }

    public async Task<List<SelectListItem>> SetEngineeringControl()
    {
        var data = await engineeringControlRepository.GetAsync();
        return [.. data.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Description
        })];
    }

    public async Task<List<SelectListItem>> SetExposure()
    {
        var data = await exposureRepository.GetComAsync();
        return [.. data.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Description
        })];
    }

    public async Task<List<SelectListItem>> SetFrequency()
    {
        var data = await frequencyRepository.GetComAsync();
        return [.. data.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Description
        })];
    }

    public async Task<List<SelectListItem>> SetLegalRequirementControl()
    {
        var data = await legalRequirementControlRepository.GetAsync();
        return [.. data.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Description
        })];
    }

    public async Task<List<SelectListItem>> SetManagementSuperControl()
    {
        var data = await managementSuperControlRepository.GetAsync();
        return [.. data.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Description
        })];
    }

    public async Task<List<SelectListItem>> SetPPEControl()
    {
        var data = await ppeControlRepository.GetAsync();
        return [.. data.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Description
        })];
    }

    public async Task<List<SelectListItem>> SetSeverity()
    {
        var data = await severityRepository.GetComAsync();
        return [.. data.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Description
        })];
    }
}
