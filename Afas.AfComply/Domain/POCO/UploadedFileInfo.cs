using Afas.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.POCO
{
    /// <summary>
    /// This class holds all the meta-data that we need when a file is uploaded to the system.
    /// </summary>
    public class UploadedFileInfo : BaseAfasModel
    {
        /// <summary>
        /// Database PK for this object
        /// </summary>
        public int UploadedFileInfoId { get; set; }

        /// <summary>
        /// The Id of the Employer that this file was uploaded for
        /// </summary>
        public int EmployerId { get; set; }
        
        /// <summary>
        /// The user Id of the person that uploaded the file to the site. 
        /// </summary>
        public string UploadedByUser { get; set; }

        /// <summary>
        /// The time and date when the file was origonal uloaded
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// Text denoting how upload was done, ex: Bulk Import, Agent, Client, Correction, etc.
        /// </summary>
        public string UploadSourceDescription { get; set; } 

        /// <summary>
        /// Text denoting what type of upload was done, ex: Demographics, Payroll, Coverage, etc. 
        /// </summary>
        public string UploadTypeDescription { get; set; }

        /// <summary>
        /// Text denoting what type of file was uploaded, ex: CSV, TSV, excel, etc.
        /// </summary>
        public string FileTypeDescription { get; set; }

        /// <summary>
        /// The File (+Path) That the File started at. 
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Returns just the File Name without the path.
        /// </summary>
        public string FileNameNoPath { get { return Path.GetFileName(FileName); } }

        /// <summary>
        /// True if the File has already been fully Processed, false if it has not been processed.
        /// </summary>
        public bool Processed { get; set; }

        /// <summary>
        /// True if the File failed while Processing, false if it has not been processed or was successful.
        /// </summary>
        public bool ProcessingFailed { get; set; }

        /// <summary>
        /// The Id of the Archived File that was this upload (or null if it has not yet been archived)
        /// </summary>
        public int? ArchiveFileInfoId { get; set; }

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
    }
}