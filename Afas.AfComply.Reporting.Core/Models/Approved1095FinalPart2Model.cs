using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Afc.Marketing.Models;

namespace Afas.AfComply.Reporting.Core.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class Approved1095FinalPart2Model : Model
    {

        /// <summary>
        /// The employee that this part 2 is associated with (mostly for reference)
        /// </summary>
        [Required]
        public virtual int employeeID { set; get; }

        /// <summary>
        /// The Tax year that this data is for.
        /// </summary>
        [Required]
        public virtual int TaxYear { get; set; }

        /// <summary>
        /// The month that this is for
        /// </summary>
        [Required]
        public virtual int MonthId { set; get; }

        /// <summary>
        /// The line 14 value
        /// </summary>
        public virtual string Line14 { set; get; }

        /// <summary>
        /// The line 15 value
        /// </summary>
        public virtual string Line15 { set; get; }

        /// <summary>
        /// The line 16 value
        /// </summary>
        public virtual string Line16 { set; get; }

        /// <summary>
        /// If the employee was offered acceptable coverage 
        /// </summary>
        public virtual bool Offered { get; set; }

        /// <summary>
        /// If this Month is causing this employee to recieve a 1095C form.
        /// </summary>
        public virtual bool Receiving1095C { get; set; }
    }
}
