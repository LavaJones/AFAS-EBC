using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EmployerImp : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(EmployerImp));

        private string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the EmployerImp page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=20", false);
            }

            loadEmployers();
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

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {

            int employerId = 0;

            //check that data is correct
            if (
                    null == DdlEmployer.SelectedItem
                        ||
                    null == DdlEmployer.SelectedItem.Value
                        ||
                    false == int.TryParse(DdlEmployer.SelectedItem.Value, out employerId)
                )
            {

                cofein.Text = "Incorrect parameters";

                return;

            }

            employer currEmployer = employerController.getEmployer(employerId);
            cofein.Text = currEmployer.EMPLOYER_EIN;
            HfDistrictID.Value = currEmployer.EMPLOYER_ID.ToString();

            loadInitialMeasurementPeriods();
        }


        private void loadInitialMeasurementPeriods()
        {
            DdlInitialLength.DataSource = measurementController.getInitialMeasurements();
            DdlInitialLength.DataTextField = "INITIAL_NAME";
            DdlInitialLength.DataValueField = "INITIAL_ID";
            DdlInitialLength.DataBind();

            employer currEmployer = (employer)Session["CurrentDistrict"];
            int initID = currEmployer.EMPLOYER_INITIAL_MEAS_ID;

            if (initID > 0)
            {
                DdlInitialLength.SelectedIndex = currEmployer.EMPLOYER_INITIAL_MEAS_ID - 1;
            }
            else
            {
                // error case

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnInitialMeasurement_Click(object sender, EventArgs e)
        {
            int initial_meas_id = 0;
            int employerID = 0;
            bool validUpdate = true;

            employerID = int.Parse(HfDistrictID.Value);
            initial_meas_id = int.Parse(DdlInitialLength.SelectedItem.Value);

            validUpdate = measurementController.updateInitialMeasurement(employerID, initial_meas_id);

            if (validUpdate == true)
            {
                // notify of success
               


            }
            else
            {
                // notify of failure


            }
        }

    }
}