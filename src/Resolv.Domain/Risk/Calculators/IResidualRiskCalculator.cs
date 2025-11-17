namespace Resolv.Domain.Risk.Calculators;

public interface IResidualRiskCalculator
{
    int GetResidualRisk(int rawRisk, int engControl, int adminControl, int managementSuperControl, int ppeControl, int conformLegalReqControl);
}
