namespace Resolv.Web.Models;

public class User
{
    public Guid? Id { get; set; }
    public Guid? SelectedHoldingCompanyUid { get; set; }
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool HasAccess { get; set; } = true;
    public string Roles { get; set; } = string.Empty;
    public string KnownName { get; set; } = string.Empty;
}
