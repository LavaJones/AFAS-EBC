using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Corrections;
using Afas.AfComply.Reporting.Domain.Voids;
using Afc.Core.Domain;
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
    public interface IPrint1094Repository : IDomainRepository<Print1094>
    {
        /// <summary>
        /// Retrive only the Print1094 objects by Batch.
        /// </summary>
        /// <param name="PrintBatch">The Batch.</param>
        /// <returns>The Print1094 object by specified batch.</returns>
        IQueryable<Print1094> FilterForPrintBatch(PrintBatch printBatch);

        /// <summary>
        /// Retrive only the Print1094 objects by Approval Status.
        /// </summary>
        /// <param name="Approval1094">The Approved Status.</param>
        /// <returns>The Approval1094 object by specified void status.</returns>
        IQueryable<Print1094> FilterForApproval1094(Approved1094FinalPart1 approval1094);


    }
}
