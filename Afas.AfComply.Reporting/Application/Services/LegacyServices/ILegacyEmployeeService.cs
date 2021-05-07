using System.Collections.Generic;
using Afas.AfComply.Reporting.Domain.LegacyData;

namespace Afas.AfComply.Reporting.Application.Services.LegacyServices
{
    public interface ILegacyEmployeeService : ILegacyService
    {
        List<employee> GetEmployeesForEmployer(int employerId);

        employee GetEmployeeWithId(int id);

        List<employee> GetEmployeesWithIds(List<int> ids);

        employee GetEmployeeWithId(int id, AcaEntities ctx);

        List<employee> GetEmployeesWithIds(List<int> ids, AcaEntities ctx);

        List<employee> GetEmployeesForEmployer(int employerId, AcaEntities ctx);
    }
}