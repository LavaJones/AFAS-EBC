using Afc.Marketing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.UI.Areas.ViewModels
{
    /// <summary>
    /// A standard Model representing a .
    /// </summary>
    public class Correction1095ViewModel : BaseViewModel
    {

        public Correction1095ViewModel() : base() { }

        /// <summary>
        /// Voided
        /// </summary>
        [Required]
        public virtual Void1095ViewModel Voided1095 { get; set; }

        /// <summary>
        /// CorrectedApproval 
        /// </summary>
        [Required]
        public virtual Approved1095FinalViewModel Approved1095 { get; set; }


    }
}
