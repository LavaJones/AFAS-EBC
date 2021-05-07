using Afas.Domain;
using Afc.Framework.Domain;
using log4net;
using System;
using System.Linq;

namespace Afas.AfComply.Reporting.Domain.FileCabinet
{
    public class FileCabinetFolderInfoRepository : BaseDomainRepository<FileCabinetFolderInfo, IReportingDataContext>, IFileCabinetFolderInfoRepository
    {

        private readonly ILog Log = LogManager.GetLogger(typeof(FileCabinetFolderInfoRepository));

        FileCabinetFolderInfo IFileCabinetFolderInfoRepository.GetFolderByResourceId(Guid ResourceId)
        {
            return
                this.Context.Set<FileCabinetFolderInfo>()
                .FilterForActive()
                .FilterForFolderResourceID(ResourceId).SingleOrDefault();
        }

        FileCabinetFolderInfo IFileCabinetFolderInfoRepository.GetFolderInfoBy1095TaxYear(int Taxyear)
        {
            return
                this.Context.Set<FileCabinetFolderInfo>()
                .FilterForActive()
                .FilterFor1095TaxYear(Taxyear)
                .SingleOrDefault();
        }
    }
}
