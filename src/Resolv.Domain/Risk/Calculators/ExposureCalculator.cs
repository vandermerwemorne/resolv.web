namespace Resolv.Domain.Risk.Calculators;

public class ExposureCalculator : IExposureCalculator
{
    public int GetExposurePoint(Severity severityId, Frequency frequencyId)
    {
        if (severityId == Severity.Catastrophic)
            return CheckCatastrophic(frequencyId);

        if (severityId == Severity.Critical)
            return CheckCritical(frequencyId);

        if (severityId == Severity.Serious)
            return CheckSerious(frequencyId);

        if (severityId == Severity.Marginal)
            return CheckMarginal(frequencyId);

        if (severityId == Severity.Negligible)
            return CheckNegligible(frequencyId);

        return 0;
    }

    private static int CheckNegligible(Frequency frequencyId)
    {
        if (frequencyId == Frequency.Frequent)
            return 11;

        if (frequencyId == Frequency.Regular)
            return 7;

        if (frequencyId == Frequency.Occasional)
            return 4;

        if (frequencyId == Frequency.Uncommon)
            return 3;

        if (frequencyId == Frequency.Rare)
            return 1;

        return 0;
    }

    private static int CheckMarginal(Frequency frequencyId)
    {
        if (frequencyId == Frequency.Frequent)
            return 16;

        if (frequencyId == Frequency.Regular)
            return 13;

        if (frequencyId == Frequency.Occasional)
            return 9;

        if (frequencyId == Frequency.Uncommon)
            return 6;

        if (frequencyId == Frequency.Rare)
            return 2;

        return 0;
    }

    private static int CheckSerious(Frequency frequencyId)
    {
        if (frequencyId == Frequency.Frequent)
            return 20;

        if (frequencyId == Frequency.Regular)
            return 18;

        if (frequencyId == Frequency.Occasional)
            return 15;

        if (frequencyId == Frequency.Uncommon)
            return 10;

        if (frequencyId == Frequency.Rare)
            return 5;

        return 0;
    }

    private static int CheckCritical(Frequency frequencyId)
    {
        if (frequencyId == Frequency.Frequent)
            return 23;

        if (frequencyId == Frequency.Regular)
            return 22;

        if (frequencyId == Frequency.Occasional)
            return 19;

        if (frequencyId == Frequency.Uncommon)
            return 14;

        if (frequencyId == Frequency.Rare)
            return 8;

        return 0;
    }

    private static int CheckCatastrophic(Frequency frequencyId)
    {
        if (frequencyId == Frequency.Frequent)
            return 25;

        if (frequencyId == Frequency.Regular)
            return 24;

        if (frequencyId == Frequency.Occasional)
            return 21;

        if (frequencyId == Frequency.Uncommon)
            return 17;

        if (frequencyId == Frequency.Rare)
            return 12;

        return 0;
    }
}
