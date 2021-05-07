using System;
using System.Collections.Generic;
using System.Linq;

using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    public interface IV1094InitialPart1Repository : IDomainRepository<V1094InitialPart1>
    {

        IQueryable<V1094InitialPart1> FilterForEmployerIdAndTaxYear(int employerId, int taxYearId);

    }

}
