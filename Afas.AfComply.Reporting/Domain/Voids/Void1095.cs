using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using Afc.Core.Domain;
using Afc.Core;
using Afas.AfComply.Reporting.Domain.Printing;
using Afas.AfComply.Reporting.Domain.Approvals;

namespace Afas.AfComply.Reporting.Domain.Voids
{

    public class Void1095 : BaseReportingModel
    {

        /// <summary>
        /// The date and time that this was voided on
        /// </summary>
        [Required]
        public virtual DateTime VoidedOn { get; set; }

        /// <summary>
        /// The user that voided it 
        /// </summary>
        [Required]
        public virtual string VoidedBy { get; set; }

        /// <summary>
        /// The Reason that this was voided
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// The approval that was voided
        /// </summary>
        public virtual Approved1095Final Approval { get; set; }

        /// <summary>
        /// The Printing of the form with void checked 
        /// </summary>
        public virtual Print1095 Print { get; set; }

        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                SharedUtilities.ValidateString(this.VoidedBy, "VoidedBy", validationMessages);

                SharedUtilities.ValidateDate(this.VoidedOn, "VoidedOn", validationMessages);

                return validationMessages;

            }

        }

    }

}
