using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class RescanAlerts : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(RescanAlerts));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the RescanAlerts page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=40", false);
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

        protected void BtnFix_Click(object sender, EventArgs e)
        {
            BtnRescanAlerts_Click(sender, e);
            BtnReScanData_Click(sender, e);
            BtnValidateEmployees_Click(sender, e);
            BtnValidateDependents_Click(sender, e);
            BtnInvalidEmployees_Click(sender, e);
            BtnTransferRecords_Click(sender, e);
        }

        protected void BtnRescanAlerts_Click(object sender, EventArgs e)
        {
            int _employerID = 0;
            bool validData = true;
            string _modBy = ""; 

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

            if (validData == true)
            {
                _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                EmployeeController.CrossReferenceDemographicImportTableData(_employerID, 0, 0, 0, 0);

                EmployeeController.TransferDemographicImportTableData(_employerID, _modBy, false, true);
            }
            else
            {

            }
        }

        protected void BtnReScanData_Click(object sender, EventArgs e)
        {
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

            if (validData == true)
            {
                int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                string _modBy = ""; 
                DateTime _modOn = DateTime.Now;

                Payroll_Controller.CrossReferenceData(_employerID, _modBy, _modOn);
                Payroll_Controller.TransferPayrollRecords(_employerID, _modBy, _modOn);

                employerController.insertEmployerCalculation(_employerID);

            }
            else
            {
            }
        }

        
        #region from insurance carrier
        protected void BtnValidateEmployees_Click(object sender, EventArgs e)
        {
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

            if (validData == true)
            {
                int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                insuranceController.ValidateCurrentEmployee(_employerID);
            }
            else
            {
            }
        }

        protected void BtnValidateDependents_Click(object sender, EventArgs e)
        {
            bool validData = true;
            Literal litUser = (Literal)this.Master.FindControl("LitUserName");
            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            string modBy = litUser.Text;

            if (validData == true)
            {
                int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                insuranceController.validateCurrentEmployeeDependents(_employerID, modBy);
            }
            else
            {
            }
        }

        protected void BtnInvalidEmployees_Click(object sender, EventArgs e)
        {
            bool validData = true;
            DateTime _modOn = DateTime.Now;
            string _modBy = ""; 

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

            if (validData == true)
            {
                int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                insuranceController.createAlertsForMissingEmployees(_employerID, _modOn, _modBy);
            }
            else
            {
            }
        }

        protected void BtnTransferRecords_Click(object sender, EventArgs e)
        {
            Literal litUN = (Literal)this.Master.FindControl("LitUserName");
            bool validData = true;
            string modBy = litUN.Text;
            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

            if (validData == true)
            {
                int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                insuranceController.transferInsuranceCarrierImportData(_employerID, modBy);
            }
            else
            {
            }
        }

        #endregion
    }
}