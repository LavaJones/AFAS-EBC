using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Afas.AfComply.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EditEmployer : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(EditEmployer));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the EditEmployer page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=14", false);
            }
            else
            {
                loadEmployers();
                loadEmployerTypes();
                loadStates();
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

        private void loadEmployerTypes()
        {
            DdlEmployerType.DataSource = employer_typeController.getEmployerTypes();
            DdlEmployerType.DataTextField = "EMPLOYER_TYPE_NAME";
            DdlEmployerType.DataValueField = "EMPLOYER_TYPE_ID";
            DdlEmployerType.DataBind();

        }

        private void loadStates()
        {
            DdlEmployerState.DataSource = StateController.getStates();
            DdlEmployerState.DataTextField = "State_Name";
            DdlEmployerState.DataValueField = "State_ID";
            DdlEmployerState.DataBind();
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

            employer currEmployer = employerController.getEmployer(employerId);
            cofein.Text = currEmployer.EMPLOYER_EIN;

            TxtEmployerIrsName.Text = currEmployer.EMPLOYER_NAME;
            TxtEmployerDbaName.Text = currEmployer.DBAName;
            TxtEmployerAddress.Text = currEmployer.EMPLOYER_ADDRESS;
            TxtEmployerCity.Text = currEmployer.EMPLOYER_CITY;
            DdlEmployerState.ClearSelection();
            DdlEmployerState.Items.FindByValue(currEmployer.EMPLOYER_STATE_ID.ToString()).Selected = true;
            TxtEmployerZip.Text = currEmployer.EMPLOYER_ZIP;

            TxtEmployerEIN.Text = currEmployer.EMPLOYER_EIN;
            DdlEmployerType.ClearSelection();
            DdlEmployerType.Items.FindByValue(currEmployer.EMPLOYER_TYPE_ID.ToString()).Selected = true;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {

            bool validData = true;
            bool updateConfirmed = false;

            //Step 1: Validate all data.
            validData = validEmployerProfile();

            //Step 2: Process data if it's valid.
            if (validData == true)
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

                string _name = TxtEmployerIrsName.Text;
                string _dbaName = TxtEmployerDbaName.Text;
                string _address = TxtEmployerAddress.Text;
                string _city = TxtEmployerCity.Text;
                int _stateID = System.Convert.ToInt32(DdlEmployerState.SelectedItem.Value);
                string _zip = TxtEmployerZip.Text.ZeroPadZip();
                string _ein = TxtEmployerEIN.Text;

                updateConfirmed = employerController.updateEmployer(employerId, _name, _address, _city, _stateID, _zip, "", _ein, _employerTypeID, _dbaName);

                //Step 3: Update the current object. 
                if (updateConfirmed == true)
                {
                    DdlEmployer_SelectedIndexChanged(sender,e);
                    lblMsg.Text = "Success!";
                }
                else
                {
                    //Display error message. 
                    lblMsg.Text = "Update Failed";
                }
            }
            else
            {
                //Display error message.
                lblMsg.Text = "Incorrect parameters";
            }
        }

        /// <summary>
        /// 01-2) Function to validate all information on the Employer Profile.
        /// </summary>
        /// <returns></returns>
        private bool validEmployerProfile()
        {
            bool validData = true;

            //Validate the DISTRICT NAME. 
            validData = errorChecking.validateTextBoxNull(TxtEmployerIrsName, validData);
            //Validate the ADDRESS.
            validData = errorChecking.validateTextBoxNull(TxtEmployerAddress, validData);
            //Validate the CITY.
            validData = errorChecking.validateTextBoxNull(TxtEmployerCity, validData);
            //Validate the ZIP. 
            validData = errorChecking.validateTextBoxZipCode(TxtEmployerZip, validData);
            //Validate teh EIN.
            validData = errorChecking.validateTextBoxEIN(TxtEmployerEIN, validData);

            return validData;
        }

    }
}