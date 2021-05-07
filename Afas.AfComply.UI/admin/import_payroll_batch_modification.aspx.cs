using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using log4net;

public partial class admin_import_payroll_modification : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_import_payroll_modification));

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {
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
        List<FileInfo> tempList = FileProcessing.getFtpPayModFiles(_employer.EMPLOYER_IMPORT_PAY_MOD);

        if (_employer.EMPLOYER_IMPORT_PAY_MOD != null)
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
                new FileArchiverWrapper().ArchiveFile(filePath, _employer.ResourceId, "User Delete Payroll Batch", _employer.EMPLOYER_ID);
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

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            if (FuGrossPayFile.HasFile)
            {
                int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                employer emp = employerController.getEmployer(_employerID);
                string _appendFile = emp.EMPLOYER_IMPORT_PAY_MOD;
                string _filePath = Server.MapPath("..\\ftps\\paymod\\");

                String outboundFileNameNotUsedYet = String.Empty;

                FileProcessing.SaveFile(FuGrossPayFile, _filePath, LblFileUploadMessage, _appendFile, out outboundFileNameNotUsedYet);

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

    ///
    protected void BtnProcessFile_Click(object sender, EventArgs e)
    {
        /// <summary>
        /// 3-2) Import the selected .DAT File from the FTPS folder and update the payroll records. 
        ///         A) Validate that the EMPLOYER_ID and FILE is known.
        ///         B) Decision 1, is required data known.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        bool validData = true;                                                                      
        string filePath = Server.MapPath("..\\ftps\\paymod\\");                                        
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
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);                                   
            bool validFile = false;
            _employer = employerController.getEmployer(_employerID);

            Log.Info(String.Format("Ready to Process Pay E File. Employer Id: [{0}], modby: [{1}], modOn: [{2}], filePath: [{3}], fileName: [{4}]", _employerID, _modBy, _modOn, filePath, fileName));

            validFile = Payroll_Controller.process_PAY_E_files(_employerID, _modBy, _modOn, filePath, fileName);

            employerController.insertEmployerCalculation(_employerID);

            if (validFile == true)
            {
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





    protected void GvCurrentFiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/paymod/"));
        List<FileInfo> fileList = directory.GetFiles().ToList<FileInfo>();
        GridViewRow row = (GridViewRow)GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hf = (HiddenField)row.FindControl("HfFilePath");
        Label lbl = (Label)row.FindControl("LblFileName");

        string filePath = hf.Value;
        string fileName = lbl.Text;
        char delimiter = '.';
        string[] fileType = fileName.Split(delimiter);

        PIILogger.LogPII(String.Format("Downloading paymod Upload File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", fileName, Request.UserHostAddress, LitUserName.Text));

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