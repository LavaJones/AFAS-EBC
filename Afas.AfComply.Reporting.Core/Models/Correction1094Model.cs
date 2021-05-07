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
    public class Correction1094Model : Model
    {

        public Correction1094Model() : base() { }

        /// <summary>
        /// Voided
        /// </summary>
        [Required]
        public virtual Void1094Model Voided1094 { get; set; }

        /// <summary>
        /// CorrectedApproval 
        /// </summary>
        [Required]
        public virtual Approved1094FinalPart1Model Approved1094 { get; set; }


    }
}
