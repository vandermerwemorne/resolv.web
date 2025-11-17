namespace Resolv.Domain.Risk.Calculators;

public class ColourCalculator : IColourCalculator
{
    public string GetRawRiskColour(int rawRisk)
    {
        //0-50
        if (rawRisk <= 50)
        {
            return "btn-success";
        }

        //51-99
        if (rawRisk >= 51 && rawRisk <= 99)
        {
            return "btn-warning";
        }

        //100+
        return "btn-danger";
    }

    public string GetResidualRiskColour(string priority)
    {
        if (string.IsNullOrEmpty(priority))
        {
            return "";
        }

        if (priority == "P 3" || priority == "P 4")
        {
            return "btn-warning";
        }

        if (priority == "P 5")
        {
            return "btn-success";
        }

        //P 2
        //P 1
        return "btn-danger";
    }
}
