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
using Afas.AfComply.UI.Code.AFcomply.DataUpload;

public partial class import_insurance_discrepancy : Afas.AfComply.UI.admin.AdminPageBase
{

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {

        LitUserName.Text = user.User_UserName;

        loadEmployers();

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
    /// 1-5) Loads a specific employer's Insurance Offer file import.
    /// </summary>
    /// <param name="_employer"></param>
    private void loadFTPFiles()
    {

        int _employerID = 0;
        bool validData = true;
        employer emp = null;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            emp = employerController.getEmployer(_employerID);

            List<FileInfo> tempList = FileProcessing.GetFtpInsuranceDiscrepancyFiles(emp.EMPLOYER_IMPORT_IO);

            if (emp.EMPLOYER_IMPORT_EMPLOYEE != null)
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
        else
        {
        }

    }

    protected void BtnProcessFile_Click(Object sender, EventArgs eventArgs)
    {

    }

    protected void GvCurrentFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        GridViewRow row = GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hfFilePath = (HiddenField)row.FindControl("HfFilePath");

        string filePath = hfFilePath.Value;

        if (System.IO.File.Exists(filePath))
        {
            try
            {
                new FileArchiverWrapper().ArchiveFile(filePath, int.Parse(DdlEmployer.SelectedItem.Value), "User Delete Import Insurance Discrepancy");
                loadFTPFiles();
                MpeWebMessage.Show();
                LitMessage.Text = "The file has been SUCCESSFULLY DELETED.";
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                MpeWebMessage.Show();
                LitMessage.Text = "An error occurred while trying to DELETE the file, please try again.";
            }
        }
    }

    protected void BtnUploadFile_Click(Object sender, EventArgs eventArgs)
    {
    }


    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void GvCurrentFiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/InsuranceDiscrepancy"));
        List<FileInfo> fileList = directory.GetFiles().ToList<FileInfo>();
        GridViewRow row = (GridViewRow)GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hf = (HiddenField)row.FindControl("HfFilePath");
        Label lbl = (Label)row.FindControl("LblFileName");

        String filePath = hf.Value;
        String fileName = lbl.Text;
        char delimiter = '.';
        String[] fileType = fileName.Split(delimiter);

        PIILogger.LogPII(String.Format("Downloading Insurance Discrepancy File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", fileName, Request.UserHostAddress, LitUserName.Text));

        string appendText = "attachment; filename=" + fileName;
        Response.ContentType = "file/" + Path.GetExtension(fileName);
        Response.AppendHeader("Content-Disposition", appendText);
        Response.TransmitFile(filePath);
        Response.Flush();         
        Response.SuppressContent = true;                
        HttpContext.Current.ApplicationInstance.CompleteRequest();                      
        Response.End();
    }


    protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadFTPFiles();
    }

    private ILog Log = LogManager.GetLogger(typeof(import_insurance_discrepancy));

}