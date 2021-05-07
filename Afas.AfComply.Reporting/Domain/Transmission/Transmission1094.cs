using Afas.AfComply.Reporting.Domain.Approvals;
using Afc.Core;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Transmission
{
    public class Transmission1094 : BaseReportingModel
    {

        /// <summary>
        /// The Time when the Transmission 
        /// </summary>
        [Required]
        public virtual DateTime TransmissionTime { get; set; }

        /// <summary>
        /// The type of transmission 
        /// </summary>
        [Required]
        public virtual TransmissionTypes TransmissionType { get; set; }

        /// <summary>
        /// Unique Record Id
        /// </summary>
        [Required]
        public virtual string UniqueRecordId { get; set; }

        /// <summary>
        /// THe IRS status of this Transmission
        /// </summary>
        [Required]
        public virtual TransmissionStatus TransmissionStatus { get; set; }
        
        /// <summary>
        /// All of the 1095s that wer transmited with this 1094
        /// </summary>
        [Required]
        public virtual IList<Transmission1095> All1095Tranmissions { get; set; }

        /// <summary>
        /// The recipt Id returned by the IRS upon transmission success.
        /// </summary>
        public virtual string IrsReciptId { get; set; }

        /// <summary>
        /// The approved 1094 that is being transmitted
        /// </summary>
        [Required]
        public virtual Approved1094FinalPart1 Approved1094 { get; set; }


        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                SharedUtilities.ValidateString(this.UniqueRecordId, "UniqueRecordId", validationMessages);

                SharedUtilities.ValidateDate(this.TransmissionTime, "TransmissionTime", validationMessages);

                SharedUtilities.ValidateObject(this.Approved1094, "Approved1094", validationMessages);
                if (null != this.Approved1094)
                {
                    validationMessages.ToList().AddRange(this.Approved1094.EnsureIsWellFormed);
                }

                return validationMessages;

            }

        }
    }
}
