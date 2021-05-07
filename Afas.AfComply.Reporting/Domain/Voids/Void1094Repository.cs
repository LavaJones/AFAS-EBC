using Afas.Domain;
using Afc.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Voids
{
    /// <summary>
    ///  specific repository.
    /// </summary>
    public class Void1094Repository : BaseDomainRepository<Void1094, IReportingDataContext>, IVoid1094Repository
    {

        /// <summary>
        /// Filters the Values for ones Voided be as specific user. 
        /// </summary>
        /// <param name="username">The user that voided the items.</param>
        /// <returns>The filtered values</returns>
        IQueryable<Void1094> IVoid1094Repository.FilterForVoidedBy(string username)
        {

            return Context.Set<Void1094>()
                    .FilterForVoidedBy(username)
                    .FilterForActive();

        }
    }
}
