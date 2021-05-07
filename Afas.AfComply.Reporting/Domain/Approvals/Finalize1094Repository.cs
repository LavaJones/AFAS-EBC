using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.Domain;
using Afc.Framework.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Approvals
{
    public class Finalize1094Repository : BaseDomainRepository<Approved1094FinalPart1, IReportingDataContext>, IFinalize1094Repository
    {

        private ILog Log = LogManager.GetLogger(typeof(Finalize1094Repository));

        List<Approved1094FinalPart1> IFinalize1094Repository.GetApproved1094sForEmployerTaxYear(int EmployerId, int TaxYear)
        {

            return 
                this.Context.Set<Approved1094FinalPart1>()
               .FilterForActive()
               .FilterForEmployer(EmployerId)
               .FilterForTaxYear(TaxYear)

               .ToList();
        }
    }
}


