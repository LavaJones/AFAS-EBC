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
    /// Standard Request for creating a new TimeFrame
    /// </summary>
    public class UpdateManyPart2Request : UpdateManyRequest<Employee1095detailsPart2Model>
    {

        /// <summary>
        /// default constructor
        /// </summary>
        public UpdateManyPart2Request() : base() { }

        /// <summary>
        /// The resource Id of the Employer to get the summary data for
        /// </summary>
        public virtual int EmployerId { get; set; }

        /// <summary>
        /// The Tax Year for this request
        /// </summary>
        public virtual int TaxYear { get; set; }

        /// <summary>
        /// The Employees Resource Id.
        /// </summary>
        public virtual Guid ResourceId { get; set; }

    }
}
