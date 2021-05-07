using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class PlanYearGroupPage : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(PlanYearGroupPage));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the PlanYearGroup page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=38", false);
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

        private void loadData() 
        {
            int employerId = 0;

            //check that data is correct
            if (false == int.TryParse(HiddenEmployerId.Value, out employerId))
            {
                lblMsg.Text = "No Employer Selected.";

                return;
            }

            Gv_gv_Groups.DataSource = PlanYearGroupFactory.GetAllPlanYearGroupForEmployerId(employerId);
            Gv_gv_Groups.DataBind();
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

                lblMsg.Text = "Please select an employer.";

                return;

            }

            employer employ = employerController.getEmployer(employerId);

            cofein.Text = employ.EMPLOYER_EIN;

            HiddenEmployerId.Value = employerId.ToString();

            loadData();
        }

        protected void Gv_gv_Groups_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = Gv_gv_Groups.Rows[e.RowIndex];
            HiddenField hfId = (HiddenField)row.FindControl("HiddenId");
            User currUser = (User)Session["CurrentUser"];
            int GroupId = 0;
            if (int.TryParse(hfId.Value, out GroupId)) 
            {
                if (PlanYearGroupFactory.DeletePlanYearGroup(GroupId, currUser.User_UserName))
                {
                    lblMsg.Text = "Delete Successful.";
                }
            }

            Gv_gv_Groups.EditIndex = -1;

            loadData();
        }

        protected void Gv_gv_Groups_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = Gv_gv_Groups.Rows[e.RowIndex];

            HiddenField hfId = (HiddenField)row.FindControl("HiddenId");
            TextBox TxtGroupName = (TextBox)row.FindControl("TxtGroupName");
            User currUser = (User)Session["CurrentUser"];

            int GroupId = 0;

            if (int.TryParse(hfId.Value, out GroupId)
                && false == TxtGroupName.Text.IsNullOrEmpty())
            {
                if (PlanYearGroupFactory.UpdatePlanYearGroup(GroupId, currUser.User_UserName, TxtGroupName.Text))
                {
                    lblMsg.Text = "Edit Successful.";
                }
            }
            else 
            {
                lblMsg.Text = "Incorrect parameters";
            }
            
            Gv_gv_Groups.EditIndex = -1;

            loadData();
        }


        protected void BtnNewType_Click(object sender, EventArgs e) 
        {
            User currUser = (User)Session["CurrentUser"];

            int employerId = 0;

            //check that data is correct
            if (false == int.TryParse(HiddenEmployerId.Value, out employerId))
            {
                lblMsg.Text = "No Employer Selected.";

                return;
            }

            if (false == TxtNewGroupName.Text.IsNullOrEmpty())
            {
                PlanYearGroupFactory.InsertNewPlanYearGroup(employerId, TxtNewGroupName.Text, currUser.User_UserName);
            }

            Gv_gv_Groups.EditIndex = -1;

            loadData();
        }

        protected void Gv_gv_Groups_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Gv_gv_Groups.EditIndex = e.NewEditIndex;

            loadData();
        }


        protected void Gv_gv_Groups_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            Gv_gv_Groups.EditIndex = -1;

            loadData();
        }
    }
}