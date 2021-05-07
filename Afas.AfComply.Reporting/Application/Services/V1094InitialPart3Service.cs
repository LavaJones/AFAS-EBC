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
    public class V1094InitialPart3Service : ABaseCrudService<V1094InitialPart3>, IV1094InitialPart3Service
    {
        protected IV1094InitialPart3Repository Repository { get; private set; }

        public V1094InitialPart3Service(IV1094InitialPart3Repository repository) : base(repository)
        {

            this.Repository = repository;

        }
        public IQueryable<V1094InitialPart3> FilterForEmployerId(int EmployerId,int TaxYear)
        {
            return this.Repository.FilterForEmployerIdAndTaxYear(EmployerId, TaxYear);
        }

    }
}


