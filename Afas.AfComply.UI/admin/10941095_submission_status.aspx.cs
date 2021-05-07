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
using System.Xml;
using Afas.Domain;

namespace Afas.AfComply.UI.admin
{
    public partial class _10941095_submission_status : Afas.AfComply.UI.admin.AdminPageBase
    {

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            loadEmployers();
        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _employerID = 0;
            employer _employer = null;
            loadYears();

            if (DdlEmployer.SelectedItem.Text != "Select")
            {
                _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

                _employer = employerController.getEmployer(_employerID);

                int _year = 0;
                int.TryParse(DdlYears.SelectedItem.Text, out _year);
                loadSubmissions(_employer.EMPLOYER_ID, _year);
            }
            else
            {
                DdlYears.Items.Clear();
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
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

            int _year = 0;
            int.TryParse(DdlYears.SelectedItem.Text, out _year);

            loadSubmissions(_employerID, _year);
        }

        /// <summary>
        /// Load all Tax Year Employer Submissions for a specific employer and tax year.
        /// </summary>
        /// <param name="_employerID"></param>
        /// <param name="_year"></param>
        public void loadSubmissions(int _employerID, int _year)
        {
            GvCurrentReceipts.DataSource = airController.manufactureEmployerTransmissions(_employerID, _year);
            GvCurrentReceipts.DataBind();
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
            List<taxYearEmployeeTransmission> etyt = new List<taxYearEmployeeTransmission>().Where(Items => Items.createdOn > new DateTime(2018, 5, 20)).ToList();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridViewRow row = e.Row;
                Label lblRecID = (Label)row.FindControl("LblReceiptID");
                Image imgStatus = (Image)row.FindControl("ImgStatus");
                Label lblEcount = (Label)row.FindControl("LblErrorCount");
                Label lblStatus = (Label)row.FindControl("LblTransmissionStatus");
                HyperLink hlErrorReport = (HyperLink)row.FindControl("HlErrorReport");
                Label lblUtID = (Label)row.FindControl("LblUTID");
                HiddenField hfID = (HiddenField)row.FindControl("HfID");
                Literal litStatus = (Literal)row.FindControl("LitTstatus");

                int.TryParse(hfID.Value, out etytID);
                int.TryParse(lblStatus.Text, out statusID);
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
                        imgStatus.ImageUrl = "~//images//circle_red.png";
                        imgStatus.ToolTip = "Rejected";
                        litStatus.Text = "Rejected";
                        break;
                    default:
                        imgStatus.ImageUrl = "~//images//circle_orange.png";
                        imgStatus.ToolTip = "Processing";
                        litStatus.Text = "Processing";
                        break;
                }

                etyt = airController.manufactureEmployeeTransmissions(etytID);

                if (etyt.Count() > 0)
                {
                    lblEcount.Text = etyt.Count().ToString();
                    hlErrorReport.NavigateUrl = "10941095_submission_errors.aspx?status=" + etytID.ToString() + "&employer=" + etyt[0].employer_id.ToString() + "&taxyear=" + etyt[0].tax_year.ToString();
                    hlErrorReport.Enabled = true;
                }
                else
                {
                    lblEcount.Text = etyt.Count().ToString();
                    hlErrorReport.Text = "N/A";
                }

            }

        }

        protected void GvCurrentReceipts_RowEditing(object sender, GridViewUpdateEventArgs e)
        {

            GridViewRow row = (GridViewRow)GvCurrentReceipts.Rows[e.RowIndex];
            
            if (row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hfID = (HiddenField)row.FindControl("HfID");

                int etytID = 0;
                int.TryParse(hfID.Value, out etytID);

                int _employerID = 0;
                int.TryParse(DdlEmployer.SelectedItem.Value, out _employerID);

                int _year = 0;
                int.TryParse(DdlYears.SelectedItem.Text, out _year);

                if (etytID == 0 || _employerID == 0 || _year == 0)
                {
                  
                    Log.Warn(string.Format("User tried to download files, But could not find Ids. etytID: [{0}], _employerID: [{1}], _year: [{2}]", etytID, _employerID, _year));
                    return;
                }

                List<taxYearEmployerTransmission> tyetList = airController.manufactureEmployerTransmissions(_employerID, _year);
                
                taxYearEmployerTransmission transmit = tyetList.FirstOrDefault(trans => trans.tax_year_employer_transmissionID == etytID);
                if (transmit == null)
                {
                   
                    Log.Warn("User Tried to download files, but Transmit came back null.");
                    return;
                }

                string ackFileXml = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(transmit.AckFile));
                if (ackFileXml.IsNullOrEmpty() || ackFileXml.Length < 10)
                {
                  
                    Log.Warn(string.Format("User Tried to download files, but AckFile was bad. AckFile: [{0}]", ackFileXml));
                    return;
                }
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(ackFileXml);

                string Folder = System.Web.HttpContext.Current.Server.MapPath("~/ftps/transmit/");
                string fullBulkPath =Path.Combine(Folder, Path.GetFileName(transmit.BulkFile));       
                string fullManifestPath = Path.Combine(Folder, Path.GetFileName(transmit.manifestFile));       
                if (false == File.Exists(fullBulkPath) || false == File.Exists(fullManifestPath))
                {
                   
                    Log.Warn(string.Format("User Tried to download files, but Files were missing from Harddrive. BulkFileExist: [{0}], ManifestFileExist: [{1}], BulkFilePath: [{2}], ManifestFilePath: [{3}]", false == File.Exists(fullBulkPath), false == File.Exists(fullManifestPath), fullBulkPath, fullManifestPath));
                    return;
                }

                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                {
                    var xmlFileStream = new MemoryStream();
                    xml.Save(xmlFileStream);
                    string xmlFileName = "AckFile.xml";
                    xmlFileStream.Position = 0;
                    zip.AddEntry(xmlFileName, xmlFileStream);

                    zip.AddFile(fullBulkPath, string.Empty);
                    zip.AddFile(fullManifestPath, string.Empty);
                    var employ = employerController.getEmployer(_employerID);
                    string filename = _year + employ.EMPLOYER_NAME + "_Transmissions_" + etytID;
                    string attachment = "attachment; filename=" + filename.CleanFileName() + ".zip";
                    Response.ClearContent();
                    Response.BufferOutput = false;
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/zip";
                    
                    zip.Save(Response.OutputStream);

                    Response.Flush();         
                    Response.SuppressContent = true;                
                    HttpContext.Current.ApplicationInstance.CompleteRequest();                      
                    Response.End();

                }

            }

        }

        private ILog Log = LogManager.GetLogger(typeof(_10941095_submission_status));

    }

}