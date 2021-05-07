using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Corrections;
using Afas.AfComply.Reporting.Domain.Voids;
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
    public class Print1094 : BaseReportingModel
    {

        /// <summary>
        /// The approval of the Print
        /// </summary>
        [Required]
        public virtual Approved1094FinalPart1 Approved1094 { get; set; }

        /// <summary>
        /// The print batch
        /// </summary>
        [Required]
        public virtual PrintBatch PrintBatch { get; set; }

        /// <summary>
        /// The actual file that printed
        /// </summary>
        [Required]
        public virtual string OutputFilePath { get; set; }

        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

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
