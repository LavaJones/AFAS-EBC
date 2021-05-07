using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Voids
{
    public interface IVoid1095Repository : IDomainRepository<Void1095>
    {
        /// <summary>
        /// Filters the Values for ones Voided be as specific user. 
        /// </summary>
        /// <param name="username">The user that voided the items.</param>
        /// <returns>The filtered values</returns>
        IQueryable<Void1095> FilterForVoidedBy(string username);
    }
}
