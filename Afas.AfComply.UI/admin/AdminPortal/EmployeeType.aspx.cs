using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;
using Afas.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EmployeeType : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(EmployeeType));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the EmployeeType page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=19", false);
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

            Gv_gv_Types.DataSource = EmployeeTypeController.getEmployeeTypes(employerId);
            Gv_gv_Types.DataBind();
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

        protected void Gv_gv_Types_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = Gv_gv_Types.Rows[e.RowIndex];
            HiddenField hfId = (HiddenField)row.FindControl("HiddenTypeId");
            int typeId = 0;
            if (int.TryParse(hfId.Value, out typeId)) 
            {
                if (EmployeeTypeController.deleteEmployeeType(typeId))
                {
                    lblMsg.Text = "Delete Successful.";
                }
            }

            Gv_gv_Types.EditIndex = -1;

            loadData();
        }

        protected void Gv_gv_Types_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = Gv_gv_Types.Rows[e.RowIndex];

            HiddenField hfId = (HiddenField)row.FindControl("HiddenTypeId");
            TextBox hfTypeName = (TextBox)row.FindControl("TxtTypeName");
            int TypeId = 0;

            if (int.TryParse(hfId.Value, out TypeId)
                && false == hfTypeName.Text.IsNullOrEmpty())
            {
                if (EmployeeTypeController.updateEmployeeType(hfTypeName.Text, TypeId))
                {
                    lblMsg.Text = "Edit Successful.";
                }
            }
            else 
            {
                lblMsg.Text = "Incorrect parameters";
            }
            
            Gv_gv_Types.EditIndex = -1;

            loadData();
        }


        protected void BtnNewType_Click(object sender, EventArgs e) 
        {

            int employerId = 0;

            //check that data is correct
            if (false == int.TryParse(HiddenEmployerId.Value, out employerId))
            {
                lblMsg.Text = "No Employer Selected.";

                return;
            }

            if (false == TxtNewTypeName.Text.IsNullOrEmpty())
            {
                EmployeeTypeController.insertEmployeeType(TxtNewTypeName.Text, employerId);
            }

            Gv_gv_Types.EditIndex = -1;

            loadData();
        }

        protected void Gv_gv_Types_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Gv_gv_Types.EditIndex = e.NewEditIndex;

            loadData();
        }


        protected void Gv_gv_Types_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            Gv_gv_Types.EditIndex = -1;

            loadData();
        }
    }
}