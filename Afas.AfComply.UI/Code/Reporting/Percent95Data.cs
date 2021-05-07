using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Code.Reporting
{
    /// <summary>
    /// This is a POCO that represents the data in the 95% report
    /// </summary>
    public class Percent95Data
    {
        /// <summary>
        /// The month of the year this data is for (1 = Jan, 2 = Feb, .... 12 = Dec)
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// The total FTEs for the company in this month
        /// </summary>
        public int TotalFTEs { get; set; }

        /// <summary>
        /// The Number of FTEs offered Insurance as of this month
        /// </summary>
        public int FTEsOfferedIns { get; set; }

        /// <summary>
        /// The Percentage of the total FTEs that were offered insurance
        /// </summary>
        public double Percent { get { return (double)FTEsOfferedIns / (double)TotalFTEs; } }
    }
}