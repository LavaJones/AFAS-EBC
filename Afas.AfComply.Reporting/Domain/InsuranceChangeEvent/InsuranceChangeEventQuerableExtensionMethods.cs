using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.InsuranceChangeEvent
{
    /// <summary>
    /// Common Data Filtering extension methods for the TimeFrame object
    /// </summary>
    public static class InsuranceChangeEventQuerableExtensionMethods
    {

        /// <summary>
        /// Filter the Insurance Change Events by PlanYearID and EmployeeID
        /// </summary>
        /// <param name="timeFrames">The object to filter.</param>
        /// <param name="Year">The Year to filter for.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<InsuranceChangeEvent> FilterForCalendarYear(this IQueryable<InsuranceChangeEvent> changeEvents, int PlanYearID, int EmployeeID)
        {

            return (
                    from InsuranceChangeEvent in changeEvents
                    where InsuranceChangeEvent.PlanYearID == PlanYearID
                    select InsuranceChangeEvent
                    );

        }
                
      
    }
}
