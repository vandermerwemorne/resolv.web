namespace Resolv.Domain.HoldingCompany;

/// <summary>
/// Common.HoldingCompany
/// </summary>
public class ComHoldingCompany
{
    public int Id { get; set; }
    public Guid Uid { get; set; }
    public int AddedByUserId { get; set; }
    public string? Name { get; set; }
    public DateTime InsertDate { get; set; }

    /// <summary>
    /// Links the common holding company to its database schema
    /// </summary>
    public string SchemaName { get; set; } = string.Empty;
}
