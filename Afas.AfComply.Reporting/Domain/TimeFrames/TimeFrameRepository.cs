using Afas.Domain;
using Afc.Framework.Domain;
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
    public class TimeFrameRepository : BaseDomainRepository<TimeFrame, IReportingDataContext>, ITimeFrameRepository
    {

        /// <summary>
        /// Retrive only the timeframes for a specific year.
        /// </summary>
        /// <param name="Year">The Year to filter on.</param>
        /// <returns>The Timeframes for this year.</returns>
        IQueryable<TimeFrame> ITimeFrameRepository.FilterForCalendarYear(int Year) 
        {

            return Context.Set<TimeFrame>()
                    .FilterForCalendarYear(Year)
                    .FilterForActive();

        }

        /// <summary>
        /// Retrive only the timeframes for a specific Month.
        /// </summary>
        /// <param name="Month">The Month to filter on.</param>
        /// <returns>The Timeframes for this year.</returns>
        IQueryable<TimeFrame> ITimeFrameRepository.FilterForCalendarMonth(int Month)        
        {

            return Context.Set<TimeFrame>()
                    .FilterForCalendarMonth(Month)
                    .FilterForActive();

        }

        /// <summary>
        /// Retrive only the timeframes for a specific Month/Year combination.
        /// </summary>
        /// <param name="Month">The Month to filter on.</param>
        /// <param name="Year">The Year to filter on.</param>
        /// <returns>The Timeframes for this year.</returns>
        IQueryable<TimeFrame> ITimeFrameRepository.FilterForMonthYear(int Month, int Year)       
        {

            return Context.Set<TimeFrame>()
                    .FilterForCalendarYear(Year)
                    .FilterForCalendarMonth(Month)
                    .FilterForActive();

        }
    }
}
