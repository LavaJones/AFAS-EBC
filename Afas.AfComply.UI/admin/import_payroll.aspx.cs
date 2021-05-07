using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using log4net;

using Afas.AfComply.Application;
using Afas.AfComply.Domain;

using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.Application.Archiver;

public partial class admin_payroll_import : Afas.AfComply.UI.admin.AdminPageBase
{

    private ILog Log = LogManager.GetLogger(typeof(admin_payroll_import));

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {

        Server.ScriptTimeout = 1800;

        LitUserName.Text = user.User_UserName;
        loadEmployers();
    }

    /*********************************************************************************************
     GROUP 1: All functions that load data into dropdown lists & gridviews. ****************** 
    *********************************************************************************************/
    /// <summary>
    /// 1-1) Load all existing employers into a dropdown list. 
    /// </summary>
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
    /// 1-5) Loads a specific employer's EMPLOYEE DEMOGRAPHIC file import.
    /// </summary>
    /// <param name="_employer"></param>
    private void loadFTPFiles(employer _employer)
    {
        List<FileInfo> tempList = FileProcessing.getFtpFiles(_employer.EMPLOYER_IMPORT_PAYROLL);

        if (_employer.EMPLOYER_IMPORT_EMPLOYEE != null)
        {
            DdlFileList.BackColor = System.Drawing.Color.White;
        }
        else
        {
            DdlFileList.BackColor = System.Drawing.Color.Red;
        }

        DdlFileList.DataSource = tempList;
        DdlFileList.DataTextField = "Name";
        DdlFileList.DataBind();

        DdlFileList.Items.Add("Select");
        DdlFileList.SelectedIndex = DdlFileList.Items.Count - 1;

        GvCurrentFiles.DataSource = tempList;
        GvCurrentFiles.DataBind();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = 0;
        employer _employer = null;

        if (DdlEmployer.SelectedItem.Text != "Select")
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

            _employer = employerController.getEmployer(_employerID);

            loadFTPFiles(_employer); ;
        }
        else
        {
            DdlFileList.Items.Clear();
            GvCurrentFiles.DataSource = null;
            GvCurrentFiles.DataBind();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GvCurrentFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
        GridViewRow row = GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hfFilePath = (HiddenField)row.FindControl("HfFilePath");
        employer _employer = employerController.getEmployer(_employerID);

        string filePath = hfFilePath.Value;

        if (System.IO.File.Exists(filePath))
        {
            try
            {
                new FileArchiverWrapper().ArchiveFile(filePath, _employer.ResourceId, "User Delete Payroll" , _employer.EMPLOYER_ID);
                loadFTPFiles(_employer);
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnUploadFile_Click(object sender, EventArgs e)
    {
        bool validData = true;
        string fileName;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            if (FuGrossPayFile.HasFile)
            {
                bool validFile;
                int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                employer emp = employerController.getEmployer(_employerID);
                string _appendFile = emp.EMPLOYER_IMPORT_PAYROLL;
                string _filePath = Server.MapPath("..\\ftps\\");

                String savedFileName = String.Empty;

                FileProcessing.SaveFile(FuGrossPayFile, _filePath, LblFileUploadMessage, _appendFile, out savedFileName);

                String archivePath = HttpContext.Current.Server.MapPath(Archive.ArchiveFolder);

                try
                {
                    DataValidation.FileIsForEmployer(savedFileName, emp.EMPLOYER_EIN, new FileArchiverWrapper(), emp.ResourceId, emp.EMPLOYER_ID);
                }
                catch (Exception exception)
                {

                    MpeWebMessage.Show();

                    LitMessage.Text = exception.Message;

                    if (LitMessage.Text.Contains("Did Not match"))
                    {
                        LitMessage.Text = "<br/><span style='color: red;font-weight: bold;'>This file does NOT belong to this employer. Contact your manager for assistance!</span><br/>" + LitMessage.Text;
                    }


                    return;

                }

                FuGrossPayFile.BackColor = System.Drawing.Color.White;
                MpeWebMessage.Show();
                LitMessage.Text = "The file you selected has been uploaded.";
                loadFTPFiles(emp);
            }
            else
            {
                FuGrossPayFile.BackColor = System.Drawing.Color.Red;
                LblFileUploadMessage.Text = "Please select a file";
                MpeWebMessage.Show();
                LblFileUploadMessage.Text = "Please correct all red fields.";
                LitMessage.Text = "You must select a file. Use the browse button to find a file to upload.";
            }
        }
        else
        {
            MpeWebMessage.Show();
            LblFileUploadMessage.Text = "Please correct all red fields.";
            LitMessage.Text = "You must select an EMPLOYER before you can upload a file.";
        }
    }

    protected void BtnProcessFile_Click(object sender, EventArgs e)
    {

        /// <summary>
        /// 3-2) Import the selected .DAT File from the FTPS folder. 
        ///         A) Validate that the EMPLOYER_ID and FILE is known.
        ///         B) Decision 1, is required data known.
        ///         C) Get a new BATCH_ID.
        ///         D) Read the exisiting file. 
        ///         E) Loop through the existing file.
        ///         F) Decision 2, verify that file row was split into the correct array size.
        ///         G) Encrypt the SSN. 
        ///         H) INSERT file row into the import_employee table.
        ///         I) Decision 2, was the file row inserted into the import_employee table. BREAK LOOP if fails.
        ///         J) Decision 2, bad file data caused array size to be wrong. BREAK LOOP if fails.
        ///         K) Error, unknown error occurred.
        ///         L) Decision 3, did any records fail during update.
        ///         M) Delete file if Decision 3 = true.
        ///         N) Convert/Cross Referance all data to pull in actual ID's instead of descriptions.
        ///         O) Delete all import_employee rows with new batch_id if Decision 3 = false.
        /// </summary>

        bool validData = true;                                                                      
        string filePath = Server.MapPath("..\\ftps\\");                                                
        string fileName = null;
        string fullFilePath = null;
        string _modBy = LitUserName.Text;
        DateTime _modOn = System.DateTime.Now;
        employer _employer = null;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlFileList, validData);

        if (validData == true)
        {

            fileName = DdlFileList.SelectedItem.Text;                                                           
            fullFilePath = filePath + fileName;
            bool validFile = false;
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);                                   

            _employer = employerController.getEmployer(_employerID);

            validFile = Payroll_Controller.process_PAY_I_files(_employerID, _modBy, _modOn, filePath, fileName);

            if (validFile == true)
            {

                Payroll_Controller.CrossReferenceData(_employerID, _modBy, _modOn);

                Payroll_Controller.TransferPayrollRecords(_employerID, _modBy, _modOn);

                employerController.insertEmployerCalculation(_employerID);

                MpeWebMessage.Show();
                LitMessage.Text = "The file has been SUCCESSFULLY been imported.";
                loadFTPFiles(_employer);

            }
            else
            {

                MpeWebMessage.Show();

                LitMessage.Text = "An error occurred while importing this specific file.";

            }

        }
        else
        {

            MpeWebMessage.Show();

            LitMessage.Text = "You must select a DISTRICT and File. See the red fields.";

        }

    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void BtnReScanData_Click(object sender, EventArgs e)
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            string _modBy = LitUserName.Text;
            DateTime _modOn = DateTime.Now;

            Payroll_Controller.CrossReferenceData(_employerID, _modBy, _modOn);
            Payroll_Controller.TransferPayrollRecords(_employerID, _modBy, _modOn);

            employerController.insertEmployerCalculation(_employerID);

            MpeWebMessage.Show();
            LitMessage.Text = "The data has been succesfully processed.";
        }
        else
        {
            MpeWebMessage.Show();
            LitMessage.Text = "You must select a DISTRICT and File. See the red fields.";
        }
    }


    protected void GvCurrentFiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps"));
        List<FileInfo> fileList = directory.GetFiles().ToList<FileInfo>();
        GridViewRow row = (GridViewRow)GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hf = (HiddenField)row.FindControl("HfFilePath");
        Label lbl = (Label)row.FindControl("LblFileName");

        string filePath = hf.Value;
        string fileName = lbl.Text;
        char delimiter = '.';
        string[] fileType = fileName.Split(delimiter);

        PIILogger.LogPII(String.Format("Downloading payroll Upload File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", fileName, Request.UserHostAddress, LitUserName.Text));

        string appendText = "attachment; filename=" + fileName;
        Response.ContentType = "file/" + Path.GetExtension(fileName);
        Response.AppendHeader("Content-Disposition", appendText);
        Response.TransmitFile(filePath);
        Response.Flush();         
        Response.SuppressContent = true;                
        HttpContext.Current.ApplicationInstance.CompleteRequest();                      
        Response.End();
    }
}