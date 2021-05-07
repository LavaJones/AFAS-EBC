using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.TimeFrames
{
    /// <summary>
    /// Time Frame specific repository.
    /// </summary>
    public interface ITimeFrameRepository : IDomainRepository<TimeFrame>
    {

        /// <summary>
        /// Retrive only the timeframes for a specific year.
        /// </summary>
        /// <param name="Year">The Year to filter on.</param>
        /// <returns>The Timeframes for this year.</returns>
        IQueryable<TimeFrame> FilterForCalendarYear(int Year);

        /// <summary>
        /// Retrive only the timeframes for a specific Month.
        /// </summary>
        /// <param name="Month">The Month to filter on.</param>
        /// <returns>The Timeframes for this year.</returns>
        IQueryable<TimeFrame> FilterForCalendarMonth(int Month);

        /// <summary>
        /// Retrive only the timeframes for a specific Month/Year combination.
        /// </summary>
        /// <param name="Month">The Month to filter on.</param>
        /// <param name="Year">The Year to filter on.</param>
        /// <returns>The Timeframes for this year.</returns>
        IQueryable<TimeFrame> FilterForMonthYear(int Month, int Year);
    }
}
