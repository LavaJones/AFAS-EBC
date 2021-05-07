using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace Afas.AfComply.UI.admin
{
    public partial class admin_10941095_receipt_import : Afas.AfComply.UI.admin.AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(admin_admin_float_user));

        /// <summary>
        /// Page Load 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="employer"></param>
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            LitUserName.Text = user.User_UserName;
            HfDistrictID.Value = user.User_District_ID.ToString();
            
            loadEmployers();
            loadTaxYears();
        }

        /// <summary>
        /// Load Employers into the Drop Down List. 
        /// </summary>
        private void loadEmployers()
        {
            string searchText = TxtSearchEmployer.Text;
            List<employer> tempList = employerController.getAllEmployers();
            List<employer> filteredList = null;
            if (TxtSearchEmployer.Text.Count() > 0)
            {
                filteredList = employerController.FilterEmployerBySearch(searchText, tempList);
            }
            else
            {
                filteredList = tempList;
            }
            

            DdlEmployer.DataSource = filteredList;
            DdlEmployer.DataTextField = "EMPLOYER_NAME";
            DdlEmployer.DataValueField = "EMPLOYER_ID";
            DdlEmployer.DataBind();

            DdlEmployer.Items.Add("Select");
            DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
        }

        /// <summary>
        /// Load the Tax Years into the drop down list. 
        /// </summary>
        private void loadTaxYears()
        {
            DdlYears.DataSource = employerController.getTaxYears();
            DdlYears.DataBind();

            DdlYears.Items.Add("Select");
            DdlYears.SelectedIndex = DdlYears.Items.Count - 1;
        }

        /// <summary>
        /// Display Employer Transmissions that have not had a Receipt ID imported.
        /// </summary>
        /// <param name="_ein"></param>
        /// <param name="_taxyear"></param>
        private void loadUniqueTransmissionID(string _ein, int _taxyear)
        {
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            List<taxYearEmployerTransmission> tempList = airController.manufactureEmployerTransmissions(_employerID, _taxyear);
            List<taxYearEmployerTransmission> filteredList = new List<taxYearEmployerTransmission>();

            foreach (taxYearEmployerTransmission tyet in tempList)
            {
                if (tyet.ReceiptID == null)
                {
                    filteredList.Add(tyet);
                }
            }

            DdlUniqueTransmission.DataSource = filteredList;
            DdlUniqueTransmission.DataTextField = "UniqueTransmissionId";
            DdlUniqueTransmission.DataValueField = "tax_year_employer_transmissionID";
            DdlUniqueTransmission.DataBind();

            DdlUniqueTransmission.Items.Add("Select");
            DdlUniqueTransmission.SelectedIndex = DdlUniqueTransmission.Items.Count - 1;

            GvXMLBuilds.DataSource = filteredList;
            GvXMLBuilds.DataBind();
        }


        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Logout.aspx", false);
        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetEmployerData();
        }

        protected void DdlYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool validData = true;
            int employerID = 0;
            int taxyear = 0;
            string ein = null;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlYears, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployer.SelectedItem.Value, out employerID);
                int.TryParse(DdlYears.SelectedItem.Value, out taxyear);

                employer selectedEmployer = employerController.getEmployer(employerID);
                ein = selectedEmployer.EMPLOYER_EIN.Replace("-", "");
                loadUniqueTransmissionID(ein, taxyear);
            }
        }


        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            loadEmployers();
            resetEmployerData();
        }

        private void resetEmployerData()
        {
            DdlUniqueTransmission.SelectedIndex = DdlUniqueTransmission.Items.Count - 1;
            DdlYears.SelectedIndex = DdlYears.Items.Count - 1;
            DdlUniqueTransmission.BackColor = System.Drawing.Color.White;
            DdlEmployer.BackColor = System.Drawing.Color.White;
            TxtReceiptID.Text = null;
            TxtReceiptID.BackColor = System.Drawing.Color.White;
            List<manifest> tempList = new List<manifest>();
            GvXMLBuilds.DataSource = tempList;
            GvXMLBuilds.DataBind();
        }

        /// <summary>
        /// Insert the receipt ID attached to the header ID into the database. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            bool validData = true;
            int headerid = 0;
            int employerid = 0;
            int taxyear = 0;
            string utid = null;
            string receiptid = null;
            DateTime modOn = DateTime.Now;
            string modBy = LitUserName.Text;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlUniqueTransmission, validData);
            validData = errorChecking.validateDropDownSelection(DdlYears, validData);
            validData = errorChecking.validateTextBoxLength(TxtReceiptID, validData, 17);                      

            if (validData == true)
            {
                int.TryParse(DdlUniqueTransmission.SelectedItem.Value.ToString(), out headerid);
                utid = DdlUniqueTransmission.SelectedItem.Text;
                receiptid = TxtReceiptID.Text;
                int.TryParse(DdlEmployer.SelectedItem.Value.ToString(), out employerid);
                int.TryParse(DdlYears.SelectedItem.Value.ToString(), out taxyear);
                employer selectedEmployer = employerController.getEmployer(employerid);

                bool validTransaction = employerController.updateTaxYearEmployerTransmissionReceipt(employerid, taxyear, utid, receiptid, modBy, modOn);
                //******************************************************************************************************

                if (validTransaction == true)
                {
                    MpeWebMessage.Show();
                    LitMessage.Text = "Receipt ID: " + receiptid + ", has been saved to UTID: " + utid;
                    int.TryParse(DdlYears.SelectedItem.Value, out taxyear);
                    
                    string ein = selectedEmployer.EMPLOYER_EIN.Replace("-", "");
                    loadUniqueTransmissionID(ein, taxyear);
                }
                else
                {
                    MpeWebMessage.Show();
                    LitMessage.Text = "An error occured while trying to save the data.";
                }
            }
            else
            {
                MpeWebMessage.Show();
                LitMessage.Text = "Please correct all fields highlighted in red.";
            }
        }

       


    }
}