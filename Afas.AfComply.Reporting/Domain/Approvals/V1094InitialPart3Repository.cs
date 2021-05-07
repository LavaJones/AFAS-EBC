
using System;
using System.Linq;

using Afc.Framework.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    public class V1094InitialPart3Repository : BaseDomainRepository<V1094InitialPart3, IReportingDataContext>, IV1094InitialPart3Repository
    {

        IQueryable<V1094InitialPart3> IV1094InitialPart3Repository.FilterForEmployerIdAndTaxYear(int employerId,int TaxYear)
        {

            return
            (
                from V1094InitialPart3 v1094InitialPart3 in Context.Set<V1094InitialPart3>()
                where v1094InitialPart3.EmployerId == employerId
                        && 
                      v1094InitialPart3.TaxYear == TaxYear
                select v1094InitialPart3
            ).AsQueryable();

        }

    }

}
