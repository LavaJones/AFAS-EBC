using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using log4net;
using Afas.AfComply.UI.Code.POCOs;
using Ionic.Zip;
using Afas.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class DownloadEmployerFile : AdminPageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(DownloadEmployerFile));
        private IList<employer> Employers = employerController.getAllEmployers();
        private List<FileInfo> files = new List<FileInfo>();
        FileArchiverWrapper Wrapper = new FileArchiverWrapper();

        //public List<EmployerFile> fileListWithEmployerName;

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            LoadEmployers();
            LoadFileLocation();
            BtnDownload.Visible = false;
        }

        private void LoadFileLocation()
        {
            DdlFileLocation.Items.Add(new ListItem("Select", "0"));
            DdlFileLocation.Items.Add(new ListItem("Demo & Payroll", "rawdata"));
            DdlFileLocation.Items.Add(new ListItem("Insurance Carrier", "inscarrier"));
            DdlFileLocation.Items.Add(new ListItem("Insurance Offer", "insoffer"));
            DdlFileLocation.SelectedValue = "0";
        }

        private void LoadEmployers()
        {
            DdlEmployer.DataSource = Employers;
            DdlEmployer.DataTextField = "EMPLOYER_NAME";
            DdlEmployer.DataValueField = "EMPLOYER_ID";
            DdlEmployer.DataBind();
            DdlEmployer.Items.Add("All Employers");
            DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGvCurrentFiles();
        }
        protected void DdlFileLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlFileLocation.SelectedItem.Text != "Select")
            {
                UpdateGvCurrentFiles();
                if (DdlFileLocation.BackColor == System.Drawing.Color.Red)
                    DdlFileLocation.BackColor = System.Drawing.Color.White;
            }
            else
            {
                DdlFileLocation.BackColor = System.Drawing.Color.Red;
                BtnDownload.Visible = false;
                GvCurrentFiles.DataSource = "";
                GvCurrentFiles.DataBind();
                LitPayrollCorrectCount.Text = "0";
            }
        }

        /// <summary>
        /// Gets all the files in RawData that belongs to the employer.
        /// </summary>
        /// <param name="_employer">The employer to get files for.</param>
        /// <returns>A list of all files for this employer</returns>
        private List<FileInfo> GetFtpRawFilesListForEmployer(employer _employer)
        {
            if (_employer == null) throw new ArgumentNullException("_employer");

            files.AddRange(FileProcessing.getFtpRawFiles(_employer.EMPLOYER_IMPORT_PAYROLL));
            files.AddRange(FileProcessing.getFtpRawFiles(_employer.EMPLOYER_IMPORT_EMPLOYEE));
            files.AddRange(FileProcessing.getFtpRawFiles(_employer.EMPLOYER_IMPORT_GP));
            files.AddRange(FileProcessing.getFtpRawFiles(_employer.EMPLOYER_IMPORT_HR));
            files.AddRange(FileProcessing.getFtpRawFiles(_employer.EMPLOYER_IMPORT_EC));
            files.AddRange(FileProcessing.getFtpRawFiles(_employer.EMPLOYER_IMPORT_IO));
            files.AddRange(FileProcessing.getFtpRawFiles(_employer.EMPLOYER_IMPORT_IC));
            files.AddRange(FileProcessing.getFtpRawFiles(_employer.EMPLOYER_IMPORT_PAY_MOD));

            return files;
        }

        /// <summary>
        /// Gets all Insurance Carrier files in RawData that belongs to the employer.
        /// </summary>
        /// <param name="_employer">The employer to get files for.</param>
        /// <returns>A list of all files for this employer</returns>
        private List<FileInfo> GetAllInsuranceCarrierFilesListForEmployer(employer _employer)
        {
            if (_employer == null) throw new ArgumentNullException("_employer");

            files.AddRange(FileProcessing.getFtpInsuranceCarrierFiles(_employer.EMPLOYER_IMPORT_PAYROLL));
            files.AddRange(FileProcessing.getFtpInsuranceCarrierFiles(_employer.EMPLOYER_IMPORT_EMPLOYEE));
            files.AddRange(FileProcessing.getFtpInsuranceCarrierFiles(_employer.EMPLOYER_IMPORT_GP));
            files.AddRange(FileProcessing.getFtpInsuranceCarrierFiles(_employer.EMPLOYER_IMPORT_HR));
            files.AddRange(FileProcessing.getFtpInsuranceCarrierFiles(_employer.EMPLOYER_IMPORT_EC));
            files.AddRange(FileProcessing.getFtpInsuranceCarrierFiles(_employer.EMPLOYER_IMPORT_IO));
            files.AddRange(FileProcessing.getFtpInsuranceCarrierFiles(_employer.EMPLOYER_IMPORT_IC));
            files.AddRange(FileProcessing.getFtpInsuranceCarrierFiles(_employer.EMPLOYER_IMPORT_PAY_MOD));

            return files;
        }


        /// <summary>
        /// Gets all Insurance offer files in RawData that belongs to the employer.
        /// </summary>
        /// <param name="_employer">The employer to get files for.</param>
        /// <returns>A list of all files for this employer</returns>
        private List<FileInfo> GetAllInsuranceOfferFilesListForEmployer(employer _employer)
        {
            if (_employer == null) throw new ArgumentNullException("_employer");

            files.AddRange(FileProcessing.GetFtpInsuranceFiles(_employer.EMPLOYER_IMPORT_PAYROLL));
            files.AddRange(FileProcessing.GetFtpInsuranceFiles(_employer.EMPLOYER_IMPORT_EMPLOYEE));
            files.AddRange(FileProcessing.GetFtpInsuranceFiles(_employer.EMPLOYER_IMPORT_GP));
            files.AddRange(FileProcessing.GetFtpInsuranceFiles(_employer.EMPLOYER_IMPORT_HR));
            files.AddRange(FileProcessing.GetFtpInsuranceFiles(_employer.EMPLOYER_IMPORT_EC));
            files.AddRange(FileProcessing.GetFtpInsuranceFiles(_employer.EMPLOYER_IMPORT_IO));
            files.AddRange(FileProcessing.GetFtpInsuranceFiles(_employer.EMPLOYER_IMPORT_IC));
            files.AddRange(FileProcessing.GetFtpInsuranceFiles(_employer.EMPLOYER_IMPORT_PAY_MOD));

            return files;
        }


        /// <summary>
        /// ///  Gets all file in specified File Location
        /// </summary>
        /// <param name="FileLocation"></param>
        /// <returns> List of file info </returns>
        private List<FileInfo> GetAllFTPFiles(string FileLocation)
        {
            return FileProcessing.GetAllFilesByFolderName(FileLocation).ToList();
        }

        /// <summary>
        /// ///  Gets all file belongs to specidfic Employer in specified File Location
        /// </summary>
        /// <param name="FileLocation"></param>
        /// <param name="Employer"></param>
        /// <returns> List of file info </returns>
        private List<FileInfo> GetFTPFiles(string FileLocation, string Employer)
        {

            List<FileInfo> files = new List<FileInfo>();
            employer objEmployer = employerController.getEmployer(int.Parse(DdlEmployer.SelectedItem.Value));
            var employer = Employers.Where(e => e.EMPLOYER_ID == objEmployer.EMPLOYER_ID).ToList();

            switch (FileLocation)
            {
                case "rawdata":
                    return GetFtpRawFilesListForEmployer(objEmployer);

                case "inscarrier":
                    return GetAllInsuranceCarrierFilesListForEmployer(objEmployer);

                case "insoffer":
                    return GetAllInsuranceOfferFilesListForEmployer(objEmployer);

            }
            return files;

        }

        /// <summary>
        /// archives the file and update the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvCurrentFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var row = GvCurrentFiles.Rows[e.RowIndex];
            var hfFilePath = (HiddenField)row.FindControl("HfFilePath");
            var hfEmployerId = (HiddenField)row.FindControl("HfEmployerId");
            var hfFileName = (HiddenField)row.FindControl("HfFileName");
            var employer = employerController.getEmployer(int.Parse(hfEmployerId.Value));
            var filePath = hfFilePath.Value;

            //Delete the file from the webserver.
            if (File.Exists(filePath))
            {
                try
                {
                    //archive the file
                    Wrapper.ArchiveFile(filePath, employer.ResourceId, "User Deleted" + hfFileName.Value, employer.EMPLOYER_ID);

                    UpdateGvCurrentFiles();
                }
                catch (Exception exception)
                {
                    Log.Warn("Suppressing errors.", exception);
                }
            }
        }

        /// <summary>
        /// Downloads the file. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvCurrentFiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var row = GvCurrentFiles.Rows[e.RowIndex];
            var hf = (HiddenField)row.FindControl("HfFilePath");
            var lbl = (Label)row.FindControl("LblFileName");
            var filePath = hf.Value;
            var fileName = lbl.Text.Replace(",", "");
            //log the access
            PIILogger.LogPII(String.Format("Downloading Uploaded File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", fileName, Request.UserHostAddress, ((Literal)Master.FindControl("LitUserName")).Text));
            var appendText = "attachment; filename=" + fileName;
            Response.ContentType = "file/" + Path.GetExtension(fileName);
            Response.AppendHeader("Content-Disposition", appendText);
            Response.TransmitFile(filePath);
            // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
            Response.Flush(); // Sends all currently buffered output to the client.
            Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            Response.End();
        }

        private void UpdateGvCurrentFiles()
        {
            List<employer> lstEmployer = new List<employer>();
            if (DdlEmployer.SelectedItem.Value == "All Employers")
            {
                files = GetAllFTPFiles(DdlFileLocation.SelectedValue);
                lstEmployer = employerController.getAllEmployers() as List<employer>;
            }
            else
            {
                employer objEmployer = employerController.getEmployer(int.Parse(DdlEmployer.SelectedItem.Value));
                lstEmployer = Employers.Where(e => e.EMPLOYER_ID == objEmployer.EMPLOYER_ID).ToList();
                files = GetFTPFiles(DdlFileLocation.SelectedValue, DdlEmployer.SelectedItem.Text);
            }
            if (files.Count == 0)
            {
                BtnDownload.Visible = false;
                GvCurrentFiles.DataSource = "";
                GvCurrentFiles.DataBind();
                LitPayrollCorrectCount.Text = files.Count.ToString();
            }
            else
            {
                BtnDownload.Visible = true;
                BindToGrid(files, lstEmployer);
            }
        }

        private void BindToGrid(IList<FileInfo> files, IList<employer> employers)
        {
            List<EmployerFile> fileListWithEmployerName = MapEmployerAndFiles(employers, files);
            Session["fileListWithEmployerName"] = fileListWithEmployerName;

            GvCurrentFiles.DataSource = fileListWithEmployerName.OrderByDescending(t => t.CreationTimeUtc).ToList();
            GvCurrentFiles.DataBind();
            if (fileListWithEmployerName.Count == 0)
                BtnDownload.Visible = false;
            LitPayrollCorrectCount.Text = fileListWithEmployerName.Count.ToString();
            AddSortingDirectionsToSessions(GvCurrentFiles);
        }

        private void AddSortingDirectionsToSessions(GridView gv)
        {
            foreach (var column in gv.Columns)
            {
                string ColumnName = ((DataControlField)column).SortExpression.ToString();
                if (!string.IsNullOrEmpty(ColumnName))
                {
                    Session[ColumnName] = ColumnName + " " + "ASC";
                }
            }

        }

        protected void ComponentGridView_Sorting(object sender, GridViewSortEventArgs e)
        {

            List<EmployerFile> fileListWithEmployerName = (List<EmployerFile>)Session["fileListWithEmployerName"];

            if (fileListWithEmployerName != null)
            {
                switch (ConvertSortDirection(e))
                {
                    case "ASC":
                        GvCurrentFiles.DataSource = fileListWithEmployerName.OrderBy(fl => e.SortExpression);

                        break;
                    case "DESC":
                        GvCurrentFiles.DataSource = fileListWithEmployerName.OrderBy(fl => e.SortExpression).Reverse();
                        break;
                }

                GvCurrentFiles.DataBind();
                GvCurrentFiles.UseAccessibleHeader = true;


            }
        }
        private string ConvertSortDirection(GridViewSortEventArgs e)
        {
            string sortDirection = string.Empty;
            string[] sortData = Session[e.SortExpression].ToString().Trim().Split(' ');
            if (e.SortExpression == sortData[0])
            {
                if (sortData[1] == "ASC")
                {
                    sortDirection = "DESC";
                    Session[e.SortExpression] = e.SortExpression + " " + "DESC";

                }
                else
                {
                    sortDirection = "ASC";
                    Session[e.SortExpression] = e.SortExpression + " " + "ASC";
                }
            }
            else
            {
                sortDirection = "ASC";
                Session[e.SortExpression] = e.SortExpression + " " + "ASC";
            }

            return sortDirection;
        }

        private static List<EmployerFile> MapEmployerAndFiles(IList<employer> employers, IList<FileInfo> files)
        {
            return new EmployerFileService().ResolveEmployerNames(employers, files).ToList();
        }

        protected void BtnDownload_Click(object sender, EventArgs e)
        {
            int employerId = 0;
            employer employer = employerController.getEmployer(employerId);
            List<FileInfo> files = new List<FileInfo>();
            if (DdlEmployer.SelectedItem.Value == "All Employers")
                files = GetAllFTPFiles(DdlFileLocation.SelectedValue);
            else
                files = GetFTPFiles(DdlFileLocation.SelectedValue, DdlEmployer.SelectedItem.Text);
            using (var zip = new ZipFile())
            {
                var filenames = new List<string>();
                foreach (FileInfo file in files)
                {
                    filenames.Add(file.DirectoryName + '\\' + file.Name);
                }

                zip.AddFiles(filenames, false, "");

                string filename = DdlEmployer.SelectedItem.Text + "_ClientWork_" + DateTime.Now.ToShortDateString();

                string attachment = "attachment; filename=" + filename.CleanFileName() + ".zip";
                Response.ClearContent();
                Response.BufferOutput = false;
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/zip";

                zip.Save(Response.OutputStream);

                // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
                Response.Flush(); // Sends all currently buffered output to the client.
                Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                Response.End();
            }
        }
        protected void CbCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GvCurrentFiles.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");

                if (CbCheckAll.Checked == true)
                {
                    cb.Checked = true;
                }
                else
                {
                    cb.Checked = false;
                }
            }
        }
        protected void BtnDelete_Click(object sender, EventArgs e)
        {

            foreach (GridViewRow row in GvCurrentFiles.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");
                if (cb.Checked == true)
                {

                    var hfFilePath = (HiddenField)row.FindControl("HfFilePath");
                    var hfEmployerId = (HiddenField)row.FindControl("HfEmployerId");
                    var hfFileName = (HiddenField)row.FindControl("HfFileName");
                    var employer = employerController.getEmployer(int.Parse(hfEmployerId.Value));
                    var filePath = hfFilePath.Value;

                    //Delete the file from the webserver.
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            //archive the file
                            Wrapper.ArchiveFile(filePath, employer.ResourceId, "User Deleted" + hfFileName.Value, employer.EMPLOYER_ID);


                        }
                        catch (Exception exception)
                        {
                            Log.Warn("Suppressing errors.", exception);
                        }
                    }



                }
            }
            UpdateGvCurrentFiles();
            LblMessage.Text = "Files Deleted Sucessfully";

        }

    }

}