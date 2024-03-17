using System;

namespace VehiGate.Application.Common.Helpers;

public static class InspectionHelper
{
    public static bool IsAuthorized(DateTime authorizedFrom, DateTime authorizedTo)
    {
        return DateTime.Now.Date.CompareTo(authorizedFrom.Date) >= 0 && DateTime.Now.Date.CompareTo(authorizedTo.Date) <= 0;
    }
}
