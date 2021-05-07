using Afas.Domain;
using Afc.Core.Application;
using Afc.Core.Domain;
using Afc.Framework.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Application
{
    public abstract class ABaseDomainService<TEntity> : BaseDomainService<TEntity>, IDomainService<TEntity> 
        where TEntity : BaseAfasModel
    {
        public ABaseDomainService(
                IDomainRepository<TEntity> mainDomainRepository,
                params IRepository[] repositories) :
            base(mainDomainRepository, repositories) { }

        public ABaseDomainService(
            IDomainRepository<TEntity> mainDomainRepository) :
            base(mainDomainRepository) { }
        
        /// <summary>
        /// Gets an entity by the resource Id
        /// </summary>
        /// <param name="ResourceId">The Id of the entity to get.</param>
        /// <returns>The entity with that Id.</returns>
        TEntity IDomainService<TEntity>.GetByResourceId(Guid ResourceId)
        {
            
            return this.MainDomainRepository.GetByResourceId(ResourceId);

        }

      
        public override void ProcessModelDeletion(TEntity fromClient, ref TEntity fromRepository)
        {
            throw new System.InvalidOperationException("This is not valid from this interface.");
        }

        public override void ProcessModelUpdate(TEntity fromClient, ref TEntity fromRepository)
        {
            throw new System.InvalidOperationException("This is not valid from this interface.");
        }
    }
}
