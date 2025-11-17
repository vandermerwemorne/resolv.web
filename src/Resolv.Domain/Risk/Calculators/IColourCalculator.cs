namespace Resolv.Domain.Risk.Calculators;

public interface IColourCalculator
{
    string GetRawRiskColour(int rawRisk);

    string GetResidualRiskColour(string priority);
}
