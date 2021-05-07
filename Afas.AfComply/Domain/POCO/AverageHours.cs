using Afas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.POCO
{
    /// <summary>
    /// This strucutre keeps the Average Hours for an employee during a measurement period.
    /// </summary>
    public class AverageHours: BaseAfasModel
    {
        /// <summary>
        /// The Data Base Pk
        /// </summary>
        public int EmployeeMeasurementAverageHoursId { get; set; }

        /// <summary>
        /// The FK to the employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// The FK to the Measurement Period
        /// </summary>
        public int MeasurementId { get; set; }

        /// <summary>
        /// The Average Hours for the Employee-Measurement Period Combo, Per week
        /// </summary>
        public double WeeklyAverageHours { get; set; }

        /// <summary>
        /// The Average Hours for the Employee-Measurement Period Combo, per Month
        /// </summary>
        public double MonthlyAverageHours { get; set; }
    
        /// <summary>
        /// The Average Trending Hours for the Employee-Measurement Period Combo, Per week
        /// </summary>
        public double TrendingWeeklyAverageHours { get; set; }

        /// <summary>
        /// The Average Trending Hours for the Employee-Measurement Period Combo, per Month
        /// </summary>
        public double TrendingMonthlyAverageHours { get; set; }

        /// <summary>
        /// The Total number of hours gathered for this period
        /// </summary>
        public double TotalHours { get; set; }

        /// <summary>
        /// True if this record is for a new hire
        /// </summary>
        public bool IsNewHire { get; set; }

    }
}
