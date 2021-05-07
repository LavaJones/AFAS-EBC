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

public partial class admin_adp_payroll : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_adp_payroll));

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
        tempList = employerController.filterEmployerByVendor(4, tempList);

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

        if (_employer.EMPLOYER_IMPORT_PAYROLL != null)
        {
            string fname = _employer.EMPLOYER_IMPORT_PAYROLL.ToLower();
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
                new FileArchiverWrapper().ArchiveFile(filePath, _employer.ResourceId, "User Delete ADP Payroll", _employer.EMPLOYER_ID);
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
        int _employerID = 0;
        bool validData = true;

        if (FuGrossPayFile.HasFile)
        {
            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            string fileName = null;

            if (validData == true)
            {
                _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                employer tempEmployer = employerController.getEmployer(_employerID);
                fileName = tempEmployer.EMPLOYER_IMPORT_PAYROLL;
                string _filePath = Server.MapPath("..\\ftps\\rawdata\\");

                String outboundFileNameNotUsedYet = String.Empty;

                FileProcessing.SaveFile(FuGrossPayFile, _filePath, LblFileUploadMessage, fileName, out outboundFileNameNotUsedYet);

                FuGrossPayFile.BackColor = System.Drawing.Color.White;
                MpeWebMessage.Show();
                LitMessage.Text = "The file you selected has been uploaded.";
                loadFTPFiles(tempEmployer);
            }
            else
            {
                MpeWebMessage.Show();
                LitMessage.Text = "Please correct all the red fields.";
            }
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

        bool validData = true;                                                                      
        string filePath = null;                                           
        string fileName = null;
        string fullFilePath = null;
        int _batchID = 0;
        string _modBy = LitUserName.Text;
        DateTime _modOn = System.DateTime.Now;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlFileList, validData);


        if (validData == true)
        {
            fileName = DdlFileList.SelectedItem.Text;                                                           
            fullFilePath = filePath + fileName;
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);                                   
            fullFilePath = Server.MapPath("..\\ftps\\rawdata\\");
            int totalRecords = 0;
            int failedRecords = 0;
            System.IO.StreamReader file = null;
            employer currEmployer = employerController.getEmployer(_employerID);

            List<Payroll_I> tempList = new List<Payroll_I>();

            try
            {
                file = new System.IO.StreamReader(fullFilePath + fileName);
                string line = null;

                if (false == file.ReadLine().IsHeaderRow())
                {
                    file.Close();
                    file = new System.IO.StreamReader(fullFilePath + fileName);
                }

                while ((line = file.ReadLine()) != null)
                {
                    if (line.Trim() == null || line.Trim().Equals(String.Empty))
                        continue;
                    string _fname = null;
                    string _lname = null;
                    string _mname = null;
                    string _hours = null;
                    string _sdate = null;
                    string _edate = null;
                    string _ssn = null;
                    string _gpDesc = null;
                    string _gpExtID = null;
                    string _cdate = null;
                    string _empExtID = null;

                    totalRecords += 1;

                    string[] gp = CsvParse.SplitRow(line);
                    if (DataValidation.StringArrayContainsOnlyBlanks(gp))
                    {
                        this.Log.Info(
                                    String.Format("Skipping row for payroll processing in file {0}, all colums where blank.", fullFilePath)
                                );
                        continue;
                    }
                    if (gp.Count() == 11)
                    {
                        _fname = gp[0].Trim();
                        _mname = gp[1].Trim();
                        _lname = gp[2].Trim();
                        _hours = gp[3].Trim();
                        _sdate = gp[4].Trim();
                        _edate = gp[5].Trim();
                        _ssn = gp[6].Trim();
                        _gpDesc = gp[7].Trim();
                        _gpExtID = gp[8].Trim();
                        _cdate = gp[9].Trim();
                        _empExtID = gp[10].Trim();

                        Payroll_I pi = new Payroll_I(0, 0, 0, 0, _fname, _mname, _lname, _hours, 0, _sdate, null, _edate, null, _ssn, _gpDesc, _gpExtID, 0, _cdate, null, null, DateTime.Now, _empExtID);
                        tempList.Add(pi);
                    }
                    else
                    {
                        failedRecords = 1;
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                failedRecords = 1;
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
                if (System.IO.File.Exists(fullFilePath + fileName))
                {
                    bool success = false;
                    success = generateTextFile(tempList, currEmployer);

                    try
                    {
                        new FileArchiverWrapper().ArchiveFile(fullFilePath + fileName, currEmployer.ResourceId, "Processing File for ADP Payroll", currEmployer.EMPLOYER_ID);
                        MpeWebMessage.Show();
                        LitMessage.Text = "The file has been SUCCESSFULLY been imported.";

                        loadFTPFiles(currEmployer);
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
                Payroll_Controller.deleteImportedPayrollBatch(_batchID, _modBy, _modOn);

                MpeWebMessage.Show();
                LitMessage.Text = "An error occurred while importing this specific file. There may be an error within the document before it can be imported. Line Number: " + totalRecords.ToString();
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


    private bool generateTextFile(List<Payroll_I> _tempList, employer _currEmployer)
    {
        bool validData = true;
        string currDate = errorChecking.convertShortDate(System.DateTime.Now.ToShortDateString());
        string fileName = "ADP_" + _currEmployer.EMPLOYER_IMPORT_PAYROLL + "_" + currDate + "_" + System.DateTime.Now.Millisecond.ToString() + ".dat";
        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        bool success = false;
        string fullFilePath = Server.MapPath("..\\ftps\\") + fileName;
        if (validData == true)
        {
            int _employerID2 = int.Parse(DdlEmployer.SelectedItem.Value);
            try
            {
                using (StreamWriter sw = File.CreateText(fullFilePath))
                {
                    string AllEmpIds = string.Empty;

                    foreach (Payroll_I pi in _tempList)
                    {
                        AllEmpIds += pi.PAY_EMPLOYEE_ID + ", ";

                        string _hours_c = null;

                        _hours_c = errorChecking.formatHours(pi.PAY_I_HOURS);              

                        string line = pi.PAY_F_NAME + ",";
                        line += pi.PAY_M_NAME + ",";
                        line += pi.PAY_L_NAME + ",";
                        line += _hours_c + ",";
                        line += pi.PAY_I_SDATE + ",";
                        line += pi.PAY_I_EDATE + ",";
                        line += pi.PAY_SSN + ",";
                        line += pi.PAY_GP_DESC + ",";
                        line += pi.PAY_GP_EXT_ID + ",";
                        line += pi.PAY_I_CDATE + ",";
                        line += pi.EMPLOYEE_EXT_ID;

                        sw.WriteLine(line);
                    }

                    PIILogger.LogPII(String.Format("Generating ADP payroll text file for employees: [{0}]", AllEmpIds));
                }
                success = true;
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                success = false;
            }
        }

        return success;
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

        PIILogger.LogPII(String.Format("Downloading ADP Payroll Upload File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", fileName, Request.UserHostAddress, LitUserName.Text));

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