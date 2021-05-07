using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Extension methods for IList<insuranceContribution> lists.
/// </summary>
public static class IListOfInsuranceContributionExtensionMethods
{

    /// <summary>
    /// Filters a list of employee's with the same database primary key.
    /// </summary>
    public static IQueryable<insuranceContribution> FilterForInsuranceContributionId(this IList<insuranceContribution> insuranceContributions, int insuranceContributionId)
    {

        return
            (
                from insuranceContribution_ in insuranceContributions
                where
                    insuranceContribution_.INS_CONT_ID == insuranceContributionId
                select
                    insuranceContribution_
            ).AsQueryable();

    }

}
