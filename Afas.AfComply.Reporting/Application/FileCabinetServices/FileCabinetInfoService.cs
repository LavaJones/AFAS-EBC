using System;
using System.Collections.Generic;
using Afas.AfComply.Reporting.Domain.Approvals.FileCabinet;
using Afc.Core;
using Afc.Core.Domain;
using Afas.Application;
using System.Data.Entity.Validation;
using Afas.AfComply.Reporting.Domain.FileCabinet;

namespace Afas.AfComply.Reporting.Application.FileCabinetServices
{
    public class FileCabinetInfoService : ABaseCrudService<FileCabinetInfo>, IFileCabinetInfoService
    {
        protected IFileCabinetRepository Repository { get; private set; }
        public FileCabinetInfoService(
         IFileCabinetRepository repository) :
                base(repository)
        {

            this.Repository = repository;

        }

        void IFileCabinetInfoService.SaveFileCabinetInfo(FileCabinetInfo FileCabinetInfos, string requestor)
        {

            this.Repository.SaveFileCabinetInfo(FileCabinetInfos, requestor);

        }

        List<FileCabinetInfo> IFileCabinetInfoService.GetFilesInFolders(int FolderId)
        {
            return this.Repository.GetFilesInFolders(FolderId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ResourceId">Guid of a selected Folder to Display the files </param>
        /// <param name="OwnerResourceId"> Guid of a Employer</param>
        /// <returns></returns>
        List<FileCabinetInfo> IFileCabinetInfoService.GetFilesForFolderByResourceId(Guid ResourceId, Guid OwnerResourceId)
        {
            return this.Repository.GetFilesForFolderByResourceId(ResourceId, OwnerResourceId);
        }

        /// <summary>
        /// GetFilesForEmployeeResourceId is used to retieve the existing files by EmployeeResourceID
        /// </summary>
        /// <param name="otherResourceId">Guid of a Employee(EmployeeResourceId)</param>
        /// <param name="FileCabinetFolderInfo_ID">FolderId of a selected tax year</param>
        /// <returns>Exisitng Files of Employee</returns>
        FileCabinetInfo IFileCabinetInfoService.GetFilesForEmployeeResourceId(Guid otherResourceId, long FileCabinetFolderInfo_ID)
        {
            return this.Repository.GetFilesForEmployeeResourceId(otherResourceId, FileCabinetFolderInfo_ID);
        }

        /// <summary>
        /// UpdateFileCabinetInfo Method is used to Deactivate the EntityStatus of a Existing File
        /// </summary>
        /// <param name="otherResourceId">Guid of a Employee(EmployeeResourceID)</param>
        /// <param name="FileCabinetFolderInfo_ID">FolderId of a selected tax year</param>
        /// <param name="requestor">The user that edited the data</param>
        void IFileCabinetInfoService.DeactivateFileCabinetInfo(Guid otherResourceId, string requestor, long FileCabinetFolderInfo_ID)
        {
            SharedUtilities.VerifyStringParameter(requestor, "requestor");
            FileCabinetInfo FileCabinetFiles = this.Repository.GetFilesForEmployeeResourceId(otherResourceId, FileCabinetFolderInfo_ID);

            if (null != FileCabinetFiles)
            {
                FileCabinetFiles.EntityStatus = EntityStatusEnum.Inactive;
                FileCabinetFiles.ModifiedBy = requestor;
                FileCabinetFiles.ModifiedDate = DateTime.Now;

                if (FileCabinetFiles.EnsureIsWellFormed.Count > 0)
                {
                    this.Log.Warn(
                       String.Format("Validation messages: {0}",
                       FileCabinetFiles.EnsureIsWellFormed.GetLongDescription())
                   );
                    throw new InvalidOperationException("Object must be well formed to be stored.");
                }

                this.Repository.SaveChanges();
            }

        }
        /// <summary>
        /// DeactivateFolderInFileCabinetInfo is used to Deactivate the Folder
        /// Deactivate Entitystatus = changing Entitty status = 2
        /// </summary>
        /// <param name="ResourceId">Guid of a SelectedFolder</param>
        /// <param name="OwnerResourceId">Guid of Employer</param>
        /// <param name="requestor">The user that edited the data.</param>
        void IFileCabinetInfoService.DeactivateFolderInFileCabinetInfo(Guid ResourceId, Guid OwnerResourceId, string requestor)
        {
            this.Repository.DeactivateFolderInFileCabinetInfo(ResourceId, OwnerResourceId, requestor);
        }
        /// <summary>
        /// Adds a new Entity to the database
        /// </summary>
        /// <param name="toAdd">The entity to Add</param>
        /// <param name="authorizingUser">The User adding the entity</param>
        /// <returns>The Added Entity.</returns>
        FileCabinetInfo ICrudDomainService<FileCabinetInfo>.AddNewEntity(FileCabinetInfo entity, string authorizingUser)
        {

            SharedUtilities.VerifyStringParameter(authorizingUser, "authorizingUser");

            // Set initial state 
            entity.CreatedBy = authorizingUser;
            entity.ModifiedBy = authorizingUser;
            entity.ModifiedDate = DateTime.Now;
            entity.ResourceId = Guid.NewGuid();
            entity.EntityStatus = EntityStatusEnum.Active;

            this.MainDomainRepository.InsertOnSubmit(entity);
            this.MainDomainRepository.SaveChanges();

            return this.MainDomainRepository.GetByResourceId(entity.ResourceId);
        }
    }
}
