namespace Resolv.Domain.Risk.Calculators;

public class PriorityCalculator : IPriorityCalculator
{
    public string GetPriority(int residualRisk)
    {
        if (residualRisk >= 91)
            return "P 1";
        else if (residualRisk >= 61)
            return "P 2";
        else if (residualRisk >= 25)
            return "P 3";
        else if (residualRisk >= 6)
            return "P 4";
        else if (residualRisk >= 1)
            return "P 5";
        else
            return "";
    }
}
