using Afas.AfComply.Reporting.Domain.LegacyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Application.Services.LegacyServices
{
    public class LegacyClassifictaionService : LegacyService, ILegacyClassifictaionService
    {
        //public employee_classification GetClassificationWithId(int id)
        //{
        //    using (var ctx = new AcaEntities())
        //    {
        //        
        //        ctx.Configuration.ProxyCreationEnabled = false;
        //        ctx.Configuration.LazyLoadingEnabled = false;

        //        return GetClassificationWithId(id, ctx);
        //    }
        //}

        //public employee_classification GetClassificationWithId(int id, AcaEntities ctx)
        //{
        //    employee_classification result = new employee_classification();

        //    result = (from employee_classification classification in ctx.employee_classification
        //              where id == classification.classification_id
        //              select classification).Single();

        //    return result;
        //}

        //public List<employee_classification> GetClassificationsForEmployer(int employerId)
        //{
        //    using (var ctx = new AcaEntities())
        //    {
        //        
        //        ctx.Configuration.ProxyCreationEnabled = false;
        //        ctx.Configuration.LazyLoadingEnabled = false;

        //        return GetClassificationsForEmployer(employerId, ctx);
        //    }
        //}

        //public List<employee_classification> GetClassificationsForEmployer(int employerId, AcaEntities ctx)
        //{
        //    List<employee_classification> result = new List<employee_classification>();

        //    result = (from employee_classification classification in ctx.employee_classification
        //              where employerId = classification.employer_id
        //              select classification).ToList();

        //    return result;
        //}
    }
}
