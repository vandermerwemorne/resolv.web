namespace Resolv.Domain.Risk.Calculators;

public interface IRawRiskCalculator
{
    /// <summary>
    /// The raw risk value is calculated where the exposureId and exposureValue intersect.
    /// </summary>
    /// <param name="exposureId">
    /// Selected from a drop down
    /// </param>
    /// <param name="exposureValue">
    /// Calculated with IExposureCalculator
    /// </param>
    /// <returns>
    /// The raw risk value
    /// </returns>
    int GetRawRisk(Exposure exposureId, int exposureValue);
}