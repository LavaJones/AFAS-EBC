using Afc.Core.Domain;
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
    public interface IVoid1094Repository : IDomainRepository<Void1094>
    {

        /// <summary>
        /// Filters the Values for ones Voided be as specific user. 
        /// </summary>
        /// <param name="username">The user that voided the items.</param>
        /// <returns>The filtered values</returns>
        IQueryable<Void1094> FilterForVoidedBy(string username);

    }
}
