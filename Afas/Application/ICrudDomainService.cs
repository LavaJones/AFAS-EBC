using Afas.Domain;
using Afc.Core.Application;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Application
{
    /// <summary>
    /// Standard CRUD methods for an Entity
    /// </summary>
    /// <typeparam name="TEntity">An Entity of type BaseReportingModel</typeparam>
    public interface ICrudDomainService<TEntity> : IDomainService<TEntity> where TEntity : BaseAfasModel
    {
        //Note: base interface has get by Id

        /// <summary>
        /// Gets all availible entities
        /// </summary>
        /// <returns>all availible entities</returns>
        [RequiresSharedTransaction]
        IQueryable<TEntity> GetAllEntities();

        /// <summary>
        /// Adds a new Entity to the database
        /// </summary>
        /// <param name="toAdd">The entity to Add</param>
        /// <param name="authorizingUser">The User adding the entity</param>
        /// <returns>The Added Entity.</returns>
        [RequiresSharedTransaction]
        TEntity AddNewEntity(TEntity toAdd, String authorizingUser);

        /// <summary>
        /// Deactivates an entity based on the provided Resource Id (aka soft delete)
        /// </summary>
        /// <param name="ResourceId">The Id of the Entity to deactivate</param>
        /// <param name="authorizingUser">The user deactivating the Entity</param>
        /// <returns>The Deactivated Entity</returns>
        [RequiresSharedTransaction]
        TEntity DeactivateEntity(Guid ResourceId, String authorizingUser);

        /// <summary>
        /// Update the values of an entity
        /// </summary>
        /// <param name="toUpdate">The Entity with updated Values</param>
        /// <param name="authorizingUser">The User updating the entity</param>
        /// <returns>The Updated Entity</returns>
        [RequiresSharedTransaction]
        TEntity UpdateEntity(TEntity toUpdate, String authorizingUser);

    }
}
