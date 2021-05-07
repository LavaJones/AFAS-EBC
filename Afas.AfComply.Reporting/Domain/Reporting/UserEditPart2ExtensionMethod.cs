using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.Reporting
{
    public static class UserEditPart2ExtensionMethod 
    {
        public static IQueryable<UserEditPart2> FilterForOther(this IQueryable<UserEditPart2> All, UserEditPart2 Other)
        {

            return (
                    from UserEditPart2 in All
                    where UserEditPart2.EmployeeId == Other.EmployeeId
                    && UserEditPart2.EmployerId == Other.EmployerId
                    && UserEditPart2.TaxYear == Other.TaxYear
                    && UserEditPart2.MonthId == Other.MonthId
                    && UserEditPart2.LineId == Other.LineId
                    select UserEditPart2
                    );

        }

        /// <summary>
        /// Filters the Values for ones belonging to an employer. 
        /// </summary>
        /// <param name="employerId">The Employer Id.</param>
        /// <returns>The filtered values</returns>
        public static IQueryable<UserEditPart2> FilterForEmployer(this IQueryable<UserEditPart2> All, int employerId)
        {

            return (
                from UserEditPart2 in All
                where UserEditPart2.EmployerId == employerId
                select UserEditPart2
                );

        }

        /// <summary>
        /// Filters the Values for ones belonging to a Tax Year. 
        /// </summary>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values</returns>
        public static IQueryable<UserEditPart2> FilterForTaxYear(this IQueryable<UserEditPart2> All, int taxYear)
        {

            return (
                from UserEditPart2 in All
                where UserEditPart2.TaxYear == taxYear
                select UserEditPart2
                );

        }

    }

}
