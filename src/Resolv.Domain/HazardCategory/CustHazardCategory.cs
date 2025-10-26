namespace Resolv.Domain.HazardCategory;

public class CustHazardCategory
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime InsertDate { get; set; }
    public bool IncInCalc { get; set; }
    public bool Enabled { get; set; }
}
