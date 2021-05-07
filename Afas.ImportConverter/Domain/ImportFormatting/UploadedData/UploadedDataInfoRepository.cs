using Afc.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.ImportFormatting.UploadedData
{
    public class UploadedDataInfoRepository
        : BaseDomainRepository<BaseUploadedDataInfo, 
            IImportConverterDataContext>,
        IUploadedDataInfoRepository
    {
    }
}
