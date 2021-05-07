using Afas.Application;
using Afas.ImportConverter.Domain.ImportFormatting;
using Afas.ImportConverter.Domain.ImportFormatting.Staging;
using Afas.ImportConverter.Domain.ImportFormatting.UploadedData;
using Afas.ImportConverter.Domain.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Application.Services
{
    public interface IUploadedFileInfoService : ICrudDomainService<UploadedFileInfo>
    {
    }
}
