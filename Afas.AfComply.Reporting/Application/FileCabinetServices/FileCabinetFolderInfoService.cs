using Afas.AfComply.Reporting.Domain.FileCabinet;
using Afas.Application;
using Afc.Core;
using Afc.Core.Domain;
using System;

namespace Afas.AfComply.Reporting.Application.FileCabinetServices
{
    public class FileCabinetFolderInfoService : ABaseCrudService<FileCabinetFolderInfo>, IFileCabinetFolderInfoService
    {
        protected IFileCabinetFolderInfoRepository Repository { get; private set; }

        public FileCabinetFolderInfoService(
         IFileCabinetFolderInfoRepository repository) :
                base(repository)
        {
            this.Repository = repository;
        }

        FileCabinetFolderInfo IFileCabinetFolderInfoService.GetFolderByResourceId(Guid ResourceId)
        {
            return this.Repository.GetFolderByResourceId(ResourceId);
        }

        FileCabinetFolderInfo IFileCabinetFolderInfoService.GetFolderInfoBy1095TaxYear(int TaxYear)
        {
            return this.Repository.GetFolderInfoBy1095TaxYear(TaxYear);
        }

        FileCabinetFolderInfo ICrudDomainService<FileCabinetFolderInfo>.AddNewEntity(FileCabinetFolderInfo entity, string authorizingUser)
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