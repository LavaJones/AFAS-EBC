using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.AfComply.Reporting.Domain.Approvals.SsisFileTransfer;
using Afas.Application;
using Afas.AfComply.Reporting.Application.Ssis;
using Afc.Core;
using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Application.Ssis
{

    public class SsisFileTransferService : ABaseCrudService<SsisFileTransfer>, ISsisFileTransferService
    {
        protected ISsisFileTransferRepository Repository { get; private set; }
        public SsisFileTransferService(ISsisFileTransferRepository repository):
            base(repository)
        {
            this.Repository = repository;
        }

      List<SsisFileTransfer> ISsisFileTransferService.GetFileTransferredThroughSsis(DateTime StartDate, DateTime EndDate)
        {
            return this.Repository.GetFileTransferredThroughSsis(StartDate, EndDate);
        }

        SsisFileTransfer ICrudDomainService<SsisFileTransfer>.AddNewEntity(SsisFileTransfer entity, string authorizingUser)
        {
            SharedUtilities.VerifyStringParameter(authorizingUser, "authorizingUser");

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
