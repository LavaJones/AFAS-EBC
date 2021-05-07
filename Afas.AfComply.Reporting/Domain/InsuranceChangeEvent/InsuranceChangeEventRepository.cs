using Afc.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.InsuranceChangeEvent
{
    /// <summary>
    /// Time Frame specific repository.
    /// </summary>
    public class InsuranceChangeEventRepository : BaseDomainRepository<InsuranceChangeEvent, IReportingDataContext>, IInsuranceChangeEventRepository
    {

        /// <summary>
        /// Retrive only the timeframes for a specific year.
        /// </summary>
        /// <param name="Year">The Year to filter on.</param>
        /// <returns>The Timeframes for this year.</returns>
        IQueryable<InsuranceChangeEvent> IInsuranceChangeEventRepository.FilterForEmployeeIDAndPlanYearID(int EmployeeID, int PlanYearID)
        {

            return Context.Set<InsuranceChangeEvent>();

        }
    }
}
