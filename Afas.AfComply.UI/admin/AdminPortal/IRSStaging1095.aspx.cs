using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class IRSStaging1095 : AdminPageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(IRSStaging1095));

        private int EmployerId
        {
            get
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

                }

                return employerId;
            }
        }

        private int TaxYearId
        {
            get
            {
                return int.Parse(DdlCalendarYear.SelectedItem.Value);
            }
        }

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            Server.ScriptTimeout = 1800;

            if (false == Feature.NewAdminPanelEnabled)
            {
                
                Log.Info("A user tried to access the IRSTransmit page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=31", false);

            }
            else if(Feature.EtlProcessEnabled == false)
            {
                DdlEmployer.Enabled = false;
                DdlCalendarYear.Enabled = false;
                BtnTransmit.Enabled = false;
                lblMsg.BackColor = System.Drawing.Color.Yellow;
                lblMsg.Text = "The 1095 ETL Build is currently disabled, please contact your IT administrator if you have questions";
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

            if (EmployerId == 0) return;

            employer employ = employerController.getEmployer(EmployerId);

            cofein.Text = employ.EMPLOYER_EIN;

            chkConfirm.Checked = false;

            List<User> users = UserController.getDistrictUsers(EmployerId);
            List<User> IrsContacts = (from User contact in users where contact.User_IRS_CONTACT == true select contact).ToList();

            List<Employee> finalizedEmployees = EmployeeController.ManufactureEmployeeList1095Finalized(EmployerId, 2016);  // in a hurry at this stage.

            Gv_gv_Finalized.DataSource = finalizedEmployees;
            Gv_gv_Finalized.DataBind();

            gvContacts.DataSource = IrsContacts;
            gvContacts.DataBind();

            Gv_gv_Alerts.DataSource = alert_controller.manufactureEmployerAlertListAll(EmployerId);
            Gv_gv_Alerts.DataBind();

            var tys = employerController.manufactureTaxYearSubmission(EmployerId, TaxYearId);
            tys.IRS_EMPLOYER_ID = EmployerId;
            tys.IRS_TAX_YEAR = TaxYearId;

            errorChecking.setDropDownList(Ddl_step1, tys.IRS_DGE);
            errorChecking.setDropDownList(Ddl_step2, tys.IRS_ALE);
            errorChecking.setDropDownList(Ddl_step3, tys.IRS_TR);
            errorChecking.setDropDownList(Ddl_step5, tys.IRS_UNPAID_LEAVE);
            errorChecking.setDropDownList(Ddl_step6, tys.IRS_ASH);

            //Highlight Missing Steps in RED.
            errorChecking.validateDropDownSelection(Ddl_step1, false);
            errorChecking.validateDropDownSelection(Ddl_step2, false);
            errorChecking.validateDropDownSelection(Ddl_step3, false);
            errorChecking.validateDropDownSelection(Ddl_step5, false);
            errorChecking.validateDropDownSelection(Ddl_step6, false);

        }

        protected void BtnTransmit_Click(object sender, EventArgs e)
        {

            if (EmployerId == 0) return;

            if (Feature.EtlProcessEnabled == true)
            {
                if (chkConfirm.Checked)
                {
                    User user = ((User)Session["CurrentUser"]);

                    PIILogger.LogPII(String.Format("Transferring to AIR database all employees with Employer with Id: {0}, requested by {1}.", EmployerId, user.User_UserName));
                    bool validPrepare = employerController.PrepIRS(EmployerId, TaxYearId);
                    if (validPrepare)
                    {
                        bool validSave = employerController.Transmit(EmployerId, TaxYearId);
                        lblMsg.Text = validSave.ToString();

                        if (validSave)
                        {

                            EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EmployerId, TaxYearId);
                            if (currentEmployerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.F1094_Collected)
                            {

                                var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                                    currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                                    TransmissionStatusEnum.ETL,
                                    user.User_UserName,
                                    DateTime.Now
                                );

                                employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);

                            }
                            else
                            {

                                Log.Warn(string.Format("EmployerId {0}, TransmissionStatusId {1}, should be {2}", EmployerId, currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionStatusId, TransmissionStatusEnum.F1094_Collected));
                            }

                        }
                        else
                        {

                            lblMsg.Text = "IRS Staging Failed, please contact IT.";
                            Log.Warn("IRS Staging Failed to save on employerController.Transmit");

                        }

                    }
                    else
                    {

                        lblMsg.Text = "Prepare ACA for IRS Staging failed, please contact IT.";
                        Log.Warn("Prepare ACA for IRS staging failed to run");

                    }

                }
                else
                {
                    lblMsg.Text = "You must certify that you are authorized to transmit this employer.";
                }
            }
            else
            {
                lblMsg.Text = "This function has been disabled.";
            }
        
        }

    }

}