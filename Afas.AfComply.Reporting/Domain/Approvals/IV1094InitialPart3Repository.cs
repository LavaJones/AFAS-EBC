

using System;
using System.Collections.Generic;
using System.Linq;

using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    public interface IV1094InitialPart3Repository : IDomainRepository<V1094InitialPart3>
    {

        IQueryable<V1094InitialPart3> FilterForEmployerIdAndTaxYear(int employerId, int TaxYear);

    }

}

