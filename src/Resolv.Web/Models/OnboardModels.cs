namespace Resolv.Web.Models
{
    public class HoldingCompany
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class Division
    {
        public Guid Id { get; set; }
        public Guid HoldingCompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AssessmentSite
    {
        public Guid Id { get; set; }
        public Guid DivisionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IdentityCode { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}