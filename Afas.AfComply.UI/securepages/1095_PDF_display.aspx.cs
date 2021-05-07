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
using System.IO.Compression;
using Ionic.Zip;
using Afas.Domain;

namespace Afas.AfComply.UI.securepages
{

    public partial class securepages_1095_PDF_display : Afas.AfComply.UI.securepages.SecurePageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(securepages_1095_PDF_display));

        public int TaxYear
        {

            get
            {

                int year = 0;
                int.TryParse(DdlTaxYear.SelectedValue, out year);
                return year;

            }

        }

        protected override void PageLoadLoggedIn(User user, employer employer)
        {
            if (null == employer || false == employer.IrsEnabled)
            {
                Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [securepages_1095_PDF_display] which is is not yet enabled for them.");

                Response.Redirect("~/default.aspx?error=59", false);

                return;
            }

            HfUserName.Value = user.User_UserName;

            HfDistrictID.Value = user.User_District_ID.ToString();

            var years = employerController.getTaxYears();
            DdlTaxYear.DataSource = years;
            DdlTaxYear.SelectedIndex = years.IndexOf(Feature.CurrentReportingYear);
            DdlTaxYear.DataBind();

            loadPDFFiles();
            load1094PDFFiles();
        }

        public void DownloadFiles_Click(object sender, EventArgs e)
        {
            int EmployerId = int.Parse(HfDistrictID.Value);

            if (TaxYear == 0)
            {
                MpeWebMessage.Show();
                LitMessage.Text = "You must select a tax year.";
                return;
            }

            employer employ = employerController.getEmployer(EmployerId);

            string BasePath = HttpContext.Current.Server.MapPath("~\\client_content\\" + TaxYear + "\\") + employ.ResourceId.ToString() + "\\";
            if ((!Directory.Exists(BasePath)))
            {
                return;
            }

            Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile();
            zip.AddDirectory(BasePath);
            string filename = employ.EMPLOYER_NAME + "_PrintedPDFs";
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
        public void Download1094File_Click(object sender, EventArgs e)
        {
            int EmployerId = int.Parse(HfDistrictID.Value);
            if (TaxYear == 0)
            {
                MpeWebMessage.Show();
                LitMessage.Text = "You must select a tax year.";
                return;
            }

            employer employ = employerController.getEmployer(EmployerId);

            string BasePath1094 = HttpContext.Current.Server.MapPath("~\\client_content\\Pdf1094\\" + TaxYear + "\\") + employ.ResourceId.ToString() + "\\";
            if ((!Directory.Exists(BasePath1094)))
            {  
                return;
            }

            Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile();
            zip.AddDirectory(BasePath1094);
            string filename = employ.EMPLOYER_NAME + "_PrintedPDFs";
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
        protected void DdlTaxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadPDFFiles();
            load1094PDFFiles();
        }

        private void loadPDFFiles()
        {
            try
            {
                int EmployerId = int.Parse(HfDistrictID.Value);

                if (TaxYear == 0)
                {
                    return;
                }

                employer employ = employerController.getEmployer(EmployerId);

                List<FileInfo> pdfFiles = get1095C("~\\client_content\\" + TaxYear + "\\" + employ.ResourceId.ToString() + "\\");

                GvCurrentFiles.DataSource = pdfFiles;
                LitCount.Text = pdfFiles != null ? pdfFiles.Count().ToString() : "0";
                GvCurrentFiles.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error("Errors while loading  PDF Files from 1095_PDF_display.aspx page.", ex);
            }
        }

        private void load1094PDFFiles()
        {
            try
            {
                int EmployerId = int.Parse(HfDistrictID.Value);

                if (TaxYear == 0)
                {
                    return;
                }

                employer employ = employerController.getEmployer(EmployerId);

                List<FileInfo> pdfFiles = get1094C("~\\client_content\\Pdf1094\\" + TaxYear + "\\" + employ.ResourceId.ToString() + "\\");

                GvFiles.DataSource = pdfFiles;
                GvFiles.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error("Errors while loading  PDF Files from 1095_PDF_display.aspx page.", ex);
            }
        }

        protected void GvCurrentFiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            int EmployerId = int.Parse(HfDistrictID.Value);

            if (TaxYear == 0)
            {
                MpeWebMessage.Show();
                LitMessage.Text = "You must select a tax year.";
                return;
            }

            employer employ = employerController.getEmployer(EmployerId);

            string filePath = "~\\client_content\\" + TaxYear + "\\" + employ.ResourceId.ToString() + "\\";

            GridViewRow row = (GridViewRow)GvCurrentFiles.Rows[e.RowIndex];

            Label lbl = (Label)row.FindControl("LblFileName");

            string fileName = Path.GetFileNameWithoutExtension(lbl.Text).CleanFileName().Trim() + Path.GetExtension(lbl.Text).Trim();                

            string fileType = Path.GetExtension(lbl.Text).Replace(".", String.Empty).Trim();

            string appendText = "attachment; filename=" + fileName;
            Response.ContentType = "file/" + fileType;
            Response.AppendHeader("Content-Disposition", appendText);
            Response.TransmitFile(filePath + fileName);
            Response.Flush();         
            Response.SuppressContent = true;                
            HttpContext.Current.ApplicationInstance.CompleteRequest();                      
            Response.End();

        }

        protected void GvCurrentFiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvCurrentFiles.PageIndex = e.NewPageIndex;
            loadPDFFiles();
        }

        protected void GvCurrentFiles_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortBy = e.SortExpression;
            string lastSortExpression = "";
            string lastSortDirection = "ASC";
            int EmployerId = int.Parse(HfDistrictID.Value);

            if (TaxYear == 0)
            {
                return;
            }

            employer employ = employerController.getEmployer(EmployerId);

            List<FileInfo> tempList = get1095C("~\\client_content\\" + TaxYear + "\\" + employ.ResourceId.ToString() + "\\");

            if (Session["sortExp"] != null)
            {
                lastSortExpression = Session["sortExp"].ToString();
            }
            if (Session["sortDir"] != null)
            {
                lastSortDirection = Session["sortDir"].ToString();
            }

            try
            {
                if (e.SortExpression != lastSortExpression)
                {
                    lastSortExpression = e.SortExpression;
                    lastSortDirection = "ASC";
                }
                else
                {
                    if (lastSortDirection == "ASC")
                    {
                        lastSortExpression = e.SortExpression;
                        lastSortDirection = "DESC";
                    }
                    else
                    {
                        lastSortExpression = e.SortExpression;
                        lastSortDirection = "ASC";
                    }
                }

                switch (lastSortDirection)
                {
                    case "ASC":
                        switch (e.SortExpression)
                        {
                            case "Name":
                                tempList = tempList.OrderBy(o => o.Name).ToList();
                                break;
                            default:
                                break;
                        }
                        break;
                    case "DESC":
                        tempList.Reverse();
                        break;
                    default:
                        break;
                }

                Session["sortDir"] = lastSortDirection;
                Session["sortExp"] = lastSortExpression;
                GvCurrentFiles.DataSource = tempList;
                GvCurrentFiles.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error("Errors while sorting grid in 1095_PDF_display.aspx page.", ex);

            }

        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session["ValidLogon"] = null;
            Session["CurrentUser"] = null;
            Session["CurrentDistrict"] = null;
            Response.Redirect("../default.aspx", false);
        }

        protected void GvCurrentFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            int EmployerId = int.Parse(HfDistrictID.Value);

            if (TaxYear == 0)
            {
                MpeWebMessage.Show();
                LitMessage.Text = "You must select a tax year.";
                return;
            }

            employer employ = employerController.getEmployer(EmployerId);

            string filePath = "~\\client_content\\" + TaxYear + "\\" + employ.ResourceId.ToString() + "\\";

            employer currDist = (employer)Session["CurrentDistrict"];

            int index = Convert.ToInt32(e.RowIndex);

            GridViewRow row = (GridViewRow)GvCurrentFiles.Rows[e.RowIndex];

            Label Namelbl = (Label)row.FindControl("LblFileName");

            string fileName = Path.GetFileNameWithoutExtension(Namelbl.Text).CleanFileName().Trim() + Path.GetExtension(Namelbl.Text).Trim();                

            if (0 != new FileArchiverWrapper().ArchiveFile(filePath + fileName, currDist.ResourceId, "User Delete PDF 1095C Form On Front End", currDist.EMPLOYER_ID))
            {
                MpeWebMessage.Show();
                LitMessage.Text = Namelbl.Text + " has been DELETED.";
            }

            loadPDFFiles();
        }

        /// <summary>
        /// Find Employees who do not get a 1095c. 
        /// </summary>
        private void filterEmployees()
        {
            List<Employee> tempList = new List<Employee>();
            List<Employee> filteredList = new List<Employee>();
            List<int> employeesInYearlyDetail = new List<int>();

            int _employerID = int.Parse(HfDistrictID.Value);

            if (TaxYear == 0)
            {
                MpeWebMessage.Show();
                LitMessage.Text = "You must select a tax year.";
                return;
            }
            int _taxYear = TaxYear;

            employeesInYearlyDetail = airController.GetEmployeesReceiving1095(_employerID, _taxYear, true);
            tempList = EmployeeController.manufactureEmployeeList(_employerID);

            foreach (Employee emp in tempList)
            {
                bool found = false;
                if (found == false)
                {
                    filteredList.Add(emp);
                }
            }

            Session["EmployeesNotGetting1095c"] = filteredList;
        }

        protected void GvFiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            int EmployerId = int.Parse(HfDistrictID.Value);

            if (TaxYear == 0)
            {
                MpeWebMessage.Show();
                LitMessage.Text = "You must select a tax year.";
                return;
            }

            employer employ = employerController.getEmployer(EmployerId);
            
            string filePath = "~\\client_content\\" + TaxYear + "\\" + employ.ResourceId.ToString() + "\\";

            GridViewRow row = (GridViewRow)GvFiles.Rows[e.RowIndex];
            Label lbl = (Label)row.FindControl("Lbl1094FileName");

            string fileName = Path.GetFileNameWithoutExtension(lbl.Text).CleanFileName().Trim() + Path.GetExtension(lbl.Text).Trim();                

            string fileType = Path.GetExtension(lbl.Text).Replace(".", String.Empty).Trim();

            string appendText = "attachment; filename=" + fileName;
            Response.ContentType = "file/" + fileType;
            Response.AppendHeader("Content-Disposition", appendText);
            Response.TransmitFile(filePath + fileName);
            Response.Flush();         
            Response.SuppressContent = true;                
            HttpContext.Current.ApplicationInstance.CompleteRequest();                      
            Response.End();

        }

        protected void GvFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            int EmployerId = int.Parse(HfDistrictID.Value);

            if (TaxYear == 0)
            {
                MpeWebMessage.Show();
                LitMessage.Text = "You must select a tax year.";
                return;
            }

            employer employ = employerController.getEmployer(EmployerId);

            string filePath = "~\\client_content\\" + TaxYear + "\\" + employ.ResourceId.ToString() + "\\";
            
            employer currDist = (employer)Session["CurrentDistrict"];

            GridViewRow row = (GridViewRow)GvFiles.Rows[e.RowIndex];
            Label lbl = (Label)row.FindControl("Lbl1094FileName");

            string fileName = Path.GetFileNameWithoutExtension(lbl.Text).CleanFileName().Trim() + Path.GetExtension(lbl.Text).Trim();                

            if (0 != new FileArchiverWrapper().ArchiveFile(filePath + fileName, currDist.ResourceId, "User Delete PDF 1094C Form On Front End", currDist.EMPLOYER_ID))
            {
                MpeWebMessage.Show();
                LitMessage.Text = "1094 has been DELETED.";
            }

            load1094PDFFiles();

        }

        /// <summary>
        /// This function will return a list of FileInfo objects for a specific employer and file type. 
        /// </summary>
        /// <param name="_find">This is the DEM_xxxx or PAY_xxxx</param>
        /// <returns></returns>
        public List<FileInfo> get1095C(int _employerID)
        {

            string path = "~/client_content/1095PDF/" + _employerID.ToString();
            return get1095C(path);
        }

        /// <summary>
        /// This function will return a list of FileInfo objects for a specific employer and file type. 
        /// </summary>
        /// <param name="_find">This is the DEM_xxxx or PAY_xxxx</param>
        /// <returns></returns>
        public List<FileInfo> get1095C(string path)
        {
            try
            {
                string mappedPath = HttpContext.Current.Server.MapPath(path);
                DirectoryInfo directory = new DirectoryInfo(mappedPath);
                return directory.GetFiles().ToList<FileInfo>();
            }
            catch (Exception ex)
            {
                Log.Error("Errors while retriving PFD's info from 1095_PDF_display.aspx page.", ex);
                return null;
            }
        }

        public List<FileInfo> get1094C(string path)
        {
            try
            {
                string mappedPath = HttpContext.Current.Server.MapPath(path);
                DirectoryInfo directory = new DirectoryInfo(mappedPath);
                return directory.GetFiles().ToList<FileInfo>();
            }
            catch (Exception ex)
            {
                Log.Error("Errors while retriving 1094PDF's info from 1095_PDF_display.aspx page.", ex);
                return null;
            }
        }
    }
}