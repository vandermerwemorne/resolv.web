using Microsoft.AspNetCore.Mvc;
using Resolv.Domain.Risk.Calculators;

namespace Resolv.Web.Controllers;

public class HelperTextController : Controller
{
    public IActionResult SeverityChange(int severityId)
    {
        var response = "";
        var severity = (Severity)severityId;

        if (severity == Severity.Catastrophic)
            response = "Catastrophic- multiple fatalities";

        if (severity == Severity.Critical)
            response = "Critical- single fatality/multiple disabling";

        if (severity == Severity.Serious)
            response = "Serious- Permanent disabling";

        if (severity == Severity.Marginal)
            response = "Marginal- Minor or temporary disabling";

        if (severity == Severity.Negligible)
            response = "Negligible- first aid treatment";

        return Json(response);
    }

    public IActionResult ExposureChange(int exposureId)
    {
        var response = "";
        var exposure = (Exposure)exposureId;

        if (exposure == Exposure.Extensive)
            response = "Extensive- 80%-100%";

        if (exposure == Exposure.Widespread)
            response = "Widespread- 60%-80%";

        if (exposure == Exposure.Significant)
            response = "Significant- 40%-60%";

        if (exposure == Exposure.Restricted)
            response = "Restricted- 20%-40%";

        if (exposure == Exposure.Negligible)
            response = "Negligible- 1%-20%";

        return Json(response);
    }

    public IActionResult FrequencyChange(int frequencyId)
    {
        var response = "";
        var frequency = (Frequency)frequencyId;

        if (frequency == Frequency.Frequent)
            response = "Frequent- risk results in specific consequence continuously or daily";

        if (frequency == Frequency.Regular)
            response = "Regular- risk results in specific consequence more often than once per month";

        if (frequency == Frequency.Occasional)
            response = "Occasional- risk results in specific consequence a few times a year";

        if (frequency == Frequency.Uncommon)
            response = "Uncommon- risk results in specific consequence once or twice per 10 years";

        if (frequency == Frequency.Rare)
            response = "Rare- risk results in specific consequence less than once per 100 years";

        return Json(response);
    }
}
