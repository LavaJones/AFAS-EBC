using Afas.Application;
using Afas.Application.Archiver;
using Afas.Domain.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Application.Services
{
    public interface IArchiveFileInfoService : ICrudDomainService<ArchiveFileInfo>, IArchiveFileInfoAccess
    {

        /// <summary>
        /// Get an archive record by it's database id
        /// </summary>
        /// <param name="id">The DB key of the Record</param>
        /// <returns>The record.</returns>
        [Obsolete("This is only remaining to support legacy applications.")]
        ArchiveFileInfo GetById(int id);

    }
}
