using Afc.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afc.Core.Domain;

namespace Afas.ImportConverter.Domain.ImportFormatting.UploadedData
{
    public class UploadedFileInfoRepository
        : BaseDomainRepository<UploadedFileInfo,
            IImportConverterDataContext>,
            IUploadedFileInfoRepository
    {
    }
}
