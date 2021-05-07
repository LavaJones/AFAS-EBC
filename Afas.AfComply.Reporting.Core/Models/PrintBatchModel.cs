using Afc.Marketing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Core.Models
{
    public class PrintBatchModel : Model
    {

        public PrintBatchModel() : base() { }


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
        /// All the 1095s that were printed 
        /// </summary>
        [Required]
        public virtual IList<Print1095Model> AllPrinted1095s { get; set; }

        /// <summary>
        /// All the 1094s that were printed
        /// </summary>
        [Required]
        public virtual IList<Print1094Model> AllPrinted1094s { get; set; }

        /// <summary>
        /// When we processed the PDF files from printed
        /// </summary>
        public virtual DateTime PdfReceivedOn { get; set; }

    }
}
