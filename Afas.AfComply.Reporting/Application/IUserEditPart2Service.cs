using Afas.AfComply.Reporting.Core.Models;
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

    /// <summary>
    /// 
    /// </summary>
    public interface IUserEditPart2Service : ICrudDomainService<UserEditPart2>
    {

        /// <summary>
        /// Updates the database with teh user Edits provided
        /// </summary>
        /// <param name="edits">The new User Edits to store.</param>
        /// <summary>
        /// Updates the database with the user Edits provided
        /// </summary>
        /// <param name="edits">The User Edits to store.</param>
        /// <param name="currentSystem">The current values in the system.</param>
        /// <param name="EmployerId">The Id of the employer for this edit.</param>
        /// <param name="TaxYear">The Id of the tax year for this edit.</param>
        /// <param name="requestor">The new User making the edits.</param>
        [RequiresSharedTransaction]
        void UpdateWithEdits(IList<Employee1095detailsPart2Model> userSave, IDictionary<int, List<Employee1095detailsPart2Model>> currentSystem, int EmployerId, int TaxYear, string requestor);

        /// <summary>
        /// Filters the Values for ones belonging to an employer. 
        /// </summary>
        /// <param name="employerId">The Employer Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values</returns>
        Dictionary<int, Dictionary<int, List<UserEditPart2>>> GetForEmployerTaxYear(int employerId, int taxYear);

    }
}
