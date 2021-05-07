using Afc.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.Domain;

namespace Afas.AfComply.Reporting.Domain.Voids
{
    
        public class Void1095Repository : BaseDomainRepository<Void1095, IReportingDataContext>, IVoid1095Repository
        {

            /// <summary>
            /// Filters the Values for ones Voided be as specific user. 
            /// </summary>
            /// <param name="username">The user that voided the items.</param>
            /// <returns>The filtered values</returns>
            IQueryable<Void1095> IVoid1095Repository.FilterForVoidedBy(string username)
            {

                return Context.Set<Void1095>()
                        .FilterForVoidedBy(username)
                        .FilterForActive();

            }
        }
    }
