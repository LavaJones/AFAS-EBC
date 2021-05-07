using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.Domain.POCO;
using Afc.Core;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Printing
{
    public class PrintBatch : BaseReportingModel
    {

        public PrintBatch() : base()
        {
            this.AllPrinted1095s = new List<Print1095>();
            this.AllPrinted1094s = new List<Print1094>();
        }

        /// <summary>
        /// Boolean flag if this Print bacth is of reprinted Files
        /// </summary>
        [Required]
        public virtual bool Reprint { get; set; }

        /// <summary>
        /// The full path the Archive of the File that was sent to print
        /// </summary>
        [Required]
        public virtual string PrintFileArchivePath { get; set; }

        /// <summary>
        /// The Archived File for easy retreival
        /// </summary>
        public virtual ArchiveFileInfo ArchivedFile { get; set; }

        /// <summary>
        /// The full path the Archive of the File that was sent to print
        /// </summary>
        [Required]
        public virtual string PrintFileName { get; set; }

        /// <summary>
        /// The Username of the person who clicked for it to be Printed
        /// </summary>
        [Required]
        public virtual string RequestedBy { get; set; }

        /// <summary>
        /// When it was Queued for print
        /// </summary>
        [Required]
        public virtual DateTime RequestedOn { get; set; }

        /// <summary>
        /// When we wrote the file out to the Moveit location to be printed
        /// </summary>
        [Required]
        public virtual DateTime SentOn { get; set; }

        /// <summary>
        /// All the 1095s that were printed 
        /// </summary>
        [Required]
        public virtual IList<Print1095> AllPrinted1095s { get; set; }

        /// <summary>
        /// All the 1094s that were printed
        /// </summary>
        [Required]
        public virtual IList<Print1094> AllPrinted1094s { get; set; }

        /// <summary>
        /// The Id of the employer that this print batch belongs to
        /// </summary>
        [Required]
        public virtual int EmployerId { set; get; }

        /// <summary>
        /// The TaxYear that this is printed for.
        /// </summary>
        [Required]
        public virtual int TaxYear { set; get; }

        /// <summary>
        /// True if this print was done on behalf of AFAS, not the client.
        /// </summary>
        [Required]
        public virtual bool AfasRequested { get; set; }
        
        /// <summary>
        /// When we processed the PDF files from printed
        /// </summary>
        public virtual DateTime? PdfReceivedOn { get; set; }


        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                List<ValidationMessage> validationMessages = base.EnsureIsWellFormed.ToList();
                 
                SharedUtilities.ValidateString(this.PrintFileArchivePath, "PrintFileArchivePath", validationMessages);

                SharedUtilities.ValidateString(this.RequestedBy, "RequestedBy", validationMessages);

                SharedUtilities.ValidateDate(this.RequestedOn, "RequestedOn", validationMessages);

                SharedUtilities.ValidateDate(this.SentOn, "SentOn", validationMessages);

                SharedUtilities.ValidateObject(this.AllPrinted1095s, "AllPrint1095s", validationMessages);

                SharedUtilities.ValidateObject(this.AllPrinted1094s, "AllPrint1094s", validationMessages);

                if (null != this.AllPrinted1095s)
                {
                    foreach (Print1095 approval in this.AllPrinted1095s)
                    {
                        validationMessages.AddRange(approval.EnsureIsWellFormed);
                    }
                }

                if (null != this.AllPrinted1094s)
                {
                    foreach (Print1094 approval in this.AllPrinted1094s)
                    {
                        validationMessages.AddRange(approval.EnsureIsWellFormed);
                    }
                }

                return validationMessages;

            }
        }
    }
}
