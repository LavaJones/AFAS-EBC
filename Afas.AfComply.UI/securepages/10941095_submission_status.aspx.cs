using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Hosting;
using System.Threading.Tasks;
using Afas.AfComply.Domain;
using log4net;
using Afas.Domain;

namespace Afas.AfComply.UI.securepages
{
    public partial class _10941095_submission_status : Afas.AfComply.UI.securepages.SecurePageBase
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="user"></param>
        /// <param name="employer"></param>
        protected override void PageLoadLoggedIn(User user, employer employer)
        {

            if (null == employer || false == employer.IrsEnabled)
            {

                Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [10941095_submission_status] which is is not yet enabled for them.");
                Response.Redirect("~/default.aspx?error=56", false);
                return;
            }

            loadYears();
            HfUserName.Value = user.User_UserName;

            HfDistrictID.Value = user.User_District_ID.ToString();
            int _year = 0;
            int.TryParse(DdlYears.SelectedItem.Text, out _year);

            loadSubmissions(employer.EMPLOYER_ID, _year);
        }

        /// <summary>
        /// Load the current Tax Years in to the drop down list.
        /// </summary>
        private void loadYears()
        {
            int lastYear = DateTime.Now.AddYears(-1).Year;
            DdlYears.DataSource = employerController.getTaxYears();
            DdlYears.DataBind();
            DdlYears.Items.Add("Select");
            errorChecking.setDropDownList(DdlYears, lastYear);
        }

        /// <summary>
        /// Log User out.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Logout.aspx", false);
        }

        /// <summary>
        /// Reload Tax Year Employer Submission data based on the tax year.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DdlYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            employer currEmployer = (employer)Session["CurrentDistrict"];
            int _year = 0;
            int.TryParse(DdlYears.SelectedItem.Text, out _year);

            loadSubmissions(currEmployer.EMPLOYER_ID, _year);
        }

        /// <summary>
        /// Load all Tax Year Employer Submissions for a specific employer and tax year.
        /// </summary>
        /// <param name="_employerID"></param>
        /// <param name="_year"></param>
        public void loadSubmissions(int _employerID, int _year)
        {
            Log.Info("loadSubmissions started");
            Errors1094.Visible = false;
            Errors1095.Visible = false;

            List<taxYearEmployerTransmission> transmissions = airController.manufactureEmployerTransmissions(_employerID, _year);
            List<taxYearEmployerTransmission> DisplayTransmissions = new List<taxYearEmployerTransmission>();

            Log.Info("transmissions loaded from DB");

            transmissions = transmissions.Where(item => item.ReceiptID.IsNullOrEmpty() == false)
                .OrderBy(item => item.createdOn).ToList();

            Log.Info("transmissions filtered");

            if (transmissions.Count > 0)
            {

                if (2017 == _year)
                {
                    Log.Info("2017 logic hit");
                    DisplayTransmissions.Add(
                        transmissions
                            .Where(item => item.TransmissionType.ToUpper() == "O")
                            .OrderBy(item => item.createdOn)
                            .FirstOrDefault()
                        );

                    DisplayTransmissions.AddRange(
                        transmissions
                            .Where(item => item.createdOn > new DateTime(2018, 8, 10))
                            .ToList()
                            );
                }
                else
                {
                    DisplayTransmissions.AddRange(transmissions);
                }

                Log.Info("Displaying Transmissions: " + DisplayTransmissions.Count());

                DisplayTransmissions = DisplayTransmissions.OrderBy(item => item.createdOn).ToList();
                GvCurrentReceipts.DataSource = DisplayTransmissions;
                GvCurrentReceipts.DataBind();


                transmissions = transmissions.OrderByDescending(item => item.createdOn).ToList();


                var item1094 = transmissions.Where(item => item.Is1094Only == true).FirstOrDefault();

                if (item1094 != null
                        && item1094.transmission_status_code_id != 1
                        && item1094.transmission_status_code_id != 3

                        && false == item1094.AckFile.IsNullOrEmpty())
                {
                    Log.Info("Displaying 1094 errors");

                    var etyt = airController.manufactureEmployeeTransmissions(item1094.tax_year_employer_transmissionID);

                    LblErrorCount1094.Text = etyt.Count().ToString();

                    HlErrorReport1094.NavigateUrl = "10941095_submission_errors.aspx?status=" + item1094.tax_year_employer_transmissionID.ToString() + "&taxyear=" + item1094.tax_year.ToString();
                    HlErrorReport1094.Enabled = true;

                    Errors1094.Visible = true;
                }
                else
                {
                    Errors1094.Visible = false;
                }

                var item1095 = transmissions.Where(item => item.Is1094Only == false).FirstOrDefault();

                if (item1095 != null
                        && item1095.transmission_status_code_id != 1
                        && item1095.transmission_status_code_id != 3

                        && false == item1095.AckFile.IsNullOrEmpty())
                {
                    Log.Info("Displaying 1095 errors");

                    var etyt = airController.manufactureEmployeeTransmissions(item1095.tax_year_employer_transmissionID);

                    LblErrorCount1095.Text = etyt.Count().ToString();

                    HlErrorReport1095.NavigateUrl = "10941095_submission_errors.aspx?status=" + item1095.tax_year_employer_transmissionID.ToString() + "&taxyear=" + item1095.tax_year.ToString();
                    HlErrorReport1095.Enabled = true;

                    Errors1095.Visible = true;
                }
                else
                {
                    Errors1095.Visible = false;
                }

                if (item1095 != null)
                {
                    string status = "Processing";

                    if (item1095.transmission_status_code_id == 1)
                    {
                        status = "Accepted";
                    }

                    if (item1095.transmission_status_code_id == 2)
                    {
                        status = "Accepted with Errors";
                    }

                    litStatus.Text = status;

                }
            }
        }

        /// <summary>
        /// Changes some values based on Gridview Row Data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvCurrentReceipts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string receiptID = null;
            string utID = null;
            int etytID = 0;
            int statusID = 0;
            List<taxYearEmployeeTransmission> etyt = new List<taxYearEmployeeTransmission>();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridViewRow row = e.Row;
                Label lblRecID = (Label)row.FindControl("LblReceiptID");
                Image imgStatus = (Image)row.FindControl("ImgStatus");
                HiddenField lblStatus = (HiddenField)row.FindControl("LblTransmissionStatus");
                Label lblUtID = (Label)row.FindControl("LblUTID");
                HiddenField hfID = (HiddenField)row.FindControl("HfID");
                Literal litStatus = (Literal)row.FindControl("LitTstatus");

                int.TryParse(hfID.Value, out etytID);
                int.TryParse(lblStatus.Value, out statusID);
                receiptID = lblRecID.Text;
                utID = lblUtID.Text;

                switch (statusID)
                {
                    case 1:
                        imgStatus.ImageUrl = "~//images//circle_green.png";
                        imgStatus.ToolTip = "Accepted";
                        litStatus.Text = "Accepted";
                        break;
                    case 2:  
                        imgStatus.ImageUrl = "~//images//circle_green.png";
                        imgStatus.ToolTip = "Accepted with Errors";
                        litStatus.Text = "Accepted with Errors";
                        break;
                    case 3:
                        imgStatus.ImageUrl = "~//images//circle_orange.png";
                        imgStatus.ToolTip = "Processing";
                        litStatus.Text = "Processing";
                        break;
                    default:
                        imgStatus.ImageUrl = "~//images//circle_orange.png";
                        imgStatus.ToolTip = "Processing";
                        litStatus.Text = "Processing";
                        break;
                }

                etyt = airController.manufactureEmployeeTransmissions(etytID);

                /*
                    if (etyt.Count() > 0)
                    {
                        lblEcount.Text = etyt.Count().ToString();
                        hlErrorReport.NavigateUrl = "10941095_submission_errors.aspx?status=" + etytID.ToString() + "&taxyear=" + etyt[0].tax_year.ToString();
                        hlErrorReport.Enabled = true;
                    }
                    else
                    {
                        lblEcount.Text = etyt.Count().ToString();
                        hlErrorReport.Text = "N/A";
                    }
                */
            }

        }


        private ILog Log = LogManager.GetLogger(typeof(_10941095_submission_status));

    }

}