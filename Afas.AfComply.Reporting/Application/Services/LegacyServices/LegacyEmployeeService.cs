using Afas.AfComply.Reporting.Domain.LegacyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Application.Services.LegacyServices
{
    public class LegacyEmployeeService : LegacyService, ILegacyEmployeeService
    {

        public List<employee> GetEmployeesForEmployer(int employerId)
        {
            using (var ctx = new AcaEntities())
            {
                ctx.Database.CommandTimeout = 180;
                //ctx.Configuration.ProxyCreationEnabled = false;
                //ctx.Configuration.LazyLoadingEnabled = false;

                return GetEmployeesForEmployer(employerId, ctx);
            }
        }

        public List<employee> GetEmployeesForEmployer(int employerId, AcaEntities ctx)
        {
            List<employee> result = new List<employee>();

            result = (from employee emp in ctx.employees
                      where emp.employer_id == employerId
                      select emp).ToList();

            return result;
        }

        public employee GetEmployeeWithId(int id)
        {
            using (var ctx = new AcaEntities())
            {
                ctx.Database.CommandTimeout = 180;
                //ctx.Configuration.ProxyCreationEnabled = false;
                //ctx.Configuration.LazyLoadingEnabled = false;

                return GetEmployeeWithId(id, ctx);
            }
        }

        public employee GetEmployeeWithId(int id, AcaEntities ctx)
        {
            employee result = new employee();

            result = (from employee emp in ctx.employees
                        where id == emp.employee_id
                        select emp).Single();

            return result;
        }

        public List<employee> GetEmployeesWithIds(List<int> ids)
        {
            using (var ctx = new AcaEntities())
            {
                ctx.Database.CommandTimeout = 180;
                //ctx.Configuration.ProxyCreationEnabled = false;
                //ctx.Configuration.LazyLoadingEnabled = false;

                return GetEmployeesWithIds(ids, ctx);
            }
        }

        public List<employee> GetEmployeesWithIds(List<int> ids, AcaEntities ctx)
        {
            List<employee> result = new List<employee>();
            
            result = (from employee emp in ctx.employees
                        where ids.Contains(emp.employee_id)
                        select emp).ToList();
            
            return result;
        }
    }
}
