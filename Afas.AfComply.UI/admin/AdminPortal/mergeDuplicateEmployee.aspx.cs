using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using log4net;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class mergeDuplicateEmployee : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(admin_admin_float_user));

        /// <summary>
        /// Page load function. 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="employer"></param>
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
                loadEmployers();
                LnkBtnPayrollView.BackColor = System.Drawing.ColorTranslator.FromHtml("#33CCFF");   //Set the default tab color.
        }

    #region Search Functions
        protected void BtnSearchEmployer_Click(object sender, EventArgs e)
        {
            loadEmployers();
            resetLeftEmployeeControls();
            resetRightEmployeeControls();
            resetLeftViewControls();
            resetRightViewControls();
        }

        protected void BtnSearchEmployeeLeft_Click(object sender, EventArgs e)
        {
            loadEmployeesRightAndLeft();
            resetLeftViewControls();
        }

        protected void BtnSearchEmployeeRight_Click(object sender, EventArgs e)
        {
            loadEmployeesRightOnly();
            resetRightViewControls();
        }
        #endregion

    #region drop down list functions
        protected void DdlEmployeeLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadPayrollDataLeft();
            loadDependentsLeft();
            loadInsuranceOfferLeft();
            loadInsuranceCarrierLeft();
            loadInsuranceChangeEventsLeft();
            loadInsuranceCarrierDependentLeft();
            loadInsuranceCarrierEditableLeft();
        }

        protected void DdlEmployeeRight_SelectedIndexChanged(object sender, EventArgs e)
        {
            LitEmployeeName.Text = DdlEmployeeRight.SelectedItem.Text;
            loadPayrollDataRight();
            loadDependentsRight();
            loadInsuranceOfferRight();
            loadInsuranceCarrierRight();
            loadInsuranceChangeEventsRight();
            loadInsuranceCarrierDependentRight();
            loadInsuranceCarrierEditableRight();
        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetLeftEmployeeControls();
            resetRightEmployeeControls();
            resetLeftViewControls();
            resetRightViewControls();
            loadEmployeesRightAndLeft();
        }

    #endregion

    #region Load Data
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

            DdlEmployer.Items.Add("Select Employer");
            DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
        }

        private void loadEmployeesRightAndLeft()
        {
            int employerID = 0;
            string searchTextLeft = null;
            string searchTextRight = null;
            bool validData = true;
            bool validLeftSearch = true;
            bool validRightSearch = true;

            List<Employee> filteredEmployeesLeft = new List<Employee>();
            List<Employee> filteredEmployeesRight = new List<Employee>();

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validLeftSearch = errorChecking.validateTextBoxNull(TxtSearchEmployeeNameLeft, validLeftSearch);
            TxtSearchEmployeeNameLeft.BackColor = System.Drawing.Color.White;
            validRightSearch = errorChecking.validateTextBoxNull(TxtSearchEmployeeNameRight, validRightSearch);
            TxtSearchEmployeeNameRight.BackColor = System.Drawing.Color.White;

            if (validData == true)
            {
                int.TryParse(DdlEmployer.SelectedItem.Value, out employerID);
                List<Employee> currEmployees = EmployeeController.manufactureEmployeeList(employerID);

                if (validLeftSearch == true)
                {
                    searchTextLeft = TxtSearchEmployeeNameLeft.Text.ToLower();

                    foreach (Employee emp in currEmployees)
                    {
                        if (emp.EMPLOYEE_LAST_NAME.ToLower().Contains(searchTextLeft) == true)
                        {
                            filteredEmployeesLeft.Add(emp);
                        }
                    }
                }
                else { filteredEmployeesLeft = currEmployees; }

                if (validRightSearch == true)
                {
                    searchTextRight = TxtSearchEmployeeNameRight.Text.ToLower();

                    foreach (Employee emp in currEmployees)
                    {
                        if (emp.EMPLOYEE_LAST_NAME.ToLower().Contains(searchTextRight) == true)
                        {
                            filteredEmployeesRight.Add(emp);
                        }
                    }
                }
                else { filteredEmployeesRight = filteredEmployeesLeft; }
            }

            //Load the Left Employee Dropdown List.
            DdlEmployeeLeft.DataSource = filteredEmployeesLeft;
            DdlEmployeeLeft.DataTextField = "EMPLOYEE_FULL_NAME";
            DdlEmployeeLeft.DataValueField = "EMPLOYEE_ID";
            DdlEmployeeLeft.DataBind();
            DdlEmployeeLeft.Items.Add("Select Employee");
            DdlEmployeeLeft.SelectedIndex = DdlEmployeeLeft.Items.Count - 1;

            //Load the Right Employee Dropdown List
            DdlEmployeeRight.DataSource = filteredEmployeesRight;
            DdlEmployeeRight.DataTextField = "EMPLOYEE_FULL_NAME";
            DdlEmployeeRight.DataValueField = "EMPLOYEE_ID";
            DdlEmployeeRight.DataBind();
            DdlEmployeeRight.Items.Add("Select Employee");
            DdlEmployeeRight.SelectedIndex = DdlEmployeeRight.Items.Count - 1;
        }

        private void loadEmployeesRightOnly()
        {
            int employerID = 0;
            string searchTextRight = null;
            bool validData = true;
            bool validRightSearch = true;

            List<Employee> filteredEmployeesRight = new List<Employee>();

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validRightSearch = errorChecking.validateTextBoxNull(TxtSearchEmployeeNameRight, validRightSearch);
            TxtSearchEmployeeNameRight.BackColor = System.Drawing.Color.White;

            if (validData == true)
            {
                int.TryParse(DdlEmployer.SelectedItem.Value, out employerID);
                List<Employee> currEmployees = EmployeeController.manufactureEmployeeList(employerID);

                if (validRightSearch == true)
                {
                    searchTextRight = TxtSearchEmployeeNameRight.Text.ToLower();

                    foreach (Employee emp in currEmployees)
                    {
                        if (emp.EMPLOYEE_LAST_NAME.ToLower().Contains(searchTextRight) == true)
                        {
                            filteredEmployeesRight.Add(emp);
                        }
                    }
                }
                else { filteredEmployeesRight = currEmployees; }
            }

            DdlEmployeeRight.DataSource = filteredEmployeesRight;
            DdlEmployeeRight.DataTextField = "EMPLOYEE_FULL_NAME";
            DdlEmployeeRight.DataValueField = "EMPLOYEE_ID";
            DdlEmployeeRight.DataBind();
            DdlEmployeeRight.Items.Add("Select Employee");
            DdlEmployeeRight.SelectedIndex = DdlEmployeeRight.Items.Count - 1;
        }

        private void resetLeftViewControls()
        {
            List<Payroll> blankPay = new List<Payroll>();
            List<insurance_coverage> blankIO = new List<insurance_coverage>();
            List<Dependent> blankD = new List<Dependent>();
            List<alert_insurance> blankAI = new List<alert_insurance>();

            GvPayrollCorrect.DataSource = blankPay;
            GvPayrollCorrect.DataBind();

            GvInsuranceOfferCorrect.DataSource = blankAI;
            GvInsuranceOfferCorrect.DataBind();

            GvInsuranceCarrierDataCorrect.DataSource = blankIO;
            GvInsuranceCarrierDataCorrect.DataBind();

            GvInsuranceCarrierDataDependentCorrect.DataSource = blankIO;
            GvInsuranceCarrierDataDependentCorrect.DataBind();

            GvInsuranceCarrierDataEditableCorrect.DataSource = blankIO;
            GvInsuranceCarrierDataEditableCorrect.DataBind();

            GvDependentsCorrect.DataSource = blankD;
            GvDependentsCorrect.DataBind();

            DdlInsuranceCarrierDependentsCorrect.Items.Clear();
        }

        private void resetRightViewControls()
        {
            List<Payroll> blankPay = new List<Payroll>();
            List<insurance_coverage> blankIO = new List<insurance_coverage>();
            List<Dependent> blankD = new List<Dependent>();
            List<alert_insurance> blankAI = new List<alert_insurance>();

            GvPayrollIncorrect.DataSource = blankPay;
            GvPayrollIncorrect.DataBind();

            GvInsuranceOfferInCorrect.DataSource = blankAI;
            GvInsuranceOfferInCorrect.DataBind();

            GvInsuranceCarrierDataInCorrect.DataSource = blankIO;
            GvInsuranceCarrierDataInCorrect.DataBind();

            GvInsuranceCarrierDataDependentInCorrect.DataSource = blankIO;
            GvInsuranceCarrierDataDependentInCorrect.DataBind();

            GvInsuranceCarrierDataEditableInCorrect.DataSource = blankIO;
            GvInsuranceCarrierDataEditableInCorrect.DataBind();

            GvDependentsInCorrect.DataSource = blankD;
            GvDependentsInCorrect.DataBind();

            DdlInsuranceCarrierDependentsInCorrect.Items.Clear();
        }

        private void resetLeftEmployeeControls()
        {
            TxtSearchEmployeeNameLeft.Text = null;
            DdlEmployeeLeft.Items.Clear();
        }

        private void resetRightEmployeeControls()
        {
            TxtSearchEmployeeNameRight.Text = null;
            DdlEmployeeRight.Items.Clear();
        }
    #endregion

    #region Load Employee Specific Data Functions
        /// <summary>
        /// Load the payroll data on the RIGHT side of the screen for the employee selected.
        /// </summary>
        private void loadPayrollDataLeft()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeLeft, validData);

            if (validData == true)
            {
                DateTime sdate = DateTime.Now.AddYears(-50); //Set the start date to 50 years ago. 
                DateTime edate = DateTime.Now;               //Set the end date to today. 
                int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out employeeID);
                List<Payroll> employeePayroll = Payroll_Controller.getEmployeePayroll(employeeID, sdate, edate);

                LitPayrollCorrectCount.Text = employeePayroll.Count.ToString();
                GvPayrollCorrect.DataSource = employeePayroll;
                GvPayrollCorrect.DataBind();
            }
        }

        /// <summary>
        /// Load the payroll data on the LEFT side of the screen for the employee selected.
        /// </summary>
        private void loadPayrollDataRight()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeRight, validData);

            if (validData == true)
            {
                DateTime sdate = DateTime.Now.AddYears(-50); //Set the start date to 50 years ago. 
                DateTime edate = DateTime.Now;               //Set the end date to today. 
                int.TryParse(DdlEmployeeRight.SelectedItem.Value, out employeeID);
                List<Payroll> employeePayroll = Payroll_Controller.getEmployeePayroll(employeeID, sdate, edate);

                LitPayrollInCorrectCount.Text = employeePayroll.Count.ToString();
                GvPayrollIncorrect.DataSource = employeePayroll;
                GvPayrollIncorrect.DataBind();
            }
        }

        /// <summary>
        /// Load the Insurance Offer of the LEFT side of the screen for the employee selected.
        /// </summary>
        private void loadInsuranceOfferLeft()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeLeft, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out employeeID);
                List<alert_insurance> employeeInsuranceOffers = insuranceController.findEmployeeInsuranceOffers(employeeID);

                LitInsuranceOfferCorrectCount.Text = employeeInsuranceOffers.Count.ToString();
                GvInsuranceOfferCorrect.DataSource = employeeInsuranceOffers;
                GvInsuranceOfferCorrect.DataBind();
            }
        }

        /// <summary>
        /// Load the Insurance Offer on the RIGHT side of the screen for the employee selected.
        /// </summary>
        private void loadInsuranceOfferRight()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeRight, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployeeRight.SelectedItem.Value, out employeeID);
                List<alert_insurance> employeeInsuranceOffers = insuranceController.findEmployeeInsuranceOffers(employeeID);

                LitInsuranceOfferInCorrectCount.Text = employeeInsuranceOffers.Count.ToString();
                GvInsuranceOfferInCorrect.DataSource = employeeInsuranceOffers;
                GvInsuranceOfferInCorrect.DataBind();
            }
        }

        /// <summary>
        /// Load the Insurance Carrier Data on the LEFT side of the screen for the employee selected. 
        /// </summary>
        private void loadInsuranceCarrierLeft()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeLeft, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out employeeID);
                List<insurance_coverage> employeeInsuranceCoverage = insuranceController.manufactureAllInsuranceCoverageForEmployee(employeeID);
                GvInsuranceCarrierDataCorrect.DataSource = employeeInsuranceCoverage;
                GvInsuranceCarrierDataCorrect.DataBind();
                LitInsuranceCarrierRecordCountCorrect.Text = (employeeInsuranceCoverage.Count == 0) ? "0" : employeeInsuranceCoverage.Count.ToString();
            }
        }

        /// <summary>
        /// Load the Insurance Carrier Data on the RIGHT side of the screen for the employee selected.
        /// </summary>
        private void loadInsuranceCarrierRight()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeRight, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployeeRight.SelectedItem.Value, out employeeID);
                List<insurance_coverage> employeeInsuranceCoverage = insuranceController.manufactureAllInsuranceCoverageForEmployee(employeeID);
                GvInsuranceCarrierDataInCorrect.DataSource = employeeInsuranceCoverage;
                GvInsuranceCarrierDataInCorrect.DataBind();
                LitInsuranceCarrierRecordCountInCorrect.Text = (employeeInsuranceCoverage.Count == 0) ? "0" : employeeInsuranceCoverage.Count.ToString();
            }
        }

        /// <summary>
        /// Load the Insurance Carrier Data on the LEFT side of the screen for the employee's dependents.
        /// </summary>
        private void loadInsuranceCarrierDependentLeft()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeLeft, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out employeeID);
                List<insurance_coverage> employeeInsuranceCoverage = insuranceController.manufactureAllInsuranceCoverageForEmployeeDependents(employeeID);
                LitInsuranceCarrierDependentCorrectCount.Text = employeeInsuranceCoverage.Count.ToString();
                GvInsuranceCarrierDataDependentCorrect.DataSource = employeeInsuranceCoverage;
                GvInsuranceCarrierDataDependentCorrect.DataBind();
            }
        }

        /// <summary>
        /// Load the Insurance Carrier Data on the RIGHT side of the screen for the employee's dependents.
        /// </summary>
        private void loadInsuranceCarrierDependentRight()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeLeft, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployeeRight.SelectedItem.Value, out employeeID);
                List<insurance_coverage> employeeInsuranceCoverage = insuranceController.manufactureAllInsuranceCoverageForEmployeeDependents(employeeID);
                LitInsuranceCarrierDependentInCorrectCount.Text = employeeInsuranceCoverage.Count.ToString();
                GvInsuranceCarrierDataDependentInCorrect.DataSource = employeeInsuranceCoverage;
                GvInsuranceCarrierDataDependentInCorrect.DataBind();
            }
        }

        private void loadInsuranceCarrierEditableLeft()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeLeft, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out employeeID);
                List<insurance_coverage> employeeInsuranceCoverage = insuranceController.ManufactureInsuranceCoverageEditableWithNames(employeeID);
                LitInsuranceCarrierEditableCorrectCount.Text = employeeInsuranceCoverage.Count.ToString();
                GvInsuranceCarrierDataEditableCorrect.DataSource = employeeInsuranceCoverage;
                GvInsuranceCarrierDataEditableCorrect.DataBind();
            }
        }

        private void loadInsuranceCarrierEditableRight()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeRight, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployeeRight.SelectedItem.Value, out employeeID);
                List<insurance_coverage> employeeInsuranceCoverage = insuranceController.ManufactureInsuranceCoverageEditableWithNames(employeeID);
                LitInsuranceCarrierEditableInCorrectCount.Text = employeeInsuranceCoverage.Count.ToString();
                GvInsuranceCarrierDataEditableInCorrect.DataSource = employeeInsuranceCoverage;
                GvInsuranceCarrierDataEditableInCorrect.DataBind();
            }
        }

        /// <summary>
        /// Load the Insurance Change Events on the LEFT side of the screen for the employee selected.
        /// </summary>
        private void loadInsuranceChangeEventsLeft()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeLeft, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out employeeID);
                List<alert_insurance> employeeInsuranceOfferChangeEvents = insuranceController.findEmployeeInsuranceOfferChangeEvents(employeeID);

                LitInsuranceChangeEventCorrectCount.Text = employeeInsuranceOfferChangeEvents.Count.ToString();
                GvInsuranceOfferChangeEventCorrect.DataSource = employeeInsuranceOfferChangeEvents;
                GvInsuranceOfferChangeEventCorrect.DataBind();
            }
        }

        /// <summary>
        /// Load the Insurance Change Events on the RIGHT side of the screen for the employee selected.
        /// </summary>
        private void loadInsuranceChangeEventsRight()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeRight, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployeeRight.SelectedItem.Value, out employeeID);
                List<alert_insurance> employeeInsuranceOfferChangeEvents = insuranceController.findEmployeeInsuranceOfferChangeEvents(employeeID);

                LitInsuranceChangeEventInCorrectCount.Text = employeeInsuranceOfferChangeEvents.Count.ToString();
                GvInsuranceOfferChangeEventInCorrect.DataSource = employeeInsuranceOfferChangeEvents;
                GvInsuranceOfferChangeEventInCorrect.DataBind();
            }
        }

        /// <summary>
        /// Load the Employee Dependents on the LEFT side of the screen for the employee selected.
        /// </summary>
        private void loadDependentsLeft()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeLeft, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out employeeID);
                List<Dependent> employeeDependents = EmployeeController.manufactureEmployeeDependentList(employeeID);

                LitDependentsCorrectCount.Text = employeeDependents.Count.ToString();
                GvDependentsCorrect.DataSource = employeeDependents;
                GvDependentsCorrect.DataBind();

                DdlInsuranceCarrierDependentsCorrect.DataSource = employeeDependents;
                DdlInsuranceCarrierDependentsCorrect.DataTextField = "DEPENDENT_FULL_NAME";
                DdlInsuranceCarrierDependentsCorrect.DataValueField = "DEPENDENT_ID";
                DdlInsuranceCarrierDependentsCorrect.DataBind();
                DdlInsuranceCarrierDependentsCorrect.Items.Add("Select Dependent");
                DdlInsuranceCarrierDependentsCorrect.SelectedIndex = DdlInsuranceCarrierDependentsCorrect.Items.Count - 1;
            }
        }

        /// <summary>
        /// Load the Employee Dependents on the RIGHT side of the screen for the employee selected.
        /// </summary>
        private void loadDependentsRight()
        {
            int employeeID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeRight, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployeeRight.SelectedItem.Value, out employeeID);
                List<Dependent> employeeDependents = EmployeeController.manufactureEmployeeDependentList(employeeID);

                LitDependentsInCorrectCount.Text = employeeDependents.Count.ToString();
                GvDependentsInCorrect.DataSource = employeeDependents;
                GvDependentsInCorrect.DataBind();

                DdlInsuranceCarrierDependentsInCorrect.DataSource = employeeDependents;
                DdlInsuranceCarrierDependentsInCorrect.DataTextField = "DEPENDENT_FULL_NAME";
                DdlInsuranceCarrierDependentsInCorrect.DataValueField = "DEPENDENT_ID";
                DdlInsuranceCarrierDependentsInCorrect.DataBind();
                DdlInsuranceCarrierDependentsInCorrect.Items.Add("Select Dependent");
                DdlInsuranceCarrierDependentsInCorrect.SelectedIndex = DdlInsuranceCarrierDependentsInCorrect.Items.Count - 1;
            }
        }

        #endregion

    #region Power Merge/Delete Functions

        /// <summary>
        /// Load the Insurance Offer on the RIGHT side of the screen for the employee selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtnPowerMerge_Click(object sender, EventArgs e)
        {
            //Need to create a SP to return all employee Insurance Offers.
        }

        #endregion

    #region View Change Functions

        protected void LnkBtnPayrollView_Click(object sender, EventArgs e)
        {
            MvEmployeeDetails.ActiveViewIndex = 0;
            LnkBtnPayrollView.BackColor = System.Drawing.ColorTranslator.FromHtml("#33CCFF");
            LnkBtnInsuranceOfferView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierDependentView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierEditableView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnDependentView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnEmployeeView.BackColor = System.Drawing.Color.Transparent;
        }

        protected void LnkBtnInsuranceOfferView_Click(object sender, EventArgs e)
        {
            MvEmployeeDetails.ActiveViewIndex = 1;
            LnkBtnPayrollView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceOfferView.BackColor = System.Drawing.ColorTranslator.FromHtml("#33CCFF");
            LnkBtnInsuranceCarrierView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierDependentView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierEditableView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnDependentView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnEmployeeView.BackColor = System.Drawing.Color.Transparent;
        }

        protected void LnkBtnInsuranceCarrierView_Click(object sender, EventArgs e)
        {
            MvEmployeeDetails.ActiveViewIndex = 2;
            LnkBtnPayrollView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceOfferView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierView.BackColor = System.Drawing.ColorTranslator.FromHtml("#33CCFF"); 
            LnkBtnInsuranceCarrierDependentView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierEditableView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnDependentView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnEmployeeView.BackColor = System.Drawing.Color.Transparent;
        }

        protected void LnkBtnInsuranceCarrierDependentView_Click(object sender, EventArgs e)
        {
            MvEmployeeDetails.ActiveViewIndex = 3;
            LnkBtnPayrollView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceOfferView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierDependentView.BackColor = System.Drawing.ColorTranslator.FromHtml("#33CCFF"); 
            LnkBtnInsuranceCarrierEditableView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnDependentView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnEmployeeView.BackColor = System.Drawing.Color.Transparent;
        }

        protected void LnkBtnInsuranceCarrierEditableView_Click(object sender, EventArgs e)
        {
            MvEmployeeDetails.ActiveViewIndex = 4;
            LnkBtnPayrollView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceOfferView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierDependentView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierEditableView.BackColor = System.Drawing.ColorTranslator.FromHtml("#33CCFF");
            LnkBtnDependentView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnEmployeeView.BackColor = System.Drawing.Color.Transparent;
        }
   
        protected void LnkBtnDependentView_Click(object sender, EventArgs e)
        {
            MvEmployeeDetails.ActiveViewIndex = 5;
            LnkBtnPayrollView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceOfferView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierDependentView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierEditableView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnDependentView.BackColor = System.Drawing.ColorTranslator.FromHtml("#33CCFF"); 
            LnkBtnEmployeeView.BackColor = System.Drawing.Color.Transparent;
        }

        protected void LnkBtnEmployeeView_Click(object sender, EventArgs e)
        {
            MvEmployeeDetails.ActiveViewIndex = 6;
            LnkBtnPayrollView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceOfferView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierDependentView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnInsuranceCarrierEditableView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnDependentView.BackColor = System.Drawing.Color.Transparent;
            LnkBtnEmployeeView.BackColor = System.Drawing.ColorTranslator.FromHtml("#33CCFF"); 
        }

        
    #endregion

    #region Gridview Functions
        /// <summary>
        /// Select/Deselect All records in the Gridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvInsuranceOfferInCorrect_Sorting(object sender, GridViewSortEventArgs e)
        {
            string columnSelected = e.SortExpression;

            //Loop through the gridview and check or uncheck the radio buttons. 
            foreach (GridViewRow row in GvInsuranceOfferInCorrect.Rows)
            {
                //Skip over the header and footer of the gridview its being used.
                if (row.RowType == DataControlRowType.DataRow)
                {
                    RadioButton rbDelete = (RadioButton)row.FindControl("Rb_gv_io_Delete");
                    RadioButton rbMigrate = (RadioButton)row.FindControl("Rb_gv_io_Migrate");

                    switch (columnSelected)
                    {
                        case "delete":
                            rbDelete.Checked = true;
                            rbMigrate.Checked = false;
                            break;
                        case "migrate":
                            rbDelete.Checked = false;
                            rbMigrate.Checked = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Select/Deselect All records in the Gridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvPayrollIncorrect_Sorting(object sender, GridViewSortEventArgs e)
        {
            string columnSelected = e.SortExpression;

            //Loop through the gridview and check or uncheck the radio buttons. 
            foreach (GridViewRow row in GvPayrollIncorrect.Rows)
            {
                //Skip over the header and footer of the gridview its being used.
                if (row.RowType == DataControlRowType.DataRow)
                {
                    RadioButton rbDelete = (RadioButton)row.FindControl("Rb_gv_p_Delete");
                    RadioButton rbMigrate = (RadioButton)row.FindControl("Rb_gv_p_Migrate");

                    switch (columnSelected)
                    {
                        case "delete":
                            rbDelete.Checked = true;
                            rbMigrate.Checked = false;
                            break;
                        case "migrate":
                            rbDelete.Checked = false;
                            rbMigrate.Checked = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Select/Deselect All records in the Gridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvDependentsInCorrect_Sorting(object sender, GridViewSortEventArgs e)
        {
            string columnSelected = e.SortExpression;

            //Loop through the gridview and check or uncheck the radio buttons. 
            foreach (GridViewRow row in GvDependentsInCorrect.Rows)
            {
                //Skip over the header and footer of the gridview its being used.
                if (row.RowType == DataControlRowType.DataRow)
                {
                    RadioButton rbDelete = (RadioButton)row.FindControl("Rb_gv_d_Delete");
                    RadioButton rbMigrate = (RadioButton)row.FindControl("Rb_gv_d_Migrate");

                    switch (columnSelected)
                    {
                        case "delete":
                            rbDelete.Checked = true;
                            rbMigrate.Checked = false;
                            break;
                        case "migrate":
                            rbDelete.Checked = false;
                            rbMigrate.Checked = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Select/Deselect All records in the Gridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvInsuranceCarrierDataInCorrect_Sorting(object sender, GridViewSortEventArgs e)
        {
            string columnSelected = e.SortExpression;

            //Loop through the gridview and check or uncheck the radio buttons. 
            foreach (GridViewRow row in GvInsuranceCarrierDataInCorrect.Rows)
            {
                //Skip over the header and footer of the gridview its being used.
                if (row.RowType == DataControlRowType.DataRow)
                {
                    RadioButton rbDelete = (RadioButton)row.FindControl("Rb_gv_ic_Delete");
                    RadioButton rbMigrate = (RadioButton)row.FindControl("Rb_gv_ic_Migrate");

                    switch (columnSelected)
                    {
                        case "delete":
                            rbDelete.Checked = true;
                            rbMigrate.Checked = false;
                            break;
                        case "migrate":
                            rbDelete.Checked = false;
                            rbMigrate.Checked = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Select/Deselect All records in the Gridview. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvInsuranceCarrierDataDependentInCorrect_Sorting(object sender, GridViewSortEventArgs e)
        {
            string columnSelected = e.SortExpression;

            //Loop through the gridview and check or uncheck the radio buttons. 
            foreach (GridViewRow row in GvInsuranceCarrierDataDependentInCorrect.Rows)
            {
                //Skip over the header and footer of the gridview its being used.
                if (row.RowType == DataControlRowType.DataRow)
                {
                    RadioButton rbDelete = (RadioButton)row.FindControl("Rb_gv_icd_Delete");
                    RadioButton rbMigrate = (RadioButton)row.FindControl("Rb_gv_icd_Migrate");

                    switch (columnSelected)
                    {
                        case "delete":
                            rbDelete.Checked = true;
                            rbMigrate.Checked = false;
                            break;
                        case "migrate":
                            rbDelete.Checked = false;
                            rbMigrate.Checked = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Select/Deselect All records in the Gridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvInsuranceCarrierDataEditableInCorrect_Sorting(object sender, GridViewSortEventArgs e)
        {
            string columnSelected = e.SortExpression;

            //Loop through the gridview and check or uncheck the radio buttons. 
            foreach (GridViewRow row in GvInsuranceCarrierDataEditableInCorrect.Rows)
            {
                //Skip over the header and footer of the gridview its being used.
                if (row.RowType == DataControlRowType.DataRow)
                {
                    RadioButton rbDelete = (RadioButton)row.FindControl("Rb_gv_icd_Delete");
                    RadioButton rbMigrate = (RadioButton)row.FindControl("Rb_gv_icd_Migrate");

                    switch (columnSelected)
                    {
                        case "delete":
                            rbDelete.Checked = true;
                            rbMigrate.Checked = false;
                            break;
                        case "migrate":
                            rbDelete.Checked = false;
                            rbMigrate.Checked = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion

    #region All Migrate Data Functions
        /// <summary>
        /// This function will migrate/delete the inCorrect employee payroll.
        /// </summary>
        /// <returns></returns>
        private bool migratePayroll()
        {
            bool succesfulMigration = true;
            int correctEmployeeID = 0;
            int inCorrectEmployeeID = 0;
            DateTime modifiedOn = DateTime.Now;
            Literal litusername = (Literal)this.Master.FindControl("LitUserName");
            string modifiedBy = litusername.Text;
            DateTime oldDate = modifiedOn.AddYears(-50);
            int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out correctEmployeeID);
            int.TryParse(DdlEmployeeRight.SelectedItem.Value, out inCorrectEmployeeID);
            List<Payroll> currPayrollList = Payroll_Controller.getEmployeePayroll(inCorrectEmployeeID, oldDate, modifiedOn);
            int migratedCount = 0;
            int deletedCount = 0;
            int recordsFailed = 0;
                   
            //Loop through and migrate each row that has a checkmark in it.
            for(int i=0; i<GvPayrollIncorrect.Rows.Count;i++)
            {
                RadioButton rbMigrate = GvPayrollIncorrect.Rows[i].FindControl("Rb_gv_p_Migrate") as RadioButton;
                RadioButton rbDelete = GvPayrollIncorrect.Rows[i].FindControl("Rb_gv_p_Delete") as RadioButton;

                //Migrate the row if it was checked.
                if (rbMigrate.Checked)
                {
                    HiddenField hfRowID = GvPayrollIncorrect.Rows[i].FindControl("hf_gv_p_rowid") as HiddenField;

                    int rowID = 0;
                    string history = null;

                    //Collect information needed to update the payroll record. 
                    int.TryParse(hfRowID.Value, out rowID);
                    Payroll currPayroll = Payroll_Controller.getSinglePayroll(rowID, currPayrollList);
                    history = currPayroll.PAY_HISTORY;
                    history += System.Environment.NewLine + "Migration Event Occured" + System.Environment.NewLine;
                    history += "Record Migrated from Employee ID:" + inCorrectEmployeeID.ToString() + " to Employee ID:" + correctEmployeeID.ToString();
                    history += System.Environment.NewLine;
                    history += "Record Migrated by " + correctEmployeeID.ToString() + " on " + modifiedOn.ToString();
                    history += System.Environment.NewLine + "End of Migration Event" + System.Environment.NewLine;

                    //Set the historical field message. 
                    //Update the actual payroll record with the Correct Employee ID. 
                    bool validTransaction = Payroll_Controller.migratePayrollSingle(rowID, currPayroll.PAY_EMPLOYER_ID, correctEmployeeID, modifiedBy, modifiedOn, history);

                    //If any transaction fails, mark the migration as invalid.
                    if (validTransaction == false) { succesfulMigration = false; recordsFailed += 1; }
                    else { migratedCount += 1; }
                }
                //Delete the row if it checked.
                if (rbDelete.Checked)
                {
                    HiddenField hfRowID = GvPayrollIncorrect.Rows[i].FindControl("hf_gv_p_rowid") as HiddenField;
                    int rowID = 0;
                    //Collect information needed to update the payroll record. 
                    int.TryParse(hfRowID.Value, out rowID);

                    //Update the actual payroll record with the Correct Employee ID. 
                    bool validTransaction = Payroll_Controller.deletePayroll(rowID, modifiedBy, modifiedOn);

                    //If any transaction fails, mark the migration as invalid.
                    if (validTransaction == false) { succesfulMigration = false; recordsFailed += 1; }
                    else { deletedCount += 1; }
                }
            }

            string message = null;

            if (succesfulMigration == true)
            {
                message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
            }
            else
            {
                message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
                message += "Record(s) count that failed to migrated/delete: " + recordsFailed.ToString() + "<br />";
            }

            MpeMessage.Show();
            LitMessage.Text = message;

            //Reload the payroll data to see the changes. 
            loadPayrollDataLeft();
            loadPayrollDataRight();

            return succesfulMigration;
        }

        /// <summary>
        /// This function will migrate/delete the incorrect employee insurance offers and insurance offer change events. 
        /// </summary>
        /// <returns></returns>
        private bool migrateInsuranceOffers()
        {
            bool succesfulMigration = true;
            int correctEmployeeID = 0;
            int inCorrectEmployeeID = 0;
            int employerID = 0;
            DateTime modifiedOn = DateTime.Now;
            Literal litusername = (Literal)this.Master.FindControl("LitUserName");
            string modifiedBy = litusername.Text;
            int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out correctEmployeeID);
            int.TryParse(DdlEmployeeRight.SelectedItem.Value, out inCorrectEmployeeID);
            int.TryParse(DdlEmployer.SelectedItem.Value, out employerID);int migratedCount = 0;
            int deletedCount = 0;
            int recordsFailed = 0;
            string message = null;
            string errorMessage = null;

            //Loop through and migrate/delete each row that has a checkmark in it.
            for (int i = 0; i < GvInsuranceOfferInCorrect.Rows.Count; i++)
            {               
                RadioButton rbMigrate = GvInsuranceOfferInCorrect.Rows[i].FindControl("Rb_gv_io_Migrate") as RadioButton;
                RadioButton rbDelete = GvInsuranceOfferInCorrect.Rows[i].FindControl("Rb_gv_io_Delete") as RadioButton;

                //Migrate the row if it was checked.
                if (rbMigrate.Checked)
                {
                    HiddenField hfRowID = GvInsuranceOfferInCorrect.Rows[i].FindControl("hf_gv_io_rowid") as HiddenField;
                    Literal litPlanYearID = GvInsuranceOfferInCorrect.Rows[i].FindControl("lit_gv_io_plan_year_id") as Literal;

                    int rowID = 0;
                    int planYearID = 0;
                    string history = null;

                    //Collect information needed to update the insurance offer records. 
                    int.TryParse(hfRowID.Value, out rowID);
                    int.TryParse(litPlanYearID.Text, out planYearID);
                    alert_insurance currAI = insuranceController.findSingleInsuranceOffer(planYearID, inCorrectEmployeeID);

                    history = currAI.IALERT_HISTORY;
                    history += System.Environment.NewLine + "Migration Event Occured" + System.Environment.NewLine;
                    history += "Record Migrated from Employee ID:" + inCorrectEmployeeID.ToString() + " to Employee ID:" + correctEmployeeID.ToString();
                    history += System.Environment.NewLine;
                    history += "Record Migrated by " + correctEmployeeID.ToString() + " on " + modifiedOn.ToString();
                    history += System.Environment.NewLine + "End of Migration Event" + System.Environment.NewLine;

                    //Set the historical field message. 
                    //Update the actual payroll record with the Correct Employee ID. 
                    bool validTransaction = insuranceController.migrateInsuranceOffers(rowID, currAI.IALERT_EMPLOYERID, inCorrectEmployeeID, correctEmployeeID, modifiedBy, modifiedOn,  history);

                    //If any transaction fails, mark the migration as invalid.
                    if (validTransaction == false) 
                    { 
                        succesfulMigration = false; 
                        recordsFailed += 1;
                        errorMessage = "<h4>Error Message</h4>An Insurance Offer has failed to migrate, this occurs when there is already an Insurance Offer with the same Plan Year. Take a look at the Plan Year columns to see if they are the same Plan Year ID, if they are, the record on the in-correct employee side will have to be Deleted instead of Migrating it.";
                    }
                    else { migratedCount += 1; }
                }
                //Delete the row if it checked.
                if (rbDelete.Checked)
                {
                    HiddenField hfRowID = GvInsuranceOfferInCorrect.Rows[i].FindControl("hf_gv_io_rowid") as HiddenField;
                    Literal litPlanYearID = GvInsuranceOfferInCorrect.Rows[i].FindControl("lit_gv_io_plan_year_id") as Literal;

                    int rowID = 0;

                    //Collect information needed to update the insurance offer records. 
                    int.TryParse(hfRowID.Value, out rowID);

                    //This will DELETE all of the insurance offers and insurance offer change events for an employee.  
                    bool validTransaction = insuranceController.deleteInsuranceOffers(rowID, employerID, inCorrectEmployeeID);

                    //If any transaction fails, mark the migration as invalid.
                    if (validTransaction == false) { succesfulMigration = false;  recordsFailed += 1;}
                    else { deletedCount += 1; }
                }    
            }

            if (succesfulMigration == true)
            {
                message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
            }
            else
            {
                message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
                message += "Record(s) count that failed to migrated/delete: " + recordsFailed.ToString() + "<br />";
            }

            MpeMessage.Show();
            LitMessage.Text = message + errorMessage;

            //Reload the Insurance Offer & Insurance Change Events data to see the changes.
            loadInsuranceOfferLeft();
            loadInsuranceOfferRight();
            loadInsuranceChangeEventsLeft();
            loadInsuranceChangeEventsRight();

            return succesfulMigration;
        }

        /// <summary>
        /// This function will migrate/delete the incorrect employee insurance carrier data.
        /// </summary>
        /// <returns></returns>
        private bool migrateInsuranceCarrierData()
        {
            bool succesfulMigration = true;
            int correctEmployeeID = 0;
            int inCorrectEmployeeID = 0;
            int employerID = 0;
            DateTime modifiedOn = DateTime.Now;
            int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out correctEmployeeID);
            int.TryParse(DdlEmployeeRight.SelectedItem.Value, out inCorrectEmployeeID);
            int.TryParse(DdlEmployer.SelectedItem.Value, out employerID);
            int migratedCount = 0;
            int deletedCount = 0;
            int recordsFailed = 0;
            string message = null;
            string errorMessage = null;

            List<insurance_coverage> currCoverage = insuranceController.manufactureAllInsuranceCoverageForEmployee(inCorrectEmployeeID);

            //Loop through and migrate/delete each row that has a checkmark in it.
            for (int i = 0; i < GvInsuranceCarrierDataInCorrect.Rows.Count; i++)
            {
                RadioButton rbMigrate = GvInsuranceCarrierDataInCorrect.Rows[i].FindControl("Rb_gv_ic_Migrate") as RadioButton;
                RadioButton rbDelete = GvInsuranceCarrierDataInCorrect.Rows[i].FindControl("Rb_gv_ic_Delete") as RadioButton;

                //Migrate the row if it was checked.
                if (rbMigrate.Checked)
                {
                    HiddenField hfRowID = GvInsuranceCarrierDataInCorrect.Rows[i].FindControl("Hf_gv_ic_rowid") as HiddenField;

                    int rowID = 0;
                    string history = null;

                    //Collect information needed to update the insurance offer records. 
                    int.TryParse(hfRowID.Value, out rowID);
                    insurance_coverage currIC = insuranceController.getSingleInsuranceCoverage(rowID, currCoverage);

                    history = currIC.IC_HISTORY;
                    history += System.Environment.NewLine + "Migration Event Occured" + System.Environment.NewLine;
                    history += "Record Migrated from Employee ID:" + inCorrectEmployeeID.ToString() + " to Employee ID:" + correctEmployeeID.ToString();
                    history += System.Environment.NewLine;
                    history += "Record Migrated by " + correctEmployeeID.ToString() + " on " + modifiedOn.ToString();
                    history += System.Environment.NewLine + "End of Migration Event" + System.Environment.NewLine;

                    //Update the actual insurance coverage row with the correct employee ID. 
                    bool validTransaction = insuranceController.updateInsuranceCoverageRow(rowID, correctEmployeeID, inCorrectEmployeeID, history);

                    //If any transaction fails, mark the migration as invalid.
                    if (validTransaction == false){ succesfulMigration = false; recordsFailed += 1; }
                    else { migratedCount += 1; }
                }
                //Delete the row if it checked.
                if (rbDelete.Checked)
                {
                    HiddenField hfRowID = GvInsuranceCarrierDataInCorrect.Rows[i].FindControl("Hf_gv_ic_rowid") as HiddenField;

                    int rowID = 0;

                    //Collect information needed to update the insurance offer records. 
                    int.TryParse(hfRowID.Value, out rowID);

                    //This will DELETE all of the insurance offers and insurance offer change events for an employee.  
                    bool validTransaction = insuranceController.deleteInsuranceCoverageSingleRow(rowID, inCorrectEmployeeID);

                    //If any transaction fails, mark the migration as invalid.
                    if (validTransaction == false) { succesfulMigration = false; recordsFailed += 1; }
                    else { deletedCount += 1; }
                }
            }

            if (succesfulMigration == true)
            {
                message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
            }
            else
            {
                message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
                message += "Record(s) count that failed to migrated/delete: " + recordsFailed.ToString() + "<br />";
            }

            MpeMessage.Show();
            LitMessage.Text = message + errorMessage;

            //Reload the Insurance Offer & Insurance Change Events data to see the changes.
            loadInsuranceCarrierLeft();
            loadInsuranceCarrierRight();

            return succesfulMigration;
        }

        /// <summary>
        /// This function will migrate all dependent insurance carrer related to the ones selected in the drop down lists. 
        /// </summary>
        /// <returns></returns>
        private bool migrateInsuranceCarrierDependentDataRows()
        {
            bool succesfulMigration = true;
            int correctEmployeeID = 0;
            int inCorrectEmployeeID = 0;
            int correctDependentID = 0;
            int inCorrectDependentID = 0;
            bool validData = true;
            int migratedCount = 0;
            int deletedCount = 0;
            int recordsFailed = 0;
            string message = null;
            DateTime modifiedOn = DateTime.Now;
            
   

            validData = errorChecking.validateDropDownSelection(DdlEmployeeLeft, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeRight, validData);
            validData = errorChecking.validateDropDownSelection(DdlInsuranceCarrierDependentsInCorrect, validData);
            validData = errorChecking.validateDropDownSelection(DdlInsuranceCarrierDependentsCorrect, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out correctEmployeeID);
                int.TryParse(DdlEmployeeRight.SelectedItem.Value, out inCorrectEmployeeID);
                int.TryParse(DdlInsuranceCarrierDependentsCorrect.SelectedItem.Value, out correctDependentID);
                int.TryParse(DdlInsuranceCarrierDependentsInCorrect.SelectedItem.Value, out inCorrectDependentID);

                List<insurance_coverage> currCoverage = insuranceController.manufactureAllInsuranceCoverageForEmployeeDependents(inCorrectEmployeeID);

                //Loop through and find the records related to the In-correct dependent ID and move them to the correct dependent ID and employee ID.
                for (int i = 0; i < GvInsuranceCarrierDataDependentInCorrect.Rows.Count; i++)
                {
                    int gvDependentID = 0;
                    int rowID = 0;
                    HiddenField hfRowID = GvInsuranceCarrierDataDependentInCorrect.Rows[i].FindControl("Hf_gv_icd_rowid") as HiddenField;
                    HiddenField hfdependentID = GvInsuranceCarrierDataDependentInCorrect.Rows[i].FindControl("Hf_gv_icd_dependentid") as HiddenField;
                    //Collect information needed to update the insurance offer records. 
                    int.TryParse(hfRowID.Value, out rowID);
                    int.TryParse(hfdependentID.Value, out gvDependentID);

                    //Migrate the depdent insurance carrier/offer records.
                    if (gvDependentID == inCorrectDependentID)
                    {
                        string history = null;

                        insurance_coverage currIC = insuranceController.getSingleInsuranceCoverage(rowID, currCoverage);

                        history = currIC.IC_HISTORY;
                        history += System.Environment.NewLine + "Migration Event Occured" + System.Environment.NewLine;
                        history += "Record Migrated from Employee ID:" + inCorrectEmployeeID.ToString() + " to Employee ID:" + correctEmployeeID.ToString();
                        history += System.Environment.NewLine;
                        history += "Record Migrated by " + correctEmployeeID.ToString() + " on " + modifiedOn.ToString();
                        history += System.Environment.NewLine + "End of Migration Event" + System.Environment.NewLine;

                        //Update the actual insurance coverage row with the correct employee ID. 
                        bool validTransaction = insuranceController.updateInsuranceCoverageRowsDependent(rowID, correctEmployeeID, inCorrectEmployeeID, correctDependentID, inCorrectDependentID, history);

                        //If any transaction fails, mark the migration as invalid.
                        if (validTransaction == false) { succesfulMigration = false; recordsFailed += 1; }
                        else { migratedCount += 1; }
                    }
                }

                if (succesfulMigration == true)
                {
                    message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                    message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
                }
                else
                {
                    message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                    message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
                    message += "Record(s) count that failed to migrated/delete: " + recordsFailed.ToString() + "<br />";
                }

                MpeMessage.Show();
                LitMessage.Text = message;

                //Reload the Insurance Offer & Insurance Change Events data to see the changes.
                loadInsuranceCarrierDependentLeft();
                loadInsuranceCarrierDependentRight();
            }
            else { succesfulMigration = false; }

            return succesfulMigration;
        }

        /// <summary>
        /// This function will migrate/delete all dependent insurance carrier data to the ones selected in the drop down lists. 
        /// </summary>
        /// <returns></returns>
        private bool migrateInsuranceCarrierDependentDataRow()
        {
            bool succesfulMigration = true;
            int correctEmployeeID = 0;
            int inCorrectEmployeeID = 0;
            int employerID = 0;
           int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out correctEmployeeID);
            int.TryParse(DdlEmployeeRight.SelectedItem.Value, out inCorrectEmployeeID);
            int.TryParse(DdlEmployer.SelectedItem.Value, out employerID);
            int migratedCount = 0;
            int deletedCount = 0;
            int recordsFailed = 0;
            string message = null;
        

            List<insurance_coverage> currCoverage = insuranceController.manufactureAllInsuranceCoverageForEmployeeDependents(inCorrectEmployeeID);

            //Loop through and migrate/delete each row that has a checkmark in it.
            for (int i = 0; i < GvInsuranceCarrierDataDependentInCorrect.Rows.Count; i++)
            {
                RadioButton rbDelete = GvInsuranceCarrierDataDependentInCorrect.Rows[i].FindControl("Rb_gv_icd_Delete") as RadioButton;

                //Delete the row if it checked.
                if (rbDelete.Checked)
                {
                    HiddenField hfRowID = GvInsuranceCarrierDataDependentInCorrect.Rows[i].FindControl("Hf_gv_icd_rowid") as HiddenField;

                    int rowID = 0;

                    //Collect information needed to update the insurance offer records. 
                    int.TryParse(hfRowID.Value, out rowID);

                    //This will DELETE all of the insurance offers and insurance offer change events for an employee.  
                    bool validTransaction = insuranceController.deleteInsuranceCoverageSingleRow(rowID, inCorrectEmployeeID);

                    //If any transaction fails, mark the migration as invalid.
                    if (validTransaction == false) { succesfulMigration = false; recordsFailed += 1; }
                    else { deletedCount += 1; }
                }
            }

            if (succesfulMigration == true)
            {
                message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
            }
            else
            {
                message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
                message += "Record(s) count that failed to migrated/delete: " + recordsFailed.ToString() + "<br />";
            }

            MpeMessage.Show();
            LitMessage.Text = message;

            //Reload the Insurance Offer & Insurance Change Events data to see the changes.
            loadInsuranceCarrierDependentLeft();
            loadInsuranceCarrierDependentRight();

            return succesfulMigration;
        }

        /// <summary>
        /// This function will delete editable insurance carrier data for the employee selected.
        /// </summary>
        /// <returns></returns>
        private bool migrateInsuranceCarrierEditableData()
        {
            bool succesfulMigration = true;
            int correctEmployeeID = 0;
            int inCorrectEmployeeID = 0;
            int employerID = 0;
           int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out correctEmployeeID);
            int.TryParse(DdlEmployeeRight.SelectedItem.Value, out inCorrectEmployeeID);
            int.TryParse(DdlEmployer.SelectedItem.Value, out employerID);
            int migratedCount = 0;
            int deletedCount = 0;
            int recordsFailed = 0;
            string message = null;
           

            List<insurance_coverage> currCoverage = insuranceController.ManufactureInsuranceCoverageEditableWithNames(inCorrectEmployeeID);

            //Loop through and migrate/delete each row that has a checkmark in it.
            for (int i = 0; i < GvInsuranceCarrierDataEditableInCorrect.Rows.Count; i++)
            {
                RadioButton rbDelete = GvInsuranceCarrierDataEditableInCorrect.Rows[i].FindControl("Rb_gv_icd_Delete") as RadioButton;

                //Delete the row if it checked.
                if (rbDelete.Checked)
                {
                    HiddenField hfRowID = GvInsuranceCarrierDataEditableInCorrect.Rows[i].FindControl("Hf_gv_icd_rowid") as HiddenField;

                    int rowID = 0;

                    //Collect information needed to update the insurance offer records. 
                    int.TryParse(hfRowID.Value, out rowID);

                    //This will DELETE all of the insurance offers and insurance offer change events for an employee.  
                    bool validTransaction = insuranceController.deleteInsuranceCoverageRow(rowID);

                    //If any transaction fails, mark the migration as invalid.
                    if (validTransaction == false) { succesfulMigration = false; recordsFailed += 1; }
                    else { deletedCount += 1; }
                }
            }

            if (succesfulMigration == true)
            {
                message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
            }
            else
            {
                message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
                message += "Record(s) count that failed to migrated/delete: " + recordsFailed.ToString() + "<br />";
            }

            MpeMessage.Show();
            LitMessage.Text = message;

            //Reload the Insurance Offer & Insurance Change Events data to see the changes.
            loadInsuranceCarrierEditableLeft();
            loadInsuranceCarrierEditableRight();

            return succesfulMigration;
        }

        /// <summary>
        /// This function will migrate/delete employee dependents.
        /// </summary>
        /// <returns></returns>
        private bool migrateDependents()
        {
            bool succesfulMigration = true;
            int correctEmployeeID = 0;
            int inCorrectEmployeeID = 0;
            DateTime modifiedOn = DateTime.Now;
            Literal litusername = (Literal)this.Master.FindControl("LitUserName");
         
            int.TryParse(DdlEmployeeLeft.SelectedItem.Value, out correctEmployeeID);
            int.TryParse(DdlEmployeeRight.SelectedItem.Value, out inCorrectEmployeeID);
            int migratedCount = 0;
            int deletedCount = 0;
            int recordsFailed = 0;
            string message = null;
          

            List<Dependent> currDependentList = EmployeeController.manufactureEmployeeDependentList(inCorrectEmployeeID);

            //Loop through and migrate each row that has a checkmark in it.
            for (int i = 0; i < GvDependentsInCorrect.Rows.Count; i++)
            {
                RadioButton rbMigrate = GvDependentsInCorrect.Rows[i].FindControl("Rb_gv_d_Migrate") as RadioButton;
                RadioButton rbDelete = GvDependentsInCorrect.Rows[i].FindControl("Rb_gv_d_Delete") as RadioButton;

                //Migrate the row if it was checked.
                if (rbMigrate.Checked)
                {
                    HiddenField hfdependentID = GvDependentsInCorrect.Rows[i].FindControl("hf_gv_d_dependentid") as HiddenField;
                    int dependentID = 0;
                    //string history = null;

                    //Collect information needed to update the payroll record. 
                    int.TryParse(hfdependentID.Value, out dependentID);
                    Dependent currDependent = EmployeeController.findSingleDependent(currDependentList, dependentID);

                    //Set the historical field message. 
                    //Update the actual dependent record with the Correct Employee ID. 
                    bool validTransaction = EmployeeController.migrateEmployeeDependent(dependentID, inCorrectEmployeeID, correctEmployeeID);

                    //If any transaction fails, mark the migration as invalid.
                    if (validTransaction == false) { succesfulMigration = false; recordsFailed += 1; }
                    else { migratedCount += 1; }
                }
                //Delete the row if it checked.
                if (rbDelete.Checked)
                {
                    HiddenField hfdependentID = GvDependentsInCorrect.Rows[i].FindControl("hf_gv_d_dependentid") as HiddenField;
                    int dependentID = 0;
                    //Collect information needed to update the payroll record. 
                    int.TryParse(hfdependentID.Value, out dependentID);

                    //Update the actual payroll record with the Correct Employee ID. 
                    bool validTransaction = EmployeeController.DeleteEmployeeDependent(dependentID, inCorrectEmployeeID);

                    //If any transaction fails, mark the migration as invalid.
                    if (validTransaction == false) { succesfulMigration = false; recordsFailed += 1; }
                    else { deletedCount += 1; }
                }
            }

            if (succesfulMigration == true)
            {
                message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
            }
            else
            {
                message = migratedCount.ToString() + " record(s) were successfully migrated. " + "<br />";
                message += deletedCount.ToString() + " record(s) were successfully deleted. " + "<br />";
                message += "Record(s) count that failed to migrated/delete: " + recordsFailed.ToString() + "<br />";
            }

            MpeMessage.Show();
            LitMessage.Text = message;

            //Reload the payroll data to see the changes. 
            loadDependentsLeft();
            loadDependentsRight();
            loadInsuranceCarrierDependentLeft();
            loadInsuranceCarrierDependentRight();

            return succesfulMigration;
        }

        /// <summary>
        /// This will remove the EMPLOYEE with one function.
        /// </summary>
        /// <returns></returns>
        private bool nukeEmployee()
        {
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployeeRight, validData);

            if (validData == true)
            {
                int inCorrectEmployeeID = 0;
                int.TryParse(DdlEmployeeRight.SelectedItem.Value, out inCorrectEmployeeID);
                string employeeName = DdlEmployeeRight.SelectedItem.Text;
                bool validTransaction = EmployeeController.nukeEmployeeFromACA(inCorrectEmployeeID);

                loadEmployeesRightOnly();
                resetRightEmployeeControls();
                resetRightViewControls();

                if (validTransaction == true)
                {
                    MpeMessage.Show();
                    LitMessage.Text = employeeName + " has been removed from the System.";
                }
                else
                {
                    MpeMessage.Show();
                    LitMessage.Text = "An error occured while trying to remove " + employeeName + " from the system.";
                }
            }

            return validData;
        }

    #endregion

    #region All View/Tab Button Click Functions
        protected void BtnPayrollSave_Click(object sender, EventArgs e)
        {
            migratePayroll();
        }

        protected void BtnInsuranceOfferSave_Click(object sender, EventArgs e)
        {
            migrateInsuranceOffers();
        }

        protected void BtnDependentsSave_Click(object sender, EventArgs e)
        {
            migrateDependents();
        }

        protected void BtnInsuranceCarrierSave_Click(object sender, EventArgs e)
        {
            migrateInsuranceCarrierData();
        }

        protected void BtnInsuranceCarrierDependentSave_Click(object sender, EventArgs e)
        {
            migrateInsuranceCarrierDependentDataRow();
        }

        protected void BtnMergeInsuranceCarrierDependentRecord_Click(object sender, EventArgs e)
        {
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlInsuranceCarrierDependentsCorrect, validData);
            validData = errorChecking.validateDropDownSelection(DdlInsuranceCarrierDependentsInCorrect, validData);

            if (validData == true)
            {
                migrateInsuranceCarrierDependentDataRows();
            }
           
        }

        protected void BtnInsuranceCarrierEditableSave_Click(object sender, EventArgs e)
        {
            migrateInsuranceCarrierEditableData();
        }

        protected void BtnDeleteEmployee_Click(object sender, EventArgs e)
        {
            nukeEmployee();
        }

        protected void LnkBtnPowerDelete_Click(object sender, EventArgs e)
        {
            nukeEmployee();
        }

        public bool ProcessMyDataItem(object myValue)
        {
            return myValue != null && Convert.ToBoolean(myValue.ToString());
        }


    #endregion




    }
}