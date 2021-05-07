using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;
using Afas.Application.CSV;
using Afas.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class NewEmployer : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(EmployerRegistration));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.AdministrationEnabled)
            {
                Log.Info("A user tried to access the Administration NewEmployer page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=34", false);
            }

            DdlState.DataSource = StateController.getStates();
            DdlState.DataTextField = "State_Name";
            DdlState.DataValueField = "State_ID";
            DdlState.DataBind();
            DdlState.SelectedIndex = 0;

            loadEmployerTypes();
        }

        private void loadEmployerTypes()
        {
            DdlEmployerType.DataSource = employer_typeController.getEmployerTypes();
            DdlEmployerType.DataTextField = "EMPLOYER_TYPE_NAME";
            DdlEmployerType.DataValueField = "EMPLOYER_TYPE_ID";
            DdlEmployerType.DataBind();

        }

        protected void BtnUploadFile_Click(object sender, EventArgs e)
        {
            if (IsInputValid())
            {
                LblFileUploadMessage.Text = CreateEmployer();
            }
        }

        private bool IsInputValid()
        {
            LblFileUploadMessage.Text = string.Empty;
            string InvalidValues = string.Empty;

            //employer
            if (TxtEmployerEIN.Text.IsNullOrEmpty()
                || false == errorChecking.validateTextBoxEIN(TxtEmployerEIN, true))
            {
                InvalidValues = InvalidValues.AddCommaIfNotEmpty();
                InvalidValues += "Employer Id Number";
            }

            if (TxtEmployerIrsName.Text.IsNullOrEmpty())
            {
                InvalidValues = InvalidValues.AddCommaIfNotEmpty();
                InvalidValues += "Employer Name";
            }

            if (TxtAddress.Text.IsNullOrEmpty())
            {
                InvalidValues = InvalidValues.AddCommaIfNotEmpty();
                InvalidValues += "Address";
            }

            if (TxtCity.Text.IsNullOrEmpty())
            {
                InvalidValues = InvalidValues.AddCommaIfNotEmpty();
                InvalidValues += "City";
            }

            if (DdlState.SelectedItem.Text.IsNullOrEmpty())
            {
                InvalidValues = InvalidValues.AddCommaIfNotEmpty();
                InvalidValues += "State";
            }

            if (TxtZip.Text.IsNullOrEmpty())
            {
                InvalidValues = InvalidValues.AddCommaIfNotEmpty();
                InvalidValues += "Zip";
            }

            //user
            if (TxtUserFname.Text.IsNullOrEmpty())
            {
                InvalidValues = InvalidValues.AddCommaIfNotEmpty();
                InvalidValues += "First Name";
            }

            if (TxtUserLname.Text.IsNullOrEmpty())
            {
                InvalidValues = InvalidValues.AddCommaIfNotEmpty();
                InvalidValues += "Last Name";
            }

            if (TxtUserEmail.Text.IsNullOrEmpty()
                || false == errorChecking.validateTextBoxEmail(TxtUserEmail, true))
            {
                InvalidValues = InvalidValues.AddCommaIfNotEmpty();
                InvalidValues += "User Email";
            }

            if (TxtUserPhone.Text.IsNullOrEmpty()
                || false == errorChecking.validateTextBoxPhone(TxtUserPhone, true))
            {
                InvalidValues = InvalidValues.AddCommaIfNotEmpty();
                InvalidValues += "Phone Number";
            }

            if (TxtUserName.Text.IsNullOrEmpty()
                || false == errorChecking.validateTextBox6Length(TxtUserName, true))
            {
                InvalidValues = InvalidValues.AddCommaIfNotEmpty();
                InvalidValues += "User Name";
            }

            //password            
            if (TxtUserPass.Text.IsNullOrEmpty())
            {
                InvalidValues = InvalidValues.AddCommaIfNotEmpty();
                InvalidValues += "Password";
            }

            if (TxtUserPass2.Text.IsNullOrEmpty())
            {
                InvalidValues = InvalidValues.AddCommaIfNotEmpty();
                InvalidValues += "Retype Password";
            }

            if (InvalidValues != string.Empty)
            {
                LblFileUploadMessage.Text += "Invalid: " + InvalidValues + ". ";
            }

            //check if passwords meet requirements, and matches
            if (false == errorChecking.validateTextBoxPassword(TxtUserPass, TxtUserPass2, true))
            {
                LblFileUploadMessage.Text += "Invalid Password. ";
            }



            //Check if Company alreay exists.
            var existing = (from emp in employerController.getAllEmployers() where emp.EMPLOYER_EIN.Equals(TxtEmployerEIN.Text) select emp).FirstOrDefault();

            if (existing != null)
            {
                LblFileUploadMessage.Text += "An employer with that Id already exists. ";
            }

            return string.Empty == LblFileUploadMessage.Text;
        }

        private string CreateEmployer()
        {
            string results = string.Empty;

            string _employerState = "";
            _employerState = DdlState.SelectedItem.Text;

            int _employerTypeID = 0;                                            //00) Employer Type
            try
            {
                _employerTypeID = int.Parse(DdlEmployerType.SelectedItem.Value);
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                _employerTypeID = 0;
            }

            results += new EmployerCreation().CreateEmployer(_employerTypeID,
                TxtEmployerIrsName.Text, TxtAddress.Text, TxtCity.Text, _employerState, TxtZip.Text.ZeroPadZip(), TxtEmployerEIN.Text,
                TxtUserFname.Text, TxtUserLname.Text, TxtUserEmail.Text, TxtUserPhone.Text, TxtUserName.Text, TxtUserPass.Text, TxtEmployerDbaName.Text);
            
            //return either the list of failures, or Sucess
            if (results != string.Empty)
            {
                return results;
            }
            else
            {
                TxtEmployerIrsName.Text = string.Empty;
                TxtEmployerDbaName.Text = string.Empty;
                TxtAddress.Text = string.Empty;
                TxtCity.Text = string.Empty;
                TxtZip.Text = string.Empty;
                TxtEmployerEIN.Text = string.Empty;
                TxtUserFname.Text = string.Empty;
                TxtUserLname.Text = string.Empty;
                TxtUserEmail.Text = string.Empty;
                TxtUserPhone.Text = string.Empty;
                TxtUserName.Text = string.Empty;
                TxtUserPass.Text = string.Empty;

                return "Success!";
            }
        }
    }
}