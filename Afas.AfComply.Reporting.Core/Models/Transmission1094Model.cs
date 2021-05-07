using Afc.Marketing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Core.Models
{
    public class Transmission1094Model : Model
    {
        public Transmission1094Model() : base() { }

        /// <summary>
        /// The Time when the Transmission 
        /// </summary>
        [Required]
        public virtual DateTime TransmissionTime { get; set; }

        /// <summary>
        /// The type of transmission 
        /// </summary>
        [Required]
        public virtual string TransmissionType { get; set; }

        /// <summary>
        /// Unique Record Id
        /// </summary>
        [Required]
        public virtual string UniqueRecordId { get; set; }

        /// <summary>
        /// THe IRS status of this Transmission
        /// </summary>
        [Required]
        public virtual string TransmissionStatus { get; set; }

        /// <summary>
        /// All of the 1095s that wer transmited with this 1094
        /// </summary>
        [Required]
        public virtual IList<Transmission1095Model> All1095Tranmissions { get; set; }

        /// <summary>
        /// The recipt Id returned by the IRS upon transmission success.
        /// </summary>
        public virtual string IrsReciptId { get; set; }

        /// <summary>
        /// The approved 1094 that is being transmitted
        /// </summary>
        [Required]
        public virtual Employer1094SummaryModel Approved1094 { get; set; }
    }
}
