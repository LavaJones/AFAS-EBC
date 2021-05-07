using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Approvals
{
    public interface IFinalize1094Repository : IDomainRepository<Approved1094FinalPart1>
    {

        /// <summary>
        /// Gets all the finalized froms for a tax year and an employer 
        /// </summary>
        /// <param name="EmployerId">The employer to get forms for.</param>
        /// <param name="TaxYear">The tax Year to get forms for.</param>
        /// <returns>The finalized forms for this tax year.</returns>
        List<Approved1094FinalPart1> GetApproved1094sForEmployerTaxYear(int EmployerId, int TaxYear);

    }
}
