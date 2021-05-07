using Afas.AfComply.Reporting.Domain.Reporting;
using Afas.Application;
using System.Collections.Generic;
using System.Linq;

namespace Afas.AfComply.Reporting.Application
{

    /// <summary>
    /// 
    /// </summary>
    public interface IUserReviewedService : ICrudDomainService<UserReviewed>
    {

        /// <summary>
        /// Dactivates the reviewed for this value.
        /// </summary>
        /// <param name="review">The review data to update.</param>
        /// <param name="requestor">The user making the edit.</param>
        void DeactivateReview(UserReviewed review, string requestor);

        /// <summary>
        /// Updates the Reviews.
        /// </summary>
        /// <param name="review">The Reviews to update.</param>
        /// <param name="requestor">The user making the edit.</param>
        void UpdateReviewed(UserReviewed review, string requestor);

        /// <summary>
        /// Filters the Values for ones belonging to an employer. 
        /// </summary>
        /// <param name="employerId">The Employer Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values</returns>
        IQueryable<UserReviewed> GetForEmployerTaxYear(int employerId, int taxYear);

        /// <summary>
        /// Filters the Values for ones belonging to an employer and this tax year. 
        /// </summary>
        /// <param name="employerId">The Employer Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values, as an Employee Id Keyed Dictionary</returns>
        Dictionary<int, List<UserReviewed>> GetForEmployerTaxYearDictionary(int employerId, int taxYear);

        /// <summary>
        /// Filters the Values for ones belonging to a perticular employee. 
        /// </summary>
        /// <param name="employeeId">The Employee Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values</returns>
        IQueryable<UserReviewed> GetForEmployeeTaxYear(int employeeId, int taxYear);

        /// <summary>
        /// This method marks many Employees as reviewed, Identified by using their EmployeeId.
        /// </summary>
        /// <param name="ReviewedEmployeesIds"> The list if Ids for the employees that are to be marked as reviewed</param>
        /// <param name="employerId">The Employer these Employee's belong to</param>
        /// <param name="taxYearId">The Tax Year to Review them for.</param>
        /// <param name="userId">The User who Reviewed them.</param>
        void MarkReviewed(List<int> ReviewedEmployeesIds, int employerId, int taxYearId, string userId);

        /// <summary>
        /// This method marks many Employees as NOT Reviewed, Identified by using their EmployeeId.
        /// </summary>
        /// <param name="UnReviewedEmployeesIds"> The list if Ids for the employees that are to be marked as NOT reviewed</param>
        /// <param name="employerId">The Employer these Employee's belong to</param>
        /// <param name="taxYearId">The Tax Year to UnReview them for.</param>
        /// <param name="userId">The User who UnReviewed them.</param>
        void MarkAsNotReviewed(List<int> UnReviewedEmployeesIds, int employerId, int taxYearId, string userId);

    }
}
