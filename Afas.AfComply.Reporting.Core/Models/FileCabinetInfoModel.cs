using System;
using System.ComponentModel.DataAnnotations;
using Afc.Marketing.Models;
using Afas.Domain.POCO;

namespace Afas.AfComply.Reporting.Core.Models
{
    public class FileCabinetInfoModel : Model
    {
        public FileCabinetInfoModel() : base() { }

        [MaxLength(256)]
        [Required]
        public virtual String Filename { get; set; }

        [MaxLength(1000)]
        public virtual String FileDescription { get; set; }

        public virtual String FileType { get; set; }
        [Required]
        public virtual Guid OwnerResourceId { get; set; }
        [Required]
        public virtual int ApplicationId { get; set; }

        /// <summary>
        /// Other ResourceId Represents Employee ResourceId
        /// </summary>
        public virtual Guid? OtherResourceId { get; set; }

        [Required]
        public virtual ArchiveFileInfo ArchiveFileInfo { get; set; }

        public virtual FileCabinetFolderInfoModel FileCabinetFolderInfo { get; set; }
    }
}
