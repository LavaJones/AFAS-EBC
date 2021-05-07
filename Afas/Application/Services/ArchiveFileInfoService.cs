using Afas.Application;
using Afas.Application.Archiver;
using Afas.Domain.POCO;
using Afc.Core;
using Afc.Core.Domain;
using Afc.Framework.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Application.Services
{
    /// <summary>
    /// A service explosing access to the  domain models.
    /// </summary>
    public class ArchiveFileInfoService : ABaseCrudService<ArchiveFileInfo>, IArchiveFileInfoService
    {
        protected IArchiveFileInfoRepository Repository { get; private set; }

        /// <summary>
        /// Standard Constructor taking the dependencies as parameters. 
        /// </summary>
        /// <param name="Repository">The Repository to get the .</param>
        public ArchiveFileInfoService(
            IArchiveFileInfoRepository repository) : 
                base(repository)
        {

            this.Repository = repository;

        }

        /// <summary>
        /// Get an archive record by it's database id
        /// </summary>
        /// <param name="id">THe DB key of the Record</param>
        /// <returns>The record.</returns>
        [Obsolete("This is only remaining to support legacy applications.")]
        ArchiveFileInfo IArchiveFileInfoService.GetById(int id)
        {

            return (from ArchiveFileInfo info in this.Repository.AreActive() where info.ArchiveFileInfoId == id select info).Single();

        }

        int IArchiveFileInfoAccess.SaveArchiveFileInfo(Guid employerGuid, int employerId, string FileName, string SourceFilePath, string ArchiveFilePath, string userId, string reason)
        {
            SharedUtilities.VerifyStringParameter(userId, "authorizingUser");

            ArchiveFileInfo entity = new ArchiveFileInfo();

            entity.ArchivedTime = DateTime.Now;
            entity.ArchiveFilePath = ArchiveFilePath;
            entity.ArchiveReason = reason;
            entity.EmployerGuid = employerGuid;
            entity.EmployerId = employerId;
            entity.FileName = FileName;
            entity.SourceFilePath = SourceFilePath;

            // Set initial state 
            entity.CreatedBy = userId;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            entity.ResourceId = Guid.NewGuid();
            entity.EntityStatus = EntityStatusEnum.Active;

            // Verify that Object is valid
            if (entity.EnsureIsWellFormed.Count > 0)
            {

                throw new InvalidOperationException("Object must be well formed to be stored.");

            }

            this.MainDomainRepository.InsertOnSubmit(entity);
            this.MainDomainRepository.SaveChanges();

            //this.MainDomainRepository.GetByResourceId(entity.ResourceId);

            return (int)entity.ArchiveFileInfoId;
        }
    }

}
