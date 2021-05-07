using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Areas.ViewModels.Reporting
{
    public class ConfirmationIRSEmployees : BaseViewModel
     {
        public int EMPLOYEE_ID { get; set; }
        public int EMPLOYEE_TYPE_ID { get; set; }
        public int EMPLOYEE_HR_STATUS_ID { get; set; }
        public int EMPLOYEE_EMPLOYER_ID { get; set; }
        public string EMPLOYEE_FIRST_NAME { get; set; }
        public string EMPLOYEE_MIDDLE_NAME { get; set; }
        public string EMPLOYEE_LAST_NAME { get; set; }
        public string EMPLOYEE_FULL_NAME { get; set; }
        public string EMPLOYEE_FULL_NAME_SSN { get; set; }
        public string EMPLOYEE_FULL_NAME_ExtID { get; set; }

        public string EMPLOYEE_ADDRESS { get; set; }
        public string EMPLOYEE_CITY { get; set; }
        public int EMPLOYEE_STATE_ID { get; set; }
        public string EMPLOYEE_ZIP { get; set; }

        public DateTime EMPLOYEE_HIRE_DATE { get; set; }
        public DateTime? EMPLOYEE_C_DATE { get; set; }
        public string Employee_SSN_Visible { get; set; }
        public string Employee_SSN_Hidden { get; set; }
        public DateTime? EMPLOYEE_TERM_DATE { get; set; }
        public DateTime EMPLOYEE_DOB { get; set; }
        public string EMPLOYEE_EXT_ID { get; set; }
        public double EMPLOYEE_PERCENT_MPP { get; set; }
        public double EMPLOYEE_PERCENT_HWP { get; set; }
        public double EMPLOYEE_PERCENT_QT { get; set; }
        public DateTime? EMPLOYEE_IMP_END { get; set; }
        public int EMPLOYEE_PLAN_YEAR_ID { get; set; }
        public int EMPLOYEE_PLAN_YEAR_ID_LIMBO { get; set; }
        public int EMPLOYEE_PLAN_YEAR_ID_MEAS { get; set; }
        public double EMPLOYEE_AVG_HOURS_WORKED { get; set; }
        public int EMPLOYEE_HOURS_WORKED { get; set; }
        public double EMPLOYEE_PY_AVG_STABILITY_HOURS { get; set; }
        public double EMPLOYEE_PY_AVG_ADMIN_HOURS { get; set; }
        public double EMPLOYEE_PY_AVG_MEAS_HOURS { get; set; }
        public double EMPLOYEE_PY_AVG_INIT_HOURS { get; set; }
        public int EMPLOYEE_CLASS_ID { get; set; }
        public int EMPLOYEE_ACT_STATUS_ID { get; set; }
        public string StateAbbreviation { get; set; }
        public bool EMPLOYEE_REC_1095c { get; set; }

        public Guid ResourceId { get; set; }


    }
}