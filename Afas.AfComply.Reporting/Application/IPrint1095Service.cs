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
    public interface IPrint1095Service : ICrudDomainService<Print1095>
    {

        /// <summary>
        /// Get the IDs of all the Approved Final 1095's that have been printed in the specified batches.
        /// </summary>
        /// <param name="batchIds">All the IDs of the batches to get printed for.</param>
        /// <returns>All the Id's to Approved FInals 1095s that have been printed.</returns>
        IList<long> GetApprovedIdsForBatchIds(IList<long> batchIds);

        /// <summary>
        /// Get the  Approved Final 1095's that have been printed in the specified batches.
        /// </summary>
        /// <param name="batchIds">All the IDs of the batches to get printed for.</param>
        /// <returns>All the Approved FInals 1095s that have been printed.</returns>
        IList<Print1095> GetForBatchIds(IList<long> batchIds);

        /// <summary>
        /// Get the IDs of all the Approved Final 1095's that have been printed in the specified batch.
        /// </summary>
        /// <param name="batchId">The ID of the batch to get printed for.</param>
        /// <returns>All the Id's to Approved FInals 1095s that have been printed.</returns>
        IList<long> GetApprovedIdsForBatchId(long batchId);

        /// <summary>
        /// Get the Approved Final 1095's that have been printed in the specified batch.
        /// </summary>
        /// <param name="batchId">The ID of the batch to get printed for.</param>
        /// <returns>All the Approved FInals 1095s that have been printed.</returns>
        IList<Print1095> GetForBatchId(long batchId);






    }
}
