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
    public class Print1095Repository : BaseDomainRepository<Print1095, IReportingDataContext>, IPrint1095Repository
    {

        IQueryable<Print1095> IPrint1095Repository.FilterForBatchIds(IList<long> BatchIds)
        {

            return Context.Set<Print1095>()
                     .FilterForPrintBatchIds(BatchIds);
        
        }

        IQueryable<Print1095> IPrint1095Repository.FilterForBatch(PrintBatch Batch)
        {

           return Context.Set<Print1095>()
                    .FilterForPrintBatch(Batch);

        }

        IQueryable<Print1095> IPrint1095Repository.FilterForApproval1095(Approved1095Final approval1095)
        {

            return Context.Set<Print1095>()
                    .FilterForApproval1095(approval1095);

        }


        // Mass Insert

        // Mass Update




    }
}
