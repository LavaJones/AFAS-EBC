using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Reporting
{
    /// <summary>
    ///  specific repository.
    /// </summary>
    public interface IUserEditPart2Repository : IDomainRepository<UserEditPart2>
    {

        /// <summary>
        /// Updates the Part 2 data with edited values.
        /// </summary>
        /// <param name="edit">The Edit data to update.</param>
        /// <param name="requestor">The user that edited the data.</param>
        /// <returns>If the save was successfull.</returns>
        void UpdateWithEdit(UserEditPart2 edit, string requestor);

        /// <summary>
        /// Updates the Part 2 data with edited values.
        /// </summary>
        /// <param name="edit">The Edit data to update.</param>
        /// <param name="employerId">The Employer Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <param name="requestor">The user making the edit.</param>
        void UpdateWithEditsMany(List<UserEditPart2> edits, int employerId, int taxYear, string requestor);

        /// <summary>
        /// Filters the Values for ones belonging to an employer. 
        /// </summary>
        /// <param name="employerId">The Employer Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values</returns>
        Dictionary<int, Dictionary<int, List<UserEditPart2>>> GetForEmployerTaxYear(int employerId, int taxYear);
        
    }

}
