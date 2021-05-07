using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afc.Marketing.Models;

namespace Afas.AfComply.UI.Areas.ViewModels
{
    public class Print1095ViewModel : BaseViewModel
    {
        public Print1095ViewModel() : base() { }

        /// <summary>
        /// The approval of the Print
        /// </summary>
        [Required]
        public virtual Approved1095FinalViewModel Approved1095 { get; set; }

        /// <summary>
        /// The print batch
        /// </summary>
        [Required]
        public virtual PrintBatchViewModel PrintBatch { get; set; }

    }
}
