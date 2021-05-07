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
    public class UserEditPart2Repository : BaseDomainRepository<UserEditPart2, IReportingDataContext>, IUserEditPart2Repository
    {
        private static readonly PerformanceTiming PerfTimer = new PerformanceTiming(typeof(UserEditPart2Repository), null, SystemSettings.UsePerformanceLog);

        private readonly ILog Log = LogManager.GetLogger(typeof(UserEditPart2Repository));

        /// <summary>
        /// Updates the Part 2 data with edited values.
        /// </summary>
        /// <param name="edit">The Edit data to update.</param>
        /// <param name="requestor">The user making the edit.</param>
        void IUserEditPart2Repository.UpdateWithEdit(UserEditPart2 edit, string requestor)
        {

            edit.CreatedBy = requestor;
            edit.ModifiedBy = requestor;
            edit.ModifiedDate = DateTime.Now;
            edit.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
            IQueryable<UserEditPart2> oldItems = this.Context.Set<UserEditPart2>()
                .FilterForActive()
                .FilterForOther(edit);

            foreach (UserEditPart2 old in oldItems)
            {
                old.ModifiedBy = requestor;
                old.ModifiedDate = DateTime.Now;
                old.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Inactive;
            }

            this.Context.Set<UserEditPart2>().Add(edit);

        }

        /// <summary>
        /// Saves a batch of Data to the DB using SqlBulkCopy to improve performance
        /// </summary>
        /// <param name="edits">The data to save</param>
        /// <param name="requestor">The user saving this data</param>
        /// <param name="IsUpdate">Is Update tells the system that it should keep any Id's that are provided.</param>
        /// <param name="Deactivate"> Deactivate is a special flag that marks all of the edits as inactive</param>
        private void SqlBulkUpdate(IEnumerable<UserEditPart2> edits, string requestor, bool IsUpdate = false, bool Deactivate = false)
        {

            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(UserEditPart2Repository), "SqlBulkUpdate", SystemSettings.UsePerformanceLog))
            {

                methodTimer.StartTimer("SqlBulkCopy All Loops");
                methodTimer.StartTimer("Prepare the Data Table and Fill in Rows");
                methodTimer.StartLapTimerPaused("Process every item into a DataTable Row");


                DateTime modOnTime = DateTime.Now;

                // These 3 values are used to batch the SQL Bulk calls so that we don't overload the DB
                List<UserEditPart2> allEdits = edits.ToList();
                int bulkBatchSize = 100000;
                int iteration = 0;

                // Loop through everything as long as we have more data to save.
                while (allEdits.Count > bulkBatchSize * iteration)
                {
                    // skip any rows we've already processed, then take the next batch worth
                    edits = allEdits.Skip(bulkBatchSize * iteration).Take(bulkBatchSize);
                    // increment the counter so next loop we grab the next batch
                    iteration++;


                    // Set up the Data Table so that we can run the bulk update
                    DataTable table = new DataTable("UserEditPart2");
                    // Set each DB colum as a column in the DataTable
                    table.Columns.Add("UserEditPart2Id", typeof(long));//[UserEditPart2Id] [bigint] IDENTITY(1,1) NOT NULL,
                    table.Columns.Add("OldValue", typeof(string));//[OldValue] [nvarchar](max) NULL,
                    table.Columns.Add("NewValue", typeof(string));//[NewValue] [nvarchar](max) NULL,
                    table.Columns.Add("MonthId", typeof(int));//[MonthId] [int] NOT NULL,
                    table.Columns.Add("LineId", typeof(int));//[LineId] [int] NOT NULL,
                    table.Columns.Add("EmployeeId", typeof(int));//[EmployeeId] [int] NOT NULL,
                    table.Columns.Add("EmployerId", typeof(int));//[EmployerId] [int] NOT NULL,
                    table.Columns.Add("ResourceId", typeof(Guid)); //[ResourceId] [uniqueidentifier] NOT NULL,
                    table.Columns.Add("EntityStatusId", typeof(int));//[EntityStatusId] [int] NOT NULL,
                    table.Columns.Add("CreatedBy", typeof(string));//[CreatedBy] [nvarchar](50) NOT NULL,
                    table.Columns.Add("CreatedDate", typeof(DateTime));//[CreatedDate] [datetime] NOT NULL,
                    table.Columns.Add("ModifiedBy", typeof(string));//[ModifiedBy] [nvarchar](50) NOT NULL,
                    table.Columns.Add("ModifiedDate", typeof(DateTime));//[ModifiedDate] [datetime] NOT NULL,
                    table.Columns.Add("TaxYear", typeof(int));//[TaxYear] [int] NOT NULL,

                    methodTimer.ResumeLap("Process every item into a DataTable Row");

                    // PreProcess each item for the database
                    foreach (UserEditPart2 edit in edits)
                    {
                        // Make sure that the Created By is set
                        if (edit.CreatedBy.IsNullOrEmpty())
                        {
                            edit.CreatedBy = requestor;
                        }

                        edit.ModifiedBy = requestor;
                        edit.ModifiedDate = modOnTime;

                        // Create a new Data Table row
                        DataRow row = table.NewRow();

                        // if it's new, set the ID to null, if it's an edit then set the value
                        row["UserEditPart2Id"] = DBNull.Value;
                        if (IsUpdate)
                        {
                            row["UserEditPart2Id"] = edit.ID.OrDBNull();
                        }
                        // Set the rest of the Values
                        row["OldValue"] = edit.OldValue.OrDBNull();
                        row["NewValue"] = edit.NewValue.OrDBNull();
                        row["MonthId"] = edit.MonthId.OrDBNull();
                        row["LineId"] = edit.LineId.OrDBNull();
                        row["EmployeeId"] = edit.EmployeeId.OrDBNull();
                        row["EmployerId"] = edit.EmployerId.OrDBNull();
                        // Set the standard Values
                        row["ResourceId"] = edit.ResourceId.OrDBNull();
                        // Deactivate is a special flag that marks all of the edits as inactive
                        if (Deactivate || edit.EntityStatus == Afc.Core.Domain.EntityStatusEnum.Inactive)
                        {// if it is inactive then set this
                            row["EntityStatusId"] = 2;
                        }//Note: Deleted is not supported in Bulk Updates
                        else
                        {//default the entity to active
                            row["EntityStatusId"] = 1;
                        }
                        row["CreatedBy"] = edit.CreatedBy.OrDBNull();
                        row["CreatedDate"] = edit.CreatedDate.OrDBNull();
                        row["ModifiedBy"] = edit.ModifiedBy.OrDBNull();
                        row["ModifiedDate"] = edit.ModifiedDate.OrDBNull();
                        row["TaxYear"] = edit.TaxYear.OrDBNull();

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

                        bulkCopy.DestinationTableName = "[aca].[dbo].[UserEditPart2]";
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

                            methodTimer.LogTimeAndDispose("SqlBulkCopy WriteToServer Method Call");

                            this.Log.Error(ex.Message);

                        }

                    }

                    methodTimer.LogTimeAndDispose("SqlBulkCopy Save Section");

                    methodTimer.Lap("SqlBulkCopy All Loops");

                }

                methodTimer.LogAllLapsAndDispose("SqlBulkCopy All Loops");

            }

        }

        /// <summary>
        /// Updates the Part 2 data with edited values.
        /// </summary>
        /// <param name="edit">The Edit data to update.</param>
        /// <param name="employerId">The Employer Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <param name="requestor">The user making the edit.</param>
        void IUserEditPart2Repository.UpdateWithEditsMany(List<UserEditPart2> edits, int employerId, int taxYear, string requestor)
        {

            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(UserEditPart2Repository), "UpdateWithEditsMany", SystemSettings.UsePerformanceLog))
            {

                methodTimer.StartTimer("Data Prep Section");

                methodTimer.StartTimer("Pull fresh User Edits data from DB");

                // Get any active User Edits
                Dictionary<int, Dictionary<int, List<UserEditPart2>>> allOld = ((IUserEditPart2Repository)this).GetForEmployerTaxYear(employerId, taxYear);

                methodTimer.LogTimeAndDispose("Pull fresh User Edits data from DB");

                // Keep a list of the items needing to be deactivated
                List<UserEditPart2> deactivated = new List<UserEditPart2>();

                methodTimer.StartTimer("Individual User Edits PreProcessing");
                methodTimer.StartLapTimerPaused("Deactivate Old User Edits during PreProcessing");
                methodTimer.StartLapTimerPaused("Select the items that match this Edit");

                foreach (UserEditPart2 edit in edits)
                {
                    // For every edit we are saving, we check for existing edits on the employee
                    if (allOld.ContainsKey(edit.EmployeeId))
                    {
                        // Check if the existing edits match the specific month
                        if (allOld[edit.EmployeeId].ContainsKey(edit.MonthId))
                        {
                            methodTimer.ResumeLap("Select the items that match this Edit");

                            // grab any old edits that might be replaced
                            List<UserEditPart2> existingEdits = allOld[edit.EmployeeId][edit.MonthId];

                            methodTimer.Lap("Select the items that match this Edit");
                            methodTimer.PauseLap("Select the items that match this Edit");

                            // There should be a max of 4 items in this list.
                            foreach (UserEditPart2 existing in existingEdits)
                            {

                                // Make sure the Edit is for the correct line item
                                if (edit.LineId == existing.LineId)
                                {

                                    methodTimer.ResumeLap("Deactivate Old User Edits during PreProcessing");

                                    // Deactivate each existing edits that we find
                                    deactivated.Add(existing);

                                    methodTimer.Lap("Deactivate Old User Edits during PreProcessing");
                                    methodTimer.PauseLap("Deactivate Old User Edits during PreProcessing");

                                }
                            }
                        }
                    }

                    methodTimer.Lap("Individual User Edits PreProcessing");

                }

                methodTimer.LogAllLapsAndDispose("Select the items that match this Edit");
                methodTimer.LogAllLapsAndDispose("Deactivate Old User Edits during PreProcessing");
                methodTimer.LogAllLapsAndDispose("Individual User Edits PreProcessing");
                methodTimer.LogTimeAndDispose("Data Prep Section");

                methodTimer.StartTimer("SqlBulkUpdate Disabled Old User Edits");

                // disable the Existing Old Edits
                this.SqlBulkUpdate(deactivated, requestor, true, true);// the second true is for Deactivate, will force all to be inactive.
                methodTimer.LogTimeAndDispose("SqlBulkUpdate Disabled Old User Edits");

                methodTimer.StartTimer("SqlBulkUpdate Insert New User Edits");

                // insert the New Edits
                this.SqlBulkUpdate(edits, requestor, false);

                methodTimer.LogTimeAndDispose("SqlBulkUpdate Insert New User Edits");
            }
        }

        /// <summary>
        /// Filters the Values for ones belonging to an employer. 
        /// </summary>
        /// <param name="employerId">The Employer Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values</returns>
        Dictionary<int, Dictionary<int, List<UserEditPart2>>> IUserEditPart2Repository.GetForEmployerTaxYear(int employerId, int taxYear)
        {

            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(UserEditPart2Repository), "GetForEmployerTaxYear", SystemSettings.UsePerformanceLog))
            {

                //((DbContext)this.Context).Configuration.LazyLoadingEnabled = false;
                //((DbContext)this.Context).Configuration.AutoDetectChangesEnabled = false;
                //((DbContext)this.Context).Configuration.ProxyCreationEnabled = false;
                //((DbContext)this.Context).Configuration.ValidateOnSaveEnabled = false;

                methodTimer.StartTimer("Query for just the Edits for Employer and Tax Year");

                IQueryable<UserEditPart2> query = this.Context.Set<UserEditPart2>()
                    .AsNoTracking()
                    .FilterForActive()
                    .FilterForTaxYear(taxYear)
                    .FilterForEmployer(employerId);

                methodTimer.LogTimeAndDispose("Query for just the Edits for Employer and Tax Year");
                methodTimer.StartTimer("Force Query to run all at once.");

                // Force the DB to execute the query and then we'll do the rest on the web server
                List<UserEditPart2> querycalled = query.ToList();

                this.Log.Debug(string.Format("GetForEmployerTaxYear pulled [{0}] User Edits from the Database", querycalled.Count()));

                methodTimer.LogTimeAndDispose("Force Query to run all at once.");
                methodTimer.StartTimer("Convert the List to a Dictionary");

                return querycalled
                        .GroupBy(outer => outer.EmployeeId)
                        .ToDictionary(
                            outer => outer.First().EmployeeId,
                            outer => outer.GroupBy(inner => inner.MonthId)
                            .ToDictionary(inner => inner.First().MonthId, inner => inner.ToList()));

            }

        }

    }

}
