using Afas.Application;
using Afas.ImportConverter.Domain.ImportFormatting;
using Afas.ImportConverter.Domain.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Application.Services
{
    public interface IFormatCommandService : ICrudDomainService<AImportFormatCommand>
    {

        /// <summary>
        /// Gets a new instance of a specific type of command.
        /// </summary>
        /// <param name="tyoe">The type to get the command for.</param>
        /// <returns>The command or null.</returns>
        IImportFormatCommand GetNewOfType(ImportFormatCommandTypes type);

    }
}
