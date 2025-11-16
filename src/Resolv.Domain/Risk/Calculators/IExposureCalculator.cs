namespace Resolv.Domain.Risk.Calculators;

public interface IExposureCalculator
{
    /// <summary>
    /// The exposure point value is calculated where the severity and frequency intersect.
    /// Example Severity of ‘Serious’ and Frequency of ‘Occasional’ returns a value of 13
    /// </summary>
    /// <param name="severityId">
    /// Selected from a drop down
    /// </param>
    /// <param name="frequencyId">
    /// Selected from a drop down
    /// </param>
    /// <returns>The exposure point value</returns>
    int GetExposurePoint(Severity severityId, Frequency frequencyId);
}
