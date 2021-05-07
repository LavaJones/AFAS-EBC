using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Application.Archiver
{
    /// <summary>
    /// This object is used to provide information about the systems configuration.
    /// </summary>
    public interface IArchiveLocationProvider
    {
        /// <summary>
        /// The locationthat the archiver should use
        /// </summary>
        string ArchiveFolderLocation { get; }

    }
}
