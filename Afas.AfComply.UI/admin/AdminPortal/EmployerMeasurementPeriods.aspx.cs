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
    public partial class EmployerMeasurementPeriods : AdminPageBase
    {





        private ILog Log = LogManager.GetLogger(typeof(EmployerMeasurementPeriods));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the Administration EmployerMeasurementPeriods page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=21", false);
            }
            else 
            {
                HfUserName.Value = user.User_UserName;
                loadEmployers();
                loadMeasurementTypes();
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

            employerType(currEmployer.EMPLOYER_ID);
            loadPlanYears();
            loadEmployeeTypes(employerId);
            loadMeasurements(2);//always ongoing
        }

        private void loadMeasurementTypes()
        {
            Ddl_M_MeasurementType.DataSource = measurementController.getMeasurementTypes();
            Ddl_M_MeasurementType.DataTextField = "MEASUREMENT_TYPE_NAME";
            Ddl_M_MeasurementType.DataValueField = "MEASUREMENT_TYPE_ID";
            Ddl_M_MeasurementType.DataBind();
            Ddl_M_MeasurementType.SelectedIndex = Ddl_M_MeasurementType.Items.Count - 1;            
            Ddl_M_MeasurementType.Items.Add("Select");
        }

        private void loadMeasurements(int _measType)
        {
            int _employerID = 0;
            int _planYearID = 0;
            int _employeeTypeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlPlanYear, validData);

            if (validData == true)
            {
                try
                {
                    _employerID = int.Parse(HfDistrictID.Value);
                    _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
                    _employeeTypeID = int.Parse(DdlEmployeeType.SelectedItem.Value);

                    if (_measType == 1)
                    {
                        Session["tm"] = measurementController.manufactureMeasurementList(_employerID, _planYearID, _employeeTypeID, _measType);
                        RptTransitionMeasurments.DataSource = (List<Measurement>)Session["tm"];
                        RptTransitionMeasurments.DataBind();
                    }
                    else
                    {
                        Session["om"] = measurementController.manufactureMeasurementList(_employerID, _planYearID, _employeeTypeID, _measType);
                        RptTransitionMeasurments.DataSource = (List<Measurement>)Session["om"];
                        RptTransitionMeasurments.DataBind();
                    }
                }
                catch(Exception ex)
                {
                    Log.Warn("Exception while loading data on Admin Meaurement period page", ex);
                    LitMessage.Text = "Error Loading page please check Employee Type and PLAN YEAR set-up.";
                    MpeWebPageMessage.Show();
                }
            }
            else
            {
                LitMessage.Text = "You must create a PLAN YEAR before measurment periods can be set-up!";
                MpeWebPageMessage.Show();
            }
        }

        private void loadEmployeeTypes(int _employerID)
        {
            List<global::EmployeeType> types = EmployeeTypeController.getEmployeeTypes(_employerID);

            DdlEmployeeType.DataSource = types;
            DdlEmployeeType.DataTextField = "EMPLOYEE_TYPE_NAME";
            DdlEmployeeType.DataValueField = "EMPLOYEE_TYPE_ID";
            DdlEmployeeType.DataBind();

            Ddl_M_EmployeeType.DataSource = types;
            Ddl_M_EmployeeType.DataTextField = "EMPLOYEE_TYPE_NAME";
            Ddl_M_EmployeeType.DataValueField = "EMPLOYEE_TYPE_ID";
            Ddl_M_EmployeeType.DataBind();
            Ddl_M_EmployeeType.Items.Add("Select");
            Ddl_M_EmployeeType.SelectedIndex = Ddl_M_EmployeeType.Items.Count - 1;
        }

        private void loadPlanYears()
        {
            int _employerID = int.Parse(HfDistrictID.Value);
            List<PlanYear> years = PlanYear_Controller.getEmployerPlanYear(_employerID);

            DdlPlanYear.DataSource = years;
            DdlPlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
            DdlPlanYear.DataValueField = "PLAN_YEAR_ID";
            DdlPlanYear.DataBind();

            Ddl_M_PlanYear.DataSource = years;
            Ddl_M_PlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
            Ddl_M_PlanYear.DataValueField = "PLAN_YEAR_ID";
            Ddl_M_PlanYear.DataBind();
            Ddl_M_PlanYear.Items.Add("Select");
            Ddl_M_PlanYear.SelectedIndex = Ddl_M_PlanYear.Items.Count - 1;
        }

        protected void BtnSaveMeasurementPeriod_Click(object sender, EventArgs e)
        {
            bool validData = true;
            int _employerID = 0;
            int _planID = 0;
            int _employeeTypeID = 0;
            int _measurementTypeID = 0;
            DateTime _meas_start = System.DateTime.Now.AddYears(-50);
            DateTime _meas_end = System.DateTime.Now.AddYears(-50);
            DateTime _admin_start = System.DateTime.Now.AddYears(-50);
            DateTime _admin_end = System.DateTime.Now.AddYears(-50);
            DateTime _open_start = System.DateTime.Now.AddYears(-50);
            DateTime _open_end = System.DateTime.Now.AddYears(-50);
            DateTime _stab_start = System.DateTime.Now.AddYears(-50);
            DateTime _stab_end = System.DateTime.Now.AddYears(-50);
            string _notes = null;
            string _modBy = null;
            DateTime _modOn = System.DateTime.Now;
            string _history = null;
            int measurementID = 0;
            DateTime? _swStart = null;
            DateTime? _swEnd = null;
            DateTime? _swStart2 = null;
            DateTime? _swEnd2 = null;

            //Error check all fields. 
            validData = errorChecking.validateDropDownSelection(Ddl_M_PlanYear, validData);
            validData = errorChecking.validateDropDownSelection(Ddl_M_EmployeeType, validData);
            validData = errorChecking.validateDropDownSelection(Ddl_M_MeasurementType, validData);
            validData = errorChecking.validateTextBoxDate(Txt_M_Admin_end, validData);
            validData = errorChecking.validateTextBoxDate(Txt_M_Admin_start, validData);
            validData = errorChecking.validateTextBoxDate(Txt_M_Meas_end, validData);
            validData = errorChecking.validateTextBoxDate(Txt_M_Meas_start, validData);
            validData = errorChecking.validateTextBoxDate(Txt_M_Open_end, validData);
            validData = errorChecking.validateTextBoxDate(Txt_M_Open_start, validData);
            validData = errorChecking.validateTextBoxDate(Txt_M_Stab_end, validData);
            validData = errorChecking.validateTextBoxDate(Txt_M_Stab_start, validData);

            _employerID = int.Parse(HfDistrictID.Value);
            employer emp = employerController.getEmployer(_employerID);

            //*************************** END of special rules for the School Districts. *********************************

            if (validData == true)
            {
                //Collect data from the user interface.
                _planID = int.Parse(Ddl_M_PlanYear.SelectedItem.Value);
                _employeeTypeID = int.Parse(Ddl_M_EmployeeType.SelectedItem.Value);
                _measurementTypeID = int.Parse(Ddl_M_MeasurementType.SelectedItem.Value);
                _meas_start = System.Convert.ToDateTime(Txt_M_Meas_start.Text);
                _meas_end = System.Convert.ToDateTime(Txt_M_Meas_end.Text);
                _admin_end = System.Convert.ToDateTime(Txt_M_Admin_end.Text);
                _admin_start = System.Convert.ToDateTime(Txt_M_Admin_start.Text);
                _open_end = System.Convert.ToDateTime(Txt_M_Open_end.Text);
                _open_start = System.Convert.ToDateTime(Txt_M_Open_start.Text);
                _stab_end = System.Convert.ToDateTime(Txt_M_Stab_end.Text);
                _stab_start = System.Convert.ToDateTime(Txt_M_Stab_start.Text);
                _notes = null;
                _history = "Created on " + _modOn.ToString() + " by " + HfUserName.Value.ToString();
                _modBy = HfUserName.Value.ToString();

                //Send data to the database. 
                measurementID = measurementController.manufactureNewMeasurement(_employerID, _planID, _employeeTypeID, _measurementTypeID, _meas_start, _meas_end, _admin_start, _admin_end, _open_start, _open_end, _stab_start, _stab_end, _notes, _modBy, _modOn, _history, _swStart, _swEnd, _swStart2, _swEnd2);

                // Queue up the calculation
                employerController.insertEmployerCalculation(_employerID);

                // Save our new Breaks in service if any are added 
                if (Session["TempBreaks"] != null && measurementID != 0)
                {
                    List<BreakInService> breaks = (List<BreakInService>)Session["TempBreaks"];
                    foreach (var Break in breaks)
                    {
                        BreakInServiceFactory factory = new BreakInServiceFactory();
                        factory.InsertNewBreakInService(measurementID, Break);
                    }
                }

                //If INSERT was succesful, generate the correct objects to display the information. 
                if (measurementID > 0)
                {
                    List<Measurement> tempList = new List<Measurement>();
                    if (Session["tm"] != null)
                    {
                        tempList = (List<Measurement>)Session["tm"];
                    }
                    tempList.Add(new Measurement(measurementID, _employerID, _planID, _employeeTypeID, _measurementTypeID, _meas_start, _meas_end, _admin_start, _admin_end, _open_start, _open_end, _stab_start, _stab_end, _notes, _history, _modOn, _modBy, _swStart, _swEnd, _swStart2, _swEnd2, false, false, null, null, null, null));
                    Session["tm"] = tempList;
                    loadMeasurements(_measurementTypeID);

                    //Reset all the entered values to their default status. 
                    resetNewMeasurementPopup();
                }
                else
                {
                    Lbl_M_message.Text = "An error occurred, You may already have a measurement period for this PLAN, EMPLOYEE TYPE and MEASUREMENT TYPE.";
                    MpeMeasurementPeriod.Show();
                }
            }
            else
            {
                Lbl_M_message.Text = "Please correct all the red fields.";
                MpeMeasurementPeriod.Show();
            }
        }

        private void resetNewMeasurementPopup()
        {
            Ddl_M_PlanYear.SelectedIndex = Ddl_M_PlanYear.Items.Count - 1;
            Ddl_M_EmployeeType.SelectedIndex = Ddl_M_EmployeeType.Items.Count - 1;
            Ddl_M_MeasurementType.SelectedIndex = Ddl_M_MeasurementType.Items.Count - 1;

            Txt_M_Meas_start.Text = null;
            Txt_M_Meas_end.Text = null;
            Txt_M_Admin_end.Text = null;
            Txt_M_Admin_start.Text = null;
            Txt_M_Open_end.Text = null;
            Txt_M_Open_start.Text = null;
            Txt_M_Stab_end.Text = null;
            Txt_M_Stab_start.Text = null;
            TxtSummerBreakEnd.Text = null;
            TxtSummerBreakStart.Text = null;

            //reset breaks in service code
            TxtSummerBreakEnd.Text = null;
            TxtSummerBreakStart.Text = null;

            Session["TempBreaks"] = new List<BreakInService>();
            Gv_TempBreakOfService.DataSource = null;
            Gv_TempBreakOfService.DataBind();
        }

        protected void RptTransitionMeasurments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal litPlanName = (Literal)e.Item.FindControl("LitPlanName");
                Literal litEmployeeType = (Literal)e.Item.FindControl("LitEmployeeType");
                Literal litPlanStart = (Literal)e.Item.FindControl("LitStartDate");
                Literal litPlanEnd = (Literal)e.Item.FindControl("LitEndDate");
                Literal litMeasName = (Literal)e.Item.FindControl("LitMeasurementType");
                HiddenField hfMeasID = (HiddenField)e.Item.FindControl("Hf_M_MeasurementTypeID");
                HiddenField Hf_M_MeasurementID = (HiddenField)e.Item.FindControl("Hf_M_MeasurementID");

                Panel PnlEditSummerWindow = (Panel)e.Item.FindControl("PnlEditSummerWindow");
                Panel PnlSummerWindow3 = (Panel)e.Item.FindControl("PnlSummerWindow3");

                GridView Gv_BreakOfService = (GridView)e.Item.FindControl("Gv_BreakOfService");
                GridView Gv_DisplayBreakOfService = (GridView)e.Item.FindControl("Gv_DisplayBreakOfService");

                litPlanName.Text = DdlPlanYear.SelectedItem.Text;
                litEmployeeType.Text = DdlEmployeeType.SelectedItem.Text;
                litPlanStart.Text = "n/a";
                litPlanEnd.Text = "n/a";

                int measTypeID = int.Parse(hfMeasID.Value);
                int measurementID = int.Parse(Hf_M_MeasurementID.Value);

                if (measTypeID == 1)
                {
                    litMeasName.Text = "Transitional Measurement Period";
                }
                else
                {
                    litMeasName.Text = "Ongoing Measurement Cycle";
                }

                int _employerID = System.Convert.ToInt32(HfDistrictID.Value);

                employer emp = employerController.getEmployer(_employerID);
                if (emp.HasBreaksInService())
                {
                    PnlEditSummerWindow.Visible = true;
                    PnlSummerWindow3.Visible = true;
                }
                else
                {
                    PnlEditSummerWindow.Visible = false;
                    PnlSummerWindow3.Visible = false;
                }

                Gv_DisplayBreakOfService.DataSource = GetBreaks(measurementID).OrderBy(breaks => breaks.StartDate);
                Gv_DisplayBreakOfService.DataBind();

                Gv_BreakOfService.DataSource = GetBreaks(measurementID).OrderBy(breaks => breaks.StartDate);
                Gv_BreakOfService.DataBind();
            }
        }

        protected void RptTransitionMeasurments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                int measurementID = 0;
                DateTime _meas_start = System.DateTime.Now.AddYears(-50);
                DateTime _meas_end = System.DateTime.Now.AddYears(-50);
                DateTime _admin_start = System.DateTime.Now.AddYears(-50);
                DateTime _admin_end = System.DateTime.Now.AddYears(-50);
                DateTime _open_start = System.DateTime.Now.AddYears(-50);
                DateTime _open_end = System.DateTime.Now.AddYears(-50);
                DateTime _stab_start = System.DateTime.Now.AddYears(-50);
                DateTime _stab_end = System.DateTime.Now.AddYears(-50);
                string _notes = null;
                string _modBy = HfUserName.Value.ToString();
                DateTime _modOn = System.DateTime.Now;
                string _history = null;
                bool validData = true;
                DateTime? _swStart = null;
                DateTime? _swEnd = null;
                DateTime? _swStart2 = null;
                DateTime? _swEnd2 = null;
                int _employerID = int.Parse(HfDistrictID.Value);
                employer emp = employerController.getEmployer(_employerID);

                HiddenField hfMeasID = (HiddenField)e.Item.FindControl("Hf_M_MeasurementID");
                TextBox txtMeasStart_new = (TextBox)e.Item.FindControl("Txt_M_Meas_start");
                TextBox txtMeasEnd_new = (TextBox)e.Item.FindControl("Txt_M_Meas_end");
                TextBox txtAdminStart_new = (TextBox)e.Item.FindControl("Txt_M_Admin_start");
                TextBox txtAdminEnd_new = (TextBox)e.Item.FindControl("Txt_M_Admin_end");
                TextBox txtOpenStart_new = (TextBox)e.Item.FindControl("Txt_M_Open_start");
                TextBox txtOpenEnd_new = (TextBox)e.Item.FindControl("Txt_M_Open_end");
                TextBox txtStabStart_new = (TextBox)e.Item.FindControl("Txt_M_Stab_start");
                TextBox txtStabEnd_new = (TextBox)e.Item.FindControl("Txt_M_Stab_end");
                TextBox txtNotes_new = (TextBox)e.Item.FindControl("Txt_M_Notes");
                TextBox txtHistory_new = (TextBox)e.Item.FindControl("TxtTransitionMeasurementHistory");
                Label lblMessage = (Label)e.Item.FindControl("Lbl_M_message");
                ModalPopupExtender mpe = (ModalPopupExtender)e.Item.FindControl("MpeEditTransitionPeriod");

                validData = errorChecking.validateTextBoxDate(txtMeasStart_new, validData);
                validData = errorChecking.validateTextBoxDate(txtMeasEnd_new, validData);
                validData = errorChecking.validateTextBoxDate(txtAdminStart_new, validData);
                validData = errorChecking.validateTextBoxDate(txtAdminEnd_new, validData);
                validData = errorChecking.validateTextBoxDate(txtOpenEnd_new, validData);
                validData = errorChecking.validateTextBoxDate(txtOpenStart_new, validData);
                validData = errorChecking.validateTextBoxDate(txtStabEnd_new, validData);
                validData = errorChecking.validateTextBoxDate(txtStabStart_new, validData);

                if (validData == true)
                {
                    measurementID = int.Parse(hfMeasID.Value);
                    _meas_start = System.Convert.ToDateTime(txtMeasStart_new.Text);
                    _meas_end = System.Convert.ToDateTime(txtMeasEnd_new.Text);
                    _admin_start = System.Convert.ToDateTime(txtAdminStart_new.Text);
                    _admin_end = System.Convert.ToDateTime(txtAdminEnd_new.Text);
                    _open_end = System.Convert.ToDateTime(txtOpenEnd_new.Text);
                    _open_start = System.Convert.ToDateTime(txtOpenStart_new.Text);
                    _stab_end = System.Convert.ToDateTime(txtStabEnd_new.Text);
                    _stab_start = System.Convert.ToDateTime(txtStabStart_new.Text);
                    _notes = txtNotes_new.Text;
                    _history = txtHistory_new.Text;

                    //Determine the type of Measurement Period being evaluated. Ongoing or Transitional
                    List<Measurement> newMeasList = null;
                    int _measTypeID = 2;//everything is now ongoing, no transitioning.//int.Parse(HfMeasurementTypeID.Value);

                    if (_measTypeID == 1)
                    {
                        newMeasList = (List<Measurement>)Session["tm"];
                    }
                    else if (_measTypeID == 2)
                    {
                        newMeasList = (List<Measurement>)Session["om"];
                    }

                    //Loop through the current Measurements to pull past values for the history archive. 
                    //This will keep a running log of any changes made to this specific measurement period.
                    foreach (Measurement m in newMeasList)
                    {
                        if (m.MEASUREMENT_ID == measurementID)
                        {
                            _history += Environment.NewLine + Environment.NewLine; ;
                            _history += "Record Altered on " + _modOn.ToString() + " by " + _modBy + Environment.NewLine;
                            _history += "Measerment Period: " + _meas_start.ToString() + " to " + _meas_end.ToString() + Environment.NewLine;
                            _history += "Administrative Period: " + _admin_start.ToString() + " to " + _admin_end.ToString() + Environment.NewLine;
                            _history += "Open Period: " + _open_start.ToString() + " to " + _open_end.ToString() + Environment.NewLine;
                            _history += "Stability Period: " + _stab_start.ToString() + " to " + _stab_end.ToString() + Environment.NewLine;

                            break;
                        }
                    }

                    //Update the database. 
                    validData = measurementController.updateMeasurement(measurementID, _meas_start, _meas_end, _admin_start, _admin_end, _open_start, _open_end, _stab_start, _stab_end, _notes, _modBy, _modOn, _history, _swStart, _swEnd, _swStart2, _swEnd2);

                    //Update the object list if database update was succesful.
                    if (validData == true)
                    {
                        foreach (Measurement m in newMeasList)
                        {
                            if (m.MEASUREMENT_ID == measurementID)
                            {
                                m.MEASUREMENT_START = _meas_start;
                                m.MEASUREMENT_END = _meas_end;
                                m.MEASUREMENT_ADMIN_START = _admin_start;
                                m.MEASUREMENT_ADMIN_END = _admin_end;
                                m.MEASUREMENT_OPEN_START = _open_start;
                                m.MEASUREMENT_OPEN_END = _open_end;
                                m.MEASUREMENT_STAB_START = _stab_start;
                                m.MEASUREMENT_STAB_END = _stab_end;
                                m.MEASUREMENT_NOTES = _notes;
                                m.MEASUREMENT_HISTORY = _history;
                                m.MEASUREMENT_MOD_BY = _modBy;
                                m.MEASUREMENT_MOD_ON = _modOn;

                                break;
                            }
                        }

                        loadMeasurements(_measTypeID);

                        // Queue up the calculation
                        employerController.insertEmployerCalculation(_employerID);
                    }
                    else
                    {
                        lblMessage.Text = "An error occurred while updating the measurement period.";
                        mpe.Show();
                    }
                }
                else
                {
                    lblMessage.Text = "Please correct all the red fields.";
                    mpe.Show();
                }
            }
        }

        protected void AddNewTempBreak_Click(object sender, EventArgs e)
        {
            if (Session["TempBreaks"] == null)
            {
                Session["TempBreaks"] = new List<BreakInService>();
            }

            List<BreakInService> breaks = (List<BreakInService>)Session["TempBreaks"];

            BreakInService newBreak = new BreakInService();

            User currUser = (User)Session["CurrentUser"];

            newBreak.CreatedBy = currUser.User_UserName;
            //newBreak.CreatedDate = DateTime.Now;

            DateTime start = new DateTime();
            DateTime end = new DateTime();

            try
            {
                start = System.Convert.ToDateTime(TxtSummerBreakStart.Text);
                end = System.Convert.ToDateTime(TxtSummerBreakEnd.Text);
                newBreak.StartDate = start;
                newBreak.EndDate = end;
            }
            catch (Exception exception)
            {
                Log.Info("User probably put in bad info, casuing an excetpion: ", exception);
            }

            if (end != new DateTime() &&
                start != new DateTime() &&
                end > start &&
                false == CheckForOverlap(breaks, start, end) //&&

                )
            {
                breaks.Add(newBreak);

                TxtSummerBreakEnd.Text = null;
                TxtSummerBreakStart.Text = null;
            }

            RefreshTempBreaksInService();
        }

        private void RefreshTempBreaksInService()
        {
            //refresh the display of multiple breaks so that they can see the new break 

            List<BreakInService> breaks = (List<BreakInService>)Session["TempBreaks"];
            Gv_TempBreakOfService.DataSource = breaks.OrderBy(breakinservice => breakinservice.StartDate);
            Gv_TempBreakOfService.DataBind();
            MpeMeasurementPeriod.Show();
        }

        protected void Gv_TempBreakOfService_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = Gv_TempBreakOfService.Rows[e.RowIndex];

            TextBox startText = (TextBox)row.FindControl("TxtSummerBreakStart");
            TextBox endText = (TextBox)row.FindControl("TxtSummerBreakEnd");

            DateTime start;
            DateTime end;
            List<BreakInService> breaks = (List<BreakInService>)Session["TempBreaks"];

            if (DateTime.TryParse(startText.Text, out start) &&
                DateTime.TryParse(endText.Text, out end) &&
                breaks != null &&
                false == CheckForOverlap(breaks, start, end)
           
                )
            {
                BreakInService toEdit = breaks[e.RowIndex];
                if (toEdit != null)
                {
                    toEdit.StartDate = start;
                    toEdit.EndDate = end;
                }
            }

            Gv_TempBreakOfService.EditIndex = -1;

            RefreshTempBreaksInService();
        }

        protected void Gv_TempBreakOfService_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            Gv_TempBreakOfService.EditIndex = -1;

            RefreshTempBreaksInService();
        }

        protected void Gv_TempBreakOfService_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Gv_TempBreakOfService.EditIndex = e.NewEditIndex;

            RefreshTempBreaksInService();
        }

        protected void Gv_TempBreakOfService_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = Gv_TempBreakOfService.Rows[e.RowIndex];

            HiddenField hfId = (HiddenField)row.FindControl("HiddenBreakId");

            List<BreakInService> breaks = (List<BreakInService>)Session["TempBreaks"];

            breaks.RemoveAt(e.RowIndex);

            RefreshTempBreaksInService();
        }

        protected void AddNewEditBreak_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            RepeaterItem ritem = (RepeaterItem)btn.NamingContainer;
            ModalPopupExtender MpeEditTransitionPeriod = (ModalPopupExtender)ritem.FindControl("MpeEditTransitionPeriod");
            GridView Gv_BreakOfService = (GridView)ritem.FindControl("Gv_BreakOfService");
            TextBox TxtEditSummerBreakStart = (TextBox)ritem.FindControl("TxtSummerBreakStart");
            TextBox TxtEditSummerBreakEnd = (TextBox)ritem.FindControl("TxtSummerBreakEnd");
            HiddenField Hf_M_MeasurementID = (HiddenField)ritem.FindControl("Hf_M_MeasurementID");
            int measurementId = int.Parse(Hf_M_MeasurementID.Value);

            BreakInService newBreak = new BreakInService();
            User currUser = (User)Session["CurrentUser"];
            DateTime start = new DateTime();
            DateTime end = new DateTime();

            try
            {
                start = System.Convert.ToDateTime(TxtEditSummerBreakStart.Text);
                end = System.Convert.ToDateTime(TxtEditSummerBreakEnd.Text);
                newBreak.StartDate = start;
                newBreak.EndDate = end;
            }
            catch (Exception exception)
            {
                Log.Info("User probably put in bad info, casuing an excetpion: ", exception);
            }

            if (end != new DateTime() &&
                start != new DateTime() &&
                end > start &&
                false == CheckForOverlap(GetBreaks(measurementId), start, end) //&&

                )
            {
                newBreak.CreatedBy = currUser.User_UserName;
                //newBreak.CreatedDate = DateTime.Now;

                BreakInServiceFactory factory = new BreakInServiceFactory();
                factory.InsertNewBreakInService(measurementId, newBreak);

                // Queue up the calculation
                employerController.insertEmployerCalculation(int.Parse(HfDistrictID.Value));

                TxtSummerBreakEnd.Text = null;
                TxtSummerBreakStart.Text = null;
            }

            RefreshBreaksInService(MpeEditTransitionPeriod, Gv_BreakOfService, measurementId);
        }

        private List<BreakInService> GetBreaks(int measurementId)
        {
            BreakInServiceFactory factory = new BreakInServiceFactory();
            return factory.SelectBreaksInService(measurementId);
        }

        private void RefreshBreaksInService(ModalPopupExtender MpeEditTransitionPeriod, GridView Gv_BreakOfService, int measurementId)
        {
            //refresh the display of multiple breaks so that they can see the new break 
            Gv_BreakOfService.DataSource = GetBreaks(measurementId).OrderBy(breaks => breaks.StartDate);
            Gv_BreakOfService.DataBind();
            MpeEditTransitionPeriod.Show();
        }

        protected void Gv_BreakOfService_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            GridView Gv_BreakOfService = (GridView)sender;
            RepeaterItem ritem = (RepeaterItem)Gv_BreakOfService.NamingContainer;
            ModalPopupExtender MpeEditTransitionPeriod = (ModalPopupExtender)ritem.FindControl("MpeEditTransitionPeriod");
            HiddenField Hf_M_MeasurementID = (HiddenField)ritem.FindControl("Hf_M_MeasurementID");
            int measurementId = int.Parse(Hf_M_MeasurementID.Value);

            GridViewRow row = Gv_BreakOfService.Rows[e.RowIndex];

            HiddenField hfId = (HiddenField)row.FindControl("HiddenBreakId");
            TextBox startText = (TextBox)row.FindControl("TxtSummerBreakStart");
            TextBox endText = (TextBox)row.FindControl("TxtSummerBreakEnd");

            List<BreakInService> breaks = GetBreaks(measurementId);

            int Id;
            DateTime start;
            DateTime end;


            if (int.TryParse(hfId.Value, out Id) &&
                DateTime.TryParse(startText.Text, out start) &&
                DateTime.TryParse(endText.Text, out end) &&
                breaks != null //&&

                )
            {

                BreakInService toEdit = breaks.Where(Break => Break.BreakInServiceId == Id).FirstOrDefault();

                if (toEdit != null)
                {
                    User currUser = (User)Session["CurrentUser"];

                    toEdit.StartDate = start;
                    toEdit.EndDate = end;
                    toEdit.ModifiedBy = currUser.User_UserName;
                    toEdit.ModifiedDate = DateTime.Now;

                    BreakInServiceFactory factory = new BreakInServiceFactory();
                    factory.UpdateBreakInService(measurementId, toEdit);

                    // Queue up the calculation
                    employerController.insertEmployerCalculation(int.Parse(HfDistrictID.Value));
                }
            }

            Gv_BreakOfService.EditIndex = -1;

            RefreshBreaksInService(MpeEditTransitionPeriod, Gv_BreakOfService, measurementId);
        }

        protected void Gv_BreakOfService_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView Gv_BreakOfService = (GridView)sender;
            RepeaterItem ritem = (RepeaterItem)Gv_BreakOfService.NamingContainer;
            ModalPopupExtender MpeEditTransitionPeriod = (ModalPopupExtender)ritem.FindControl("MpeEditTransitionPeriod");
            HiddenField Hf_M_MeasurementID = (HiddenField)ritem.FindControl("Hf_M_MeasurementID");
            int measurementId = int.Parse(Hf_M_MeasurementID.Value);

            Gv_BreakOfService.EditIndex = -1;

            RefreshBreaksInService(MpeEditTransitionPeriod, Gv_BreakOfService, measurementId);
        }

        protected void Gv_BreakOfService_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView Gv_BreakOfService = (GridView)sender;
            RepeaterItem ritem = (RepeaterItem)Gv_BreakOfService.NamingContainer;
            ModalPopupExtender MpeEditTransitionPeriod = (ModalPopupExtender)ritem.FindControl("MpeEditTransitionPeriod");
            HiddenField Hf_M_MeasurementID = (HiddenField)ritem.FindControl("Hf_M_MeasurementID");
            int measurementId = int.Parse(Hf_M_MeasurementID.Value);

            Gv_BreakOfService.EditIndex = e.NewEditIndex;

            RefreshBreaksInService(MpeEditTransitionPeriod, Gv_BreakOfService, measurementId);
        }

        protected void Gv_BreakOfService_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView Gv_BreakOfService = (GridView)sender;
            RepeaterItem ritem = (RepeaterItem)Gv_BreakOfService.NamingContainer;
            ModalPopupExtender MpeEditTransitionPeriod = (ModalPopupExtender)ritem.FindControl("MpeEditTransitionPeriod");
            HiddenField Hf_M_MeasurementID = (HiddenField)ritem.FindControl("Hf_M_MeasurementID");
            int measurementId = int.Parse(Hf_M_MeasurementID.Value);

            GridViewRow row = Gv_BreakOfService.Rows[e.RowIndex];

            HiddenField hfId = (HiddenField)row.FindControl("HiddenBreakId");

            int Id;

            if (int.TryParse(hfId.Value, out Id))
            {
                User currUser = (User)Session["CurrentUser"];

                BreakInServiceFactory factory = new BreakInServiceFactory();
                factory.DeleteBreakInService(Id, currUser.User_UserName);

                // Queue up the calculation
                employerController.insertEmployerCalculation(int.Parse(HfDistrictID.Value));
            }

            RefreshBreaksInService(MpeEditTransitionPeriod, Gv_BreakOfService, measurementId);
        }

        /// <summary>
        /// Check that the passed in start and end do not overlap with an existing break.
        /// </summary>
        /// <param name="breaks">The list of all current breaks.</param>
        /// <param name="start">The start of the new break.</param>
        /// <param name="end">The end of the new break.</param>
        /// <returns></returns>
        public static bool CheckForOverlap(List<BreakInService> breaks, DateTime start, DateTime end)
        {
            foreach (var Break in breaks)
            {
                //I use >= so that it will fail on breaks that end on another start date, or start on another end date.
                if ((start <= Break.EndDate && start >= Break.StartDate) ||
                    (end <= Break.EndDate && end >= Break.StartDate) ||
                    (Break.EndDate <= end && Break.EndDate >= start) ||
                    (Break.StartDate <= end && Break.StartDate >= start))
                {
                    return true;
                }
            }

            return false;
        }

        //Check to see if this EMPLOYER is a school district. 
        private void employerType(int _employerID)
        {
            employer emp = employerController.getEmployer(_employerID);
            if (emp.HasBreaksInService())
            {
                PnlSummerWindow.Visible = true;
            }
            else
            {
                PnlSummerWindow.Visible = false;
            }
        }

        protected void DdlPlanYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadMeasurements(2);//always ongoing
        }

        protected void DdlEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadMeasurements(2);//always ongoing
        }

        protected void ImgBtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            //reloading here was casusing a loop that you couldn't exit.
        }

        protected void Ddl_M_PlanYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _planID = int.Parse(Ddl_M_PlanYear.SelectedItem.Value);
            int _empID = int.Parse(HfDistrictID.Value);

            PlanYear year = PlanYear_Controller.findPlanYear(_planID, _empID);

            //set the defaults
            if(year.Default_Meas_Start != null){
                Txt_M_Meas_start.Text = ((DateTime)year.Default_Meas_Start).ToShortDateString();
            }                        
            if(year.Default_Meas_End != null){
                Txt_M_Meas_end.Text = ((DateTime)year.Default_Meas_End).ToShortDateString();
            }            
            
            if(year.Default_Admin_Start != null){
                Txt_M_Admin_start.Text = ((DateTime)year.Default_Admin_Start).ToShortDateString();
            }            
            if(year.Default_Admin_End != null){
                Txt_M_Admin_end.Text = ((DateTime)year.Default_Admin_End).ToShortDateString();
            }       
     
            if(year.Default_Open_Start != null){
                Txt_M_Open_start.Text = ((DateTime)year.Default_Open_Start).ToShortDateString();
            }            
            if(year.Default_Open_End != null){
                Txt_M_Open_end.Text = ((DateTime)year.Default_Open_End).ToShortDateString();
            }
            
            if(year.Default_Stability_Start != null){
                Txt_M_Stab_start.Text = ((DateTime)year.Default_Stability_Start).ToShortDateString();
            }            
            if(year.Default_Stability_End != null){
                Txt_M_Stab_end.Text = ((DateTime)year.Default_Stability_End).ToShortDateString();
            }
  
            MpeMeasurementPeriod.Show();
        }
    }
}