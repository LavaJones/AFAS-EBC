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
    public class Print1095Service : ABaseCrudService<Print1095>, IPrint1095Service
    {
        protected IPrint1095Repository Repository { get; private set; }
        
        /// <summary>
        /// Standard COnstructor taking the dependencies as parameters. 
        /// </summary>
        /// <param name="repository">The Repository to get the Time frames.</param>
        public Print1095Service(
            IPrint1095Repository repository) : 
                base(repository)
        {
            
            this.Repository = repository;

        }

        /// <summary>
        /// Get the IDs of all the Approved Final 1095's that have been printed in the specified batches.
        /// </summary>
        /// <param name="batchIds">All the IDs of the batches to get printed for.</param>
        /// <returns>All the Id's to Approved FInals 1095s that have been printed.</returns>
        IList<long> IPrint1095Service.GetApprovedIdsForBatchIds(IList<long> batchIds)
        {
            return this.Repository.FilterForBatchIds(batchIds).FilterToOnlyApproved1095Ids().ToList();
        }

        /// <summary>
        /// Get the  Approved Final 1095's that have been printed in the specified batches.
        /// </summary>
        /// <param name="batchIds">All the IDs of the batches to get printed for.</param>
        /// <returns>All the Approved FInals 1095s that have been printed.</returns>
        IList<Print1095> IPrint1095Service.GetForBatchIds(IList<long> batchIds)
        {
            return this.Repository.FilterForBatchIds(batchIds).ToList();
        }

        /// <summary>
        /// Get the IDs of all the Approved Final 1095's that have been printed in the specified batch.
        /// </summary>
        /// <param name="batchId">The ID of the batch to get printed for.</param>
        /// <returns>All the Id's to Approved FInals 1095s that have been printed.</returns>
        IList<long> IPrint1095Service.GetApprovedIdsForBatchId(long batchId)
        {
            return this.Repository.FilterForBatchIds(new List<long>() { batchId }).FilterToOnlyApproved1095Ids().ToList();
        }

        /// <summary>
        /// Get the Approved Final 1095's that have been printed in the specified batch.
        /// </summary>
        /// <param name="batchId">The ID of the batch to get printed for.</param>
        /// <returns>All the Approved FInals 1095s that have been printed.</returns>
        IList<Print1095> IPrint1095Service.GetForBatchId(long batchId)
        {
            return this.Repository.FilterForBatchIds(new List<long>() { batchId }).ToList();
        }

    }

}
