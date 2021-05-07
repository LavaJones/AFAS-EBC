using Afas.AfComply.Reporting.Domain.Printing;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Printing
{
    /// <summary>
    ///  specific repository.
    /// </summary>
    public interface IPrintBatchRepository : IDomainRepository<PrintBatch>
    {

        /// <summary>
        /// Get all batches and their child 1095s
        /// </summary>
        /// <returns>All print batches</returns>
        IQueryable<PrintBatch> GetAllPrintBatchesAndPrints();

        /// <summary>
        /// Filters to only batches that match the Employer and Tax Year
        /// </summary>
        /// <param name="employerId">The Employer to match to.</param>
        /// <param name="taxYear">The Tax Year to match for.</param>
        /// <returns>All print batches that match that employer tax year combo</returns>
        IQueryable<PrintBatch> FilterForEmployerTaxYear(int employerId, int taxYear);

        /// <summary>
        /// Gets a Print Batch by it's Filename
        /// </summary>
        /// <param name="fileName">The File Name</param>
        /// <returns>Any Print Batches that match the file name</returns>
        IQueryable<PrintBatch> FilterForFileName(string fileName);

        /// <summary>
        /// Saves a new Print Batch to the DB
        /// </summary>
        /// <param name="toSave">The Batch to save</param>
        /// <param name="requestor">The User that created this batch</param>
        /// <returns>Success</returns>
        bool SaveBatch(PrintBatch toSave, string requestor);

        /// <summary>
        /// Updates a Print Batch in the DB
        /// </summary>
        /// <param name="toSave">The Batch to save</param>
        /// <returns>Success</returns>
        bool UpdateBatchReceived(PrintBatch toUpdate, string requestor);

    }
}
 