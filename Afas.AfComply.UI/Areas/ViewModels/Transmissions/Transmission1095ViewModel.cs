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
    public class Transmission1095ViewModel : BaseViewModel
    {

        public Transmission1095ViewModel() : base() { }

        /// <summary>
        /// The Time when the Transmission O
        /// </summary>
        [Required]
        public virtual DateTime TransmissionTime { get; set; }

        /// <summary>
        /// The type of transmission 
        /// </summary>
        [Required]
        public virtual string TransmissionType { get; set; }

        /// <summary>
        /// Unique Record Id
        /// </summary>
        [Required]
        public virtual string UniqueRecordId { get; set; }

        /// <summary>
        /// THe IRS status of this Transmission
        /// </summary>
        [Required]
        public virtual string TransmissionStatus { get; set; }

        /// <summary>
        /// The 1094 that this was transmitted with
        /// </summary>
        [Required]
        public virtual Transmission1094ViewModel Transmission1094 { get; set; }

        /// <summary>
        /// The Approval that this transmission is for
        /// </summary>
        [Required]
        public virtual Approved1095FinalViewModel Approval { get; set; }

    }
}
