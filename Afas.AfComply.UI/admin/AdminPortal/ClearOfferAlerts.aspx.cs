using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class ClearOfferAlerts : AdminPageBase
    {

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            if (false == Feature.NewAdminPanelEnabled)
            {

                this.Log.Info("A user tried to access the ClearOfferAlerts page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=8", false);
            }
            else
            {
                loadEmployers();
            }

        }

        private void loadEmployers()
        {

            DdlEmployer.DataSource = employerController.getAllEmployers();
            DdlEmployer.DataTextField = "EMPLOYER_NAME";
            DdlEmployer.DataValueField = "EMPLOYER_ID";
            DdlEmployer.DataBind();

            DdlEmployer.Items.Add("Select");
            DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;

        }

        /// <summary>
        /// 1-3) Loads a specific employer's Plan Years into a dropdown list. 
        /// </summary>
        /// <param name="_employerID"></param>
        private void loadPlanYears(int _employerID)
        {
            List<PlanYear> planYear = PlanYear_Controller.getEmployerPlanYear(_employerID);

            DdlPlanYearNew.DataSource = planYear;
            DdlPlanYearNew.DataTextField = "PLAN_YEAR_DESCRIPTION";
            DdlPlanYearNew.DataValueField = "PLAN_YEAR_ID";
            DdlPlanYearNew.DataBind();

            DdlPlanYearNew.Items.Add("Select");
            DdlPlanYearNew.SelectedIndex = DdlPlanYearNew.Items.Count - 1;

        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {

            int employerId = 0;

          
            if (
                    null == DdlEmployer.SelectedItem
                        ||
                    null == DdlEmployer.SelectedItem.Value
                        ||
                    false == int.TryParse(DdlEmployer.SelectedItem.Value, out employerId)
                )
            {

                lblMsg.Text = "Incorrect parameters";

                return;

            }

            employer employ = employerController.getEmployer(employerId);

            cofein.Text = employ.EMPLOYER_EIN;

            loadPlanYears(employ.EMPLOYER_ID);
        }

        protected void BtnRun_Click(object sender, EventArgs e)
        {

            int employerId = 0;
            int planyearId = 0; 

            //check that data is correct
            if (
                    null == DdlEmployer.SelectedItem
                        ||
                    null == DdlEmployer.SelectedItem.Value
                        ||
                    false == int.TryParse(DdlEmployer.SelectedItem.Value, out employerId)
                )
            {

                lblMsg.Text = "Please select an employer and confirm the action.";

                return;

            }

   
            if (
                    null == DdlPlanYearNew.SelectedItem
                        ||
                    null == DdlPlanYearNew.SelectedItem.Value
                        ||
                    false == int.TryParse(DdlPlanYearNew.SelectedItem.Value, out planyearId)
                )
            {

                lblMsg.Text = "Please select a planyear and confirm the action.";

                return;

            }

            try
            {

                lblMsg.Text = "";

                DateTime modifedOn = DateTime.Now;
   
                String modifedBy = ((User)Session["CurrentUser"]).User_UserName;

                employer employ = employerController.getEmployer(employerId);

                List<alert_insurance> alerts = alert_controller.manufactureEmployerInsuranceAlertList(employ.EMPLOYER_ID);

                List<alert_insurance> pyAlerts = (from alert_insurance alert in alerts where alert.IALERT_PLANYEARID == planyearId select alert).ToList();

                List<alert_insurance> saferAlertClears = new List<alert_insurance>();

                foreach (alert_insurance ai in pyAlerts)
                {
                    if (ai.IALERT_OFFERED == null)
                    {
                        saferAlertClears.Add(ai);
                    }
                }

                foreach (alert_insurance alert in saferAlertClears)
                {
                    bool validTransaction = insuranceController.updateInsuranceOffer(alert.ROW_ID, null, null, 0.0, false, null, null, null, modifedOn, modifedBy, "Auto-Cleared Alert", "Auto-Cleared Alert", null, 0.0);
                    if (false == validTransaction) 
                    {
                        Log.Warn("Failed to clear alert with row Id:[" + alert.ROW_ID + "]");
                    }
                }

                lblMsg.Text = "Complete!";

            }
            catch (Exception exception)
            {
                
                this.Log.Fatal("Errors during clear offer alerts!", exception);
                
                lblMsg.Text = exception.Message;

            }

        }

        private ILog Log = LogManager.GetLogger(typeof(ClearOfferAlerts));
    
    }

}