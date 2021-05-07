using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Reporting;
using Afas.Application;
using Afc.Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Afas.AfComply.Reporting.Application
{
    public interface IV1094InitialPart4Service : ICrudDomainService<V1094InitialPart4>
    {
        /// <summary>
        /// Gets all the records for that employer in that Calendar Year.
        /// </summary>
        /// <param name="year">The Year to get the time frames for.</param>

        IQueryable<V1094InitialPart4> FilterForEmployerId(int EmployerId);

    }
}





