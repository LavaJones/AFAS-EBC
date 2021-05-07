using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Afas.AfComply.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EditEmployees : AdminPageBase
    {

        private const string errorMsg = "Failed to update Employee";
        
        private const string successMsg = "Succesfully Saved!";
        private ILog Log = LogManager.GetLogger(typeof(EditEmployer));
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the EditEmployer page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=13", false);
            }
            else
            {
                loadStates();
                loadACAstatuses();
                loadEmployers();
                loadEmployeeTypes();
            }
        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            int employerId = 0;
            Session["SelectedEmployerID"] = null;
            Session["SelectedEditEmployee"] = null;
            //check that data is correct
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
            Session["SelectedEmployerID"] = employerId;
            getEmployerAndEmployees(employerId);
           
            clearFields();
        }

        private void getEmployerAndEmployees(int employerId)
        {
            employer currEmployer = employerController.getEmployer(employerId);
            loadEmployees(employerId);
            loadEmployeeTypes(employerId);
            loadHRStatuses(employerId);
            loadEmployeeClassifications(employerId);
        }
        protected void DdlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SelectedEditEmployee"] = null;

            if (DdlEmployee.SelectedIndex > 0)
            {
                int SelectedEmployeeID = Convert.ToInt32(DdlEmployee.SelectedItem.Value);
                Session["SelectedEditEmployee"] = SelectedEmployeeID;

                setFields(SelectedEmployeeID);

            }
            else
            {
                clearFields();
            }

            lblMsg.Text = String.Empty;
        }


        protected void BtnFindEmployees_Click(object sender, EventArgs e)
        {
            if (TxtEmployee.Text == String.Empty)
                return;

            Session["FilteredEditEmployees"] = null;
            var employees = (List<Employee>)Session["EditEmployees"];
            var filteredEmployees = employees.Where(emp => emp.EMPLOYEE_FIRST_NAME.ToLower().Contains(TxtEmployee.Text.ToLower()) || emp.EMPLOYEE_LAST_NAME.ToLower().Contains(TxtEmployee.Text.ToLower())).ToList();
            Session["FilteredEditEmployees"] = filteredEmployees;
            Session["DdlEmployeeDataSource"] = filteredEmployees;
            formatDdl(DdlEmployee, filteredEmployees, "EMPLOYEE_FULL_NAME", "EMPLOYEE_ID");
            lblMsg.Text = String.Empty;
            if (filteredEmployees.Count > 0)
            {
                DdlEmployee.SelectedIndex = 1;
                setFields(int.Parse(DdlEmployee.SelectedValue));
            }
            else
            {
                clearFields();
            }
        }

        protected void BtnResetEmployees_Click(object sender, EventArgs e)
        {
            clearFields();
            loadEmployees(int.Parse(DdlEmployer.SelectedItem.Value));
            TxtEmployee.Text = String.Empty;
            lblMsg.Text = String.Empty;
            DdlEmployee.SelectedIndex = 0;
        }
   
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Log.Info("Save button has been clicked");

                var result = updateEmployee();
                showLblMsg(result);
                getEmployerAndEmployees((int)Session["SelectedEmployerID"]);
                setFields((int)Session["SelectedEditEmployee"]);
            }
            catch (Exception ex)
            {
                Log.Error(errorMsg, ex);
                showLblMsg(false);
            }
            
        }


        private void showLblMsg(bool result)
        {
            lblMsg.ForeColor = System.Drawing.Color.Black;

            if (result)
            {
                lblMsg.Text = successMsg;
                lblMsg.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                lblMsg.Text = errorMsg;
                lblMsg.BackColor = System.Drawing.Color.Red;
            }
        }


        private Boolean updateEmployee()
        {
            this.Log.Info("entered updateEmployee");
            User currUser = (User)Session["CurrentUser"];
            string _username = currUser.User_UserName.ToLower();
            List<Employee> employees = (List<Employee>)Session["DdlEmployeeDataSource"];
            Employee selectedEmployee = employees.First(i => i.EMPLOYEE_ID == (int)Session["SelectedEditEmployee"]);
            this.Log.Info(string.Format("The Selected Employee ID is {0}", selectedEmployee.EMPLOYEE_ID));

            int employeeTypeID = int.Parse(DdlType.SelectedValue);
            int hrStatusID = int.Parse(DdlHireStatus.SelectedValue);
            string fName = TxtFirstName.Text;
            string mName = TxtMiddleName.Text;
            string lName = TxtLastName.Text;
            string address = TxtAddress.Text;
            string city = TxtCity.Text;
            int stateID = int.Parse(DdlState.SelectedValue);
            string zip = TxtZip.Text;
            DateTime? hireDate = TryParseNullableDateTime(TxtHireDate.Text);
            DateTime? currDate = TryParseNullableDateTime(TxtCurrDate.Text);
            string ssn = TxtSSN.Text;
            string extID = TxtExtID.Text;
            DateTime? termDate = TryParseNullableDateTime(TxtTermDate.Text);
            DateTime? dob = TryParseNullableDateTime(TxtDOB.Text);
            DateTime? impEnd = TryParseNullableDateTime(TxtImpEnd.Text);
            int classID = int.Parse(DdlClassification.SelectedValue);
            int acaStatusID = int.Parse(DdlAcaStatus.SelectedValue);

            if (DdlEmployee.SelectedIndex > 0)
            {
                var employee = new Employee(
                    selectedEmployee.EMPLOYEE_ID,
                    employeeTypeID,
                    hrStatusID,
                    1,
                    fName,
                    mName,
                    lName,
                    address,
                    city,
                    stateID,
                    zip,
                    hireDate,
                    currDate,
                    ssn,
                    extID,
                    termDate,
                    dob,
                    impEnd,
                    selectedEmployee.EMPLOYEE_PLAN_YEAR_ID,
                    selectedEmployee.EMPLOYEE_PLAN_YEAR_ID_LIMBO,
                    selectedEmployee.EMPLOYEE_PLAN_YEAR_ID_MEAS,
                    selectedEmployee.EMPLOYEE_PY_AVG_MEAS_HOURS,
                    selectedEmployee.EMPLOYEE_PY_AVG_ADMIN_HOURS,
                    selectedEmployee.EMPLOYEE_PY_AVG_MEAS_HOURS,
                    selectedEmployee.EMPLOYEE_PY_AVG_INIT_HOURS,
                    classID,
                    acaStatusID);

                return EmployeeController.UpdateEmployee(employee, _username);

            }

            return false;
        }

        private void setFields(int SelectedEmployeeID)
        {
            var index = DdlEmployee.SelectedIndex - 1;
            List<Employee> employees = (List<Employee>)Session["DdlEmployeeDataSource"];
            Employee employee = employees.First(i => i.EMPLOYEE_ID == SelectedEmployeeID);
            //Employee employeee = employees.ElementAt(index);
                       
            TxtFirstName.Text = employee.EMPLOYEE_FIRST_NAME;
            TxtMiddleName.Text = employee.EMPLOYEE_MIDDLE_NAME;
            TxtLastName.Text = employee.EMPLOYEE_LAST_NAME;
            TxtAddress.Text = employee.EMPLOYEE_ADDRESS;
            TxtCity.Text = employee.EMPLOYEE_CITY;
            TxtZip.Text = employee.EMPLOYEE_ZIP;
            TxtSSN.Text = employee.Employee_SSN_Visible;
            TxtExtID.Text = employee.EMPLOYEE_EXT_ID;
           
            List<Dependent> dependants=EmployeeController.manufactureEmployeeDependentList(employee.EMPLOYEE_ID);
            if(dependants.Count>0)
            {
                LbViewDependents.Visible = true;
                Session["Dependants"] = dependants;
            }
            else
            {
                LbViewDependents.Visible = false;
                Session["Dependants"] = null;

            }
            

            if (employee.EMPLOYEE_DOB.HasValue)
                TxtDOB.Text = employee.EMPLOYEE_DOB.Value.ToString("MM/dd/yyyy");
            else
                TxtDOB.Text = String.Empty;

            if (employee.EMPLOYEE_HIRE_DATE.HasValue)
                TxtHireDate.Text = employee.EMPLOYEE_HIRE_DATE.Value.ToString("MM/dd/yyyy");
            else
                TxtHireDate.Text = String.Empty;

            if (employee.EMPLOYEE_C_DATE.HasValue)
                TxtCurrDate.Text = employee.EMPLOYEE_C_DATE.Value.ToString("MM/dd/yyyy");
            else
                TxtCurrDate.Text = String.Empty;

            if (employee.EMPLOYEE_TERM_DATE.HasValue)
                TxtTermDate.Text = employee.EMPLOYEE_TERM_DATE.Value.ToString("MM/dd/yyyy");
            else
                TxtTermDate.Text = String.Empty;

            if (employee.EMPLOYEE_IMP_END.HasValue)
                TxtImpEnd.Text = employee.EMPLOYEE_IMP_END.Value.ToString("MM/dd/yyyy");
            else
                TxtImpEnd.Text = String.Empty;


            if (DdlState.Items.FindByValue(employee.EMPLOYEE_STATE_ID.ToString()) == null)
                DdlState.SelectedIndex = 0;
            else
                DdlState.SelectedValue = employee.EMPLOYEE_STATE_ID.ToString();

            if (DdlType.Items.FindByValue(employee.EMPLOYEE_TYPE_ID.ToString()) == null)
                DdlType.SelectedIndex = 0;
            else
                DdlType.SelectedValue = employee.EMPLOYEE_TYPE_ID.ToString();

            if (DdlHireStatus.Items.FindByValue(employee.EMPLOYEE_HR_STATUS_ID.ToString()) == null)
                DdlHireStatus.SelectedIndex = 0;
            else
                DdlHireStatus.SelectedValue = employee.EMPLOYEE_HR_STATUS_ID.ToString();

            if (DdlClassification.Items.FindByValue(employee.EMPLOYEE_CLASS_ID.ToString()) == null)
                DdlClassification.SelectedIndex = 0;
            else
                DdlClassification.SelectedValue = employee.EMPLOYEE_CLASS_ID.ToString();

            if (DdlAcaStatus.Items.FindByValue(employee.EMPLOYEE_ACT_STATUS_ID.ToString()) == null)
                DdlAcaStatus.SelectedIndex = 0;
            else
                DdlAcaStatus.SelectedValue = employee.EMPLOYEE_ACT_STATUS_ID.ToString();

            toggleBackgroundColorDdl(DdlState);
            toggleBackgroundColorDdl(DdlType);
            toggleBackgroundColorDdl(DdlHireStatus);
            toggleBackgroundColorDdl(DdlClassification);
            toggleBackgroundColorDdl(DdlAcaStatus);
           
            
        }

        private void clearFields()
        {
            TxtFirstName.Text = String.Empty;
            TxtMiddleName.Text = String.Empty;
            TxtLastName.Text = String.Empty;
            TxtAddress.Text = String.Empty;
            TxtCity.Text = String.Empty;
            TxtZip.Text = String.Empty;
            TxtSSN.Text = String.Empty;
            TxtExtID.Text = String.Empty;

            TxtDOB.Text = String.Empty;
            TxtHireDate.Text = String.Empty;
            TxtCurrDate.Text = String.Empty;
            TxtTermDate.Text = String.Empty;
            TxtImpEnd.Text = String.Empty;

            DdlState.SelectedIndex = 0;
            DdlType.SelectedIndex = 0;
            DdlHireStatus.SelectedIndex = 0;
            DdlClassification.SelectedIndex = 0;
            DdlAcaStatus.SelectedIndex = 0;
        }

        private void toggleBackgroundColorDdl(DropDownList Ddl)
        {
            if (Ddl.SelectedIndex == 0)
            {
                Ddl.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                Ddl.BackColor = System.Drawing.Color.White;
            }
        }
        
        private void loadEmployees(int employerId)
        {
            List<Employee> employees=null;
            Session["EditEmployees"] = null;
            Session["FilteredEditEmployees"] = null;
            employees = EmployeeController.manufactureEmployeeList(employerId).OrderBy(e => e.EMPLOYEE_FULL_NAME).ToList();
            Session["EditEmployees"] = employees;
            Session["Employees"] = employees;
            Session["DdlEmployeeDataSource"] = employees;
            DdlEmployee.DataSource = "";

            DdlEmployee.DataSource = employees;
            DdlEmployee.DataTextField = "EMPLOYEE_FULL_NAME_ExtID";
            DdlEmployee.DataValueField = "EMPLOYEE_ID";
            DdlEmployee.DataBind();
            DdlEmployee.Items.Insert(0, "Select");
            //formatDdl(DdlEmployee, employees, "EMPLOYEE_FULL_NAME_ExtID", "EMPLOYEE_ID");
            if (Session["SelectedEditEmployee"] != null)
            {
                DdlEmployee.SelectedValue = ((int)Session["SelectedEditEmployee"]).ToString();    
            } 
            else
            {
                
                DdlEmployee.SelectedIndex = 0;
            }
           
            
        }

        protected void LbViewDependents_Click(object sender, EventArgs e)
        {
            MpeDependents.Show();
            List<Dependent> dependants = (List<Dependent>)Session["Dependants"];
            List<Employee> employees = (List<Employee>)Session["DdlEmployeeDataSource"];
            Employee employee = employees.First(i => i.EMPLOYEE_ID == (int)Session["SelectedEditEmployee"]);
            PnlDependents.Visible = true;
            GvDependents.DataSource = dependants;
            GvDependents.DataBind();
            Lit_dep_employeeName.Text = employee.EMPLOYEE_FULL_NAME;
        }

        protected void GvDependents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            List<Employee> employees = (List<Employee>)Session["DdlEmployeeDataSource"];
            Employee employee = employees.First(i => i.EMPLOYEE_ID == (int)Session["SelectedEditEmployee"]);
            GvDependents.EditIndex = -1;
            GvDependents.DataSource = EmployeeController.manufactureEmployeeDependentList(employee.EMPLOYEE_ID);
            GvDependents.DataBind();
            MpeDependents.Show();
        }

        protected void GvDependents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            List<Employee> employees = (List<Employee>)Session["DdlEmployeeDataSource"];
            Employee employee = employees.First(i => i.EMPLOYEE_ID == (int)Session["SelectedEditEmployee"]);
            GvDependents.EditIndex = e.NewEditIndex;
            GvDependents.DataSource = EmployeeController.manufactureEmployeeDependentList(employee.EMPLOYEE_ID);
            GvDependents.DataBind();
            MpeDependents.Show();
        }

        protected void GvDependents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                List<Employee> employees = (List<Employee>)Session["DdlEmployeeDataSource"];
                Employee employee = employees.First(i => i.EMPLOYEE_ID == (int)Session["SelectedEditEmployee"]);
                GridViewRow row = (GridViewRow)GvDependents.Rows[e.RowIndex];
                HiddenField hf = (HiddenField)row.FindControl("Hf_dep_id");
                int _dependentID = int.Parse(hf.Value);
               
                bool validData = true;

                validData = EmployeeController.DeleteEmployeeDependent(_dependentID, employee.EMPLOYEE_ID);

                if (validData == true)
                {
                    Lit_dep_message.Text = "The record has been DELETED.";
                    GvDependents.EditIndex = -1;
                    GvDependents.DataSource = EmployeeController.manufactureEmployeeDependentList(employee.EMPLOYEE_ID);
                    GvDependents.DataBind();
                    MpeDependents.Show();
                }
                else
                {
                    Lit_dep_message.Text = "This Dependent is not able to be DELETED. It is tied to other records in the System.";
                    MpeDependents.Show();
                }
            }
            catch (Exception exception)
            {

                this.Log.Warn("Suppressing errors.", exception);

                Lit_dep_message.Text = "An error occurred while trying to DELETE this record. Please contact " + Branding.CompanyShortName + " if this problem continues.";
                MpeDependents.Show();
            }
        }

        protected void GvDependents_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Literal LitUser = (Literal)this.Master.FindControl("LitUserName");
                List<Employee> employees = (List<Employee>)Session["DdlEmployeeDataSource"];
                Employee employee = employees.First(i => i.EMPLOYEE_ID == (int)Session["SelectedEditEmployee"]);
                GridViewRow row = (GridViewRow)GvDependents.Rows[e.RowIndex];
                HiddenField hf = (HiddenField)row.FindControl("Hf_dep_id2");
                TextBox txtFname = (TextBox)row.FindControl("Txt_dep_Fname");
                TextBox txtMname = (TextBox)row.FindControl("Txt_dep_Mname");
                TextBox txtLname = (TextBox)row.FindControl("Txt_dep_Lname");
                TextBox txtSSN = (TextBox)row.FindControl("Txt_dep_SSN");
                TextBox txtDOB = (TextBox)row.FindControl("Txt_dep_DOB");
                int _dependentID = int.Parse(hf.Value);
                int _employeeID = employee.EMPLOYEE_ID;
                bool validData = true;
               

                string _fname = null;
                string _mname = null;
                string _lname = null;
                string _ssn = null;
                string dob = null;
                DateTime? _dob = null;
                string modBy = LitUser.Text;

                validData = errorChecking.validateTextBoxNull(txtFname, validData);
                validData = errorChecking.validateTextBoxNull(txtLname, validData);

                if (txtSSN.Text.Length > 0)
                {
                    validData = errorChecking.validateTextBoxSSN(txtSSN, validData);
                    _ssn = txtSSN.Text.Trim(new char[] { ' ', '"' });
                }
                else
                {
                    txtSSN.BackColor = System.Drawing.Color.White;
                }

                if (txtDOB.Text.Length > 0)
                {
                    validData = errorChecking.validateTextBoxDate(txtDOB, validData);
                }
                else
                {
                    txtDOB.BackColor = System.Drawing.Color.White;
                }

                if (validData == true)
                {
                    //Collect Information from UI.
                    _fname = txtFname.Text;
                    _mname = txtMname.Text;
                    _lname = txtLname.Text;

                    dob = txtDOB.Text;
                    try
                    {
                        _dob = DateTime.Parse(dob);
                    }
                    catch (Exception exception)
                    {

                        this.Log.Warn("Suppressing errors.", exception);

                        _dob = null;

                    }
                    //Attempt to update the database. 
                    Dependent currDependent = EmployeeController.updateEmployeeDependent(_dependentID, _employeeID, _fname, _mname, _lname, _ssn, _dob, modBy, 1);

                    if (currDependent != null)
                    {
                        Lit_dep_message.Text = "The record has been UPDATED.";
                        GvDependents.EditIndex = -1;
                        GvDependents.DataSource = EmployeeController.manufactureEmployeeDependentList(_employeeID);
                        GvDependents.DataBind();
                        MpeDependents.Show();
                    }
                    else
                    {
                        Lit_dep_message.Text = "An error occurred while trying to UPDATE this record. Please try again and if the problem continues, contact " + Branding.CompanyShortName;
                        MpeDependents.Show();
                    }
                }
                else
                {
                    Lit_dep_message.Text = "Please correct any fields highlighted in RED.";
                    MpeDependents.Show();
                }
            }
            catch (Exception exception)
            {

                this.Log.Warn("Suppressing errors.", exception);

                Lit_dep_message.Text = "An error occurred while validating data for this record. Please try again and if the problem continues, contact " + Branding.CompanyShortName;
                MpeDependents.Show();
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

        private void loadEmployeeTypes(int employerId)
        {
            var employeeTypes = EmployeeTypeController.getEmployeeTypes(employerId);
            formatDdl(DdlType, employeeTypes, "EMPLOYEE_TYPE_NAME", "EMPLOYEE_TYPE_ID");
        }

        private void loadHRStatuses(int employerId)
        {
            var hrStatuses = hrStatus_Controller.manufactureHRStatusList(employerId);
            formatDdl(DdlHireStatus, hrStatuses, "HR_STATUS_NAME", "HR_STATUS_ID");
        }

        private void loadEmployeeClassifications(int employerId)
        {
            var classifications = classificationController.ManufactureEmployerClassificationList(employerId, false);
            formatDdl(DdlClassification, classifications, "CLASS_DESC", "CLASS_ID");
        }

        private void loadACAstatuses()
        {
            var acaStatuses = classificationController.getACAstatusList();
            formatDdl(DdlAcaStatus, acaStatuses, "ACA_STATUS_NAME", "ACA_STATUS_ID");
        }

        private void loadEmployeeTypes()
        {
            var employeeTypes = EmployeeTypeController.getEmployeeTypes(6);
            formatDdl(DdlType, employeeTypes, "EMPLOYEE_TYPE_NAME", "EMPLOYEE_TYPE_ID");
        }
        private void loadStates()
        {
            DdlState.DataSource = StateController.getStates();
            DdlState.DataTextField = "State_Name";
            DdlState.DataValueField = "State_ID";
            DdlState.DataBind();
        }

        private void formatDdl(DropDownList Ddl, Object DataSource, String DataTextField, String DataValueField)
        {
            Ddl.ClearSelection();
            Ddl.DataSource = DataSource;
            Ddl.DataTextField = DataTextField;
            Ddl.DataValueField = DataValueField;
            Ddl.DataBind();

            Ddl.Items.Insert(0, "Select");
            Ddl.SelectedIndex = 0;
        }


        protected void DdlAcaStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            toggleBackgroundColorDdl(DdlAcaStatus);
            if (DdlAcaStatus.SelectedItem.Text == "Termed")
            {
                mpAcaStatus.Show();
            }

        }

        private DateTime? TryParseNullableDateTime(string text)
        {
            DateTime date;
            if (DateTime.TryParse(text, out date))
            {
                return date;
            }
            else
            {
                return null;
            }
        }

    }
}