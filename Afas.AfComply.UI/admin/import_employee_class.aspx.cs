using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using log4net;
using Afas.AfComply.Domain;
using Afas.Application.CSV;

public partial class admin_employee_class_import : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_employee_class_import));

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
    /// 1-5) Loads a specific employer's EMPLOYEE CLASS file import.
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

            List<FileInfo> tempList = FileProcessing.getFtpEcFiles(emp.EMPLOYER_IMPORT_EC);

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

            loadFTPFiles();
        }
        else
        {
            DdlFileList.Items.Clear();
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
            if (FuEmployeeClassFile.HasFile)
            {
                int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                employer emp = employerController.getEmployer(_employerID);
                string _appendFile = emp.EMPLOYER_IMPORT_EC;
                string _filePath = Server.MapPath("..\\ftps\\eclass\\");

                String outboundFileNameNotUsedYet = String.Empty;

                FileProcessing.SaveFile(FuEmployeeClassFile, _filePath, LblFileUploadMessage, _appendFile, out outboundFileNameNotUsedYet);

                FuEmployeeClassFile.BackColor = System.Drawing.Color.White;
                MpeWebMessage.Show();
                LitMessage.Text = "The file you selected has been uploaded.";
                loadFTPFiles();
            }
            else
            {
                FuEmployeeClassFile.BackColor = System.Drawing.Color.Red;
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
    /// 3-2) Import the selected .CSV or .DAT File from the FTPS folder. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnProcessFile_Click(object sender, EventArgs e)
    {
        string filePath = Server.MapPath("..\\ftps\\eclass\\");
        string fileName = null;
        string fullFilePath = null;
        int _employerID = 0;
        bool validData = true;
        int totalRecords = 0;
        int failedRecords = 0;

        System.IO.StreamReader file = null;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlFileList, validData);
        List<classification> tempList = new List<classification>();

        if (validData == true)
        {

            fileName = DdlFileList.SelectedItem.Text;                                                           
            fullFilePath = filePath + fileName;
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

            tempList = classificationController.ManufactureEmployerClassificationList(_employerID, true); //*******

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
                    string extID = null;
                    string actID = null;
                    string fname = null;
                    string lname = null;
                    string hrStatusDesc = null;
                    string classID = null;
                    string classDesc = null;
                    string acaStatusID = null;
                    string acaStatsDesc = null;
                    DateTime _modOn = DateTime.Now;
                    string _modBy = LitUserName.Text;

                    string[] gp = CsvParse.SplitRow(line);
                    if (DataValidation.StringArrayContainsOnlyBlanks(gp))
                    {
                        this.Log.Info(
                                String.Format("Skipping row for employee class processing in file {0}, all colums where blank.", fullFilePath)
                            );
                        continue;
                    }
                    totalRecords += 1;

                    if (gp.Count() == 9)
                    {
                        extID = gp[0].ToLower().Trim(new char[] { ' ', '"' });
                        actID = gp[1].Trim(new char[] { ' ', '"' });
                        fname = gp[2].Trim(new char[] { ' ', '"' });
                        lname = gp[3].Trim(new char[] { ' ', '"' });
                        hrStatusDesc = gp[4].Trim(new char[] { ' ', '"' });
                        classID = gp[5].Trim(new char[] { ' ', '"' });
                        classDesc = gp[6].Trim(new char[] { ' ', '"' });
                        acaStatusID = gp[7].Trim(new char[] { ' ', '"' });
                        acaStatsDesc = gp[8].Trim(new char[] { ' ', '"' });

                        int _employeeID = int.Parse(actID);
                        int _acaID = acaStatusID.checkStringToIntNull();
                        int _classID = classID.checkStringToIntNull();

                        validData = classificationController.validClassification(_classID, tempList);

                        if (validData == true)
                        {
                            bool validTransaction = EmployeeController.UpdateEmployeeClassAcaStatus(_employerID, _employeeID, _classID, _acaID, _modBy, _modOn);

                            if (validTransaction == false)
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
                        new FileArchiverWrapper().ArchiveFile(fullFilePath, _employerID, "Processing File for Employee Class");
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
                LitMessage.Text = "An error occurred while importing this specific file. There may be an error within the document that must be corrected before it can be imported. Either the data is bad, the Class ID does not exist, or the Class ID is not the Employer's Classification.";
                LitMessage.Text += "Line: " + totalRecords.ToString();
            }
        }
        else
        {
            MpeWebMessage.Show();
            LitMessage.Text = "You must select an EMPLOYER and File. See the red fields.";
        }
    }



    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }



    protected void GvCurrentFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
        GridViewRow row = GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hfFilePath = (HiddenField)row.FindControl("HfFilePath");

        string filePath = hfFilePath.Value;

        if (System.IO.File.Exists(filePath))
        {
            try
            {
                new FileArchiverWrapper().ArchiveFile(filePath, _employerID, "User Delete Employee Class");
                loadFTPFiles();
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

        PIILogger.LogPII(String.Format("Downloading employee classification Upload File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", fileName, Request.UserHostAddress, LitUserName.Text));

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