using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.AfComply.Reporting.Domain.Approvals.SsisFileTransfer;
using Afas.Application;

namespace Afas.AfComply.Reporting.Application.Ssis
{
    public interface ISsisFileTransferService : ICrudDomainService<SsisFileTransfer>
    {
        List<SsisFileTransfer> GetFileTransferredThroughSsis(DateTime StartDate, DateTime EndDate);
    }
}
