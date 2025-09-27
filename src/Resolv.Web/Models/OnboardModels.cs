namespace Resolv.Web.Models
{
    public class HoldingCompany
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class Division
    {
        public Guid? Id { get; set; }
        public Guid HoldingCompanyUid { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AssessmentSite
    {
        public Guid? Id { get; set; }
        public Guid HoldingCompanyUid { get; set; }
        public Guid DivisionUid { get; set; }
        public string SiteName { get; set; } = string.Empty;
        public string IdentityCode { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int ProvinceId { get; set; }
        public string RefCode { get; set; } = string.Empty;
        public int TownId { get; set; }
    }
}