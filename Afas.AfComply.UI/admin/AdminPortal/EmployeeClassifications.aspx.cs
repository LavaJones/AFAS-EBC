using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EmployeeClassifications : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(EmployeeClassifications));
        
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the Administration EmployeeClassifications page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=17", false);
            }

            loadEmployers();
            loadWaitingPeriods(DdlWaitingPeriod);
            loadAshCodes(DdlASH);
            loadDefaultOfferCodes(DdlOoc);
        }

        private void loadAshCodes(DropDownList ddl)
        {
            ddl.DataSource = employerController.get4980HReliefCodes();
            ddl.DataTextField = "ASH_ID_DESCRIPTION";
            ddl.DataValueField = "ASH_ID";
            ddl.DataBind();

            ddl.Items.Add("Select");
            ddl.SelectedIndex = ddl.Items.Count - 1;
        }

        private void loadEntityStatusCodes(DropDownList ddl)
        {
            ddl.DataSource = employerController.getEntityStatusCodes();
            ddl.DataTextField = "ES_NAME";
            ddl.DataValueField = "ES_ID";
            ddl.DataBind();

            ddl.Items.Add("Select");
            ddl.SelectedIndex = ddl.Items.Count - 1;
        }

        private void loadDefaultOfferCodes(DropDownList ddl)
        {
            ddl.DataSource = airController.ManufactureOOCList(true);
            ddl.DataTextField = "OOC_ID";
            ddl.DataValueField = "OOC_ID";
            ddl.DataBind();

            ddl.Items.Add("Select");
            ddl.SelectedIndex = ddl.Items.Count - 1;
        }

        private void loadWaitingPeriods(DropDownList ddl)
        {
            ddl.DataSource = insuranceController.manufactureWaitingPeriod();
            ddl.DataTextField = "description";
            ddl.DataValueField = "id";
            ddl.DataBind();

            ddl.Items.Add("Select");
            ddl.SelectedIndex = ddl.Items.Count - 1;
        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshGrid();
            GvClassifications.EditIndex = -1;
        }

        private employer GetCurrentSelectedEmployer() 
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
                lblMsg.Text = "Please Select an Employer.";

                return null;
            }

            return  employerController.getEmployer(employerId);        
        }

        protected void BtnSaveNew_Click(object sender, EventArgs e)
        {
            bool validData = true;
            employer employ = GetCurrentSelectedEmployer();
            Literal litUser = (Literal)this.Master.FindControl("LitUserName");
            validData = errorChecking.validateTextBoxNull(TxtEmployeeClass, validData);
            validData = errorChecking.validateDropDownSelection(DdlWaitingPeriod, validData);

            if (validData == true)
            {
                string description = null;
                string safeHarbor = null;
                string ooc = null;
                int waitingPeriodID = 0;
                DateTime modOn = DateTime.Now;
                string modBy = litUser.Text;
                string history = null;

                description = TxtEmployeeClass.Text;
                int.TryParse(DdlWaitingPeriod.SelectedItem.Value, out waitingPeriodID);

                if (errorChecking.validateDropDownSelectionNoRed(DdlASH, true) == false) { safeHarbor = string.Empty;}
                else { safeHarbor = DdlASH.SelectedItem.Value.ToLower(); }

                if (errorChecking.validateDropDownSelectionNoRed(DdlOoc, true) == false) { ooc = string.Empty; }
                else { ooc = DdlOoc.SelectedItem.Value.ToLower(); }

                history = "Created on " + modOn.ToString() + " by " + modBy + System.Environment.NewLine + description;
                history += Environment.NewLine + "Code: " + safeHarbor + ", WaitingPeriod: " + DdlWaitingPeriod.SelectedItem.Text;

                bool validTransaction =  classificationController.ManufactureEmployeeClassification(employ.EMPLOYER_ID, description, safeHarbor, modOn, modBy, history, waitingPeriodID, ooc);

                if (validTransaction == true)
                {
                    TxtEmployeeClass.Text = string.Empty;
                    DdlWaitingPeriod.SelectedIndex = DdlWaitingPeriod.Items.Count - 1;
                    DdlASH.SelectedIndex = DdlASH.Items.Count - 1;
                    RefreshGrid();
                }
                else
                {
                    lblMsg.Text = "An error occured while trying to save the data, please try again.";
                }
            }
        }

        protected void GvClassifications_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            User current = (User)Session["CurrentUser"];
            int waitingPeriodID = 0;
            int entityStatusID = 0;
            string history = null;
            string ashCode = null;
            string ooc = null;
            string name = null;
            string modBy = current.User_UserName;
            DateTime modOn = DateTime.Now;
            bool validData = true;

            //Save the Edits to a Classification
            GridViewRow row = GvClassifications.Rows[e.RowIndex];
            int employerID = 0;
            int id = 0;
            HiddenField hfId = (HiddenField)row.FindControl("Hf_Edit_id");
            TextBox TxtDescriptione = (TextBox)row.FindControl("Txt_Edit_name");
            DropDownList ddlWaitingPeriode = (DropDownList)row.FindControl("Ddl_Edit_waitingPeriod");
            DropDownList ddlEntityStatuse = (DropDownList)row.FindControl("Ddl_Edit_entityStatus");
            DropDownList ddlAshe = (DropDownList)row.FindControl("Ddl_Edit_ASH");
            DropDownList ddlOoc = (DropDownList)row.FindControl("Ddl_Edit_ooc");

            validData = errorChecking.validateTextBoxNull(TxtDescriptione, validData);
            validData = errorChecking.validateDropDownSelection(ddlEntityStatuse, validData);
            validData = errorChecking.validateDropDownSelection(ddlWaitingPeriode, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

            if (validData == true)
            {
                if (int.TryParse(hfId.Value, out id))
                {
                    int.TryParse(DdlEmployer.SelectedItem.Value, out employerID);
                    int.TryParse(ddlEntityStatuse.SelectedItem.Value, out entityStatusID);
                    int.TryParse(ddlWaitingPeriode.SelectedItem.Value, out waitingPeriodID);
                    name = TxtDescriptione.Text;

                    if (errorChecking.validateDropDownSelectionNoRed(ddlAshe, true) == false) { ashCode = string.Empty; }
                    else { ashCode = ddlAshe.SelectedItem.Value.ToLower(); }

                    if (errorChecking.validateDropDownSelectionNoRed(ddlOoc, true) == false) { ooc = string.Empty; }
                    else { ooc = ddlOoc.SelectedItem.Value.ToLower(); }

                    List<classification> tempList = classificationController.ManufactureEmployerClassificationList(employerID, true);
                    classification myClassification = classificationController.findClassification(id, tempList);

                    if (myClassification != null)
                    {
                        history = myClassification.CLASS_HISTORY + System.Environment.NewLine;
                        history += System.Environment.NewLine + "Record modfied on " + modOn.ToString() + " by " + modBy + System.Environment.NewLine + name;
                        history += System.Environment.NewLine + "Code: " + ashCode + ", WaitingPeriod: " + ddlWaitingPeriode.SelectedItem.Text;
                        classificationController.UpdateEmployeeClassification(id, name, ashCode, DateTime.Now, current.User_UserName, history, waitingPeriodID, entityStatusID, ooc);
                    }
                   
                }

                GvClassifications.EditIndex = -1;

                RefreshGrid();
            }
        }

        protected void GvClassifications_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //Cancel editing a Classification
            GvClassifications.EditIndex = -1;

            RefreshGrid();
        }

        protected void GvClassifications_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //Begin editing a Classification
            GvClassifications.EditIndex = e.NewEditIndex;
            RefreshGrid();
        }

        private void RefreshGrid()
        {

            //refresh the display of classifications
            employer emp = GetCurrentSelectedEmployer();
            
            if (null != emp)
            {
            
                GvClassifications.DataSource = classificationController.ManufactureEmployerClassificationList(emp.EMPLOYER_ID, true);
                GvClassifications.DataBind();
            
            }
        
        }

        protected void GvClassifications_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;

            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                DropDownList ddlWaitingPeriod = (DropDownList)row.FindControl("Ddl_gv_waitingPeriod");
                DropDownList ddlEntityStatus = (DropDownList)row.FindControl("Ddl_gv_entityStatus");
                HiddenField hfWaitingPeriodId = (HiddenField)row.FindControl("Hf_gv_waitingPeriod");
                HiddenField hfEntityStatusId = (HiddenField)row.FindControl("Hf_gv_entityStatus");
                

                loadWaitingPeriods(ddlWaitingPeriod);
                loadEntityStatusCodes(ddlEntityStatus);

                errorChecking.setDropDownList(ddlWaitingPeriod, hfWaitingPeriodId.Value);
                errorChecking.setDropDownList(ddlEntityStatus, hfEntityStatusId.Value);
            }
            else if(e.Row.RowState == DataControlRowState.Edit || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate))
            {
                DropDownList ddlWaitingPeriod = (DropDownList)row.FindControl("Ddl_Edit_waitingPeriod");
                DropDownList ddlEntityStatus = (DropDownList)row.FindControl("Ddl_Edit_entityStatus");
                DropDownList ddlOfferOfCoverage = (DropDownList)row.FindControl("Ddl_Edit_ooc");
                DropDownList ddlAsh = (DropDownList)row.FindControl("Ddl_Edit_ASH");
                HiddenField hfWaitingPeriodId = (HiddenField)row.FindControl("Hf_Edit_waitingPeriod");
                HiddenField hfEntityStatusId = (HiddenField)row.FindControl("Hf_Edit_entityStatus");
                HiddenField hfAshId = (HiddenField)row.FindControl("Hf_Edit_Ash");
                HiddenField hfOfferOfCoverage = (HiddenField)row.FindControl("Hf_Edit_ooc");

                loadWaitingPeriods(ddlWaitingPeriod);
                loadEntityStatusCodes(ddlEntityStatus);
                loadAshCodes(ddlAsh);
                loadDefaultOfferCodes(ddlOfferOfCoverage);

                errorChecking.setDropDownList(ddlWaitingPeriod, hfWaitingPeriodId.Value);
                errorChecking.setDropDownList(ddlEntityStatus, hfEntityStatusId.Value);
                errorChecking.setDropDownList(ddlAsh, hfAshId.Value);
                errorChecking.setDropDownList(ddlOfferOfCoverage, hfOfferOfCoverage.Value);
            }
        }

        protected void BtnSearchEmployer_Click(object sender, EventArgs e)
        {
            loadEmployers();
        }

        /// <summary>
        /// This will filter and load the employer drop down list. 
        /// </summary>
        private void loadEmployers()
        {
            string searchText = null;
            bool validData = true;
            List<employer> currEmployers = employerController.getAllEmployers();
            List<employer> filteredEmployers = new List<employer>();

            validData = errorChecking.validateTextBoxNull(TxtEmployerSearch, validData);
            TxtEmployerSearch.BackColor = System.Drawing.Color.White;


            //This section can be replaced with the function that is built within AfComply. 
            if (validData == true)
            {
                searchText = TxtEmployerSearch.Text.ToLower();
                filteredEmployers = employerController.FilterEmployerBySearch(searchText, currEmployers);
            }
            else { filteredEmployers = currEmployers; }

            DdlEmployer.DataSource = filteredEmployers;
            DdlEmployer.DataTextField = "EMPLOYER_NAME";
            DdlEmployer.DataValueField = "EMPLOYER_ID";
            DdlEmployer.DataBind();

            DdlEmployer.Items.Add("Select");
            DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
        }
    }

}