using Afas.AfComply.Reporting.Core.Models;
using Afc.Marketing.Models;
using Afc.Marketing.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Core.Request
{
    /// <summary>
    /// Standard Request for getting all the timeframes with a specified year
    /// </summary>
    public class TimeFramesForYearRequest : BaseRequest
    {
        
        /// <summary>
        /// default constructor
        /// </summary>
        public TimeFramesForYearRequest() : base() { }

        /// <summary>
        /// The Year for this request
        /// </summary>
        public virtual int Year { get; set; }
        
    }
}
