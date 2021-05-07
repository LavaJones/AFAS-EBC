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
using Afas.AfComply.Domain;
using Afas.Application.CSV;

public partial class admin_hrstatus_import : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_hrstatus_import));

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
    /// 1-5) Loads a specific employer's EMPLOYEE DEMOGRAPHIC file import.
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

            List<FileInfo> tempList = FileProcessing.getFtpHrFiles(emp.EMPLOYER_IMPORT_HR);

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
                string _appendFile = emp.EMPLOYER_IMPORT_HR;
                string _filePath = Server.MapPath("..\\ftps\\hrstatus\\");

                String outboundFileNameNotUsedYet = String.Empty;

                FileProcessing.SaveFile(FuGrossPayFile, _filePath, LblFileUploadMessage, _appendFile, out outboundFileNameNotUsedYet);

                loadFTPFiles();

                MpeWebMessage.Show();
                LitMessage.Text = "The file has been SUCCESSFULLY uploaded.";
            }
            else
            {
                MpeWebMessage.Show();
                LitMessage.Text = "Please select a file to upload.";
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
        string filePath = Server.MapPath("..\\ftps\\hrstatus\\");
        string fileName = null;
        string fullFilePath = null;
        int _employerID = 0;
        bool validData = true;
        int totalRecords = 0;
        int failedRecords = 0;
        List<hrStatus> tempList = new List<hrStatus>();

        System.IO.StreamReader file = null;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlFileList, validData);

        if (validData == true)
        {
            fileName = DdlFileList.SelectedItem.Text;
            fullFilePath = filePath + fileName;
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            tempList = hrStatus_Controller.manufactureHRStatusList(_employerID);
            try
            {
                file = new System.IO.StreamReader(fullFilePath);
                string line = null;

                if (false == file.ReadLine().IsHeaderRow())
                {
                    file.Close();
                    file = new System.IO.StreamReader(fullFilePath);
                }

                while ((line = file.ReadLine()) != null)
                {
                    if (line.Trim() == null || line.Trim().Equals(String.Empty))
                        continue;
                    string recordType = null;
                    string _extID = null;
                    string _name = null;
                    string[] gp = CsvParse.SplitRow(line);
                    if (DataValidation.StringArrayContainsOnlyBlanks(gp))
                    {
                        this.Log.Info(
                                String.Format("Skipping row for hr status processing in file {0}, all colums where blank.", fullFilePath)
                            );
                        continue;
                    }

                    if (gp.Count() == 4)
                    {
                        recordType = gp[1].ToLower().Trim(new char[] { ' ', '"' }); ;
                        _name = gp[2].Trim(new char[] { ' ', '"' }); ;
                        _extID = gp[3].Trim(new char[] { ' ', '"' }); ;

                        totalRecords += 1;

                        if (recordType == "hr status")
                        {
                            int _id = hrStatus_Controller.validateHRStatus(_employerID, _extID, _name, tempList);

                            if (_id == 0)
                            {
                                failedRecords += 1;
                                break;
                            }
                        }
                        else
                        {
                            failedRecords += 1;
                            break;
                        }
                    }
                    else
                    {
                        failedRecords += 1;
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                failedRecords += 1;
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
            }
            if (failedRecords == 0)
            {
                if (System.IO.File.Exists(fullFilePath))
                {
                    try
                    {
                        new FileArchiverWrapper().ArchiveFile(fullFilePath, _employerID, "Processing File for HR Status");
                        MpeWebMessage.Show();
                        LitMessage.Text = "The file has been SUCCESSFULLY been imported.";
                        loadFTPFiles();
                    }
                    catch (Exception exception)
                    {
                        Log.Warn("Suppressing errors.", exception);
                        MpeWebMessage.Show();
                        LitMessage.Text = "The file has been SUCCESSFULLY been imported, but could not be DELETED.";
                    }
                }
            }
            else
            {
                MpeWebMessage.Show();
                LitMessage.Text = "An error occurred while importing this specific file. There may be an error within the document that must be corrected before it can be imported.";
                LitMessage.Text += "Line: " + totalRecords.ToString();
            }
        }
        else
        {
            MpeWebMessage.Show();
            LitMessage.Text = "You must select a DISTRICT and File. See the red fields.";
        }
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
                new FileArchiverWrapper().ArchiveFile(filePath, int.Parse(DdlEmployer.SelectedItem.Value), "User Delete HR Status");
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


    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void GvCurrentFiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/grosspay"));
        List<FileInfo> fileList = directory.GetFiles().ToList<FileInfo>();
        GridViewRow row = (GridViewRow)GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hf = (HiddenField)row.FindControl("HfFilePath");
        Label lbl = (Label)row.FindControl("LblFileName");

        string filePath = hf.Value;
        string fileName = lbl.Text;
        char delimiter = '.';
        string[] fileType = fileName.Split(delimiter);

        PIILogger.LogPII(String.Format("Downloading grosspay Upload File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", fileName, Request.UserHostAddress, LitUserName.Text));

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

}