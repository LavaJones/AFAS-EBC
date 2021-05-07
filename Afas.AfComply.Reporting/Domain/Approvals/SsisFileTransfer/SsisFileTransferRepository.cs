using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.AfComply.Reporting.Domain.Approvals.SsisFileTransfer;
using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.Domain;
using Afc.Framework.Domain;
using System.Runtime.Remoting.Contexts;

namespace Afas.AfComply.Reporting.Domain.Approvals.SsisFileTransfer
{
    public class SsisFileTransferRepository : BaseDomainRepository<SsisFileTransfer, IReportingDataContext>, ISsisFileTransferRepository
    {
        List<SsisFileTransfer> ISsisFileTransferRepository.GetFileTransferredThroughSsis(DateTime StartDate, DateTime EndDate)
        {
            return this.Context.Set<SsisFileTransfer>()
                .FilterForActive()
                .FilterForStartRunTime(StartDate)
                .FilterForEndRunTime(EndDate)
                .ToList();

        }
    }
}
