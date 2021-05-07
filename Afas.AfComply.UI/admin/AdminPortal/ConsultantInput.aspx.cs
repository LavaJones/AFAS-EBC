using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;
namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class ConsultantInput : AdminPageBase
    {
       
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            if (false == Feature.NewAdminPanelEnabled)
            {

                this.Log.Info("A user tried to access the ConfirmNewHires page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=10", false);
            }
            else
            {
                loadEmployers();
                loadConsultants(employer.EMPLOYER_ID);
            }

        }

        private void loadEmployers()
        {
           // object DdlEmployer = null;
            DdlEmployer.DataSource = employerController.getAllEmployers();
            DdlEmployer.DataTextField = "EMPLOYER_NAME";
            DdlEmployer.DataValueField = "EMPLOYER_ID";
            DdlEmployer.DataBind();

            DdlEmployer.Items.Add("Select");
            DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
             

        }

        private void loadConsultants(int EmployerId)
        {
            gvEmployerConsultant.DataSource = employerController.getEmployerConsultant(EmployerId);
            gvEmployerConsultant.DataBind();
        }

        //Q: Do we need this dropdown event? 
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

                ClblMsg.Text = "Incorrect parameters";

                return;

            }

            employer employ = employerController.getEmployer(employerId);
        }

        protected void BtnRun_Click(object sender, EventArgs e)
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

                ClblMsg.Text = "Please select an employer and confirm the action.";

                return;

            }

            try
            {

                ClblMsg.Text = "";

                DateTime modifedOn = DateTime.Now;
                // not sure why we can not reach the AdminPortal.Master assets. gc5
                String modifedBy = ((User)Session["CurrentUser"]).User_UserName;

                employer employ = employerController.getEmployer(employerId);

                // UI logic
                string name = ConsultantName.Text;
                int phonenumber = int.Parse(ConsultantPhoneNum.Text);
                string title = Title.Text;
                int empId = int.Parse(DdlEmployer.SelectedItem.Value);

                bool rtn = employerController.InsertUpdateEmployerConsultant(name, title, phonenumber, empId, modifedBy);

                if (rtn == true)
                {
                    ClblMsg.Text = "Save Complete!";
                }
                else
                {
                    ClblMsg.Text = "Unable to save, please try again!";
                }

               

            }
            catch (Exception exception)
            {
                
                this.Log.Fatal("Errors during confirming new hires!", exception);
                
                ClblMsg.Text = exception.Message;

            }

        }

        private ILog Log = LogManager.GetLogger(typeof(ConfirmNewHires));
    
    }

}