using Afas.Domain;
using Afc.Framework.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Afas.AfComply.Reporting.Domain.FileCabinet
{
    public class FileCabinetAccessRepository : BaseDomainRepository<FileCabinetAccess, IReportingDataContext>, IFileCabinetAccessRepository
    {
        private ILog Log = LogManager.GetLogger(typeof(FileCabinetAccessRepository));

        private IQueryable<FileCabinetAccess> ContextWithChildrenLoaded
        {
            get
            {
                return Context
                    .Set<FileCabinetAccess>()
                    .Include(batch => batch.FileCabinetFolderInfo);
            }
        }

        List<FileCabinetAccess> IFileCabinetAccessRepository.GetByOwnerGuid(Guid EmployerResourcerId, int CurrentApplicationId)
        {
            return
                ContextWithChildrenLoaded
                .FilterForActive()
                .FilterForOwnerGuid(EmployerResourcerId, CurrentApplicationId).ToList();
                 
        }

        // For Future Use
        //List<FileCabinetAccess> IFileCabinetAccessRepository.GetByApplicationID(int CurrentApplicationId)
        //{
        //    return
        //      ContextWithChildrenLoaded
        //        .FilterForActive()
        //        .FilterForApplicationId(CurrentApplicationId).ToList();
        //}


    }
}
