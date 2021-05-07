using System;
using System.Collections.Generic;
using System.Linq;

using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    public interface IV1094InitialPart2Repository : IDomainRepository<V1094InitialPart2>
    {

        IQueryable<V1094InitialPart2> FilterForEmployerId(int employerId);

    }

}
