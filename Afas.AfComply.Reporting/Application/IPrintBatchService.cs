using Afas.AfComply.Reporting.Domain.Printing;
using Afas.AfComply.Reporting.Domain.Transmission;
using Afas.Application;
using Afc.Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Application
{

    /// <summary>
    /// 
    /// </summary>
    public interface IPrintBatchService : ICrudDomainService<PrintBatch>
    {

        /// <summary>
        /// Get all batches and their child 1095s
        /// </summary>
        /// <returns>All print batches</returns>
        IList<PrintBatch> GetAllPrintBatchesAndPrints();

        /// <summary>
        /// Filters to only batches that match the Employer and Tax Year and gets just their IDs
        /// </summary>
        /// <param name="employerId">The Employer to match to.</param>
        /// <param name="taxYear">The Tax Year to match for.</param>
        /// <returns>All the IDs of the print batches that match that employer tax year combo</returns>
        IList<long> GetIdsForEmployerTaxYear(int employerId, int taxYear);

        /// <summary>
        /// Filters to only batches that match the Employer and Tax Year
        /// </summary>
        /// <param name="employerId">The Employer to match to.</param>
        /// <param name="taxYear">The Tax Year to match for.</param>
        /// <returns>All print batches that match that employer tax year combo</returns>
        IList<PrintBatch> GetForEmployerTaxYear(int employerId, int taxYear);

        /// <summary>
        /// Checks if there are any open print batches for this employer year combination
        /// </summary>
        /// <param name="employerId">The Employer to match to.</param>
        /// <param name="taxYear">The Tax Year to match for.</param>
        /// <returns>True if there is a valid batch for the paramaeters</returns>
        bool HasPrintedEmployerTaxYear(int employerId, int taxYear);

        /// <summary>
        /// Gets a Print Batch by it's Filename
        /// </summary>
        /// <param name="fileName">The File Name</param>
        /// <returns>Any Print Batches that match the file name</returns>
        IList<PrintBatch> GetForFileName(string fileName);

        /// <summary>
        /// Saves a new Print Batch to the DB
        /// </summary>
        /// <param name="toSave">The Batch to save</param>
        /// <returns>Success</returns>
        bool SaveBatch(PrintBatch toSave, string requestor);

        /// <summary>
        /// Updates a Print Batch to the DB
        /// </summary>
        /// <param name="toSave">The Batch to save</param>
        /// <returns>Success</returns>
        bool UpdateBatchReceived(PrintBatch toSave, string requestor);

    }
}
