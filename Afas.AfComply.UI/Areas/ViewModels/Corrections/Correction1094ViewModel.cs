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
    public class Correction1094ViewModel : BaseViewModel
    {

        public Correction1094ViewModel() : base() { }

        /// <summary>
        /// Voided
        /// </summary>
        [Required]
        public virtual Void1094ViewModel Voided1094 { get; set; }

        /// <summary>
        /// CorrectedApproval 
        /// </summary>
        [Required]
        public virtual Approved1094FinalViewModel Approved1094 { get; set; }


    }
}
