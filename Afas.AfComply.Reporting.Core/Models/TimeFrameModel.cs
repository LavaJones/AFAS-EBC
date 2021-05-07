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
    /// A standard Model representing a timeframe.
    /// </summary>
    public class TimeFrameModel : Model
    {

        public TimeFrameModel() : base() { }

        /// <summary>
        /// The Year that corresponds with this time frame. 
        /// </summary>
        [Required]
        public virtual int Year { get; set; }

        /// <summary>
        /// The Id of the Month that corresponds with this time frame.
        /// </summary>
        [Required]
        public virtual int Month { get; set; }

    }
}
