using Afc.Marketing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Core.Models
{
    /// <summary>
    /// A standard Model representing a .
    /// </summary>
    public class Correction1095Model : Model
    {

        public Correction1095Model() : base() { }

        /// <summary>
        /// Voided
        /// </summary>
        [Required]
        public virtual Void1095Model Voided1095 { get; set; }

        /// <summary>
        /// CorrectedApproval 
        /// </summary>
        [Required]
        public virtual Approved1095FinalModel Approved1095 { get; set; }


    }
}
