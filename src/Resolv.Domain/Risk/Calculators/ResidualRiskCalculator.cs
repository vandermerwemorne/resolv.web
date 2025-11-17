namespace Resolv.Domain.Risk.Calculators;

public class ResidualRiskCalculator : IResidualRiskCalculator
{
    const int NA = -1;

    public int GetResidualRisk(int rawRisk, int engControl, int adminControl, int managementSuperControl, int ppeControl, int conformLegalReqControl)
    {
        var controls = new List<int>() { engControl, adminControl, managementSuperControl, ppeControl, conformLegalReqControl };
        var countNa = controls.Where(x => x.Equals(NA)).Count();

        if (countNa == 5)
            return rawRisk;

        var mitigationFactor = 0m;
        var engCompliance = 0m;
        var adminCompliance = 0m;
        var managementSuperCompliance = 0m;
        var ppeCompliance = 0m;
        var legalCompliance = 0m;

        if (countNa == 4)
            mitigationFactor = 100m;
        else if (countNa == 3)
            mitigationFactor = 50m;
        else if (countNa == 2)
            mitigationFactor = 33.33m;
        else if (countNa == 1)
            mitigationFactor = 25m;
        else if (countNa == 0)
            mitigationFactor = 20m;

        mitigationFactor = rawRisk * (mitigationFactor / 100);

        if (engControl != NA)
        {
            engCompliance = mitigationFactor * (engControl / 100m);
            engCompliance = mitigationFactor - engCompliance;
        }

        if (adminControl != NA)
        {
            adminCompliance = mitigationFactor * (adminControl / 100m);
            adminCompliance = mitigationFactor - adminCompliance;
        }

        if (managementSuperControl != NA)
        {
            managementSuperCompliance = mitigationFactor * (managementSuperControl / 100m);
            managementSuperCompliance = mitigationFactor - managementSuperCompliance;
        }

        if (ppeControl != NA)
        {
            ppeCompliance = mitigationFactor * (ppeControl / 100m);
            ppeCompliance = mitigationFactor - ppeCompliance;
        }

        if (conformLegalReqControl != NA)
        {
            legalCompliance = mitigationFactor * (conformLegalReqControl / 100m);
            legalCompliance = mitigationFactor - legalCompliance;
        }

        decimal total = engCompliance + adminCompliance + managementSuperCompliance + ppeCompliance + legalCompliance;
        total = Math.Max(1, Math.Round(total));
        return Convert.ToInt32(total);
    }
}
