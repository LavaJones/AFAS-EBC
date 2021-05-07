using Afas.Application;
using Afas.ImportConverter.Domain.ImportFormatting;
using Afas.ImportConverter.Domain.ImportFormatting.Commands;
using Afas.ImportConverter.Domain.ImportFormatting.Staging;
using Afas.ImportConverter.Domain.ImportFormatting.UploadedData;
using Afas.ImportConverter.Domain.POCO;
using Afc.Core;
using Afc.Core.Domain;
using Afc.Framework.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afc.Core.Application;

namespace Afas.ImportConverter.Application.Services
{
    /// <summary>
    /// A service explosing access to the  domain models.
    /// </summary>
    public class UploadedFileInfoService : ABaseCrudService<UploadedFileInfo>, IUploadedFileService
    {
        protected IUploadedFileInfoRepository Repository { get; private set; }

        /// <summary>
        /// Standard Constructor taking the dependencies as parameters. 
        /// </summary>
        /// <param name="Repository">The Repository to get the .</param>
        public UploadedFileInfoService(
            IUploadedFileInfoRepository repository) : 
                base(repository)
        {

            this.Repository = repository;

        }

        IQueryable<BaseUploadedDataInfo> ICrudDomainService<BaseUploadedDataInfo>.GetAllEntities()
        {
            return ((IUploadedFileInfoService)this).GetAllEntities();
        }

        BaseUploadedDataInfo ICrudDomainService<BaseUploadedDataInfo>.AddNewEntity(BaseUploadedDataInfo toAdd, string authorizingUser)
        {
            return ((IUploadedFileInfoService)this).AddNewEntity((UploadedFileInfo)toAdd, authorizingUser);
        }

        BaseUploadedDataInfo ICrudDomainService<BaseUploadedDataInfo>.DeactivateEntity(Guid ResourceId, string authorizingUser)
        {
            return ((IUploadedFileInfoService)this).DeactivateEntity(ResourceId, authorizingUser);
        }

        BaseUploadedDataInfo ICrudDomainService<BaseUploadedDataInfo>.UpdateEntity(BaseUploadedDataInfo toUpdate, string authorizingUser)
        {
            return ((IUploadedFileInfoService)this).UpdateEntity((UploadedFileInfo)toUpdate, authorizingUser);
        }

        BaseUploadedDataInfo IDomainService<BaseUploadedDataInfo>.GetByResourceId(Guid resourceId)
        {
            return ((IUploadedFileInfoService)this).GetByResourceId(resourceId);
        }

        IQueryable<BaseUploadedDataInfo> IEntityService<BaseUploadedDataInfo>.AreActive()
        {
            return ((IUploadedFileInfoService)this).AreActive();
        }

        IQueryable<BaseUploadedDataInfo> IEntityService<BaseUploadedDataInfo>.AreDeleted()
        {
            return ((IUploadedFileInfoService)this).AreDeleted();
        }

        IQueryable<BaseUploadedDataInfo> IEntityService<BaseUploadedDataInfo>.AreInactive()
        {
            return ((IUploadedFileInfoService)this).AreInactive();
        }

        BaseUploadedDataInfo IService<BaseUploadedDataInfo>.Add(BaseUploadedDataInfo entity)
        {
            return ((IUploadedFileInfoService)this).Add((UploadedFileInfo)entity);
        }

        BaseUploadedDataInfo IService<BaseUploadedDataInfo>.Delete(BaseUploadedDataInfo entity)
        {
            return ((IUploadedFileInfoService)this).Delete((UploadedFileInfo)entity);
        }

        BaseUploadedDataInfo IService<BaseUploadedDataInfo>.Update(BaseUploadedDataInfo entity)
        {
            return ((IUploadedFileInfoService)this).Update((UploadedFileInfo)entity);
        }
    }
}
