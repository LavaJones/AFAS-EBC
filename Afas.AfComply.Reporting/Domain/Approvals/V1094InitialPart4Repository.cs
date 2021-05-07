using System;
using System.Linq;

using Afc.Framework.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    public class V1094InitialPart4Repository : BaseDomainRepository<V1094InitialPart4, IReportingDataContext>, IV1094InitialPart4Repository
    {

        IQueryable<V1094InitialPart4> IV1094InitialPart4Repository.FilterForEmployerId(int employerId)
        {

            return 
            (
                from V1094InitialPart4 v1094InitialPart4 in Context.Set<V1094InitialPart4>()
                where v1094InitialPart4.EmployerId == employerId
                select v1094InitialPart4
            ).AsQueryable();

        }

    }

}
