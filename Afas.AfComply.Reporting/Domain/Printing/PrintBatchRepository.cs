using Afas.Domain;
using Afas.Domain.POCO;
using Afc.Framework.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Printing
{

    /// <summary>
    ///  specific repository.
    /// </summary>
    public class PrintBatchRepository : BaseDomainRepository<PrintBatch, IReportingDataContext>, IPrintBatchRepository
    {

        private ILog Log = LogManager.GetLogger(typeof(PrintBatchRepository));

        /// <summary>
        /// This is a base that allows us to pull data from the table without lazy loading the 1094 and 1095 prints, but those sub children do not get loaded
        /// </summary>
        private IQueryable<PrintBatch> ContextWithChildrenLoaded
        {
            get
            {
                return Context
                        .Set<PrintBatch>()
                        .Include(batch => batch.AllPrinted1094s)
                        .Include(batch => batch.AllPrinted1095s)
                        ;
            }
        }

        /// <summary>
        /// This is a base that allows us to pull data from the table without lazy loading the 1094 and 1095 prints and sub children of the approveds
        /// </summary>
        private IQueryable<PrintBatch> ContextWithSubChildrenLoaded
        {
            get
            {
                return Context
                        .Set<PrintBatch>()
                        .Include(batch => batch.AllPrinted1095s.Select(print => print.Approved1095))
                        .Include(batch => batch.AllPrinted1094s.Select(print => print.Approved1094))
                        ;
            }
        }

        /// <summary>
        /// Get all batches and their child 1095s
        /// </summary>
        /// <returns>All print batches</returns>
        IQueryable<PrintBatch> IPrintBatchRepository.GetAllPrintBatchesAndPrints()
        {

            return Context
                        .Set<PrintBatch>()
                        .FilterForActive();

        }

        /// <summary>
        /// Filters to only batches that match the Employer and Tax Year
        /// </summary>
        /// <param name="employerId">The Employer to match to.</param>
        /// <param name="taxYear">The Tax Year to match for.</param>
        /// <returns>All print batches that match that employer tax year combo</returns>
        IQueryable<PrintBatch> IPrintBatchRepository.FilterForEmployerTaxYear(int employerId, int taxYear)
        {

            return ContextWithChildrenLoaded
                    .FilterForEmployerId(employerId)
                    .FilterForTaxYear(taxYear)
                    .FilterForActive();
                    

        }

        /// <summary>
        /// Gets a Print Batch by it's Filename
        /// </summary>
        /// <param name="fileName">The File Name</param>
        /// <returns>Any Print Batches that match the file name</returns>
        IQueryable<PrintBatch> IPrintBatchRepository.FilterForFileName(string fileName)
        {

            return ContextWithSubChildrenLoaded
                    .FilterForFileName(fileName)
                    .FilterForActive();

        }

        /// <summary>
        /// Saves a new Print Batch to the DB
        /// </summary>
        /// <param name="toSave">The Batch to save</param>
        /// <returns>Success</returns>
        bool IPrintBatchRepository.SaveBatch(PrintBatch toSave, string requestor)
        {
            var toSave1095s = toSave.AllPrinted1095s;

            toSave.AllPrinted1095s = new List<Print1095>();

            Context.Set<ArchiveFileInfo>().Attach(toSave.ArchivedFile);

            Context.Set<PrintBatch>().Add(toSave);

            Context.SaveChanges();

            SqlBulkUpdatePrint1095(toSave1095s, toSave.ID, requestor);

            return true;
        }

        /// <summary>
        /// Updates a Print Batch in the DB
        /// </summary>
        /// <param name="toSave">The Batch to save</param>
        /// <returns>Success</returns>
        bool IPrintBatchRepository.UpdateBatchReceived(PrintBatch toUpdate, string requestor)
        {

            // Detach, we only want to update PdfReceivedOn and mod by 
            ((DbContext)Context).Entry(toUpdate).State = EntityState.Detached;

            // Get it fresh from the DB
            PrintBatch fromDb = ((IPrintBatchRepository) this).GetByResourceId(toUpdate.ResourceId);
           
            // we only want to update PdfReceivedOn and mod by 
            fromDb.PdfReceivedOn = toUpdate.PdfReceivedOn;
            fromDb.ModifiedBy = requestor;
            fromDb.ModifiedDate = DateTime.Now;

            Context.SaveChanges();

            // Update 1095s in mass for any changes to the path
            SqlBulkUpdatePrint1095(toUpdate.AllPrinted1095s, toUpdate.ID, requestor, true);



            return true;

        }

        // BULK update the printed 1095s
        private void SqlBulkUpdatePrint1095(IEnumerable<Print1095> prints, long batchId, string requestor, bool IsUpdate = false)
        {

            // These 3 values are used to batch the SQL Bulk calls so that we don't overload the DB
            List<Print1095> allPrints = prints.ToList();
            int bulkBatchSize = 100000;
            int iteration = 0;

            // Loop through everything as long as we have more data to save.
            while (allPrints.Count > bulkBatchSize * iteration)
            {
                // skip any rows we've already processed, then take the next batch worth
                prints = allPrints.Skip(bulkBatchSize * iteration).Take(bulkBatchSize);
                // increment the counter so next loop we grab the next batch
                iteration++;
                

                DataTable table = new DataTable("Print1095");

                table.Columns.Add("Print1095Id", typeof(long));//[Print1095Id] [bigint] IDENTITY(1,1) NOT NULL,
                table.Columns.Add("OutputFilePath", typeof(string));//[OutputFilePath] [nvarchar](max) NOT NULL,

                table.Columns.Add("ResourceId", typeof(Guid)); //[ResourceId] [uniqueidentifier] NOT NULL,
                table.Columns.Add("EntityStatusId", typeof(int));//[EntityStatusId] [int] NOT NULL,
                table.Columns.Add("CreatedBy", typeof(string));//[CreatedBy] [nvarchar](50) NOT NULL,
                table.Columns.Add("CreatedDate", typeof(DateTime));//[CreatedDate] [datetime] NOT NULL,
                table.Columns.Add("ModifiedBy", typeof(string));//[ModifiedBy] [nvarchar](50) NOT NULL,
                table.Columns.Add("ModifiedDate", typeof(DateTime));//[ModifiedDate] [datetime] NOT NULL,

                table.Columns.Add("Approved1095_ID", typeof(long));//[Approved1095_ID] [bigint] NOT NULL,
                table.Columns.Add("PrintBatch_ID", typeof(long));//[PrintBatch_ID] [bigint] NOT NULL,


                foreach (Print1095 print in prints)
                {
                    if (null == print.CreatedBy || string.Empty == print.CreatedBy)
                    {
                        print.CreatedBy = requestor;
                    }

                    if (0 == (int)print.EntityStatus)
                    {
                        print.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
                    }

                    print.ModifiedBy = requestor;
                    print.ModifiedDate = DateTime.Now;

                    DataRow row = table.NewRow();
                    row["Print1095Id"] = print.ID.OrDBNull();

                    row["OutputFilePath"] = print.OutputFilePath.OrDBNull();

                    row["ResourceId"] = print.ResourceId.OrDBNull();
                    row["EntityStatusId"] = (int)print.EntityStatus.OrDBNull();
                    row["CreatedBy"] = print.CreatedBy.OrDBNull();
                    row["CreatedDate"] = print.CreatedDate.OrDBNull();
                    row["ModifiedBy"] = print.ModifiedBy.OrDBNull();
                    row["ModifiedDate"] = print.ModifiedDate.OrDBNull();

                    row["Approved1095_ID"] = print.Approved1095.ID.OrDBNull();
                    row["PrintBatch_ID"] = batchId.OrDBNull();

                    table.Rows.Add(row);

                }

                var connection = ((ReportingDataContext)this.Context).Database.Connection.ConnectionString;
                var options = SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.FireTriggers;
                if (IsUpdate)
                {
                    options = options | SqlBulkCopyOptions.KeepIdentity;
                }
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, options))
                {

                    bulkCopy.DestinationTableName =
                        "aca.dbo.Print1095";
                    bulkCopy.EnableStreaming = true;

                    try
                    {
                        // Write from the source to the destination.
                        bulkCopy.WriteToServer(table);
                    }
                    catch (Exception ex)
                    {
                        this.Log.Error(ex.Message);
                    }

                }

            }

        }

    }

}
