using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Extension methods for IList<insurance> lists.
/// </summary>
public static class IListOfInsuranceExtensionMethods
{

    /// <summary>
    /// Filters a list of employee's with the same database primary key.
    /// </summary>
    public static IQueryable<insurance> FilterForInsuranceId(this IList<insurance> insurances, int insuranceId)
    {

        return
            (
                from insurance_ in insurances
                where
                    insurance_.INSURANCE_ID == insuranceId
                select
                    insurance_
            ).AsQueryable();

    }

    /// <summary>
    /// Filters a list of employee's with the same name.
    /// </summary>
    public static IQueryable<insurance> FilterForInsuranceName(this IList<insurance> insurances, String insuranceName)
    {

        return
            (
                from insurance_ in insurances
                where
                    insurance_.INSURANCE_NAME == insuranceName
                select
                    insurance_
            ).AsQueryable();

    }

    /// <summary>
    /// Filter for a specific planYearId
    /// </summary>
    public static IQueryable<insurance> FilterForPlanYearByPlanYearId(this IList<insurance> insurances, int planYearId)
    {

        return
            (
                from insurance_ in insurances
                where
                    insurance_.INSURANCE_PLAN_YEAR_ID == planYearId
                select
                    insurance_
            ).AsQueryable();

    }

}
