namespace Resolv.Web.Models;

public class RawRiskModel
{
    public int RawRisk { get; set; }
    public string DisplayColour { get; set; } = "";
}

public class ResidualAndPriorityModel
{
    public int ResidualRisk { get; set; }
    public string Priority { get; set; }
    public string DisplayColour { get; set; } = "";
}
