using System;
using System.Collections.Generic;
using System.Linq;

using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    public interface IV1094InitialPart4Repository : IDomainRepository<V1094InitialPart4>
    {

        IQueryable<V1094InitialPart4> FilterForEmployerId(int employerId);

    }

}
