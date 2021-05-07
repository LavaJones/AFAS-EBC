using System.Collections.Generic;
using Afas.AfComply.Reporting.Domain.Approvals.FileCabinet;
using Afas.Application;
using Afc.Core.Application;
using System;
using Afas.AfComply.Reporting.Domain.FileCabinet;

namespace Afas.AfComply.Reporting.Application.FileCabinetServices
{


    public interface IFileCabinetInfoService : ICrudDomainService<FileCabinetInfo>
    {
        /// <summary>
        /// SaveFileCabinetInfo method is used to Save the metadata of the Uploaded File
        /// </summary>
        /// <param name="FileCabinetInfos">FileCabinetInfo's represent's the FileCabinet class</param>
        /// <param name="requestor"> The user that edited the data.</param>
        ///  <returns>If the save was successfull.</returns>
        [RequiresSharedTransaction]
        void SaveFileCabinetInfo(FileCabinetInfo FileCabinetInfos, string requestor);

        /// <summary>
        /// GetFilesinFolder Method used to display the files of the selected Folder
        /// </summary>
        /// <param name="FolderId"> The  FolderId of the SelectedFolder to Display files</param>
        /// <returns>Files of a Selected Folder </returns>
        List<FileCabinetInfo> GetFilesInFolders(int FolderId);

        /// <summary>
        /// GetFilesForFolderByResourceId Method is used to display the Files by the resourceid of the selected Folder
        /// </summary>
        /// <param name="ResourceId">Guid of a SelectedFolder to Display Files</param>
        /// <param name="OwnerResourceId">Guid of Employer</param>
        /// <returns>Files of a selected Folder</returns>
        List<FileCabinetInfo> GetFilesForFolderByResourceId(Guid ResourceId, Guid OwnerResourceId);

        /// <summary>
        ///  GetFilesForEmployeeResourceId is used to retieve the existing files by EmployeeResourceID
        /// </summary>
        /// <param name="otherResourceId">Guid of a Employee(EmployeeResourceID)</param>
        /// <param name="FileCabinetFolderInfo_ID">FolderId of a selected tax year</param>
        /// <returns>Exisitng Files of Employee</returns>
        FileCabinetInfo GetFilesForEmployeeResourceId(Guid otherResourceId, long FileCabinetFolderInfo_ID);

        /// <summary>
        /// DeactivateFileCabinetInfo Method is used to Deactivate the EntityStatus of a Existing File
        /// </summary>
        /// <param name="otherResourceId">Guid of a Employee(EmployeeResourceID)</param>
        /// <param name="FileCabinetFolderInfo_ID">FolderId of a selected tax year</param>
        /// <param name="requestor">The user that edited the data.</param>
        void DeactivateFileCabinetInfo(Guid otherResourceId, string requestor, long FileCabinetFolderInfo_ID);

        /// <summary>
        /// DeactivateFolderInFileCabinetInfo is used to Deactivate the Folder
        /// Deactivate Entitystatus = changing Entitty status = 2
        /// </summary>
        /// <param name="ResourceId">Guid of a SelectedFolder</param>
        /// <param name="OwnerResourceId">Guid of Employer</param>
        /// <param name="requestor">The user that edited the data.</param>
        void DeactivateFolderInFileCabinetInfo(Guid ResourceId, Guid OwnerResourceId, string requestor);
    }
}
