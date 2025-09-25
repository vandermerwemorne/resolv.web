namespace Resolv.Domain.AssessmentSite;

/// <summary>
/// Customer.AssessmentSite
/// Where Customer is the pgsql schema prefix, eg: Hulamin
/// 
/// This was previously called "Client" in risk master
/// </summary>
public class CustAssessmentSite
{
    public int Id { get; set; }
    public Guid Uid { get; set; }
    public int DivisionId { get; set; }
    public int AddedByUserId { get; set; }
    public string? SiteName { get; set; }
    public string? RefCode { get; set; }
    public DateTime InsertDate { get; set; }
    public int HoldingCompanyId { get; set; }
    public int ProvinceId { get; set; }
    public string? Address { get; set; }
    public int SiteTypeId { get; set; }
    public int TownId { get; set; }
    public string? IdentityCode { get; set; }
}
