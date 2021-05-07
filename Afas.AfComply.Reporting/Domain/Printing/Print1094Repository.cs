using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Voids;
using Afc.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Printing
{
    /// <summary>
    /// Time Frame specific repository.
    /// </summary>
    public class Print1094Repository : BaseDomainRepository<Print1094, IReportingDataContext>, IPrint1094Repository
    {
        /// <summary>
        /// Retrieve only the Print1094 for a specific Batch.
        /// </summary>
        /// <param name="printBatch">The Print Batch to filter on.</param>
        /// <returns>The Print1094 for a specified Print Batch.</returns>
        IQueryable<Print1094> IPrint1094Repository.FilterForPrintBatch(PrintBatch printBatch)
        {

            return Context.Set<Print1094>()
                    .FilterForPrintBatch(printBatch);

        }

        /// <summary>
        /// Retrieve only the Print1094 for a specific Void Status.
        /// </summary>
        /// <param name="approval1094">The Approval1094 status to filter on.</param>
        /// <returns>The Print1094 for a specified Approval1094 status.</returns>
        IQueryable<Print1094> IPrint1094Repository.FilterForApproval1094(Approved1094FinalPart1 approval1094)
        {

            return Context.Set<Print1094>()
                .FilterForApproval1094(approval1094);

        }
    }
}
