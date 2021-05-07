using System;
using System.Collections.Generic;
using System.Linq;

using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    public interface IApproved1094FinalPart1Repository : IDomainRepository<Approved1094FinalPart1>
    {

        IQueryable<Approved1094FinalPart1> FilterForEmployerIdAndTaxYear(int employerId, int taxYearId);

    }

}
