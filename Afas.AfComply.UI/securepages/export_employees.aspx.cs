using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;
using log4net;
using System.Data;
using Afas.Domain;

public partial class securepages_export_employees : Afas.AfComply.UI.securepages.SecurePageBase
{

    protected string DisclaimerMessage = "IMPORTANT: the file you are about to export may have been " +
"pre-populated with data from your prior year offer file. " +
"You must review the accuracy and completeness of this information " +
"to ensure accurate tracking and/or reporting results. " +
"BE SURE to make any changes necessary to bring the " +
"file up to date before resubmitting it to your "
+ Branding.ProductName +
@" team.
This file contains Social Security Numbers and I understand it must be handled with care.";

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        DataBind();
        HfUserName.Value = user.User_Full_Name;

        HfDistrictID.Value = user.User_District_ID.ToString();

        loadEmployees();
        loadGrossPayDescriptions();
        loadGrossPayExtIDs();
        loadHrStatus();
        loadACAstatus();
        loadEmployeeClassifications();
        loadPlanYears();

    }

    private void loadGrossPayDescriptions()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        Ddl_f_PayDesc.DataSource = gpType_Controller.getEmployeeTypes(_employerID);
        Ddl_f_PayDesc.DataTextField = "GROSS_PAY_DESCRIPTION";
        Ddl_f_PayDesc.DataValueField = "GROSS_PAY_ID";
        Ddl_f_PayDesc.DataBind();

        Ddl_f_PayDesc.Items.Add("Select");
        Ddl_f_PayDesc.SelectedIndex = Ddl_f_PayDesc.Items.Count - 1;
    }

    private void loadGrossPayExtIDs()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        Ddl_f_PayID.DataSource = gpType_Controller.getEmployeeTypes(_employerID);
        Ddl_f_PayID.DataTextField = "GROSS_PAY_EXTERNAL_ID";
        Ddl_f_PayID.DataValueField = "GROSS_PAY_ID";
        Ddl_f_PayID.DataBind();

        Ddl_f_PayID.Items.Add("Select");
        Ddl_f_PayID.SelectedIndex = Ddl_f_PayID.Items.Count - 1;
    }

    private void loadACAstatus()
    {
        Ddl_f_AcaStatus.DataSource = classificationController.getACAstatusList();
        Ddl_f_AcaStatus.DataTextField = "ACA_STATUS_NAME";
        Ddl_f_AcaStatus.DataValueField = "ACA_STATUS_ID";
        Ddl_f_AcaStatus.DataBind();

        Ddl_f_AcaStatus.Items.Add("Select");
        Ddl_f_AcaStatus.SelectedIndex = Ddl_f_AcaStatus.Items.Count - 1;

        Ddl_u_AcaStatus.DataSource = classificationController.getACAstatusList();
        Ddl_u_AcaStatus.DataTextField = "ACA_STATUS_NAME";
        Ddl_u_AcaStatus.DataValueField = "ACA_STATUS_ID";
        Ddl_u_AcaStatus.DataBind();

        Ddl_u_AcaStatus.Items.Add("Select");
        Ddl_u_AcaStatus.SelectedIndex = Ddl_u_AcaStatus.Items.Count - 1;
    }

    private void loadHrStatus()
    {
        int _employerID = int.Parse(HfDistrictID.Value);

        if (Session["HRStatus"] == null)
        {
            Session["HRStatus"] = hrStatus_Controller.manufactureHRStatusList(_employerID);
        }
        Ddl_f_HrStatus.DataSource = Session["HRStatus"];
        Ddl_f_HrStatus.DataTextField = "HR_STATUS_NAME";
        Ddl_f_HrStatus.DataValueField = "HR_STATUS_ID";
        Ddl_f_HrStatus.DataBind();

        Ddl_f_HrStatus.Items.Add("Select");
        Ddl_f_HrStatus.SelectedIndex = Ddl_f_HrStatus.Items.Count - 1;
    }

    private void loadEmployeeClassifications()
    {

        int _employerID = int.Parse(HfDistrictID.Value);

        Ddl_f_EmployeeClass.DataSource = classificationController.ManufactureEmployerClassificationList(_employerID, true);
        Ddl_f_EmployeeClass.DataTextField = "CLASS_DESC";
        Ddl_f_EmployeeClass.DataValueField = "CLASS_ID";
        Ddl_f_EmployeeClass.DataBind();
        Ddl_f_EmployeeClass.Items.Add("Select");
        Ddl_f_EmployeeClass.SelectedIndex = Ddl_f_EmployeeClass.Items.Count - 1;

        Ddl_u_EmployeeClass.DataSource = classificationController.ManufactureEmployerClassificationList(_employerID, true);
        Ddl_u_EmployeeClass.DataTextField = "CLASS_DESC";
        Ddl_u_EmployeeClass.DataValueField = "CLASS_ID";
        Ddl_u_EmployeeClass.DataBind();
        Ddl_u_EmployeeClass.Items.Add("Select");
        Ddl_u_EmployeeClass.SelectedIndex = Ddl_u_EmployeeClass.Items.Count - 1;
    }
    private void loadPlanYears()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        DdlPlanYear.DataSource = PlanYear_Controller.getEmployerPlanYear(_employerID);
        DdlPlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
        DdlPlanYear.DataValueField = "PLAN_YEAR_ID";
        DdlPlanYear.DataBind();
        DdlPlanYear.SelectedIndex = DdlPlanYear.Items.Count - 1;
    }

    private void loadEmployees()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        List<Employee_E> fullList = new List<Employee_E>();
        List<Employee_E> filteredList = new List<Employee_E>();
        List<Employee_E> filteredList2 = new List<Employee_E>();
        List<Employee_E> filteredList3 = new List<Employee_E>();
        List<Employee_E> filteredList4 = new List<Employee_E>();
        List<Employee_E> filteredList5 = new List<Employee_E>();
        List<Employee_E> filteredList6 = new List<Employee_E>();

        bool applyFilter = false;

        Session["FilteredEmployeeExport"] = null;
        if (Session["EmployeeExport"] == null)
        {
            Session["EmployeeExport"] = EmployeeController.manufactureEmployeeExportList(_employerID);
        }

        fullList = (List<Employee_E>)Session["EmployeeExport"];

        if (Cb_f_PayrollID.Checked == true)
        {
            string extPayrollID = Txt_f_PayrollID.Text;
            applyFilter = true;

            foreach (Employee_E emp in fullList)
            {
                if (emp.EMPLOYEE_EXT_ID.Contains(extPayrollID))
                {
                    filteredList.Add(emp);
                }
            }
        }
        else
        {
            filteredList = fullList;
        }

        if (Cb_f_LastName.Checked == true)
        {
            string lname = Txt_f_LastName.Text.ToLower();
            applyFilter = true;

            foreach (Employee_E emp in filteredList)
            {
                if (emp.EMPLOYEE_LAST_NAME.ToLower().Contains(lname))
                {
                    filteredList2.Add(emp);
                }
            }
        }
        else
        {
            filteredList2 = filteredList;
        }

        if (Cb_f_PayDesc.Checked == true)
        {
            bool validData = true;
            int _grossPayID = 0;
            int records = 0;

            validData = errorChecking.validateDropDownSelection(Ddl_f_PayDesc, validData);

            if (validData == true)
            {
                _grossPayID = int.Parse(Ddl_f_PayDesc.SelectedItem.Value);
                List<int> employeeIDList = new List<int>();
                employeeIDList = Payroll_Controller.getEmployeeIDforPayrollGrossPayID(_grossPayID);
                foreach (Employee_E emp in filteredList2)
                {
                    foreach (int i in employeeIDList)
                    {
                        if (emp.EMPLOYEE_ID == i)
                        {
                            filteredList3.Add(emp);
                            break;
                        }
                    }
                }
            }

        }
        else
        {
            filteredList3 = filteredList2;
        }

        if (Cb_f_HrStatus.Checked == true)
        {
            bool validData = true;
            validData = errorChecking.validateDropDownSelection(Ddl_f_HrStatus, validData);
            if (validData == true)
            {
                applyFilter = true;
                int hrStatusID = int.Parse(Ddl_f_HrStatus.SelectedItem.Value);
                foreach (Employee_E emp in filteredList3)
                {
                    if (emp.EMPLOYEE_HR_STATUS_ID == hrStatusID)
                    {
                        filteredList4.Add(emp);
                    }
                }
            }
        }
        else
        {
            filteredList4 = filteredList3;
        }

        if (Cb_f_EmployeeClass.Checked == true)
        {
            bool validData = true;
            validData = errorChecking.validateDropDownSelection(Ddl_f_EmployeeClass, validData);
            if (validData == true)
            {
                applyFilter = true;
                int classID = int.Parse(Ddl_f_EmployeeClass.SelectedItem.Value);
                foreach (Employee_E emp in filteredList4)
                {
                    if (emp.EMPLOYEE_CLASS_ID == classID)
                    {
                        filteredList5.Add(emp);
                    }
                }
            }
        }
        else
        {
            filteredList5 = filteredList4;
        }

        if (Cb_f_AcaStatus.Checked == true)
        {
            bool validData = true;
            validData = errorChecking.validateDropDownSelection(Ddl_f_AcaStatus, validData);
            if (validData == true)
            {
                applyFilter = true;
                int acaStatusID = int.Parse(Ddl_f_AcaStatus.SelectedItem.Value);
                foreach (Employee_E emp in filteredList5)
                {
                    if (emp.EMPLOYEE_ACT_STATUS_ID == acaStatusID)
                    {
                        filteredList6.Add(emp);
                    }
                }
            }
        }
        else
        {
            filteredList6 = filteredList5;
        }

        litAlertsShown.Text = filteredList6.Count.ToString();
        litAlertCount.Text = fullList.Count.ToString();
        GvEmployee.DataSource = filteredList6;
        GvEmployee.DataBind();
    }

    protected void Cb_f_PayrollID_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_PayrollID.Checked == true)
        {
            Txt_f_PayrollID.Text = null;
            Txt_f_PayrollID.Enabled = true;
        }
        else
        {
            Txt_f_PayrollID.Text = "n/a";
            Txt_f_PayrollID.Enabled = false;
            Txt_f_PayrollID.BackColor = System.Drawing.Color.White;
        }
    }

    protected void Cb_f_LastName_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_LastName.Checked == true)
        {
            Txt_f_LastName.Text = null;
            Txt_f_LastName.Enabled = true;
        }
        else
        {
            Txt_f_LastName.Text = "n/a";
            Txt_f_LastName.Enabled = false;
            Txt_f_LastName.BackColor = System.Drawing.Color.White;
        }
    }
    protected void Cb_f_PayDesc_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_PayDesc.Checked == true)
        {
            Ddl_f_PayDesc.SelectedIndex = Ddl_f_PayDesc.Items.Count - 1;
            Ddl_f_PayDesc.Enabled = true;
            Ddl_f_PayID.SelectedIndex = Ddl_f_PayID.Items.Count - 1;
            Ddl_f_PayID.Enabled = true;
        }
        else
        {
            Ddl_f_PayDesc.SelectedIndex = Ddl_f_PayDesc.Items.Count - 1;
            Ddl_f_PayDesc.Enabled = false;
            Ddl_f_PayDesc.BackColor = System.Drawing.Color.White;
            Ddl_f_PayID.SelectedIndex = Ddl_f_PayID.Items.Count - 1;
            Ddl_f_PayID.Enabled = false;
            Ddl_f_PayID.BackColor = System.Drawing.Color.White;
        }
    }

    protected void Cb_f_HrStatus_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_HrStatus.Checked == true)
        {
            Ddl_f_HrStatus.SelectedIndex = Ddl_f_HrStatus.Items.Count - 1;
            Ddl_f_HrStatus.Enabled = true;
        }
        else
        {
            Ddl_f_HrStatus.SelectedIndex = Ddl_f_HrStatus.Items.Count - 1;
            Ddl_f_HrStatus.Enabled = false;
            Ddl_f_HrStatus.BackColor = System.Drawing.Color.White;
        }
    }

    protected void Cb_f_EmployeeClass_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_EmployeeClass.Checked == true)
        {
            Ddl_f_EmployeeClass.SelectedIndex = Ddl_f_EmployeeClass.Items.Count - 1;
            Ddl_f_EmployeeClass.Enabled = true;
        }
        else
        {
            Ddl_f_EmployeeClass.SelectedIndex = Ddl_f_EmployeeClass.Items.Count - 1;
            Ddl_f_EmployeeClass.Enabled = false;
            Ddl_f_EmployeeClass.BackColor = System.Drawing.Color.White;
        }
    }

    protected void Cb_f_AcaStatus_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_AcaStatus.Checked == true)
        {
            Ddl_f_AcaStatus.SelectedIndex = Ddl_f_AcaStatus.Items.Count - 1;
            Ddl_f_AcaStatus.Enabled = true;
        }
        else
        {
            Ddl_f_AcaStatus.SelectedIndex = Ddl_f_AcaStatus.Items.Count - 1;
            Ddl_f_AcaStatus.Enabled = false;
            Ddl_f_AcaStatus.BackColor = System.Drawing.Color.White;
        }
    }

    protected void Cb_u_EmployeeClass_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_u_EmployeeClass.Checked == true)
        {
            Ddl_u_EmployeeClass.SelectedIndex = Ddl_u_EmployeeClass.Items.Count - 1;
            Ddl_u_EmployeeClass.Enabled = true;
        }
        else
        {
            Ddl_u_EmployeeClass.SelectedIndex = Ddl_u_EmployeeClass.Items.Count - 1;
            Ddl_u_EmployeeClass.Enabled = false;
            Ddl_u_EmployeeClass.BackColor = System.Drawing.Color.White;
        }
    }

    protected void Cb_u_ACAStatus_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_u_ACAStatus.Checked == true)
        {
            Ddl_u_AcaStatus.SelectedIndex = Ddl_u_AcaStatus.Items.Count - 1;
            Ddl_u_AcaStatus.Enabled = true;
        }
        else
        {
            Ddl_u_AcaStatus.SelectedIndex = Ddl_u_AcaStatus.Items.Count - 1;
            Ddl_u_AcaStatus.Enabled = false;
            Ddl_u_AcaStatus.BackColor = System.Drawing.Color.White;
        }
    }
    protected void CbCheckAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GvEmployee.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");

            if (CbCheckAll.Checked == true)
            {
                cb.Checked = true;
            }
            else
            {
                cb.Checked = false;
            }
        }
    }

    protected void GvEmployee_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortBy = e.SortExpression;
        string lastSortExpression = "";
        string lastSortDirection = "ASC";
        int _employerID = int.Parse(HfDistrictID.Value);

        List<Employee_E> tempList = (List<Employee_E>)Session["EmployeeExport"];

        if (Session["sortExp"] != null)
        {
            lastSortExpression = Session["sortExp"].ToString();
        }
        if (Session["sortDir"] != null)
        {
            lastSortDirection = Session["sortDir"].ToString();
        }

        try
        {
            if (e.SortExpression != lastSortExpression)
            {
                lastSortExpression = e.SortExpression;
                lastSortDirection = "ASC";
            }
            else
            {
                if (lastSortDirection == "ASC")
                {
                    lastSortExpression = e.SortExpression;
                    lastSortDirection = "DESC";
                }
                else
                {
                    lastSortExpression = e.SortExpression;
                    lastSortDirection = "ASC";
                }
            }

            switch (lastSortDirection)
            {
                case "ASC":
                    switch (e.SortExpression)
                    {
                        case "EMPLOYEE_EXT_ID":
                            tempList = tempList.OrderBy(o => o.EMPLOYEE_EXT_ID).ToList();
                            break;
                        case "EMPLOYEE_ID":
                            tempList = tempList.OrderBy(o => o.EMPLOYEE_ID).ToList();
                            break;
                        case "EMPLOYEE_FULL_NAME":
                            tempList = tempList.OrderBy(o => o.EMPLOYEE_FULL_NAME).ToList();
                            break;
                        case "EX_HR_STATUS_NAME":
                            tempList = tempList.OrderBy(o => o.EX_HR_STATUS_NAME).ToList();
                            break;
                        case "EMPLOYEE_CLASS_ID":
                            tempList = tempList.OrderBy(o => o.EMPLOYEE_CLASS_ID).ToList();
                            break;
                        case "EX_CLASS_NAME":
                            tempList = tempList.OrderBy(o => o.EX_CLASS_NAME).ToList();
                            break;
                        case "EMPLOYEE_ACT_STATUS_ID":
                            tempList = tempList.OrderBy(o => o.EMPLOYEE_ACT_STATUS_ID).ToList();
                            break;
                        case "EX_ACA_NAME":
                            tempList = tempList.OrderBy(o => o.EX_ACA_NAME).ToList();
                            break;
                        default:
                            break;
                    }
                    break;
                case "DESC":
                    tempList.Reverse();
                    break;
                default:
                    break;
            }

            Session["EmployeeExport"] = tempList;
            Session["sortDir"] = lastSortDirection;
            Session["sortExp"] = lastSortExpression;
            loadEmployees();
        }
        catch (Exception exception)
        {
            this.Log.Warn("Suppressing errors.", exception);
        }

    }

    protected void GvEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvEmployee.PageIndex = e.NewPageIndex;
        loadEmployees();
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }



    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {

        Server.ScriptTimeout = 300;

        if (Feature.FullDataExportEnabled)
        {

            try
            {


                employer currDist = (employer)Session["CurrentDistrict"];
                User currUser = (User)Session["CurrentUser"];

                List<Employee_E> tempList = EmployeeController.manufactureEmployeeExportList(currDist.EMPLOYER_ID);

                DataTable export = EmployeeController.GetEmployeesCheTable(tempList, currDist.EMPLOYER_EIN, currDist.EMPLOYER_ID);

                String body = "A " + currDist.EMPLOYER_NAME + " employee export file has been downloaded by " + HfUserName.Value + " at " + DateTime.Now.ToString();
                PIILogger.LogPII(String.Format("Employee Class Export Download: {0} -- Row Count:[{1}], IP:[{2}], User Id:[{3}]", body, export.Rows.Count, Request.UserHostAddress, currUser.User_ID));

                string filename = "EmployeeExport";
                String attachment = "attachment; filename="+ filename.CleanFileName() + ".csv";
                Response.ClearContent();
                Response.BufferOutput = false;
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";

                Response.Write(export.GetAsCsv());

                Response.Flush();         
                Response.SuppressContent = true;                
                HttpContext.Current.ApplicationInstance.CompleteRequest();                      
                Response.End();

            }
            catch (Exception exception)
            {
                this.Log.Warn("Error Durring Export using DataTable.", exception);
            }

        }
        else
        {

            try
            {

                employer currDist = (employer)Session["CurrentDistrict"];
                List<Employee_E> tempList = (List<Employee_E>)Session["EmployeeExport"];
                User currUser = (User)Session["CurrentUser"];

                string fileName = EmployeeController.generateTextFile(tempList, currDist);
                string fileFullPath = Server.MapPath("..\\ftps\\export\\") + fileName;
                string body = "A " + currDist.EMPLOYER_NAME + " employee class export file has been downloaded by " + HfUserName.Value + " at " + DateTime.Now.ToString();

                PIILogger.LogPII(String.Format("Employee Class Export Download: {0} -- File Path: [{1}], IP:[{2}], User Id:[{3}]", body, fileFullPath, Request.UserHostAddress, currUser.User_ID));

                string appendText = "attachment; filename=" + fileName;
                Response.ContentType = "file/text";
                Response.AppendHeader("Content-Disposition", appendText);
                Response.TransmitFile(fileFullPath);

                Response.Flush();         
                Response.SuppressContent = true;                
                HttpContext.Current.ApplicationInstance.CompleteRequest();                      
                Response.End();

            }
            catch (Exception exception)
            {
                this.Log.Warn("Suppressing errors.", exception);
            }

        }

    }

    protected void ImgBtnExportCarrier_Click(Object sender, ImageClickEventArgs eventArguments)
    {

        Server.ScriptTimeout = 300;

        if (Feature.EmployeeExportCarrierFileEnabled)
        {

            try
            {


                employer currDist = (employer)Session["CurrentDistrict"];
                User currUser = (User)Session["CurrentUser"];

                IList<Employee_E> employeeList = EmployeeController.manufactureEmployeeExportList(currDist.EMPLOYER_ID).FilterForActiveEmployees();

                DataTable export = EmployeeController.GetInsCarrierExport(employeeList, currDist.EMPLOYER_EIN, currDist.EMPLOYER_ID);

                String body = "An " + currDist.EMPLOYER_NAME + " employee export carrier file has been downloaded by " + HfUserName.Value + " at " + DateTime.Now.ToString();
                PIILogger.LogPII(String.Format("Employee Class Export Carrier Download: {0} -- Row Count:[{1}], IP:[{2}], User Id:[{3}]", body, export.Rows.Count, Request.UserHostAddress, currUser.User_ID));
                string filename = "EmployeeCarrierExport";
                String attachment = "attachment; filename="+filename.CleanFileName()+".csv";
                Response.ClearContent();
                Response.BufferOutput = false;
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";

                Response.Write(export.GetAsCsv());

                Response.Flush();         
                Response.SuppressContent = true;                
                HttpContext.Current.ApplicationInstance.CompleteRequest();                      
                Response.End();

            }
            catch (Exception exception)
            {
                this.Log.Warn("Error during Carrier export using DataTable.", exception);
            }

        }

    }

    protected void ImgBtnExportOffer_Click(Object sender, ImageClickEventArgs eventArguments)
    {

        Server.ScriptTimeout = 300;

        if (Feature.EmployeeExportOfferFileEnabled)
        {

            try
            {

                employer currDist = (employer)Session["CurrentDistrict"];
                User currUser = (User)Session["CurrentUser"];

                int _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
                PlanYear planyear = PlanYear_Controller.findPlanYear(_planYearID, currDist.EMPLOYER_ID);

                string fileName = "InsuranceOffer_" + planyear.PLAN_YEAR_DESCRIPTION + "_" + currDist.EMPLOYER_NAME + "_" + DateTime.Now.ToShortDateString();

                string csvString = insuranceController.generateInsuranceAlertFile(planyear, currDist);

                String body = "An " + currDist.EMPLOYER_NAME + " employee export offer file has been downloaded by " + HfUserName.Value + " at " + DateTime.Now.ToString();
                PIILogger.LogPII(String.Format("Employee Class Export Offer Download: {0} -- IP:[{1}], User Id:[{2}]", body, Request.UserHostAddress, currUser.User_ID));

                String attachment = "attachment; filename=" + fileName.CleanFileName() + ".csv";
                Response.ClearContent();
                Response.BufferOutput = false;
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";

                Response.Write(csvString);

                Response.Flush();         
                Response.SuppressContent = true;                
                HttpContext.Current.ApplicationInstance.CompleteRequest();                      
                Response.End();

            }
            catch (Exception exception)
            {
                this.Log.Warn("Error during Offer export using DataTable.", exception);
            }

        }

    }

    /// <summary>
    /// This function will filter down the Employee Gridview based on User selections. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnApplyFilters_Click(object sender, EventArgs e)
    {
        loadEmployees();
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        int i = 0;
        bool validData = true;

        if (Cb_u_ACAStatus.Checked == false && Cb_u_EmployeeClass.Checked == false)
        {
            validData = false;
            Cb_u_EmployeeClass.BackColor = System.Drawing.Color.Red;
            Cb_u_ACAStatus.BackColor = System.Drawing.Color.Red;
        }
        else
        {
            Cb_u_EmployeeClass.BackColor = System.Drawing.Color.Transparent;
            Cb_u_ACAStatus.BackColor = System.Drawing.Color.Transparent;
        }

        if (Cb_u_ACAStatus.Checked == true)
        {
            validData = errorChecking.validateDropDownSelection(Ddl_u_AcaStatus, validData);
        }

        if (Cb_u_EmployeeClass.Checked == true)
        {
            validData = errorChecking.validateDropDownSelection(Ddl_u_EmployeeClass, validData);
        }

        foreach (GridViewRow row in GvEmployee.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");
            if (cb.Checked == true)
            {
                i += 1;
            }
        }
        if (i == 0)
        {
            validData = false;
        }

        if (validData == true)
        {
            try
            {
                string _acaStatusName = null;
                int _acaStatusID = 0;
                string _className = null;
                int _classID = 0;
                int _employerID = int.Parse(HfDistrictID.Value);
                int _employeeID = 0;
                string _modBy = HfUserName.Value;
                DateTime _modOn = DateTime.Now;
                int z = 0;
                bool allUpdatesPassed = true;
                string allUpdateFailedName = null;

                List<Employee_E> tempList = (List<Employee_E>)Session["EmployeeExport"];

                foreach (GridViewRow row in GvEmployee.Rows)
                {
                    CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");
                    Literal litID = (Literal)row.FindControl("LitEmployeeID");
                    Literal litCName = (Literal)row.FindControl("Lit_gv_ClassName");
                    HiddenField hfClassID = (HiddenField)row.FindControl("HfClassID");
                    Literal litAName = (Literal)row.FindControl("LitAcaName");
                    HiddenField hfAcaID = (HiddenField)row.FindControl("HfAcaID");
                    bool validTransaction = false;

                    if (cb.Checked == true)
                    {
                        z += 1;
                        if (Cb_u_EmployeeClass.Checked == true)
                        {
                            _classID = int.Parse(Ddl_u_EmployeeClass.SelectedItem.Value);
                            _className = Ddl_u_EmployeeClass.SelectedItem.Text;
                        }
                        else
                        {
                            try
                            {
                                _classID = int.Parse(hfClassID.Value);
                                _className = litCName.Text;
                            }
                            catch (Exception exception)
                            {
                                this.Log.Warn("Suppressing errors.", exception);
                                _classID = 0;
                                _className = null;
                            }
                        }

                        if (Cb_u_ACAStatus.Checked == true)
                        {
                            _acaStatusID = int.Parse(Ddl_u_AcaStatus.SelectedItem.Value);
                            _acaStatusName = Ddl_u_AcaStatus.SelectedItem.Text;
                        }
                        else
                        {
                            try
                            {
                                _acaStatusID = int.Parse(hfAcaID.Value);
                                _acaStatusName = litAName.Text;
                            }
                            catch (Exception exception)
                            {
                                this.Log.Warn("Suppressing errors.", exception);
                                _acaStatusID = 0;
                                _acaStatusName = null;
                            }
                        }

                        _employeeID = int.Parse(litID.Text);


                        validTransaction = EmployeeController.UpdateEmployeeClassAcaStatus(_employerID, _employeeID, _classID, _acaStatusID, _modBy, _modOn);

                        if (validTransaction == true)
                        {
                            foreach (Employee_E empe in tempList)
                            {
                                if (empe.EMPLOYEE_ID == _employeeID)
                                {
                                    empe.EMPLOYEE_ACT_STATUS_ID = _acaStatusID;
                                    empe.EX_ACA_NAME = _acaStatusName;
                                    empe.EMPLOYEE_CLASS_ID = _classID;
                                    empe.EX_CLASS_NAME = _className;
                                    break;
                                }
                            }

                            if (Session["Employees"] != null)
                            {
                                List<Employee> tempList2 = (List<Employee>)Session["Employees"];
                                foreach (Employee emp in tempList2)
                                {
                                    if (emp.EMPLOYEE_ID == _employeeID)
                                    {
                                        emp.EMPLOYEE_ACT_STATUS_ID = _acaStatusID;
                                        emp.EMPLOYEE_CLASS_ID = _classID;
                                        break;
                                    }
                                }
                                Session["Employees"] = tempList2;
                            }
                        }
                        else
                        {
                            allUpdateFailedName = "Employee ID: " + litID.Text;
                            allUpdatesPassed = false;
                            break;
                        }
                    }
                }

                Session["EmployerExport"] = tempList;

                Cb_u_ACAStatus.Checked = false;
                Cb_u_EmployeeClass.Checked = false;
                CbCheckAll.Checked = false;
                Ddl_u_AcaStatus.Enabled = false;
                Ddl_u_EmployeeClass.Enabled = false;
                Ddl_u_AcaStatus.SelectedIndex = Ddl_u_AcaStatus.Items.Count - 1;
                Ddl_u_EmployeeClass.SelectedIndex = Ddl_u_EmployeeClass.Items.Count - 1;
                loadEmployees();

                MpeWebMessage.Show();
                if (allUpdatesPassed == true)
                {
                    LitMessage.Text = "All selected records have been updated. ";
                }
                else
                {
                    LitMessage.Text = "An error occurred while trying to mass update your records." + allUpdateFailedName;
                }
            }
            catch (Exception exception)
            {
                this.Log.Warn("Suppressing errors.", exception);
                MpeWebMessage.Show();
                LitMessage.Text = "An error occurred while trying to update the selected records. Please contact " + Branding.CompanyShortName + " if this issue continues. ";
                loadEmployees();
            }
        }
        else
        {
            MpeWebMessage.Show();
            LitMessage.Text = "Please correct all of the fields highlighted in RED. Check the Employee List to make sure that atleast one record is selected.";
        }

    }

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        string recordsRemoved = "Employees Removed:<br />";
        string recordsNotRemoved = "Employees who could not be DELETED:<br />";

        foreach (GridViewRow row in GvEmployee.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");
            Literal litID = (Literal)row.FindControl("LitEmployeeID");
            bool validTransaction = false;

            if (cb.Checked == true)
            {
                int _employeeID = int.Parse(litID.Text);
                validTransaction = EmployeeController.DeleteEmployee(_employerID, _employeeID);
                if (validTransaction == true)
                {
                    List<Employee_E> tempList = (List<Employee_E>)Session["EmployeeExport"];
                    Employee_E tempE = null;

                    foreach (Employee_E empe in tempList)
                    {
                        if (empe.EMPLOYEE_ID == _employeeID)
                        {
                            tempE = empe;
                            break;
                        }
                    }

                    tempList.Remove(tempE);
                    Session["EmployeeExport"] = tempList;

                    if (Session["Employees"] != null)
                    {
                        List<Employee> tempList2 = (List<Employee>)Session["Employees"];
                        Employee temp = EmployeeController.findEmployee(tempList2, _employeeID);
                        tempList2.Remove(temp);

                        Session["Employees"] = tempList2;
                    }
                    recordsRemoved += _employeeID.ToString() + "<br />";
                }
                else
                {
                    recordsNotRemoved += _employeeID.ToString() + "<br />";
                }
            }
        }

        loadEmployees();

        MpeWebMessage.Show();
        LitMessage.Text = recordsRemoved + "<br />" + recordsNotRemoved;
    }

    protected void Ddl_f_PayDesc_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool validData = true;
        int _grossPayID = 0;
        validData = errorChecking.validateDropDownSelection(Ddl_f_PayDesc, validData);

        if (validData == true)
        {
            _grossPayID = int.Parse(Ddl_f_PayDesc.SelectedItem.Value);
            errorChecking.setDropDownList(Ddl_f_PayID, _grossPayID);
        }
        else
        {
            Ddl_f_PayID.SelectedIndex = Ddl_f_PayID.Items.Count - 1;
        }
    }

    protected void Ddl_f_PayID_SelectedIndexChanged(object sender, EventArgs e)
    {

        bool validData = true;
        int _grossPayID = 0;
        validData = errorChecking.validateDropDownSelection(Ddl_f_PayID, validData);

        if (validData == true)
        {
            _grossPayID = int.Parse(Ddl_f_PayID.SelectedItem.Value);
            errorChecking.setDropDownList(Ddl_f_PayDesc, _grossPayID);
        }
        else
        {
            Ddl_f_PayDesc.SelectedIndex = Ddl_f_PayDesc.Items.Count - 1;
        }

    }

    private ILog Log = LogManager.GetLogger(typeof(securepages_export_employees));

}