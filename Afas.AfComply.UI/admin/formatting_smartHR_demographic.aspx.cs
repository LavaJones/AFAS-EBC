using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using log4net;

public partial class admin_smartHR_formatting_demographic : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_smartHR_formatting_demographic));

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
        List<employer> tempList = employerController.getAllEmployers();
        tempList = employerController.filterEmployerByVendor(3, tempList);

        DdlEmployer.DataSource = tempList;
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
        List<FileInfo> tempList = getFiles(_employer.EMPLOYER_ID);
        List<FileInfo> tempList2 = new List<FileInfo>();

        if (_employer.EMPLOYER_IMPORT_EMPLOYEE != null)
        {
            string fname = _employer.EMPLOYER_IMPORT_EMPLOYEE.ToLower();
            foreach (FileInfo fi in tempList)
            {
                string fname2 = fi.Name.ToLower();

                if (fname2.Contains(fname))
                {
                    tempList2.Add(fi);
                }
            }
            DdlFileList.BackColor = System.Drawing.Color.White;
        }
        else
        {
            DdlFileList.BackColor = System.Drawing.Color.Red;
        }

        DdlFileList.DataSource = tempList2;
        DdlFileList.DataTextField = "Name";
        DdlFileList.DataBind();

        DdlFileList.Items.Add("Select");
        DdlFileList.SelectedIndex = DdlFileList.Items.Count - 1;

        GvCurrentFiles.DataSource = tempList2;
        GvCurrentFiles.DataBind();
    }

    /// <summary>
    /// 1-6) Creates a LIST of all files in the FTPS folder. 
    /// </summary>
    /// <returns></returns>
    private List<FileInfo> getFiles(int _employerID)
    {
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/rawdata"));
        return directory.GetFiles().ToList<FileInfo>();
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
                new FileArchiverWrapper().ArchiveFile(filePath, _employer.ResourceId, "User Delete Smart Demographics", _employer.EMPLOYER_ID);
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
        if (FuGrossPayFile.HasFile)
        {
            string _filePath = Server.MapPath("..\\ftps\\rawdata\\");
            FileProcessing.SaveFile(FuGrossPayFile, _filePath, LblFileUploadMessage);
            FuGrossPayFile.BackColor = System.Drawing.Color.White;
            MpeWebMessage.Show();
            LitMessage.Text = "The file you selected has been uploaded.";
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

    ///
    protected void BtnProcessFile_Click(object sender, EventArgs e)
    {
        /// <summary>
        /// 3-2) Move the selected File from the RAW folder to the FTPS folder. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        bool validData = true;
        string fileName = null;
        string currFilePath = null;
        string newFilePath = null;
        string _modBy = LitUserName.Text;
        DateTime _modOn = System.DateTime.Now;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlFileList, validData);

        if (validData == true)
        {
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            int failedRecords = 0;

            fileName = DdlFileList.SelectedItem.Text;
            currFilePath = Server.MapPath("..\\ftps\\rawdata\\") + fileName;
            newFilePath = Server.MapPath("..\\ftps\\") + fileName;

            employer currEmployer = employerController.getEmployer(_employerID);
            List<Payroll_I> tempList = new List<Payroll_I>();

            try
            {
                File.Move(currFilePath, newFilePath);
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                failedRecords = 1;
            }

            if (failedRecords == 0)
            {
                MpeWebMessage.Show();
                LitMessage.Text = "The file has been SUCCESSFULLY been imported.";
                loadFTPFiles(currEmployer);
            }
            else
            {
                MpeWebMessage.Show();
                LitMessage.Text = "An error occurred while importing this specific file. This FILE probably already exists in the FTPS folder. Check the DEMOGRAPHIC FILE IMPORT for this particular EMPLOYER.";
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
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/rawdata"));
        List<FileInfo> fileList = directory.GetFiles().ToList<FileInfo>();
        GridViewRow row = (GridViewRow)GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hf = (HiddenField)row.FindControl("HfFilePath");
        Label lbl = (Label)row.FindControl("LblFileName");

        string filePath = hf.Value;
        string fileName = lbl.Text;
        char delimiter = '.';
        string[] fileType = fileName.Split(delimiter);

        PIILogger.LogPII(String.Format("Downloading smartHR Demographic Upload File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", fileName, Request.UserHostAddress, LitUserName.Text));


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