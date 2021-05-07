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
   public class V1094InitialPart1Service: ABaseCrudService<V1094InitialPart1>, IV1094InitialPart1Service
    {
        protected IV1094InitialPart1Repository Repository { get; private set; }

        public V1094InitialPart1Service( IV1094InitialPart1Repository repository) : base(repository)
        {

            this.Repository = repository;

        }


        IQueryable<V1094InitialPart1> IV1094InitialPart1Service.FilterForEmployerIdAndTaxYear(int EmployerId, int TaxYear)
        {
            return this.Repository.FilterForEmployerIdAndTaxYear(EmployerId, TaxYear);
        }
    }
}


