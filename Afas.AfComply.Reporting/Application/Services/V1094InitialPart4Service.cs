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
    public class V1094InitialPart4Service : ABaseCrudService<V1094InitialPart4>, IV1094InitialPart4Service
    {
        protected IV1094InitialPart4Repository Repository { get; private set; }

        public V1094InitialPart4Service(IV1094InitialPart4Repository repository) : base(repository)
        {

            this.Repository = repository;

        }
        public IQueryable<V1094InitialPart4> FilterForEmployerId(int EmployerId)
        {
            return this.Repository.FilterForEmployerId(EmployerId);
        }

       
    }
}


