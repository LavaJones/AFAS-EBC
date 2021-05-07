using Afas.AfComply.Reporting.Domain.FileCabinet;
using Afas.Application;
using Afc.Core;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Application.FileCabinetServices
{
    public class FileCabinetAccessService : ABaseCrudService<FileCabinetAccess>, IFileCabinetAccessService
    {
        protected IFileCabinetAccessRepository Repository { get; private set; }

        public FileCabinetAccessService(
           IFileCabinetAccessRepository repository) :
            base(repository)
        {
            this.Repository = repository;
        }

        // For Later Use 
        //List<FileCabinetAccess> IFileCabinetAccessService.GetByApplicationID(int CurrentApplicationId)
        //{
        //    return this.Repository.GetByApplicationID(CurrentApplicationId);
        //}

        List<FileCabinetAccess> IFileCabinetAccessService.GetByOwnerGuid(Guid EmployerResourcerId, int CurrentApplicationId)
        {

            List<FileCabinetAccess> fromDB = this.Repository.GetByOwnerGuid(EmployerResourcerId, CurrentApplicationId);

            foreach (var item in fromDB)
            {
                item.children = new List<FileCabinetAccess>();

                foreach (var child in item.FileCabinetFolderInfo.children)
                {

                    FileCabinetAccess toAdd = fromDB.Find(find => find.FileCabinetFolderInfo == child);
                    item.children.Add(toAdd);

                }

            }

            return fromDB.Where(row => row.FileCabinetFolderInfo.FolderDepth == 0).ToList();
        }


        FileCabinetAccess ICrudDomainService<FileCabinetAccess>.AddNewEntity(FileCabinetAccess entity, string authorizingUser)
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
