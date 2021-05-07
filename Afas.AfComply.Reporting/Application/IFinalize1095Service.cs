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

    public interface IFinalize1095Service : ICrudDomainService<Approved1095Final>
    {

        [RequiresSharedTransaction]
        void SaveApproved1095(IList<Approved1095Final> Approved1095s, int EmployerId, int TaxYear, string requestor, bool IsUpdate = false);

        /// <summary>
        /// Gets all the finalized froms for a tax year and an employer 
        /// </summary>
        /// <param name="EmployerId">The employer to get forms for.</param>
        /// <param name="TaxYear">The tax Year to get forms for.</param>
        /// <returns>The finalized forms for this tax year.</returns>
        List<Approved1095Final> GetApproved1095sForEmployerTaxYear(int EmployerId, int TaxYear);

        /// <summary>
        /// Gets all the Employee Id's that are finalized
        /// </summary>
        /// <param name="EmployerId">The employer to get forms for.</param>
        /// <param name="TaxYear">The tax Year to get forms for.</param>
        /// <returns>The employee Ids that are finalized for this tax year.</returns>
        List<int> GetApproved1095sEmployeeIdsForEmployerTaxYear(int EmployerId, int TaxYear);

    }
}
