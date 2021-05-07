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
    public class Correction1094 : BaseReportingModel
    {

        /// <summary>
        /// The voided status of the Correction 
        /// </summary>
        [Required]
        public virtual Void1094 Voided1094 { get; set; }

        /// <summary>
        /// The corrected approval
        /// </summary>
        [Required]
        public virtual Approved1094FinalPart1 Approved1094 { get; set; }

        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;
                
                SharedUtilities.ValidateObject(this.Approved1094,"Approved1094", validationMessages);
                if (null != this.Approved1094)
                {
                    validationMessages.ToList().AddRange(this.Approved1094.EnsureIsWellFormed);
                }

                SharedUtilities.ValidateObject(this.Voided1094, "Voided", validationMessages);
                if (null != this.Voided1094)
                {
                    validationMessages.ToList().AddRange(this.Voided1094.EnsureIsWellFormed);
                }

                return validationMessages;

            }

        }
    }
}
