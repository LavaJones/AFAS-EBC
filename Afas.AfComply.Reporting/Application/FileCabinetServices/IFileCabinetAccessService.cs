using Afas.AfComply.Reporting.Domain.FileCabinet;
using Afas.Application;
using System;
using System.Collections.Generic;

namespace Afas.AfComply.Reporting.Application.FileCabinetServices
{


    public interface IFileCabinetAccessService : ICrudDomainService<FileCabinetAccess>
    {
        // For future use
        //List<FileCabinetAccess> GetByApplicationID(int CurrentApplicationId);

        /// <summary>
        /// GetByownerGuid method allows to get the FolderInformation by Resource id and ApplicationId
        /// </summary>
        /// <param name="EmployerResourcerId"> The employer Guid to get the Folder Information</param>
        /// <param name="CurrentApplicationId">The id of the application to get access the file of that particular application.</param>
        /// <returns>FolderInformation</returns>
        List<FileCabinetAccess> GetByOwnerGuid(Guid EmployerResourcerId, int CurrentApplicationId);
    }
}
