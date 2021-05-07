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
    public class Transmission1095 : BaseReportingModel
    {

        /// <summary>
        /// The Time when the Transmission O
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
        /// The 1094 that this was transmitted with
        /// </summary>
        [Required]
        public virtual Transmission1094 Transmission1094 { get; set; }

        /// <summary>
        /// The Approval that this transmission is for
        /// </summary>
        [Required]
        public virtual Approved1095Final Approval { get; set; }

        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                SharedUtilities.ValidateString(this.UniqueRecordId, "UniqueRecordId", validationMessages);

                SharedUtilities.ValidateDate(this.TransmissionTime, "TransmissionTime", validationMessages);

                return validationMessages;

            }
        }


    }
}
