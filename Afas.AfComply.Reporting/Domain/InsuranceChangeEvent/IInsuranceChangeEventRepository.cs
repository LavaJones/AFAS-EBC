using Afc.Core.Domain;
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
    public interface IInsuranceChangeEventRepository : IDomainRepository<InsuranceChangeEvent>
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="PlanYearID"></param>
        /// <returns></returns>
        IQueryable<InsuranceChangeEvent> FilterForEmployeeIDAndPlanYearID(int EmployeeID, int PlanYearID);

    }
}
