using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afc.Core.Domain;
using Afas.AfComply.Reporting.Domain.Approvals.SsisFileTransfer;

namespace Afas.AfComply.Reporting.Domain.Approvals.SsisFileTransfer
{

    public interface ISsisFileTransferRepository : IDomainRepository<SsisFileTransfer>
    {
        List<SsisFileTransfer> GetFileTransferredThroughSsis(DateTime StartDate, DateTime EndDate);
    }
}
