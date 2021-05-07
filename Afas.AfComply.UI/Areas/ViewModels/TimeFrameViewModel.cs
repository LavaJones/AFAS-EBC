using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Areas.ViewModels
{
    public class TimeFrameViewModel : BaseViewModel
    {

        public TimeFrameViewModel() : base() { }

        /// <summary>
        /// The Year that corresponds with this time frame. 
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// The Id of the Month that corresponds with this time frame.
        /// </summary>
        public virtual int Month { get; set; }

    }
}