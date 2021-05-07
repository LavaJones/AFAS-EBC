using Afas.Domain;
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
    /// Because the way that EF deals with things, wthis was the esiest way to resolve with existing table
    /// </summary>
    public class UploadedDataInfo : BaseUploadedDataInfo
    {
        // should just maintain the base items
    }

    /// <summary>
    /// This class holds all the meta-data that we need when a file is uploaded to the system.
    /// </summary>
    public abstract class BaseUploadedDataInfo : BaseImportConverterModel
    {

        /// <summary>
        /// The user Id of the person that uploaded the file to the site. 
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string UploadedByUser { get; set; }

        /// <summary>
        /// The time and date when the file was origonal uloaded
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// Text denoting how upload was done, ex: Bulk Import, Agent, Client, Correction, etc.
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string UploadSourceDescription { get; set; }

        /// <summary>
        /// Text denoting what type of upload was done, ex: Demographics, Payroll, Coverage, etc. 
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string UploadTypeDescription { get; set; }

        /// <summary>
        /// True if the File has already been fully Processed, false if it has not been processed.
        /// </summary>
        public bool Processed { get; set; }

        /// <summary>
        /// True if the File failed while Processing, false if it has not been processed or was successful.
        /// </summary>
        public bool ProcessingFailed { get; set; }

        /// <summary>
        /// This id holds data on the owner of this upload by their external* system Id
        /// </summary>
        public int UploadOwnerId { get; set; }

        /// <summary>
        /// A high level description of the status of the file upload 
        /// </summary>
        public string Status 
        {
            get 
            {
                if (Processed)
                {
                    return "Processed";
                }
                if (ProcessingFailed)
                {
                    return "Failed";
                }

                return "Processing";
            }
        }

        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                SharedUtilities.ValidateString(this.UploadedByUser, "UploadedByUser", validationMessages);

                SharedUtilities.ValidateString(this.UploadSourceDescription, "UploadSourceDescription", validationMessages);

                SharedUtilities.ValidateString(this.UploadTypeDescription, "UploadTypeDescription", validationMessages);
                
                SharedUtilities.ValidateDate(this.UploadTime, "UploadTime", validationMessages);

                return validationMessages;

            }

        }
    }
}