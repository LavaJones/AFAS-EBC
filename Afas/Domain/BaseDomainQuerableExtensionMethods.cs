using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Domain
{
    /// <summary>
    /// High level generic 
    /// </summary>
    public static class BaseDomainQuerableExtensionMethods
    {

        /// <summary>
        /// Filter the domainObjects for only those in the active status.
        /// </summary>
        /// <param name="timeFrames">The object ot filter.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<TEntity> FilterForActive<TEntity>(this IQueryable<TEntity> entities) where TEntity : BaseAfasModel
        {

            List<EntityStatusEnum> activeStatuses = new List<EntityStatusEnum>() { EntityStatusEnum.Active };

            return (
                    from entity in entities
                    where activeStatuses.Contains(entity.EntityStatus)
                    select entity
                    );

        }

        ///// <summary>
        ///// Filter the domainObjects for only those in the active status.
        ///// </summary>
        ///// <param name="timeFrames">The object ot filter.</param>
        ///// <returns>The filtered query.</returns>
        //public static IQueryable<TEntity> FilterForActive<TEntity>(this IQueryable<TEntity> entities) where TEntity : BaseAfasModel
        //{

        //    List<EntityStatusEnum> activeStatuses = new List<EntityStatusEnum>() { EntityStatusEnum.Active };

        //    return (
        //            from entity in entities
        //            where activeStatuses.Contains(entity.EntityStatus)
        //            select entity
        //            );

        //}
    }
}
