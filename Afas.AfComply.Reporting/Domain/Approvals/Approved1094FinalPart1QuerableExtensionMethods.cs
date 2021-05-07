using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Approvals
{
    public static class Approved1094FinalPart1QuerableExtensionMethods
    {

        /// <summary>
        /// Filter the Approved1095s for only those approved by specific person.
        /// </summary>
        /// <param name="Approved1094s">The object ot filter.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Approved1094FinalPart1> FilterForApprovedBy(this IQueryable<Approved1094FinalPart1> Approved1094s, string Name)
        {
            return (
                    from Approved1094 in Approved1094s
                    where Approved1094.CreatedBy == Name
                    select Approved1094
                    );
        }


        /// <summary>
        /// Filter the Approved1095s for only those approved by specific person.
        /// </summary>
        /// <param name="Approved1094s">The object ot filter.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Approved1094FinalPart1> FilterForTaxYear(this IQueryable<Approved1094FinalPart1> Approved1094s, int TaxYear)
        {
            return (
                    from Approved1094 in Approved1094s
                    where Approved1094.TaxYearId == TaxYear
                    select Approved1094
                    );
        }

        /// <summary>
        /// Filter the Approved1095s for only those approved by specific person.
        /// </summary>
        /// <param name="Approved1094s">The object ot filter.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Approved1094FinalPart1> FilterForEmployer(this IQueryable<Approved1094FinalPart1> Approved1094s, int EmployerId)
        {
            return (
                    from Approved1095 in Approved1094s
                    where Approved1095.EmployerId == EmployerId
                    select Approved1095
                    );
        }

        /// <summary>
        /// Filter the Approved1095s for only those approved by specific person.
        /// </summary>
        /// <param name="Approved1094s">The object ot filter.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Approved1094FinalPart1> FilterForEmployee(this IQueryable<Approved1094FinalPart1> Approved1094s, int EmployeeId)
        {
            return (
                    from Approved1094 in Approved1094s
                    where Approved1094.EmployerId == EmployeeId
                    select Approved1094
                    );
        }

    }
}
