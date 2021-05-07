using Afas.AfComply.Reporting.Domain.Reporting;
using Afas.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Afas.AfComply.Reporting.Application.Services
{

    /// <summary>
    /// A service explosing access to the TimeFrame domain models.
    /// </summary>
    public class UserReviewedService : ABaseCrudService<UserReviewed>, IUserReviewedService
    {
        protected IUserReviewedRepository Repository { get; private set; }

        /// <summary>
        /// Standard Constructor taking the dependencies as parameters. 
        /// </summary>
        /// <param name="repository">The Repository to get the .</param>
        public UserReviewedService(
            IUserReviewedRepository repository) :
                base(repository)
        {

            this.Repository = repository;

        }

        /// <summary>
        /// Dactivates the reviewed for this value.
        /// </summary>
        /// <param name="review">The review data to update.</param>
        /// <param name="requestor">The user making the edit.</param>
        void IUserReviewedService.DeactivateReview(UserReviewed review, string requestor)
        {

            this.Repository.DeactivateOldReviews(review, requestor);

            this.Repository.SaveChanges();

        }

        /// <summary>
        /// Updates the Reviews.
        /// </summary>
        /// <param name="review">The Reviews to update.</param>
        /// <param name="requestor">The user making the edit.</param>
        void IUserReviewedService.UpdateReviewed(UserReviewed review, string requestor)
        {







            //This is breaking because EF doesn't like the triggers






            this.Repository.UpdateWithReviewed(review, requestor);

            this.Repository.SaveChanges();

        }

        /// <summary>
        /// Filters the Values for ones belonging to an employer. 
        /// </summary>
        /// <param name="employerId">The Employer Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values</returns>
        IQueryable<UserReviewed> IUserReviewedService.GetForEmployerTaxYear(int employerId, int taxYear)
        {

            return this.Repository.GetForEmployerTaxYear(employerId, taxYear);

        }

        /// <summary>
        /// Filters the Values for ones belonging to an employer and this tax year. 
        /// </summary>
        /// <param name="employerId">The Employer Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values, as an Employee Id Keyed Dictionary</returns>
        Dictionary<int, List<UserReviewed>> IUserReviewedService.GetForEmployerTaxYearDictionary(int employerId, int taxYear)
        {

            return this.Repository.GetForEmployerTaxYearDictionary(employerId, taxYear);

        }

        /// <summary>
        /// Filters the Values for ones belonging to a perticular employee. 
        /// </summary>
        /// <param name="employeeId">The Employee Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values</returns>
        IQueryable<UserReviewed> IUserReviewedService.GetForEmployeeTaxYear(int employeeId, int taxYear)
        {

            return this.Repository.GetForEmployeeTaxYear(employeeId, taxYear);

        }

        /// <summary>
        /// This method marks many Employees as reviewed, Identified by using their EmployeeId.
        /// </summary>
        /// <param name="ReviewedEmployeesIds"> The list if Ids for the employees that are to be marked as reviewed</param>
        /// <param name="employerId">The Employer these Employee's belong to</param>
        /// <param name="taxYearId">The Tax Year to Review them for.</param>
        /// <param name="userId">The User who Reviewed them.</param>
        void IUserReviewedService.MarkReviewed(List<int> ReviewedEmployeesIds, int employerId, int taxYearId, string userId)
        {

            List<UserReviewed> NewReviews = new List<UserReviewed>();

            // Get all the currently Reviewed Employees 
            Dictionary<int, List<UserReviewed>> AllReviewedEmployees = this.Repository.GetForEmployerTaxYearDictionary(employerId, taxYearId);

            // Iterate thorugh each one and create a new Review to save for each employee that doesn't already have a review.
            foreach (int EmployeeId in ReviewedEmployeesIds)
            {
                // Check if we have any existing reviews for this EmployeeId
                if (false == AllReviewedEmployees.ContainsKey(EmployeeId))
                {
                    // If the Employee Id is not already Reviewed, then create this as a new UserReviewed.
                    UserReviewed newReview = new UserReviewed
                    {
                        EmployeeId = EmployeeId,
                        EmployerId = employerId,
                        TaxYear = taxYearId,
                        ReviewedBy = userId,
                        ReviewedOn = DateTime.Now
                    };

                    // The repository will set the standard items like Created by, Mod By, Active, etc.

                    // Add it to the list for saving.
                    NewReviews.Add(newReview);

                }

            }

            // Save the new items to the DB
            // We already checked for duplicates
            this.Repository.MarkAllReviewedNoCheck(NewReviews, userId);

        }

        /// <summary>
        /// This method marks many Employees as NOT Reviewed, Identified by using their EmployeeId.
        /// </summary>
        /// <param name="UnReviewedEmployeesIds"> The list if Ids for the employees that are to be marked as NOT reviewed</param>
        /// <param name="employerId">The Employer these Employee's belong to</param>
        /// <param name="taxYearId">The Tax Year to UnReview them for.</param>
        /// <param name="userId">The User who UnReviewed them.</param>
        void IUserReviewedService.MarkAsNotReviewed(List<int> UnReviewedEmployeesIds, int employerId, int taxYearId, string userId)
        {

            List<UserReviewed> OldReviews = new List<UserReviewed>();

            // Get all the currently Reviewed Employees 
            Dictionary<int, List<UserReviewed>> AllReviewedEmployees = this.Repository.GetForEmployerTaxYearDictionary(employerId, taxYearId);

            // Iterate thorugh each one and for each Employee Id that has as Review, prep it for UnReview
            foreach (int EmployeeId in UnReviewedEmployeesIds)
            {
                // Check if we have any existing reviews for this EmployeeId
                if (AllReviewedEmployees.ContainsKey(EmployeeId))
                {

                    // Add the Exisiting Review(s) to the list to unreview
                    OldReviews.AddRange(AllReviewedEmployees[EmployeeId]);

                }

            }

            // Unreview the items in the DB
            // We already checked for duplicates*
            this.Repository.UnReviewAllNoCheck(OldReviews, userId);

        }

    }

}
