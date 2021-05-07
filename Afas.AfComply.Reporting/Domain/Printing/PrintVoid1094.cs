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
    class PrintVoid1094 : BaseReportingModel
    {
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
