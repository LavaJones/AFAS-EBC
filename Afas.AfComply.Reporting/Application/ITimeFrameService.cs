using Afas.AfComply.Reporting.Domain.TimeFrames;
using Afas.Application;
using Afc.Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Application
{

    /// <summary>
    /// A service explosing access to the TimeFrame domain models.
    /// </summary>
    public interface ITimeFrameService : ICrudDomainService<TimeFrame>
    {

        /// <summary>
        /// Gets all the Time Frames for a Calendar Year.
        /// </summary>
        /// <param name="year">The Year to get the time frames for.</param>
        /// <returns>The List of TimeFrame Object or an Empty list.</returns>
        [RequiresSharedTransaction]
        IQueryable<TimeFrame> GetTimeFramesByYear(int year);

    }
}
