using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Voids;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Printing
{
    public interface IPrint1095Repository : IDomainRepository<Print1095>
    {

        IQueryable<Print1095> FilterForBatchIds(IList<long> BatchIds);

        IQueryable<Print1095> FilterForBatch(PrintBatch Batch);

        IQueryable<Print1095> FilterForApproval1095(Approved1095Final approval1095);

    }
}
