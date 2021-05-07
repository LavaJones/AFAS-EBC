using Afas.Domain.POCO;
using Afc.Core;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.ImportFormatting.UploadedData
{

    /// <summary>
    /// This class holds all the meta-data that we need when a file is uploaded to the system.
    /// </summary>
    public class UploadedFileInfo : BaseUploadedDataInfo
    {

        /// <summary>
        /// Text denoting what type of file was uploaded, ex: CSV, TSV, excel, etc.
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string FileTypeDescription { get; set; }

        /// <summary>
        /// The File (+Path) That the File started at. 
        /// </summary>
        [MaxLength(256)]
        [Required]
        public string FileName { get; set; }

        /// <summary>
        /// The Archived File that was upload (or null if it has not yet been archived)
        /// </summary>
        public ArchiveFileInfo ArchiveFileInfo { get; set; }


        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                SharedUtilities.ValidateString(this.FileTypeDescription, "FileTypeDescription", validationMessages);

                SharedUtilities.ValidateString(this.FileName, "FileName", validationMessages);

                return validationMessages;

            }

        }

    }
}