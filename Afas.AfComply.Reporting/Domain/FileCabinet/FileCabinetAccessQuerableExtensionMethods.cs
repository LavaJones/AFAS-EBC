using System;
using System.Linq;

namespace Afas.AfComply.Reporting.Domain.FileCabinet
{
    public static class FileCabinetAccessQuerableExtensionMethods
    {
        //Future Use
        //public static IQueryable<FileCabinetAccess> FilterForApplicationId(this IQueryable<FileCabinetAccess> FileCabinetAccessInfo, int CurrentApplicationId)
        //{
        //    return (
        //        from FileCabinetAccessInformation in FileCabinetAccessInfo
        //        where FileCabinetAccessInformation.ApplicationId == CurrentApplicationId
        //        select FileCabinetAccessInformation
        //        );
        //}

        public static IQueryable<FileCabinetAccess> FilterForOwnerGuid(this IQueryable<FileCabinetAccess> FileCabinetAccessInfo, Guid EmployerResourcerId, int CurrentApplicationId)
        {
            return (
                from FileCabinetAccessInformation in FileCabinetAccessInfo
                where FileCabinetAccessInformation.OwnerResourceId == EmployerResourcerId && FileCabinetAccessInformation.ApplicationId == CurrentApplicationId
                select FileCabinetAccessInformation
                );
        }
    }
}
