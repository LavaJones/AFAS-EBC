using Afc.Marketing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Core.Models
{
    public class Void1095Model : Model
    {
        public Void1095Model() : base() { }

        [Required]
        public virtual DateTime VoidedOn { get; set; }

        /// <summary>
        /// The user that voided it 
        /// </summary>
        [Required]
        public virtual string VoidedBy { get; set; }

        /// <summary>
        /// The Reason that this was voided
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// The approval that was voided
        /// </summary>
        public virtual Approved1095FinalModel Approval { get; set; }

        /// <summary>
        /// The Printing of the form with void checked 
        /// </summary>
        public virtual Print1095Model Print { get; set; }

    }
}
