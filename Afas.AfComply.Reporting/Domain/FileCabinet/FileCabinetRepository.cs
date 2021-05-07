using Afas.Domain;
using Afc.Framework.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using Afas.Domain.POCO;
using Afc.Core;
using Afc.Core.Domain;
using System.Data.Entity.Validation;
using Afas.AfComply.Reporting.Domain.FileCabinet;


namespace Afas.AfComply.Reporting.Domain.Approvals.FileCabinet
{
    public class FileCabinetRepository : BaseDomainRepository<FileCabinetInfo, IReportingDataContext>, IFileCabinetRepository
    {
        private ILog Log = LogManager.GetLogger(typeof(FileCabinetRepository));

        /// <summary>
        /// Save FileCabinet Info method is used for Saving  the enw files.
        /// Even if we upload the Same files it saves a new files
        /// </summary>
        /// <param name="FileCabinetInfos">Uploaded File metaData</param>
        /// <param name="requestor">The user that uploaded the Files</param>
        void IFileCabinetRepository.SaveFileCabinetInfo(FileCabinetInfo FileCabinetInfos, string requestor)
        {
            FileCabinetInfos.ModifiedDate = DateTime.Now;
            FileCabinetInfos.ModifiedBy = requestor;

            FileCabinetInfos.CreatedBy = requestor;
            FileCabinetInfos.ResourceId = Guid.NewGuid();
            FileCabinetInfos.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
            if (FileCabinetInfos.EnsureIsWellFormed.Count > 0)
            {
                this.Log.Warn(
                   String.Format("Validation messages: {0}",
                   FileCabinetInfos.EnsureIsWellFormed.GetLongDescription())
               );

                throw new InvalidOperationException("Object must be well formed to be stored.");
            }

            //Inserting a Row that has a child that already exists in the DB
            this.Context.Set<ArchiveFileInfo>().Attach(FileCabinetInfos.ArchiveFileInfo);
            this.Context.Set<FileCabinetInfo>().Add(FileCabinetInfos);
            this.Context.SaveChanges();

        }

        List<FileCabinetInfo> IFileCabinetRepository.GetFilesInFolders(int FolderId)
        {
            return
                this.Context.Set<FileCabinetInfo>()
                .FilterForActive()
                .FilterForFolder(FolderId).ToList();
        }


        List<FileCabinetInfo> IFileCabinetRepository.GetFilesForFolderByResourceId(Guid ResourceId, Guid OwnerResourceId)
        {
            return
                this.Context.Set<FileCabinetInfo>()
                .FilterForActive()
                .FilterForFolderResourceID(ResourceId, OwnerResourceId).ToList();
        }

        FileCabinetInfo IFileCabinetRepository.GetFilesForEmployeeResourceId(Guid otherResourceId, long FileCabinetFolderInfo_ID)
        {
            return
               this.Context.Set<FileCabinetInfo>()
                .FilterForActive()
                .FilterForEmployeeResourceId(otherResourceId, FileCabinetFolderInfo_ID).SingleOrDefault();
        }

        /// <summary>
        /// DeactivateFolderInFileCabinetInfo is used to Deactivate the Folder
        /// Deactivate Entitystatus = changing Entitty status = 2
        /// </summary>
        /// <param name="ResourceId">Guid of a SelectedFolder</param>
        /// <param name="OwnerResourceId">Guid of Employer</param>
        /// <param name="requestor">The user that edited the data.</param>
        void IFileCabinetRepository.DeactivateFolderInFileCabinetInfo(Guid ResourceId, Guid OwnerResourceId, string requestor)
        {
            SharedUtilities.VerifyStringParameter(requestor, "requestor");

            //FilterForFolderResourceId method is used to retrive the Files of Selected Folder
            List<FileCabinetInfo> FolderFiles = this.FilterForFolderResourceID(ResourceId, OwnerResourceId).ToList();
            foreach (FileCabinetInfo File in FolderFiles)
            {
                File.EntityStatus = EntityStatusEnum.Inactive;
                File.ModifiedBy = requestor;
                File.ModifiedDate = DateTime.Now;

                if (File.EnsureIsWellFormed.Count > 0)
                {
                    this.Log.Warn(
                    String.Format("Validation messages: {0}",
                    File.EnsureIsWellFormed.GetLongDescription()));
                    throw new InvalidOperationException("Object must be well formed After updating.");

                }
                this.Context.SaveChanges();
            }


        }


    }

}