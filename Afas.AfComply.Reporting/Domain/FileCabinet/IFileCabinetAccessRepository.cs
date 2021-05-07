using System;
using System.Collections.Generic;
using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.FileCabinet
{
    public interface IFileCabinetAccessRepository : IDomainRepository<FileCabinetAccess>
    {
        /// <summary>
        /// GetByownerGuid method allows to get the folderInformation by Resource id and ApplicationId
        /// </summary>
        /// <param name="EmployerResourcerId"> The employer Guid to get the Folder Information </param>
        /// <param name="CurrentApplicationId"> The id of the application to get access the files of that particular application. </param>
        /// <returns></returns>
        List<FileCabinetAccess> GetByOwnerGuid(Guid EmployerResourcerId, int CurrentApplicationId);

        // Future Use
       //  List<FileCabinetAccess> GetByApplicationID(int CurrentApplicationId);

    }
}
