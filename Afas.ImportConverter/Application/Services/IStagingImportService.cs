using Afas.Application;
using Afas.ImportConverter.Domain.ImportFormatting;
using Afas.ImportConverter.Domain.ImportFormatting.Staging;
using Afas.ImportConverter.Domain.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Application.Services
{
    public interface IStagingImportService : ICrudDomainService<StagingImport>
    {
       
        /// <summary>
        /// This updates the current StagingImport then returns a new StagingImport to continue processing.
        /// </summary>
        /// <param name="import">The currently processed import. Note: the Modified DataTable should be set.</param>
        /// <param name="authorizingUser">The user deactivating the Entity</param>
        /// <returns>A new StagingImport object for continued processing.</returns>
        StagingImport UpdateAfterProcessing(StagingImport import, String authorizingUser);
        
    }
}
