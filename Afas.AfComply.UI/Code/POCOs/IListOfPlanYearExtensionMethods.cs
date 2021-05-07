using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Extension methods for IList<PlanYear> lists.
/// </summary>
public static class IListOfPlanYearExtensionMethods
{

    /// <summary>
    /// Filters a list of PlanYear's with the same database primary key.
    /// </summary>
    public static IQueryable<PlanYear> FilterForPlanYearId(this IList<PlanYear> planYears, int planYearId)
    {

        return
            (
                from planYear in planYears
                where
                    planYear.PLAN_YEAR_ID == planYearId
                select
                    planYear
            ).AsQueryable();

    }

}
