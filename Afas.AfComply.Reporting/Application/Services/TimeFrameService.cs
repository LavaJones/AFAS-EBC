using Afas.AfComply.Reporting.Domain.TimeFrames;
using Afas.Application;
using Afc.Core;
using Afc.Core.Domain;
using Afc.Framework.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Application.Services
{
    /// <summary>
    /// A service explosing access to the TimeFrame domain models.
    /// </summary>
    public class TimeFrameService : ABaseCrudService<TimeFrame>, ITimeFrameService
    {
        protected ITimeFrameRepository TimeFrameRepository { get; private set; }
        
        /// <summary>
        /// Standard COnstructor taking the dependencies as parameters. 
        /// </summary>
        /// <param name="timeFrameRepository">The Repository to get the Time frames.</param>
        public TimeFrameService(
            ITimeFrameRepository timeFrameRepository) : 
                base(timeFrameRepository)
        {
            
            this.TimeFrameRepository = timeFrameRepository;

        }        

        /// <summary>
        /// Gets all the Time Frames for a Calendar Year.
        /// </summary>
        /// <param name="year">The Year to get the time frames for.</param>
        /// <returns>The List of TimeFrame Objects or an Empty list.</returns>
        IQueryable<TimeFrame> ITimeFrameService.GetTimeFramesByYear(int year)
        {

            return this.TimeFrameRepository.FilterForCalendarYear(year);

        }
        
    }

}
