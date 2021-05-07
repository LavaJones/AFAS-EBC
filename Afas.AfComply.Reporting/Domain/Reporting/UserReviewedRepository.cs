using Afas.Domain;
using Afc.Framework.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Afas.AfComply.Reporting.Domain.Reporting
{
    /// <summary>
    ///  specific repository.
    /// </summary>
    public class UserReviewedRepository : BaseDomainRepository<UserReviewed, IReportingDataContext>, IUserReviewedRepository
    {
        private readonly ILog Log = LogManager.GetLogger(typeof(UserReviewedRepository));

        /// <summary>
        /// Saves a batch of Data to the DB using SqlBulkCopy to improve performance
        /// </summary>
        /// <param name="reviews">The data to save</param>
        /// <param name="requestor">The user saving this data</param>
        /// <param name="IsUpdate">Is Update tells the system that it should keep any Id's that are provided.</param>
        /// <param name="Deactivate"> Deactivate is a special flag that marks all of the edits as inactive</param>
        public void SqlBulkUpdate(IEnumerable<UserReviewed> reviews, string requestor, bool IsUpdate = false, bool Deactivate = false)
        {

            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(UserReviewedRepository), "SqlBulkUpdate", SystemSettings.UsePerformanceLog))
            {

                methodTimer.StartTimer("SqlBulkCopy All Loops");
                methodTimer.StartTimer("Prepare the Data Table and Fill in Rows");
                methodTimer.StartLapTimerPaused("Process every item into a DataTable Row");

                // These 3 values are used to batch the SQL Bulk calls so that we don't overload the DB
                List<UserReviewed> allReviews = reviews.ToList();
                int bulkBatchSize = 100000;
                int iteration = 0;

                // Loop through everything as long as we have more data to save.
                while (allReviews.Count > bulkBatchSize * iteration)
                {
                    // skip any rows we've already processed, then take the next batch worth
                    reviews = allReviews.Skip(bulkBatchSize * iteration).Take(bulkBatchSize);
                    // increment the counter so next loop we grab the next batch
                    iteration++;

                    // Set up the Data Table so that we can run the bulk update
                    DataTable table = new DataTable("UserRevieweds");
                    // Set each DB colum as a column in the DataTable
                    table.Columns.Add("UserReviewedId", typeof(long));//[UserReviewedId] [bigint] IDENTITY(1,1) NOT NULL,
                    table.Columns.Add("TaxYear", typeof(int));//[TaxYear] [int] NOT NULL,
                    table.Columns.Add("EmployeeId", typeof(int));//[EmployeeId] [int] NOT NULL,
                    table.Columns.Add("EmployerId", typeof(int));//[EmployerId] [int] NOT NULL,
                    table.Columns.Add("ReviewedBy", typeof(string));//[ReviewedBy] [nvarchar] (max) NOT NULL,
                    table.Columns.Add("ReviewedOn", typeof(DateTime));//[ReviewedOn] [datetime] NOT NULL,
                    table.Columns.Add("ResourceId", typeof(Guid)); //[ResourceId] [uniqueidentifier] NOT NULL,
                    table.Columns.Add("EntityStatusId", typeof(int));//[EntityStatusId] [int] NOT NULL,
                    table.Columns.Add("CreatedBy", typeof(string));//[CreatedBy] [nvarchar](50) NOT NULL,
                    table.Columns.Add("CreatedDate", typeof(DateTime));//[CreatedDate] [datetime] NOT NULL,
                    table.Columns.Add("ModifiedBy", typeof(string));//[ModifiedBy] [nvarchar](50) NOT NULL,
                    table.Columns.Add("ModifiedDate", typeof(DateTime));//[ModifiedDate] [datetime] NOT NULL,

                    methodTimer.ResumeLap("Process every item into a DataTable Row");

                    // PreProcess each item for the database
                    foreach (UserReviewed item in reviews)
                    {
                        // Make sure that the Created By is set
                        if (item.CreatedBy.IsNullOrEmpty())
                        {
                            item.CreatedBy = requestor;
                        }

                        // Set the Mod By/On values
                        item.ModifiedBy = requestor;
                        item.ModifiedDate = DateTime.Now;

                        // Create a new Data Table row
                        DataRow row = table.NewRow();

                        // if it's new, set the ID to null, if it's an edit then set the value
                        row["UserReviewedId"] = DBNull.Value;
                        if (IsUpdate)
                        {
                            row["UserReviewedId"] = item.ID.OrDBNull();
                        }

                        // Set the rest of the Values
                        row["TaxYear"] = item.TaxYear.OrDBNull();
                        row["EmployeeId"] = item.EmployeeId.OrDBNull();
                        row["EmployerId"] = item.EmployerId.OrDBNull();
                        row["ReviewedBy"] = item.ReviewedBy.OrDBNull();
                        row["ReviewedOn"] = item.ReviewedOn.OrDBNull();
                        // Set the standard Values
                        row["ResourceId"] = item.ResourceId.OrDBNull();
                        // Deactivate is a special flag that marks all of the edits as inactive
                        if (Deactivate || item.EntityStatus == Afc.Core.Domain.EntityStatusEnum.Inactive)
                        {// if it is inactive then set this
                            row["EntityStatusId"] = 2;
                        }//Note: Deleted is not supported in Bulk Updates
                        else
                        {//default the entity to active
                            row["EntityStatusId"] = 1;
                        }
                        row["CreatedBy"] = item.CreatedBy.OrDBNull();
                        row["CreatedDate"] = item.CreatedDate.OrDBNull();
                        row["ModifiedBy"] = item.ModifiedBy.OrDBNull();
                        row["ModifiedDate"] = item.ModifiedDate.OrDBNull();

                        // Add it to the Data table of rows to be ran
                        table.Rows.Add(row);

                        methodTimer.Lap("Process every item into a DataTable Row");

                    }

                    methodTimer.LogAllLapsAndDispose("Process every item into a DataTable Row");
                    methodTimer.LogTimeAndDispose("Prepare the Data Table and Fill in Rows");

                    methodTimer.StartTimer("SqlBulkCopy Save Section");

                    // Set up SqlBulkCopy settings
                    string connection = ((ReportingDataContext)this.Context).Database.Connection.ConnectionString;
                    SqlBulkCopyOptions options = SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.FireTriggers;
                    if (IsUpdate)
                    {
                        options = options | SqlBulkCopyOptions.KeepIdentity;
                    }
                    // Connect to the DB and Bulk Copy the Data over
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, options))
                    {

                        bulkCopy.DestinationTableName = "[aca].[dbo].[UserRevieweds]";
                        bulkCopy.EnableStreaming = true;

                        try
                        {
                            methodTimer.StartTimer("SqlBulkCopy WriteToServer Method Call");

                            // Write from the source to the destination.
                            bulkCopy.WriteToServer(table);

                            methodTimer.LogTimeAndDispose("SqlBulkCopy WriteToServer Method Call");

                        }
                        catch (Exception ex)
                        {

                            this.Log.Error(ex.Message);

                            methodTimer.LogTimeAndDispose("SqlBulkCopy WriteToServer Method Call");

                        }

                    }

                    methodTimer.LogTimeAndDispose("SqlBulkCopy Save Section");
                    methodTimer.Lap("SqlBulkCopy All Loops");

                }

                methodTimer.LogAllLapsAndDispose("SqlBulkCopy All Loops");

            }

        }

        /// <summary>
        /// Dactivates the reviewed for this value.
        /// </summary>
        /// <param name="review">The review data to update.</param>
        /// <param name="requestor">The user making the edit.</param>
        void IUserReviewedRepository.DeactivateThisReview(UserReviewed review, string requestor)
        {

            List<UserReviewed> reviews = new List<UserReviewed>();

            // Set the EntityStatus to Inactive and set the ModBy Values
            review.ModifiedBy = requestor;
            review.ModifiedDate = DateTime.Now;
            review.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Inactive;

            reviews.Add(review);

            // Run this using the SQLBulkCopy Code

            // Update the User Reviews
            this.SqlBulkUpdate(reviews, requestor, true, true);

        }

        /// <summary>
        /// Dactivates any existing reviews for this Review.
        /// </summary>
        /// <param name="review">The review to replace old reviews.</param>
        /// <param name="requestor">The user making the edit.</param>
        void IUserReviewedRepository.DeactivateOldReviews(UserReviewed review, string requestor)
        {

            IEnumerable<UserReviewed> oldItems = this.Context.Set<UserReviewed>()
                .FilterForActive()
                .FilterForOther(review);

            // This will set the modified values, mark as Inactive, and make sure the entity is attached ot the DB
            ((IUserReviewedRepository)this).DeactivateTheseReviews(oldItems, requestor);

        }

        /// <summary>
        /// Dactivates the reviewed for this value.
        /// </summary>
        /// <param name="reviews">The reviews data to update.</param>
        /// <param name="requestor">The user making the edit.</param>
        void IUserReviewedRepository.DeactivateTheseReviews(IEnumerable<UserReviewed> reviews, string requestor)
        {

            // We'll just reuse the other logic but in a loop
            foreach (UserReviewed review in reviews)
            {

                // Set the EntityStatus to Inactive and set the ModBy Values
                review.ModifiedBy = requestor;
                review.ModifiedDate = DateTime.Now;
                review.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Inactive;

            }

            // Run this using the SQLBulkCopy Code
            if (reviews.Count() > 0)
            {
                // Insert the New User Reviews
                this.SqlBulkUpdate(reviews, requestor, true, true);
            }

        }

        /// <summary>
        /// This inserts the provided rows into the database without first checking for duplicates.
        /// </summary>
        /// <param name="reviews">The User Revieweds to save to the DB</param>
        /// <param name="requestor">The user doing the Reviewing</param>
        void IUserReviewedRepository.MarkAllReviewedNoCheck(IEnumerable<UserReviewed> reviews, string requestor)
        {
            // This assumes that the inputs have already been checked for duplicates and will insert whatever it recieves

            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(UserReviewedRepository), "MarkAllReviewedNoCheck", SystemSettings.UsePerformanceLog))
            {

                methodTimer.StartTimer("Set Defaults and Requestor");

                // just iterate through them to make sure the defines values are set for new items.
                foreach (UserReviewed review in reviews)
                {

                    // Set the system controled values (CreatedBy, ModBy, etc.)
                    review.ReviewedBy = requestor;
                    review.ReviewedOn = DateTime.Now;
                    review.CreatedBy = requestor;
                    review.ModifiedBy = requestor;
                    review.ModifiedDate = DateTime.Now;
                    review.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;

                    methodTimer.Lap("Set Defaults and Requestor");

                }

                methodTimer.LogAllLapsAndDispose("Set Defaults and Requestor");

                //Use the Bulk Insert to put them into the Database

                methodTimer.StartTimer("SqlBulkUpdate Insert New Reviews");

                // Insert the New User Reviews
                this.SqlBulkUpdate(reviews, requestor, false);

                methodTimer.LogTimeAndDispose("SqlBulkUpdate Insert New Reviews");

            }

        }

        /// <summary>
        /// This Deactivates the provided rows in the database without checking for duplicates.
        /// </summary>
        /// <param name="reviews">The User Revieweds to Deactivate in the DB</param>
        /// <param name="requestor">The user doing the Reviewing</param>
        void IUserReviewedRepository.UnReviewAllNoCheck(IEnumerable<UserReviewed> reviews, string requestor)
        {
            // This assumes that the inputs have already been checked for duplicates and will insert whatever it recieves

            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(UserReviewedRepository), "MarkAllReviewedNoCheck", SystemSettings.UsePerformanceLog))
            {

                methodTimer.StartTimer("Set Inactive and Requestor");

                // just iterate through them to make sure the defines values are set for new items.
                foreach (UserReviewed review in reviews)
                {

                    // Set the system controled values (ModBy/On, EntityStatus, etc.)
                    review.ModifiedBy = requestor;
                    review.ModifiedDate = DateTime.Now;
                    review.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Inactive;

                    methodTimer.Lap("Set Inactive and Requestor");

                }

                methodTimer.LogAllLapsAndDispose("Set Inactive and Requestor");

                //Use the Bulk Insert to put them into the Database

                methodTimer.StartTimer("SqlBulkUpdate Update Existing Reviews");

                // Insert the New User Reviews
                this.SqlBulkUpdate(reviews, requestor, true);

                methodTimer.LogTimeAndDispose("SqlBulkUpdate Update Existing Reviews");

            }

        }

        /// <summary>
        /// Updates the Reviews.
        /// </summary>
        /// <param name="review">The Reviews to update.</param>
        /// <param name="requestor">The user making the edit.</param>
        void IUserReviewedRepository.UpdateWithReviewed(UserReviewed review, string requestor)
        {
            List<UserReviewed> reviews = new List<UserReviewed>();

            // Set the system controled values (CreatedBy, ModBy, etc.)
            review.ReviewedBy = requestor;
            review.ReviewedOn = DateTime.Now;
            review.CreatedBy = requestor;
            review.ModifiedBy = requestor;
            review.ModifiedDate = DateTime.Now;
            review.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;

            reviews.Add(review);

            // Deactivate any old reviews that may exist
            ((IUserReviewedRepository)this).DeactivateOldReviews(review, requestor);

            // Run this using the SQLBulkCopy Code

            // Insert the New User Reviews
            this.SqlBulkUpdate(reviews, requestor, false);

        }

        /// <summary>
        /// Filters the Values for ones belonging to a perticular employee and this tax year. 
        /// </summary>
        /// <param name="employeeId">The Employee Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values</returns>
        IQueryable<UserReviewed> IUserReviewedRepository.GetForEmployeeTaxYear(int employeeId, int taxYear)
        {

            return this.Context.Set<UserReviewed>()
                .FilterForActive()
                .FilterForTaxYear(taxYear)
                .FilterForEmployee(employeeId);

        }

        /// <summary>
        /// Filters the Values for ones belonging to an employer and this tax year.  
        /// </summary>
        /// <param name="employerId">The Employer Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values</returns>
        IQueryable<UserReviewed> IUserReviewedRepository.GetForEmployerTaxYear(int employerId, int taxYear)
        {

            return this.Context.Set<UserReviewed>()
                .FilterForActive()
                .FilterForTaxYear(taxYear)
                .FilterForEmployer(employerId);

        }

        /// <summary>
        /// Filters the Values for ones belonging to an employer and this tax year. 
        /// </summary>
        /// <param name="employerId">The Employer Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values, as an Employee Id Keyed Dictionary</returns>
        Dictionary<int, List<UserReviewed>> IUserReviewedRepository.GetForEmployerTaxYearDictionary(int employerId, int taxYear)
        {
            // Do a raw query for just the Active User Reviewed for this Employer & Tax Year
            IQueryable<UserReviewed> query = this.Context.Set<UserReviewed>()
                .FilterForActive()
                .FilterForTaxYear(taxYear)
                .FilterForEmployer(employerId);

            // Force the DB to execute the query and then we'll do the rest on the web server
            List<UserReviewed> querycalled = query.ToList();
            if (querycalled.Count() <= 0)// This is mostly to be sure the query ran, and acts as a psuedo escape condition.
            {
                return new Dictionary<int, List<UserReviewed>>();
            }

            // Build the Query into a Dictionary and return that
            return (from UserReviewed review in querycalled
                    group review by review.EmployeeId into emp
                    select emp.ToList()).ToDictionary(items => items.First().EmployeeId, items => items);

        }

    }

}
