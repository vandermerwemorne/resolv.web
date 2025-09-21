namespace Resolv.Domain.Users;

public class CommonUser
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
