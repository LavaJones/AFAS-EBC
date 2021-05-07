using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Areas.ViewModels.Reporting
{
    public class ConfirmationClassificationViewModel 
    {
 
       
        public int CLASS_ID { get; set; }
        public int CLASS_EMPLOYER_ID { get; set; }
        public string CLASS_DESC { get; set; }
        public string CLASS_AFFORDABILITY_CODE { get; set; }
        public DateTime CLASS_MOD_ON { get; set; }
        public string CLASS_MOD_BY { get; set; }
        public string CLASS_HISTORY { get; set; }
        public int CLASS_WAITING_PERIOD_ID { get; set; }
        public DateTime CLASS_CREATED_ON { get; set; }
        public string CLASS_CREATED_BY { get; set; }
        public int CLASS_ENTITY_STATUS { get; set; }
        public string CLASS_DEFAULT_OOC { get; set; }
        public bool CLASS_2GInValidPrice { get; set; }
    }
}