using Afas.Domain;
using Afc.Core;
using Afc.Core.Application;
using Afc.Core.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Application
{ 
    public abstract class ABaseCrudService<TEntity> : ABaseDomainService<TEntity>, ICrudDomainService<TEntity> 
        where TEntity : BaseAfasModel
    {
        public ABaseCrudService(
                IDomainRepository<TEntity> mainDomainRepository,
                params IRepository[] repositories) :
            base(mainDomainRepository, repositories) { }

        public ABaseCrudService(
            IDomainRepository<TEntity> mainDomainRepository) :
            base(mainDomainRepository) { }

        
        /// <summary>
        /// Gets all availible entities
        /// </summary>
        /// <returns>all availible entities</returns>
        IQueryable<TEntity> ICrudDomainService<TEntity>.GetAllEntities()
        {

            return this.MainDomainRepository.AreActive();

        }
        
        /// <summary>
        /// Adds a new Entity to the database
        /// </summary>
        /// <param name="toAdd">The entity to Add</param>
        /// <param name="authorizingUser">The User adding the entity</param>
        /// <returns>The Added Entity.</returns>
        TEntity ICrudDomainService<TEntity>.AddNewEntity(TEntity entity, string authorizingUser)
        {

            SharedUtilities.VerifyStringParameter(authorizingUser, "authorizingUser");

            // Set initial state 
            entity.CreatedBy = authorizingUser;
            entity.ModifiedBy = authorizingUser;
            entity.ModifiedDate = DateTime.Now;
            entity.ResourceId = Guid.NewGuid();
            entity.EntityStatus = EntityStatusEnum.Active;

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

        /// <summary>
        /// Deactivates an entity based on the provided Resource Id (aka soft delete)
        /// </summary>
        /// <param name="ResourceId">The Id of the Entity to deactivate</param>
        /// <param name="authorizingUser">The user deactivating the Entity</param>
        /// <returns>The Deactivated Entity</returns>
        TEntity ICrudDomainService<TEntity>.DeactivateEntity(Guid ResourceId, string authorizingUser)
        {

            SharedUtilities.VerifyStringParameter(authorizingUser, "authorizingUser");

            TEntity toDelete = this.MainDomainRepository.GetByResourceId(ResourceId);

            if (null != toDelete)
            {
                toDelete.ModifiedBy = authorizingUser;
                toDelete.ModifiedDate = DateTime.Now;
                toDelete.EntityStatus = EntityStatusEnum.Inactive;

                // Verify that object is valid
                if (toDelete.EnsureIsWellFormed.Count > 0)
                {

                    this.Log.Warn(
                            String.Format("Validation messages: {0}",
                            toDelete.EnsureIsWellFormed.GetLongDescription())
                        );

                    throw new InvalidOperationException("Object must be well formed to be stored.");

                }

                this.MainDomainRepository.SaveChanges();

                toDelete = this.MainDomainRepository.GetByResourceId(ResourceId);
            }

            return toDelete;

        }

        /// <summary>
        /// Update the values of an entity
        /// </summary>
        /// <param name="toUpdate">The Entity with updated Values</param>
        /// <param name="authorizingUser">The User updating the entity</param>
        /// <returns>The Updated Entity</returns>
        TEntity ICrudDomainService<TEntity>.UpdateEntity(TEntity entity, string authorizingUser)
        {

            SharedUtilities.VerifyStringParameter(authorizingUser, "authorizingUser");

            TEntity toUpdate = this.MainDomainRepository.GetByResourceId(entity.ResourceId);

            if (null != entity && toUpdate.EntityStatus == EntityStatusEnum.Active)
            {

                Mapper.Map<TEntity, TEntity>(entity, toUpdate);

                toUpdate.ModifiedBy = authorizingUser;
                toUpdate.ModifiedDate = DateTime.Now;
                
                // Verify that object is valid
                if (toUpdate.EnsureIsWellFormed.Count > 0)
                {

                    this.Log.Warn(
                            String.Format("Validation messages: {0}",
                            entity.EnsureIsWellFormed.GetLongDescription())
                        );

                    throw new InvalidOperationException("Object must be well formed to be stored.");

                }

                this.MainDomainRepository.SaveChanges();

                toUpdate = this.MainDomainRepository.GetByResourceId(toUpdate.ResourceId);

            }

            return toUpdate;

        }
    }
}
