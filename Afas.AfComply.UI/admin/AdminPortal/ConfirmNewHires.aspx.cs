using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class ConfirmNewHires : AdminPageBase
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

                lblMsg.Text = "Incorrect parameters";

                return;

            }

            employer employ = employerController.getEmployer(employerId);

            cofein.Text = employ.EMPLOYER_EIN;

        }

        protected void BtnSave_Click(object sender, EventArgs e)
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

                lblMsg.Text = "Please select an employer and confirm the action.";

                return;

            }

            try
            {

                lblMsg.Text = "";

                DateTime modifedOn = DateTime.Now;

                User user = (User)Session["CurrentUser"];

                if (user == null)
                {
                    lblMsg.Text = "Error getting logged in User.";

                    Log.Warn("Session failed to return User");

                    return;
                }

                employer employ = employerController.getEmployer(employerId);

                if (employ == null)
                {
                    lblMsg.Text = "Error getting Employer by Id.";

                    Log.Warn("Employer returned null, from employer Id: " + employerId);

                    return;
                }

                // same for everyone, fetch once.
                int initialMeasurementId = employ.EMPLOYER_INITIAL_MEAS_ID;
                int intialMeasurementLengthInMonths = measurementController.getInitialMeasurementLength(initialMeasurementId);

                var employeeList = EmployeeController.manufactureImportEmployeeList(employ.EMPLOYER_ID);

                foreach (Employee_I alertEmployee in employeeList)
                {
                    int employeeId = 0; // always new employee for this button.
                    int rowId = alertEmployee.ROW_ID;
                    int employeeTypeId = alertEmployee.EMPLOYEE_TYPE_ID;
                    int hrStatusId = alertEmployee.EMPLOYEE_HR_STATUS_ID;
                    String firstName = alertEmployee.EMPLOYEE_FIRST_NAME;
                    String middleName = alertEmployee.EMPLOYEE_MIDDLE_NAME;
                    String lastName = alertEmployee.EMPLOYEE_LAST_NAME;
                    String address = alertEmployee.EMPLOYEE_ADDRESS;
                    String city = alertEmployee.EMPLOYEE_CITY;
                    int stateId = alertEmployee.EMPLOYEE_STATE_ID;
                    String zipCode = alertEmployee.EMPLOYEE_ZIP;
                    DateTime hireDate = alertEmployee.EMPLOYEE_HIRE_DATE.Value;
                    DateTime? changeDate = alertEmployee.EMPLOYEE_C_DATE;
                    String socialSecurityNumber = alertEmployee.Employee_SSN_Visible;
                    String employersEmployeeId = alertEmployee.EMPLOYEE_EXT_ID;
                    DateTime? terminationDate = alertEmployee.EMPLOYEE_TERM_DATE;
                    DateTime dateOfBirth = alertEmployee.EMPLOYEE_DOB.Value;
                    int planYearId = alertEmployee.EMPLOYEE_PLAN_YEAR_ID;
                    int limboPlanYearId = alertEmployee.EMPLOYEE_PLAN_YEAR_ID_LIMBO;
                    int measurementPlanYearId = alertEmployee.EMPLOYEE_PLAN_YEAR_ID_MEAS;
                    bool offeredInsurance = false; // defaults.
                    int offeredPlanYearId = 0; // defaults.
                    int employeeClassId = alertEmployee.EMPLOYEE_CLASS_ID;
                    int acaStatusId = alertEmployee.EMPLOYEE_ACT_STATUS_ID;

                    DateTime intialMeasurementPeriodEnd = EmployeeController.calculateIMPEndDate(hireDate, intialMeasurementLengthInMonths);

                    Employee ee = EmployeeController.TransferImportedEmployee(
                            employeeId,
                            rowId,
                            employeeTypeId,
                            hrStatusId,
                            employerId,
                            firstName,
                            middleName,
                            lastName,
                            address,
                            city,
                            stateId,
                            zipCode,
                            hireDate,
                            changeDate,
                            socialSecurityNumber,
                            employersEmployeeId,
                            terminationDate,
                            dateOfBirth,
                            intialMeasurementPeriodEnd,
                            planYearId,
                            limboPlanYearId,
                            measurementPlanYearId,
                            modifedOn,
                            user.User_UserName,
                            offeredInsurance,
                            offeredPlanYearId,
                            employeeClassId,
                            acaStatusId
                        );

                    if (ee == null)
                    {
                        this.Log.Warn(String.Format("Could not confirm as a new hire for record {0}.", alertEmployee.EMPLOYEE_EXT_ID));
                    }
                    else
                    {

                        if (this.Log.IsInfoEnabled)
                        {
                            this.Log.Info(String.Format("Confirmed {0} as a new hire!", alertEmployee.EMPLOYEE_EXT_ID));
                        }

                    }

                }

                // Queue up the calculation
                employerController.insertEmployerCalculation(employ.EMPLOYER_ID);

                lblMsg.Text = "Confirmation Complete!";

            }
            catch (Exception exception)
            {
                
                this.Log.Fatal("Errors during confirming new hires!", exception);
                
                lblMsg.Text = exception.Message;

            }

        }

        private ILog Log = LogManager.GetLogger(typeof(ConfirmNewHires));
    
    }

}