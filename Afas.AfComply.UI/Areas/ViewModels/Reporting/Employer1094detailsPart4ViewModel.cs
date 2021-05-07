using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Areas.ViewModels.Reporting
{
    public class Employer1094detailsPart4ViewModel : BaseViewModel
    {

        public Employer1094detailsPart4ViewModel() : base() { }
        public string EIN { set; get; }
        public string EmployerName { get; set; }
        public int EmployerId { get; set; }
    }
}