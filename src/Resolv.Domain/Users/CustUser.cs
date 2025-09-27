namespace Resolv.Domain.Users;

/// <summary>
/// Customers of Resolv, example Hulamin
/// Database is [CUSTOMER SCHEMA].users
/// </summary>
public class CustUser
{
    public int Id { get; set; }
    public Guid Uid { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? FullName { get; set; }
    public DateTime InsertDate { get; set; }
    public bool HasAccess { get; set; }
    public int AddedByUserId { get; set; }
    public string? Roles { get; set; }
    public string? KnownName { get; set; }
}
