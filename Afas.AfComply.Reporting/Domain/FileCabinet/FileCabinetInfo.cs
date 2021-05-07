using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Afas.Domain.POCO;
using Afc.Core.Domain;
using Afas.Domain;
using Afc.Core;
using Afas.AfComply.Reporting.Domain.FileCabinet;

namespace Afas.AfComply.Reporting.Domain.Approvals.FileCabinet
{
    /// <summary>
    //Object stroing meta Data in the DB for files that we are uploading through FileCabinet 
    /// </summary>
    public class FileCabinetInfo : BaseReportingModel
    {
        /// <summary>
        /// Filename represents name of the file given by user while Uploading file into File Cabinet
        /// </summary>
        /// 
        [MaxLength(256)]
        [Required]
        public virtual String Filename { get; set; }

        /// <summary>
        /// FileDescription represents description given by user while Uploading file into File Cabinet
        /// </summary>
        [MaxLength(1000)]
        public virtual String FileDescription { get; set; }

        /// <summary>
        /// FileType represnts type of file Uploaded in the File Cabinet
        /// </summary>
        public virtual String FileType { get; set; }

        /// <summary>
        /// Owner Resource id represents Employer Resource id
        /// </summary>
        [Required]
        public virtual Guid OwnerResourceId { get; set; }


        [Required]
        public virtual int ApplicationId { get; set; }

        /// <summary>
        /// Other ResourceId Represents Employee ResourceId
        /// </summary>

        public virtual Guid? OtherResourceId { get; set; }

        /// <summary>
        /// ArchiveFileInfo represents the Archived information of file uploaded in FileCabinet
        /// </summary>
        [Required]
        public virtual ArchiveFileInfo ArchiveFileInfo { get; set; }
        /// <summary>
        /// FileCabinetFolderInfo represnts the Folderstructure of File system
        /// </summary>
        public virtual FileCabinetFolderInfo FileCabinetFolderInfo { get; set; }

        public override IList<ValidationMessage> EnsureIsWellFormed
        {
            get
            {
                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                if ((null == Filename) || (true == Filename.Trim().IsNullOrEmpty()))
                {
                    validationMessages.Add(new ValidationMessage("Filename", "Filename cannot be empty", ValidationMessageSeverity.Error));
                }

                if (Filename.Length > 256)
                {
                    validationMessages.Add(new ValidationMessage("Filename", "Filename should be less than 256 Characters", ValidationMessageSeverity.Error));
                }
                if (FileDescription.Length > 1000)
                {
                    validationMessages.Add(new ValidationMessage("FileDescription", "FileDescription should be less than 1000 Characters", ValidationMessageSeverity.Error));
                }

                SharedUtilities.ValidateObject(this.ArchiveFileInfo, "ArchiveFileInfo", validationMessages);

                SharedUtilities.ValidateGuid(this.OwnerResourceId, "OwnerResourceId", validationMessages);

                return validationMessages;

            }
        }

    }
}
