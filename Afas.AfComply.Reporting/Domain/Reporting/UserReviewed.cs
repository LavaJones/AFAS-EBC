using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Printing;
using Afc.Core;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Reporting
{
    public class UserReviewed : BaseReportingModel
    {

        /// <summary>
        /// The Year for which the Employee was Reviewed
        /// </summary>
        [Required]
        public virtual int TaxYear { get; set; }

        /// <summary>
        /// The employee that was Reviewed
        /// </summary>
        [Required]
        public virtual int EmployeeId { get; set; }

        /// <summary>
        /// What Employer this item was reviewed for
        /// </summary>
        [Required]
        public virtual int EmployerId { get; set; }

        /// <summary>
        /// What user Reviewed this 
        /// </summary>
        [Required]
        public virtual string ReviewedBy { get; set; }

        /// <summary>
        /// When the User Reviewed this Item
        /// </summary>
        [Required]
        public virtual DateTime ReviewedOn { get; set; }

        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                SharedUtilities.ValidateString(this.ReviewedBy, "ReviewedBy", validationMessages);

                SharedUtilities.ValidateDate(this.ReviewedOn, "ReviewedOn", validationMessages);

                if (this.TaxYear < 2000 || this.TaxYear > 2025)
                {
                    validationMessages.Add(new ValidationMessage("TaxYear", "TaxYear Id is invalid", ValidationMessageSeverity.Error));
                }

                if (this.EmployeeId <= 0)
                {
                    validationMessages.Add(new ValidationMessage("EmployeeId", "Employee Id is invalid", ValidationMessageSeverity.Error));
                }

                if (this.EmployerId <= 0)
                {
                    validationMessages.Add(new ValidationMessage("EmployerId", "Employer Id is invalid", ValidationMessageSeverity.Error));
                }

                return validationMessages;

            }
        }
    }
}
