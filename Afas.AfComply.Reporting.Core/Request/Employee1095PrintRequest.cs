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
    public class Employee1095PrintRequests : BaseRequest
    {
                
        /// <summary>
        /// default constructor
        /// </summary>
        public Employee1095PrintRequests() : base() { }

        /// <summary>
        /// The resource Id of the Employer to get the summary data for
        /// </summary>
        public virtual int EmployerId { get; set; }

        /// <summary>
        /// The Tax Year for this request
        /// </summary>
        public virtual int TaxYear { get; set; }

        public virtual bool Correction { get; set; }
        
    }
}
