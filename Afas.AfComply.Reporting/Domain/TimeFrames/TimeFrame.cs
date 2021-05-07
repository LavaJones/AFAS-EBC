using Afc.Core;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.TimeFrames
{

    /// <summary>
    /// A Model representing a reporting timeframe, since the IRS requires answeres for each Month.
    /// </summary>
    public class TimeFrame : BaseReportingModel
    {

        /// <summary>
        /// The Year that corresponds with this time frame. Values 2000 - 2050
        /// </summary>
        [Required]
        public virtual int Year { get; set; }

        /// <summary>
        /// The Id of the Month that corresponds with this time frame. Values: 1 - 12 
        /// </summary>
        [Required]
        public virtual int Month { get; set; }

        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                if (Month < 1 || Month > 12)
                {
                    validationMessages.Add(new ValidationMessage("Month", "Month is not within the accpetable range.", ValidationMessageSeverity.Error));
                } 

                if (Year < 2000 || Year > 2050) 
                {
                    validationMessages.Add(new ValidationMessage("Year", "Year is not within the accpetable range.", ValidationMessageSeverity.Error));
                } 

                //SharedUtilities.ValidateDate(this.Date, "Date", validationMessages);

                return validationMessages;

            }
        }
    }
}
