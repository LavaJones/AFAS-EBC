using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Voids;
using Afc.Core;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Corrections
{
    public class Correction1095 : BaseReportingModel
    {

        /// <summary>
        /// Voided
        /// </summary>
        [Required]
        public virtual Void1095 Voided1095 { get; set; }

        /// <summary>
        /// CorrectedApproval 
        /// </summary>
        [Required]
        public virtual Approved1095Final Approved1095 { get; set; }

        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                SharedUtilities.ValidateObject(this.Approved1095, "Approval1095", validationMessages);
                if (null != this.Approved1095)
                {
                    validationMessages.ToList().AddRange(this.Approved1095.EnsureIsWellFormed);
                }

                SharedUtilities.ValidateObject(this.Voided1095, "Voided1095", validationMessages);
                if (null != this.Voided1095)
                {
                    validationMessages.ToList().AddRange(this.Approved1095.EnsureIsWellFormed);
                }

                return validationMessages;

            }

        }
    }
}
