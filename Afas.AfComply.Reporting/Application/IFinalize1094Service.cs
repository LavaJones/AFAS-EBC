using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Reporting;
using Afas.Application;
using Afc.Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Application
{

    public interface IFinalize1094Service : ICrudDomainService<Approved1094FinalPart1>
    {

        List<Approved1094FinalPart1> GetApproved1094sForEmployerTaxYear(int EmployerId, int TaxYear);

    }
}
