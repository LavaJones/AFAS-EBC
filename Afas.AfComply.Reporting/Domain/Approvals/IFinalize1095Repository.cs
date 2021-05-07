using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Approvals
{
    public interface IFinalize1095Repository : IDomainRepository<Approved1095Final>
    {

        void DisableAutoDetectChanges();

        void EnableAutoDetectChanges();

        void DisableValidateOnSave();

        void EnableValidateOnSave();

        /// <summary>
        /// Updates the Part 2 data with edited values.
        /// </summary>
        /// <param name="Approved1095s">The Edit data to update.</param>
        /// <param name="EmployerId">The employer the forms are for.</param>
        /// <param name="TaxYear">The tax Year the forms are for.</param>
        /// <param name="requestor">The user that edited the data.</param>
        /// <param name="IsUpdate">If this is an update command and not an insert</param>
        /// <returns>If the save was successfull.</returns>
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
