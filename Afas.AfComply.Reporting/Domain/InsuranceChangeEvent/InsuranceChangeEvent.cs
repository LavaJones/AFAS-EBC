using Afc.Core;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.InsuranceChangeEvent
{

    /// <summary>
    /// A Model representing a reporting timeframe, since the IRS requires answeres for each Month.
    /// </summary>
    public class InsuranceChangeEvent : BaseReportingModel
    {
        /// <summary>
        /// The EmployeeID that corresponds to this change event.
        /// </summary>
        [Required]
        public virtual int EmployeeID { get; set; }

        /// <summary>
        /// The PlanYearID that corresponds to this change event.
        /// </summary>
        [Required]
        public virtual int PlanYearID { get; set; }

        /// <summary>
        /// The EffectiveDate that corresponds to this change event.
        /// </summary>
        [Required]
        public virtual DateTime EffectiveDate { get; set; }


        public override IList<ValidationMessage> EnsureIsWellFormed
        {
            get
            {
                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;
                SharedUtilities.ValidateDate(this.EffectiveDate, "EffectiveDate", validationMessages);
                return validationMessages;
            }
        }
    }
}
