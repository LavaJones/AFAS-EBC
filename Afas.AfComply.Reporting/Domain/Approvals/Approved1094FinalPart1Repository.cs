using System;
using System.Linq;

using Afc.Framework.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    public class Approved1094FinalPart1Repository : BaseDomainRepository<Approved1094FinalPart1, IReportingDataContext>, IApproved1094FinalPart1Repository
    {

        IQueryable<Approved1094FinalPart1> IApproved1094FinalPart1Repository.FilterForEmployerIdAndTaxYear(int employerId, int taxYearId)
        {

            return
            (
                from Approved1094FinalPart1 approved1094FinalPart1 in Context.Set<Approved1094FinalPart1>()
                where
                    approved1094FinalPart1.EmployerId == employerId
                        &&
                    approved1094FinalPart1.TaxYearId == taxYearId
                select approved1094FinalPart1
            ).AsQueryable();

        }

    }

}
