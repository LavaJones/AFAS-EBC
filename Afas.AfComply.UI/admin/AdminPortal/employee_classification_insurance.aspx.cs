using Afas.AfComply.UI.admin;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class securepages_employee_classification_insurance : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(securepages_employee_classification_insurance));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the EditEmployer page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=16", false);
            }
            else
            {
                loadEmployers();
            }
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Logout.aspx", false);
        }

        private int TaxYearId
        {
            get
            {
                int taxYearId = 0;
                int.TryParse(DdlPlanYearCurrent.SelectedItem.Value, out taxYearId);
                return taxYearId;
            }

        }

        private int EmployerId
        {
            get
            {
                int employerId = 0;
                int.TryParse(DdlFilterEmployers.SelectedItem.Value, out employerId);
                return employerId;
            }
        }

        protected void DdlFilterEmployers_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadPlanYears();
        }

        protected void DdlPlanYearCurrent_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadEmployeeClassificationInsurances();
        }

        private void loadEmployers()
        {
            DdlFilterEmployers.DataSource = employerController.getAllEmployers();
            DdlFilterEmployers.DataTextField = "EMPLOYER_NAME";
            DdlFilterEmployers.DataValueField = "EMPLOYER_ID";
            DdlFilterEmployers.DataBind();

            DdlFilterEmployers.Items.Add("Select");
            DdlFilterEmployers.SelectedIndex = DdlFilterEmployers.Items.Count - 1;
        }

        private void loadPlanYears()
        {
            DdlPlanYearCurrent.DataSource = PlanYear_Controller.getEmployerPlanYear(EmployerId);
            DdlPlanYearCurrent.DataTextField = "PLAN_YEAR_DESCRIPTION";
            DdlPlanYearCurrent.DataValueField = "PLAN_YEAR_ID";
            DdlPlanYearCurrent.DataBind();

            DdlPlanYearCurrent.Items.Add("Select");
            DdlPlanYearCurrent.SelectedIndex = DdlPlanYearCurrent.Items.Count - 1;
        }

        private void loadEmployeeClassificationInsurances()
        {
            gvEmployeeClassificationInsurance.DataSource = classificationController.getEmployeeClassificationByPlanYearAndEmployer(EmployerId, TaxYearId);
            gvEmployeeClassificationInsurance.DataBind();
        }
    }
}