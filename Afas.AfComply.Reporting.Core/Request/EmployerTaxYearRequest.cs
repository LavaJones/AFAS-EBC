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
    /// This is the Standard Request to get all the inforamtion for a specific Employer and Tax Year Combination
    /// </summary>
    public class EmployerTaxYearRequest : BaseRequest
    {

        /// <summary>
        /// default constructor
        /// </summary>
        public EmployerTaxYearRequest() : base() { }

        /// <summary>
        /// The resource Id of the Employer to get the summary data for
        /// </summary>
        public virtual int EmployerId { get; set; }

        /// <summary>
        /// The Tax Year for this request
        /// </summary>
        public virtual int TaxYear { get; set; }
        
    }
}
