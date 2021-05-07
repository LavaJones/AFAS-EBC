using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Afas.Domain.POCO
{
    /// <summary>
    /// Object storing File meta Data in the DB for files that we are archiving.
    /// </summary>
    public class ArchiveFileInfo : BaseAfasModel
    {
        /// <summary>
        /// The Data Base Pk
        /// </summary>
        public long ArchiveFileInfoId { get; set; }

        /// <summary>
        /// The FK to the employer
        /// </summary>
        public int EmployerId { get; set; }

        /// <summary>
        /// The Employer Guid at the time of archiving (unless it somehow changed?)
        /// </summary>
        public Guid EmployerGuid { get; set; }

        /// <summary>
        /// The current time when this happened
        /// </summary>
        public DateTime ArchivedTime { get; set; }

        /// <summary>
        /// Just the File name before the archive
        /// </summary>
        [MaxLength(256)]
        public string FileName { get; set; }

        /// <summary>
        /// Just the Path the File is at 
        /// </summary>
        [MaxLength(256)]
        public string SourceFilePath { get; set; }

        /// <summary>
        /// The Final Path that the File is archived to.
        /// </summary>
        [MaxLength(256)]
        public string ArchiveFilePath { get; set; }

        /// <summary>
        /// Text explaining why a file was archived.
        /// </summary>
        [MaxLength(256)]
        public string ArchiveReason { get; set; }
    }
}
