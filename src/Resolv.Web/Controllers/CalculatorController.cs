using Microsoft.AspNetCore.Mvc;
using Resolv.Domain.Risk.Calculators;
using Resolv.Web.Models;

namespace Resolv.Web.Controllers;

public class CalculatorController(
    IExposureCalculator exposureCalculator,
    IRawRiskCalculator rawRiskCalculator) : Controller
{
    public IActionResult RawRisk(int severityId, int frequencyId, int exposureId)
    {
        var exposurePoint = exposureCalculator.GetExposurePoint(
            (Severity)severityId, 
            (Frequency)frequencyId);

        var rawRisk = rawRiskCalculator.GetRawRisk(
            (Exposure)exposureId,
            exposurePoint);

        var displayColour = GetRawRiskColour(rawRisk);

        return Json(new RawRiskModel() { 
            RawRisk = rawRisk,
            DisplayColour = displayColour,
        });
    }

    private static string GetRawRiskColour(int rawRisk) 
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
}
