using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.TimeFrames
{
    /// <summary>
    /// Common Data Filtering extension methods for the TimeFrame object
    /// </summary>
    public static class TimeFrameQuerableExtensionMethods
    {

        /// <summary>
        /// Filter the Time Frames for ony those in a specific year.
        /// </summary>
        /// <param name="timeFrames">The object to filter.</param>
        /// <param name="Year">The Year to filter for.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<TimeFrame> FilterForCalendarYear(this IQueryable<TimeFrame> timeFrames, int Year)
        {

            return (
                    from timeFrame in timeFrames
                    where timeFrame.Year == Year
                    select timeFrame
                    );

        }
                
        /// <summary>
        /// Filter the Time Frames for ony those in a specific Month.
        /// </summary>
        /// <param name="timeFrames">The object to filter.</param>
        /// <param name="Year">The Month to filter for.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<TimeFrame> FilterForCalendarMonth(this IQueryable<TimeFrame> timeFrames, int Month)
        {

            return (
                    from timeFrame in timeFrames
                    where timeFrame.Month == Month
                    select timeFrame
                    );

        }        
    }
}
