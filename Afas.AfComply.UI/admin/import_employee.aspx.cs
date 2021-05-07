using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;

using log4net;

using Afas.AfComply.Application;
using Afas.AfComply.Domain;

using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.Application.Archiver;

public partial class admin_employee_import : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_employee_import));

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
    /// 1-3) Loads a specific employer's Plan Years into a dropdown list. 
    /// </summary>
    /// <param name="_employerID"></param>
    private void loadPlanYears(int _employerID)
    {
        DdlPlanYear.DataSource = PlanYear_Controller.getEmployerPlanYear(_employerID);
        DdlPlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
        DdlPlanYear.DataValueField = "PLAN_YEAR_ID";
        DdlPlanYear.DataBind();

        DdlPlanYear.Items.Add("Select");
        DdlPlanYear.SelectedIndex = DdlPlanYear.Items.Count - 1;
    }

    /// <summary>
    /// 1-4) Loads a specific employer's Employee Types into a dropdown list. 
    /// </summary>
    /// <param name="_employerID"></param>
    private void loadEmployeeTypes(int _employerID)
    {
        DdlEmployeeType.DataSource = EmployeeTypeController.getEmployeeTypes(_employerID);
        DdlEmployeeType.DataTextField = "EMPLOYEE_TYPE_NAME";
        DdlEmployeeType.DataValueField = "EMPLOYEE_TYPE_ID";
        DdlEmployeeType.DataBind();

        DdlEmployeeType.Items.Add("Select");
        DdlEmployeeType.SelectedIndex = DdlEmployeeType.Items.Count - 1;
    }

    private void loadACAstatus()
    {

        DdlACAStatus.DataSource = classificationController.getACAstatusList();
        DdlACAStatus.DataTextField = "ACA_STATUS_NAME";
        DdlACAStatus.DataValueField = "ACA_STATUS_ID";
        DdlACAStatus.DataBind();

        DdlACAStatus.Items.Add("Select");
        DdlACAStatus.SelectedIndex = DdlACAStatus.Items.Count - 1;
    }

    private void loadEmployeeClass(int _employerID)
    {
        List<classification> tempList = classificationController.ManufactureEmployerClassificationList(_employerID, true);
        DdlEmployeeClass.DataSource = tempList;
        DdlEmployeeClass.DataTextField = "CLASS_DESC";
        DdlEmployeeClass.DataValueField = "CLASS_ID";
        DdlEmployeeClass.DataBind();
        DdlEmployeeClass.Items.Add("Select");
        DdlEmployeeClass.SelectedIndex = DdlEmployeeClass.Items.Count - 1;
    }

    /// <summary>
    /// 1-5) Loads a specific employer's EMPLOYEE DEMOGRAPHIC file import.
    /// </summary>
    /// <param name="_employer"></param>
    private void loadFTPFiles(employer _employer)
    {
        List<FileInfo> tempList = FileProcessing.getFtpFiles(_employer.EMPLOYER_IMPORT_EMPLOYEE);

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




    /*********************************************************************************************
     GROUP 2: All dropdown list SelectedIndex Change Functions. ********************************** 
    *********************************************************************************************/
    /// <summary>
    /// 2-1) When the Employer is changed. 
    ///         - A) Get the new EMPLOYER ID.
    ///         - B) Get the new EMPLOYER OBJECT.
    ///         - C) Load all EMPLOYER - EMPLOYEE TYPES.
    ///         - D) Load all EMPLOYER - MEASUREMENT PERIODS.
    ///         - E) Load all EMPLOYER - PLAN YEARS.
    ///         - F) Load all EMPLOYER - DEMOGRAPHIC FILES (FTP FOLDER).
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

            loadEmployeeTypes(_employerID);

            loadPlanYears(_employerID);

            loadFTPFiles(_employer);

            loadACAstatus();

            loadEmployeeClass(_employerID);
        }
        else
        {
            DdlEmployeeType.Items.Clear();
            DdlPlanYear.Items.Clear();
            DdlFileList.Items.Clear();
            DdlEmployeeClass.Items.Clear();
            DdlACAStatus.Items.Clear();
            GvCurrentFiles.DataSource = null;
            GvCurrentFiles.DataBind();
        }
    }



    /*********************************************************************************************
    *****  GROUP 3: All File Import/Processing Functions ***************************************** 
    *********************************************************************************************/
    /// <summary>
    /// 3-1) This will upload a file to the FTPS folder. This should be automated from the 
    /// payroll companies, but incase we ever need to manually upload a file, this will allow 
    /// for it. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnUploadFile_Click(object sender, EventArgs e)
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {

            if (FuGrossPayFile.HasFile)
            {

                int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

                employer emp = employerController.getEmployer(_employerID);

                String _appendFile = emp.EMPLOYER_IMPORT_EMPLOYEE;
                String _filePath = Server.MapPath("..\\ftps\\");

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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnProcessFile_Click(object sender, EventArgs e)
    {
        bool validData = true;                                                                      
        string filePath = Server.MapPath("..\\ftps\\");                                                
        string fileName = null;
        string fullFilePath = null;
        string _modBy = LitUserName.Text;
        DateTime _modOn = System.DateTime.Now;
        int _planYearID = 0;
        int _employeeTypeID = 0;
        int _classID = 0;
        int _acaStatusID = 0;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlFileList, validData);

        if (CbInitialImport.Checked == true)
        {
            validData = errorChecking.validateDropDownSelection(DdlACAStatus, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeClass, validData);
            validData = errorChecking.validateDropDownSelection(DdlEmployeeType, validData);
            validData = errorChecking.validateDropDownSelection(DdlPlanYear, validData);
        }

        if (validData == true)
        {

            fileName = DdlFileList.SelectedItem.Text;
            fullFilePath = filePath + fileName;
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            bool validFile = false;

            if (CbInitialImport.Checked == true)
            {
                _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
                _employeeTypeID = int.Parse(DdlEmployeeType.SelectedItem.Value);
                _classID = int.Parse(DdlEmployeeClass.SelectedItem.Value);
                _acaStatusID = int.Parse(DdlACAStatus.SelectedItem.Value);
            }

            LitMessage.Text = String.Empty;

            validFile = EmployeeController.ProcessDemographicImportFiles(_employerID, _modBy, _modOn, filePath, fileName);

            if (validFile == true)
            {

                Boolean crossReferenceDataSuccess = EmployeeController.CrossReferenceDemographicImportTableData(_employerID, _planYearID, _classID, _acaStatusID, _employeeTypeID);

                if (crossReferenceDataSuccess == false)
                {

                    this.Log.Warn("EmployeeController.CrossReferenceData_DEM_I_data returned false!");

                    LitMessage.Text += "EmployeeController.CrossReferenceData_DEM_I_data returned false!";

                }

                Boolean transferDemIDataSuccess = EmployeeController.TransferDemographicImportTableData(_employerID, _modBy, CbInitialImport.Checked, true);

                if (transferDemIDataSuccess == false)
                {

                    this.Log.Warn("EmployeeController.transfer_DEM_I_data returned false!");

                    LitMessage.Text += "EmployeeController.transfer_DEM_I_data returned false!";

                }

                employerController.insertEmployerCalculation(_employerID);

                MpeWebMessage.Show();
                LitMessage.Text += "The file has been SUCCESSFULLY been imported.";
                loadFTPFiles(employerController.getEmployer(_employerID));

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

            LitMessage.Text = "Please correct all of the fields in RED.";

        }

    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }


    protected void CbInitialImport_CheckedChanged(object sender, EventArgs e)
    {
        if (CbInitialImport.Checked == true)
        {
            PnlInitial.Visible = true;
        }
        else
        {
            PnlInitial.Visible = false;
        }
    }


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
                new FileArchiverWrapper().ArchiveFile(filePath, _employer.ResourceId, "User Delete Employee Demographic", _employer.EMPLOYER_ID);
                loadFTPFiles(_employer);
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
            }
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

        PIILogger.LogPII(String.Format("Downloading employee Upload File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", fileName, Request.UserHostAddress, LitUserName.Text));

        string appendText = "attachment; filename=" + fileName;
        Response.ContentType = "file/" + Path.GetExtension(fileName);
        Response.AppendHeader("Content-Disposition", appendText);
        Response.TransmitFile(filePath);
        Response.Flush();         
        Response.SuppressContent = true;                
        HttpContext.Current.ApplicationInstance.CompleteRequest();                      
        Response.End();
    }

    protected void BtnRescanAlerts_Click(object sender, EventArgs e)
    {
        int _employerID = 0;
        bool validData = true;
        string _modBy = LitUserName.Text;

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
}