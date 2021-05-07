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
using Afc.Core;
using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Application.Services
{
    public class Finalize1094Service : ABaseCrudService<Approved1094FinalPart1>, IFinalize1094Service
    {
        protected IFinalize1094Repository Repository { get; private set; }

        public Finalize1094Service(
          IFinalize1094Repository repository) :
                base(repository)
        {

            this.Repository = repository;

        }

        List<Approved1094FinalPart1> IFinalize1094Service.GetApproved1094sForEmployerTaxYear(int EmployerId, int TaxYear)
        {

            return this.Repository.GetApproved1094sForEmployerTaxYear(EmployerId, TaxYear);

        }

        /// <summary>
        /// Adds a new Entity to the database
        /// </summary>
        /// <param name="toAdd">The entity to Add</param>
        /// <param name="authorizingUser">The User adding the entity</param>
        /// <returns>The Added Entity.</returns>
        Approved1094FinalPart1 ICrudDomainService<Approved1094FinalPart1>.AddNewEntity(Approved1094FinalPart1 entity, string authorizingUser)
        {

            SharedUtilities.VerifyStringParameter(authorizingUser, "authorizingUser");

            // Set initial state 
            entity.CreatedBy = authorizingUser;
            entity.ModifiedBy = authorizingUser;
            entity.ModifiedDate = DateTime.Now;
            entity.ResourceId = Guid.NewGuid();
            entity.EntityStatus = EntityStatusEnum.Active;

            // set the basics on all the part 3 items
            foreach (var part3 in entity.Approved1094FinalPart3s)
            {
                // Set initial state 
                part3.CreatedBy = authorizingUser;
                part3.ModifiedBy = authorizingUser;
                part3.ModifiedDate = DateTime.Now;
                part3.ResourceId = Guid.NewGuid();
                part3.EntityStatus = EntityStatusEnum.Active;

            }

            // set the basics on all the part 4 items
            foreach (var part4 in entity.Approved1094FinalPart4s)
            {
                // Set initial state 
                part4.CreatedBy = authorizingUser;
                part4.ModifiedBy = authorizingUser;
                part4.ModifiedDate = DateTime.Now;
                part4.ResourceId = Guid.NewGuid();
                part4.EntityStatus = EntityStatusEnum.Active;

            }

            // Verify that Object is valid
            if (entity.EnsureIsWellFormed.Count > 0)
            {

                this.Log.Warn(
                        String.Format("Validation messages: {0}",
                        entity.EnsureIsWellFormed.GetLongDescription())
                    );

                throw new InvalidOperationException("Object must be well formed to be stored.");

            }

            this.MainDomainRepository.InsertOnSubmit(entity);
            this.MainDomainRepository.SaveChanges();

            return this.MainDomainRepository.GetByResourceId(entity.ResourceId);

        }






    }
}
