using System;
using System.Linq;

using Afc.Framework.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    public class V1094InitialPart1Repository : BaseDomainRepository<V1094InitialPart1, IReportingDataContext>, IV1094InitialPart1Repository
    {

        IQueryable<V1094InitialPart1> IV1094InitialPart1Repository.FilterForEmployerIdAndTaxYear(int employerId, int taxYearId)
        {

            return
            (
                from V1094InitialPart1 v1094InitialPart1 in Context.Set<V1094InitialPart1>()
                where
                    v1094InitialPart1.EmployerId == employerId
                        &&
                    v1094InitialPart1.TaxYearId == taxYearId
                select v1094InitialPart1
            ).AsQueryable();

        }

    }

}
