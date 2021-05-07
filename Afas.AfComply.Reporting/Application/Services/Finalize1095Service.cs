using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.AfComply.Reporting.Core.Models;
using Afc.Framework.Domain;
using Afas.AfComply.Reporting.Domain;

namespace Afas.AfComply.Reporting.Application.Services
{
    public class Finalize1095Service : ABaseCrudService<Approved1095Final>, IFinalize1095Service
    {
        protected IFinalize1095Repository Repository { get; private set; }

        public Finalize1095Service(
          IFinalize1095Repository repository) : 
                base(repository)
        {

            this.Repository = repository;

        }

        void IFinalize1095Service.SaveApproved1095(IList<Approved1095Final> Approved1095, int EmployerId, int TaxYear, string requestor, bool IsUpdate = false)
        {

            this.Repository.SaveApproved1095(Approved1095, EmployerId, TaxYear, requestor, IsUpdate);
                
        }

        List<Approved1095Final> IFinalize1095Service.GetApproved1095sForEmployerTaxYear(int EmployerId, int TaxYear)
        {

            return this.Repository.GetApproved1095sForEmployerTaxYear( EmployerId, TaxYear);

        }

        List<int> IFinalize1095Service.GetApproved1095sEmployeeIdsForEmployerTaxYear(int EmployerId, int TaxYear)
        {

            return this.Repository.GetApproved1095sEmployeeIdsForEmployerTaxYear(EmployerId, TaxYear);

        }
    }
}
