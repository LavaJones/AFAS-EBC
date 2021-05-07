using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Approvals
{
    public static class Approved1095FinalQuerableExtensionMethods
    {

        /// <summary>
        /// Filter the Approved1095s for only those approved by specific person.
        /// </summary>
        /// <param name="Approved1095s">The object ot filter.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Approved1095Final> FilterForApprovedBy(this IQueryable<Approved1095Final> Approved1095s, string Name)
        {
            return (
                    from Approved1095 in Approved1095s
                    where Approved1095.CreatedBy == Name
                    select Approved1095
                    );
        }


        /// <summary>
        /// Filter the Approved1095s for only those approved by specific person.
        /// </summary>
        /// <param name="Approved1095s">The object ot filter.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Approved1095Final> FilterForTaxYear(this IQueryable<Approved1095Final> Approved1095s, int TaxYear)
        {
            return (
                    from Approved1095 in Approved1095s
                    where Approved1095.TaxYear == TaxYear
                    select Approved1095
                    );
        }

        /// <summary>
        /// Filter the Approved1095s for only those approved by specific person.
        /// </summary>
        /// <param name="Approved1095s">The object ot filter.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Approved1095Final> FilterForEmployer(this IQueryable<Approved1095Final> Approved1095s, int EmployerId)
        {
            return (
                    from Approved1095 in Approved1095s
                    where Approved1095.EmployerID == EmployerId
                    select Approved1095
                    );
        }

        /// <summary>
        /// Filter the Approved1095s for only those approved by specific person.
        /// </summary>
        /// <param name="Approved1095s">The object ot filter.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Approved1095Final> FilterForEmployee(this IQueryable<Approved1095Final> Approved1095s, int EmployeeId)
        {
            return (
                    from Approved1095 in Approved1095s
                    where Approved1095.EmployeeID == EmployeeId
                    select Approved1095
                    );
        }

    }
}
