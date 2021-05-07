using Afas.AfComply.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.LegacyData
{
    public partial class View_air_replacement_EmployeeYearlyDetails
    {
        /// <summary>
        /// Set in code to keep track of the months
        /// </summary>
        public virtual int month_id { get; set; }

        /// <summary>
        /// Calculated value to determine if the item is Receiving a 1095 for this period.
        /// </summary>
        public virtual bool Receiving1095C
        {
            get
            {
                if (
                        (
                            this.terminationDate == null 
                                ||
                            (month_id > 0 
                                && 
                            this.terminationDate.Value >= new DateTime(tax_year, month_id, 1))
                        )
                    &&
                        (
                            MonthlyAverageHours >= 130 
                                || 
                            (
                                this.aca_status_id == (int) ACAStatusEnum.FullTime 
                                    && 
                                true == this.IsNewHire
                            )
                        )
                   )
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Clone this object using the C# function MemberwiseClone
        /// </summary>
        /// <returns>The clone of this object.</returns>
        public virtual View_air_replacement_EmployeeYearlyDetails Clone()
        {
            return (View_air_replacement_EmployeeYearlyDetails) this.MemberwiseClone();
        }

        /// <summary>
        /// Calculate the value for Initial Admin End/Initial Stability Start
        /// </summary>
        public virtual DateTime InitialAdminEnd
        {
            get
            {
                // If teh IMP is less than 12 months, then we use a 2 month Admin Period, but we only use 1 month if it's a 12 months IMP
                if (InitialStabilityPeriodMonths < 12)
                {
                    return initialMeasurmentEnd.AddMonths(2);
                }
                else
                {
                    return initialMeasurmentEnd.AddMonths(1);
                }
            }
        }
    }
}
