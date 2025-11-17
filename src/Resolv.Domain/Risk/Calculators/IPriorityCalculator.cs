namespace Resolv.Domain.Risk.Calculators;

public interface IPriorityCalculator
{
    /// <summary>
    /// Calculated based on the Total Residual Risk value and returns either “P 1” or “P 2” or “P 3” or “P 4” or “P 5”
    /// 
    /// >90 = "P 1"
    /// >60 = "P 2"
    /// >15 = "P 3"
    /// >5  = "P 4"
    /// >0  = "P 5"
    /// </summary>
    /// <param name="residualRisk"></param>
    /// <returns>
    /// either “P 1” or “P 2” or “P 3” or “P 4” or “P 5”
    /// </returns>
    string GetPriority(int residualRisk);
}
