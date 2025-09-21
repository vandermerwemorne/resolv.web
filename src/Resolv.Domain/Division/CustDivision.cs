namespace Resolv.Domain.Division;

/// <summary>
/// Customer.Division
/// Where Customer is the pgsql schema prefix, eg: Hulamin
/// </summary>
public class CustDivision
{
    public int Id { get; set; }
    public int HoldingCompanyId { get; set; }
    public Guid Uid { get; set; }
    public int AddedByUserId { get; set; }
    public string? Name { get; set; }
    public DateTime InsertDate { get; set; }
}
