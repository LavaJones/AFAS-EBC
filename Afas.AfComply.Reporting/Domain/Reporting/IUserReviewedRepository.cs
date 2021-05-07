using Afc.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Afas.AfComply.Reporting.Domain.Reporting
{
    /// <summary>
    ///  specific repository.
    /// </summary>
    public interface IUserReviewedRepository : IDomainRepository<UserReviewed>
    {

        /// <summary>
        /// Dactivates any existing reviews for this Review.
        /// </summary>
        /// <param name="review">The review to replace old reviews.</param>
        /// <param name="requestor">The user making the edit.</param>
        void DeactivateOldReviews(UserReviewed review, string requestor);

        /// <summary>
        /// Dactivates the review provided.
        /// </summary>
        /// <param name="review">The review data to deactivate.</param>
        /// <param name="requestor">The user making the edit.</param>
        void DeactivateThisReview(UserReviewed review, string requestor);

        /// <summary>
        /// Dactivates all the reviews provided
        /// </summary>
        /// <param name="reviews">The list of review data to Deactivate.</param>
        /// <param name="requestor">The user making the edit.</param>
        void DeactivateTheseReviews(IEnumerable<UserReviewed> reviews, string requestor);

        /// <summary>
        /// Updates the Reviews.
        /// </summary>
        /// <param name="review">The Reviews to update.</param>
        /// <param name="requestor">The user making the edit.</param>
        void UpdateWithReviewed(UserReviewed review, string requestor);

        ///// <summary>
        ///// Updates the Reviews.
        ///// </summary>
        ///// <param name="reviewed">The Reviews to update.</param>
        ///// <param name="employerId">The Employer Id.</param>
        ///// <param name="taxYear">The Tax Year.</param>
        ///// <param name="requestor">The user making the edit.</param>
        //void UpdateWithReviewsMany(IList<UserReviewed> reviewed, int employerId, int taxYear, string requestor);

        /// <summary>
        /// Filters the Values for ones belonging to a perticular employee. 
        /// </summary>
        /// <param name="employeeId">The Employee Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values</returns>
        IQueryable<UserReviewed> GetForEmployeeTaxYear(int employeeId, int taxYear);

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
        /// This inserts the provided rows into the database without first checking for duplicates.
        /// </summary>
        /// <param name="reviews">The User Revieweds to save to the DB</param>
        /// <param name="requestor">The user doing the Reviewing</param>
        void MarkAllReviewedNoCheck(IEnumerable<UserReviewed> reviews, string requestor);

        /// <summary>
        /// This Deactivates the provided rows in the database without checking for duplicates.
        /// </summary>
        /// <param name="reviews">The User Revieweds to Deactivate in the DB</param>
        /// <param name="requestor">The user doing the Reviewing</param>
        void UnReviewAllNoCheck(IEnumerable<UserReviewed> reviews, string requestor);

    }

}
