using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.Application;
using Afc.Core;
using Afc.Core.Domain;
using Afc.Framework.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Application.Services
{
    public class V1094InitialPart2Service : ABaseCrudService<V1094InitialPart2>, IV1094InitialPart2Service
    {
        protected IV1094InitialPart2Repository Repository { get; private set; }

        public V1094InitialPart2Service(IV1094InitialPart2Repository repository) : base(repository)
        {

            this.Repository = repository;

        }
        public IQueryable<V1094InitialPart2> FilterForEmployerId(int EmployerId)
        {
            return this.Repository.FilterForEmployerId(EmployerId);
        }
    }
}


