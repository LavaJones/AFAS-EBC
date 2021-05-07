using Afc.Marketing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.UI.Areas.ViewModels
{
    public class Print1094ViewModel : BaseViewModel
    {
        public Print1094ViewModel() : base() { }

        /// <summary>
        /// The approval of the Print
        /// </summary>
        [Required]
        public virtual Approved1094FinalViewModel Approved1094 { get; set; }

        /// <summary>
        /// The print batch
        /// </summary>
        [Required]
        public virtual PrintBatchViewModel PrintBatch { get; set; }

    }
}
