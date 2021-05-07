using Afas.AfComply.Reporting.Domain.Printing;
using Afas.AfComply.Reporting.Domain.TimeFrames;
using Afas.Application;
using Afc.Core;
using Afc.Core.Domain;
using Afc.Framework.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Application.Services
{
    /// <summary>
    /// A service explosing access to the TimeFrame domain models.
    /// </summary>
    public class PrintBatchService : ABaseCrudService<PrintBatch>, IPrintBatchService
    {
        protected IPrintBatchRepository Repository { get; private set; }
        
        /// <summary>
        /// Standard COnstructor taking the dependencies as parameters. 
        /// </summary>
        /// <param name="repository">The Repository to get the Time frames.</param>
        public PrintBatchService(
            IPrintBatchRepository repository) : 
                base(repository)
        {
            
            this.Repository = repository;

        }

        /// <summary>
        /// Get all batches and their child 1095s
        /// </summary>
        /// <returns>All print batches</returns>
        IList<PrintBatch> IPrintBatchService.GetAllPrintBatchesAndPrints()
        {
            return this.Repository.GetAllPrintBatchesAndPrints().ToList();
        }

        /// <summary>
        /// Filters to only batches that match the Employer and Tax Year and gets just their IDs
        /// </summary>
        /// <param name="employerId">The Employer to match to.</param>
        /// <param name="taxYear">The Tax Year to match for.</param>
        /// <returns>All the IDs of the print batches that match that employer tax year combo</returns>
        IList<long> IPrintBatchService.GetIdsForEmployerTaxYear(int employerId, int taxYear)
        {
            return (from item in this.Repository.FilterForEmployerTaxYear(employerId, taxYear) select item.ID).ToList();
        }

        /// <summary>
        /// Filters to only batches that match the Employer and Tax Year
        /// </summary>
        /// <param name="employerId">The Employer to match to.</param>
        /// <param name="taxYear">The Tax Year to match for.</param>
        /// <returns>All print batches that match that employer tax year combo</returns>
        IList<PrintBatch> IPrintBatchService.GetForEmployerTaxYear(int employerId, int taxYear)
        {
            return this.Repository.FilterForEmployerTaxYear(employerId, taxYear).ToList();
        }

        /// <summary>
        /// Filters to only batches that match the Employer and Tax Year
        /// </summary>
        /// <param name="employerId">The Employer to match to.</param>
        /// <param name="taxYear">The Tax Year to match for.</param>
        /// <returns>All print batches that match that employer tax year combo</returns>
        bool IPrintBatchService.HasPrintedEmployerTaxYear(int employerId, int taxYear)
        {
            return this.Repository.FilterForEmployerTaxYear(employerId, taxYear).Count() > 0;
        }

        /// <summary>
        /// Gets a Print Batch by it's Filename
        /// </summary>
        /// <param name="fileName">The File Name</param>
        /// <returns>Any Print Batches that match the file name</returns>
        IList<PrintBatch> IPrintBatchService.GetForFileName(string fileName)
        {
            return this.Repository.FilterForFileName(fileName).ToList();

        }

        /// <summary>
        /// Saves a new Print Batch to the DB
        /// </summary>
        /// <param name="toSave">The Batch to save</param>
        /// <returns>Success</returns>
        bool IPrintBatchService.SaveBatch(PrintBatch toSave, string requestor)
        {
            return this.Repository.SaveBatch(toSave, requestor);
        }

        /// <summary>
        /// Updates a Print Batch to the DB
        /// </summary>
        /// <param name="toSave">The Batch to save</param>
        /// <returns>Success</returns>
        bool IPrintBatchService.UpdateBatchReceived(PrintBatch toSave, string requestor)
        {
            return this.Repository.UpdateBatchReceived(toSave, requestor);
        }

    }

}
