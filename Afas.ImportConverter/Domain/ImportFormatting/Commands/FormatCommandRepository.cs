using Afc.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.ImportFormatting.Commands
{
    public class FormatCommandRepository 
        : BaseDomainRepository<AImportFormatCommand, 
            IImportConverterDataContext>, 
        IFormatCommandRepository
    {
    }
}
