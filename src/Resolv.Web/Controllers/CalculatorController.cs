using Microsoft.AspNetCore.Mvc;
using Resolv.Domain.Risk.Calculators;
using Resolv.Web.Models;

namespace Resolv.Web.Controllers;

public class CalculatorController(
    IExposureCalculator exposureCalculator,
    IRawRiskCalculator rawRiskCalculator,
    IResidualRiskCalculator residualRiskCalculator,
    IPriorityCalculator priorityCalculator,
    IColourCalculator colourCalculator) : Controller
{
    public IActionResult RawRisk(int severityId, int frequencyId, int exposureId)
    {
        var exposurePoint = exposureCalculator.GetExposurePoint(
            (Severity)severityId, 
            (Frequency)frequencyId);

        var rawRisk = rawRiskCalculator.GetRawRisk(
            (Exposure)exposureId,
            exposurePoint);

        var displayColour = colourCalculator.GetRawRiskColour(rawRisk);

        return Json(new RawRiskModel() { 
            RawRisk = rawRisk,
            DisplayColour = displayColour,
        });
    }

    public IActionResult ResidualAndPriority(int rawRisk, int engControl, int adminControl, int managementSuperControl, int ppeControl, int conformLegalReqControl)
    {
        var residualRisk = residualRiskCalculator.GetResidualRisk(rawRisk, engControl, adminControl, managementSuperControl, ppeControl, conformLegalReqControl);
        var priority = priorityCalculator.GetPriority(residualRisk);
        var displayColour = colourCalculator.GetResidualRiskColour(priority);

        return Json(new ResidualAndPriorityModel()
        {
            ResidualRisk = residualRisk,
            Priority = priority,
            DisplayColour = displayColour,
        });
    }
}
