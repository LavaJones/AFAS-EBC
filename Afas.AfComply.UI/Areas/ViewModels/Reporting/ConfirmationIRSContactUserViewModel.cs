using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Areas.ViewModels.Reporting
{
    public class ConfirmationIRSContactUserViewModel : BaseViewModel
    {
        public virtual string fname { get; set; }
        public virtual string lname { get; set; }
        public virtual string email { get; set; }
        public virtual int phone { get; set; }
        public virtual string username { get; set; }
        public virtual string password { get; set; }
        public virtual int employer_id { get; set; }
        public virtual bool active { get; set; }
        public virtual bool poweruser { get; set; }
        public virtual bool reset_pwd { get; set; }
        public virtual bool billing { get; set; }
        public virtual bool irsContact { get; set; }
        public virtual bool floater { get; set; }

    }
}