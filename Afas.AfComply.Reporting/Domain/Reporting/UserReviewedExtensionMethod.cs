using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.Reporting
{
    public static class UserReviewedExtensionMethod
    {
        /// <summary>
        /// Filters for ones that match a perticular other item
        /// </summary>
        /// <param name="All">The items to filter.</param>
        /// <param name="Other">The other item to filter for matches.</param>
        /// <returns>The Items that match.</returns>
        public static IQueryable<UserReviewed> FilterForOther(this IQueryable<UserReviewed> All, UserReviewed Other)
        {

            return (
                    from UserReviewed in All
                    where UserReviewed.EmployeeId == Other.EmployeeId
                    && UserReviewed.EmployerId == Other.EmployerId
                    && UserReviewed.TaxYear == Other.TaxYear
                    select UserReviewed
                    );

        }

        /// <summary>
        /// Filters the Values for ones belonging to an perticular employee. 
        /// </summary>
        /// <param name="employeeId">The Employee Id.</param>
        /// <returns>The filtered values</returns>
        public static IQueryable<UserReviewed> FilterForEmployee(this IQueryable<UserReviewed> All, int employeeId)
        {

            return (
                from UserReviewed in All
                where UserReviewed.EmployeeId == employeeId
                select UserReviewed
                );

        }

        /// <summary>
        /// Filters the Values for ones belonging to an employer. 
        /// </summary>
        /// <param name="employerId">The Employer Id.</param>
        /// <returns>The filtered values</returns>
        public static IQueryable<UserReviewed> FilterForEmployer(this IQueryable<UserReviewed> All, int employerId)
        {

            return (
                from UserReviewed in All
                where UserReviewed.EmployerId == employerId
                select UserReviewed
                );

        }

        /// <summary>
        /// Filters the Values for ones belonging to a Tax Year. 
        /// </summary>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values</returns>
        public static IQueryable<UserReviewed> FilterForTaxYear(this IQueryable<UserReviewed> All, int taxYear)
        {

            return (
                from UserReviewed in All
                where UserReviewed.TaxYear == taxYear
                select UserReviewed
                );

        }

    }

}
