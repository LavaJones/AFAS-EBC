using System;
using System.Linq;

using Afc.Framework.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    public class V1094InitialPart2Repository : BaseDomainRepository<V1094InitialPart2, IReportingDataContext>, IV1094InitialPart2Repository
    {

        IQueryable<V1094InitialPart2> IV1094InitialPart2Repository.FilterForEmployerId(int employerId)
        {

            return
            (
                from V1094InitialPart2 v1094InitialPart2 in Context.Set<V1094InitialPart2>()
                where v1094InitialPart2.EmployerId == employerId
                select v1094InitialPart2
            ).AsQueryable();

        }

    }

}
