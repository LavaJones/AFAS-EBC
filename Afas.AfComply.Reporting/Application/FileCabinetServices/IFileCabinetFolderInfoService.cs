using Afas.AfComply.Reporting.Domain.FileCabinet;
using Afas.Application;
using System;


namespace Afas.AfComply.Reporting.Application.FileCabinetServices
{


    public interface IFileCabinetFolderInfoService : ICrudDomainService<FileCabinetFolderInfo>
    {

        /// <summary>
        /// GetFolderByResourceId method is used to get the FileCabinetFolderInfo
        /// </summary>
        /// <param name="ResourceId"> ResourceId of a Folder </param>
        /// <returns>FileCabinetFolderInfo </returns>
        FileCabinetFolderInfo GetFolderByResourceId(Guid ResourceId);

        /// <summary>
        /// This Gets the Single Folder under the root 1095 Folder, matching the Tax Year passed in.
        /// </summary>
        /// <param name="TaxYear">The Tax Year of which 1095 Folder to get</param>
        /// <returns>The single Folder Info under 1095 that matches the TaxYear Provided.</returns>
        FileCabinetFolderInfo GetFolderInfoBy1095TaxYear(int TaxYear);
    }
}
