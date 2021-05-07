using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;
using Afas.AfComply.Domain.POCO;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using AjaxControlToolkit;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EmployerPlanYear : AdminPageBase
    {




        private ILog Log = LogManager.GetLogger(typeof(EmployerPlanYear));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the Administration Employer Plan Year page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=22", false);
            }
            else 
            {
                HfUserName.Value = user.User_UserName;
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

            loadGvPlanYears();
        }


        protected void BtnPlanYearUpdate_Click(object sender, EventArgs e)
        {
            int _employerID = int.Parse(HfDistrictID.Value);
            string _name = null;
            DateTime _startDate = System.DateTime.Now.AddYears(-50);
            DateTime _endDate = System.DateTime.Now.AddYears(-50);
            string _notes = null;
            string _history = null;
            DateTime _modOn = System.DateTime.Now;
            string _modBy = HfUserName.Value;
            bool ValidData = true;

            ValidData = errorChecking.validateTextBoxNull(Txt_npy_Name, ValidData);
            ValidData = errorChecking.validateTextBoxDate(Txt_npy_StartDate, ValidData);

            if (ValidData == true)
            {
                _name = Txt_npy_Name.Text;
                _startDate = DateTime.Parse(Txt_npy_StartDate.Text);
                _endDate = _startDate.AddYears(1);
                _endDate = _endDate.AddDays(-1);
                _notes = Txt_npy_Notes.Text;

                _history = "Created on: " + _modOn.ToString() + " by: " + _modBy;

                int newPlanID = PlanYear_Controller.manufactureNewPlanYear(_employerID, _name, _startDate, _endDate, _notes, _history, _modOn, _modBy,
                    //I don't like doing this much work inline either but I'm in a hurry
                    Txt_M_Meas_start.Text.TryParseNullableDateTime(),
                    Txt_M_Meas_end.Text.TryParseNullableDateTime(),
                    Txt_M_Admin_start.Text.TryParseNullableDateTime(),
                    Txt_M_Admin_end.Text.TryParseNullableDateTime(),
                    Txt_M_Open_start.Text.TryParseNullableDateTime(),
                    Txt_M_Open_end.Text.TryParseNullableDateTime(),
                    Txt_M_Stab_start.Text.TryParseNullableDateTime(),
                    Txt_M_Stab_end.Text.TryParseNullableDateTime(),
                    int.Parse(DdlPlanYearGroupNew.SelectedValue)
                    );
                if (newPlanID > 0)
                {
                    loadGvPlanYears();

                    // Queue up the calculation
                    employerController.insertEmployerCalculation(_employerID);
                }
                else
                {
                    MpeNewPlanYear.Show();
                    Lbl_npy_Message.Text = "An error occurred while adding the new plan year, please try again.";
                }
            }
            else
            {
                MpeNewPlanYear.Show();
                Lbl_npy_Message.Text = "Please correct all red fields.";
            }

        }
        protected void GvPlanYears_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = null;
            int _employerID = int.Parse(HfDistrictID.Value);
            int rowID = GvPlanYears.EditIndex;
            int _planYearID = 0;
            bool validData = true;
            DateTime _modOn = System.DateTime.Now;
            string _modBy = HfUserName.Value;

            row = GvPlanYears.Rows[rowID];

            TextBox txtName = (TextBox)row.FindControl("Txt_epy_Name");
            TextBox txtSdate = (TextBox)row.FindControl("Txt_epy_StartDate");
            TextBox txtEdate = (TextBox)row.FindControl("Txt_epy_EndDate");
            TextBox txtNotes = (TextBox)row.FindControl("Txt_epy_Notes");

            TextBox Txt_M_Meas_start = (TextBox)row.FindControl("Txt_M_Meas_start");
            TextBox Txt_M_Meas_end = (TextBox)row.FindControl("Txt_M_Meas_end");
            TextBox Txt_M_Admin_start = (TextBox)row.FindControl("Txt_M_Admin_start");
            TextBox Txt_M_Admin_end = (TextBox)row.FindControl("Txt_M_Admin_end");
            TextBox Txt_M_Open_start = (TextBox)row.FindControl("Txt_M_Open_start");
            TextBox Txt_M_Open_end = (TextBox)row.FindControl("Txt_M_Open_end");
            TextBox Txt_M_Stab_start = (TextBox)row.FindControl("Txt_M_Stab_start");
            TextBox Txt_M_Stab_end = (TextBox)row.FindControl("Txt_M_Stab_end");

            DropDownList DdlPlanYearGroupEdit = (DropDownList)row.FindControl("DdlPlanYearGroupEdit");

            TextBox txtHistory = (TextBox)row.FindControl("TxtPlanYearHistory");
            HiddenField hfID = (HiddenField)row.FindControl("HfPlanYearID");
            ModalPopupExtender mpePYedit = (ModalPopupExtender)row.FindControl("Mpe_epy");
            Label lblMessage = (Label)row.FindControl("Lbl_npy_Message");

            validData = errorChecking.validateTextBoxNull(txtName, validData);
            validData = errorChecking.validateTextBoxDate(txtSdate, validData);
            validData = errorChecking.validateTextBoxDate(txtEdate, validData);

            if (validData == true)
            {
                string _name = txtName.Text;
                DateTime _sDate = DateTime.Parse(txtSdate.Text);
                DateTime _eDate = DateTime.Parse(txtEdate.Text);

                string _notes = txtNotes.Text;
                string _history = txtHistory.Text;
                _planYearID = int.Parse(hfID.Value);

                //Record the history.
                _history += Environment.NewLine + Environment.NewLine + "Record modified on " + _modOn.ToString() + " by " + _modBy + Environment.NewLine;
                _history += "Plan Dates: " + _sDate.ToShortDateString() + " to " + _eDate.ToShortDateString() + Environment.NewLine;

                //Update the database. 
                validData = PlanYear_Controller.updatePlanYear(_planYearID, _name, _sDate, _eDate, _notes, _history, _modOn, _modBy,
                    //I don't like doing this much work inline either but I'm in a hurry
                    Txt_M_Meas_start.Text.TryParseNullableDateTime(),
                    Txt_M_Meas_end.Text.TryParseNullableDateTime(),
                    Txt_M_Admin_start.Text.TryParseNullableDateTime(),
                    Txt_M_Admin_end.Text.TryParseNullableDateTime(),
                    Txt_M_Open_start.Text.TryParseNullableDateTime(),
                    Txt_M_Open_end.Text.TryParseNullableDateTime(),
                    Txt_M_Stab_start.Text.TryParseNullableDateTime(),
                    Txt_M_Stab_end.Text.TryParseNullableDateTime(),
                    int.Parse(DdlPlanYearGroupEdit.SelectedValue)
                    );

                if (validData == true)
                {
                    loadGvPlanYears();

                    // Queue up the calculation
                    employerController.insertEmployerCalculation(_employerID);
                }
                else
                {
                    //Show the error message and leave the popup window visible. 
                    mpePYedit.Show();
                    lblMessage.Text = "An error occurred while updating this record, please try again.";
                }
            }
            else
            {
                //Show the error message and leave the popup window visible. 
                mpePYedit.Show();
                lblMessage.Text = "Please correct all the red fields.";
            }
        }

        protected void GvPlanYears_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewRow row = null;
            int _employerID = int.Parse(HfDistrictID.Value);
            GvPlanYears.EditIndex = e.NewEditIndex;
            GvPlanYears.SelectedIndex = -1;
            loadGvPlanYears();

            int rowID = GvPlanYears.EditIndex;
            row = GvPlanYears.Rows[rowID];

            DropDownList DdlPlanYearGroupEdit = (DropDownList)row.FindControl("DdlPlanYearGroupEdit");
            HiddenField HfPlanYearGroupId = (HiddenField)row.FindControl("HfPlanYearGroupId");
            int groupId = int.Parse(HfPlanYearGroupId.Value);

            List<PlanYearGroup> groups = PlanYearGroupFactory.GetAllPlanYearGroupForEmployerId(_employerID);

            DdlPlanYearGroupEdit.DataSource = groups;
            DdlPlanYearGroupEdit.DataTextField = "GroupName";
            DdlPlanYearGroupEdit.DataValueField = "PlanYearGroupID";
            DdlPlanYearGroupEdit.SelectedIndex = groups.IndexOf(groups.Find(group => group.PlanYearGroupId == groupId));
            DdlPlanYearGroupEdit.DataBind();

            ModalPopupExtender mpeHistory = (ModalPopupExtender)row.FindControl("Mpe_epy");
            mpeHistory.Show();
        }

        protected void GvPlanYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _employerID = int.Parse(HfDistrictID.Value);
            GridViewRow row = GvPlanYears.SelectedRow;
            GvPlanYears.EditIndex = GvPlanYears.SelectedIndex;

            loadGvPlanYears();

            int rowID = GvPlanYears.SelectedIndex;

            row = GvPlanYears.Rows[rowID];
            ModalPopupExtender mpeHistory = (ModalPopupExtender)row.FindControl("MpePlanYearHistory");
            mpeHistory.Show();
        }

        private void loadGvPlanYears()
        {
            int _employerID = int.Parse(HfDistrictID.Value);

            List<PlanYear> pyList = PlanYear_Controller.getEmployerPlanYear(_employerID);

            GvPlanYears.DataSource = pyList;
            GvPlanYears.DataBind();

            LitPyShow.Text = GvPlanYears.Rows.Count.ToString();
            LitPyTotal.Text = pyList.Count.ToString();

            // also load the plan year groups.
            DdlPlanYearGroupNew.DataSource = PlanYearGroupFactory.GetAllPlanYearGroupForEmployerId(_employerID);
            DdlPlanYearGroupNew.DataTextField = "GroupName";
            DdlPlanYearGroupNew.DataValueField = "PlanYearGroupID";
            DdlPlanYearGroupNew.DataBind();
        }

        protected void GvPlanYears_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvPlanYears.PageIndex = e.NewPageIndex;
            loadGvPlanYears();
        }
    }
}