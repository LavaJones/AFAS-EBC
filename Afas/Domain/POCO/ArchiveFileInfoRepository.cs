using Afc.Framework.Domain;


namespace Afas.Domain.POCO
{
    public class ArchiveFileInfoRepository
        : BaseDomainRepository<ArchiveFileInfo,
            IAfasDataContext>,
            IArchiveFileInfoRepository
    {

    }
}
