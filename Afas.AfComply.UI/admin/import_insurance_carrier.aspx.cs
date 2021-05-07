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
using Afas.Application.CSV;
using Afas.Application.Archiver;

public partial class admin_ins_import : Afas.AfComply.UI.admin.AdminPageBase
{

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {

        Server.ScriptTimeout = 1800;

        LitUserName.Text = user.User_UserName;
        loadEmployers();
        loadCarriers();
        loadTaxYear();

    }

    /// <summary>
    /// Loads all Employers into the Drop Down List. 
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
    /// Loads the only current Tax Year being evaluated.
    /// </summary>
    private void loadTaxYear()
    {

        List<int> taxYears = employerController.getTaxYears();
        DdlTaxYear.DataSource = taxYears;
        DdlTaxYear.DataBind();
        DdlTaxYear.Items.Add("Select");
        DdlTaxYear.SelectedIndex = DdlTaxYear.Items.Count - 1;

    }

    /// <summary>
    /// Loads all available Insurance Carriers.
    /// </summary>
    private void loadCarriers()
    {

        DdlCarrier.DataSource = insuranceController.manufactureInsuranceCarriers();
        DdlCarrier.DataTextField = "CARRIER_NAME";
        DdlCarrier.DataValueField = "CARRIER_ID";
        DdlCarrier.DataBind();

        DdlCarrier.Items.Add("Select");
        DdlCarrier.SelectedIndex = DdlCarrier.Items.Count - 1;

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

            List<FileInfo> tempList = FileProcessing.getFtpInsuranceCarrierFiles(emp.EMPLOYER_IMPORT_IC);

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

    /// <summary>
    /// Reload the files that were uploaded by the employer as the Employer name has changed. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadFTPFiles();
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

                if (emp.EMPLOYER_IMPORT_IC != null)
                {

                    String _appendFile = emp.EMPLOYER_IMPORT_IC;
                    String _filePath = Server.MapPath("..\\ftps\\inscarrier\\");

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

                    loadFTPFiles();

                    MpeWebMessage.Show();
                    LitMessage.Text = "The file has been SUCCESSFULLY uploaded.";

                }
                else
                {

                    MpeWebMessage.Show();

                    LitMessage.Text = DdlEmployer.SelectedItem.Text + " has not had their Insurance Carrier Import process setup.";

                }
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

    protected void BtnProcessFileAndTransfer_Click(object sender, EventArgs e)
    {
        this.BtnProcessFile_Click(sender, e);

        this.BtnValidateEmployees_Click(sender, e);
        
        this.BtnValidateDependents_Click(sender, e);

        this.BtnInvalidEmployees_Click(sender, e);

        this.BtnTransferRecords_Click(sender, e);
    
    }

    protected void BtnProcessFile_Click(object sender, EventArgs e)
    {
        string errorLines = "";
        String filePath = Server.MapPath("..\\ftps\\inscarrier\\");
        String fileName = null;
        String fullFilePath = null;
        int _employerID = 0;
        int _carrierID = 0;
        Boolean validData = true;
        int totalRecords = 0;
        int failedRecords = 0;
        String _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;

        insurance_coverage_template currTemplate = null;

        List<insurance_coverage> tempList = new List<insurance_coverage>();

        System.IO.StreamReader file = null;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlCarrier, validData);
        validData = errorChecking.validateDropDownSelection(DdlFileList, validData);
        validData = errorChecking.validateDropDownSelection(DdlTaxYear, validData);

        if (validData == true)
        {

            fileName = DdlFileList.SelectedItem.Text;
            fullFilePath = filePath + fileName;
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            _carrierID = int.Parse(DdlCarrier.SelectedItem.Value);

            currTemplate = insuranceController.manufactureInsuranceCoverageTemplate(_carrierID);
            int count = 0;
            try
            {

                file = new System.IO.StreamReader(fullFilePath);
                String line = null;

                if (false == file.ReadLine().IsHeaderRow())
                {

                    file.Close();
                    file = new System.IO.StreamReader(fullFilePath);
                    count++;

                }

                int taxYear = 0;
                taxYear = int.Parse(DdlTaxYear.SelectedItem.Text);

                while ((line = file.ReadLine()) != null)
                {
                    count++;

                    if (line.Trim() == null || line.Trim().Equals(String.Empty))
                        continue;

                    string dependentLink = null;
                    string fname = null;
                    string mname = null;
                    string lname = null;
                    String ssn = null;
                    DateTime? dob = null;
                    string jan = null;
                    bool _jan = false;
                    string feb = null;
                    bool _feb = false;
                    string mar = null;
                    bool _mar = false;
                    string apr = null;
                    bool _apr = false;
                    string may = null;
                    bool _may = false;
                    string june = null;
                    bool _june = false;
                    string july = null;
                    bool _july = false;
                    string aug = null;
                    bool _aug = false;
                    string sep = null;
                    bool _sep = false;
                    string oct = null;
                    bool _oct = false;
                    string nov = null;
                    bool _nov = false;
                    string dec = null;
                    bool _dec = false;
                    string all12 = null;
                    bool _all12 = false;
                    String subscriber = null;
                    Boolean _subscriber = false;
                    string _address = null;
                    string _city = null;
                    string _state = null;
                    string _zip = null;

                    String[] gp = CsvParse.SplitRow(line);

                    if (DataValidation.StringArrayContainsOnlyBlanks(gp))
                    {

                        this.Log.Debug(
                                String.Format("Skipping row for insurance carrier processing in file {0}, all colums where blank.", fullFilePath)
                            );

                        continue;

                    }

                    if (gp.Count() == currTemplate.ICT_COLUMNS)
                    {

                        dependentLink = gp[currTemplate.ICT_EMPLOYEE_DEPENDENT_LINK - 1].Trim(new char[] { ' ', '"' });

                        if (currTemplate.ICT_NAME_FORMAT == "seperated")
                        {

                            fname = gp[currTemplate.ICT_FIRST_NAME - 1].Trim(new char[] { ' ', '"' });

                            if (currTemplate.ICT_MIDDLE_NAME == 0)
                            {
                                mname = null;
                            }
                            else
                            {
                                mname = gp[currTemplate.ICT_MIDDLE_NAME - 1].Trim(new char[] { ' ', '"' });
                            }

                            lname = gp[currTemplate.ICT_LAST_NAME - 1].Trim(new char[] { ' ', '"' });

                        }
                        else if (currTemplate.ICT_NAME_FORMAT == "lcfm")   
                        {

                            lname = gp[currTemplate.ICT_LAST_NAME - 1].Trim(new char[] { ' ', '"' });
                            String[] fMname = (gp[currTemplate.ICT_MIDDLE_NAME - 1].Trim(new char[] { ' ', '"' })).Split(' ');

                            fname = fMname[0].Trim(new char[] { ' ', '"' });

                            if (fMname.Length == 2)
                            {
                                mname = fMname[1].Trim(new char[] { ' ', '"' });
                            }
                            else
                            {
                                mname = null;
                            }

                        }
                        else
                        {
                            failedRecords += 1;

                            errorLines += count.ToString() + ", ";
                        }

                        ssn = gp[currTemplate.ICT_SSN - 1].Trim(new char[] { ' ', '"', '-' });

                        if (String.Compare(ssn, "000000000", true) == 0)
                        {
                            ssn = null;
                        }
                        else
                        {

                            ssn = ssn.Replace("-", "");

                            if (ssn.Length != 9)
                            {
                                ssn = null;
                            }

                        }

                        if (currTemplate.ICT_DOB == 0)
                        {
                            dob = null;
                        }
                        else
                        {
                            dob = gp[currTemplate.ICT_DOB - 1].checkDateNull();
                        }

                        jan = gp[currTemplate.ICT_JAN - 1].Trim(new char[] { ' ', '"' });
                        feb = gp[currTemplate.ICT_FEB - 1].Trim(new char[] { ' ', '"' });
                        mar = gp[currTemplate.ICT_MAR - 1].Trim(new char[] { ' ', '"' });
                        apr = gp[currTemplate.ICT_APR - 1].Trim(new char[] { ' ', '"' });
                        may = gp[currTemplate.ICT_MAY - 1].Trim(new char[] { ' ', '"' });
                        june = gp[currTemplate.ICT_JUN - 1].Trim(new char[] { ' ', '"' });
                        july = gp[currTemplate.ICT_JUL - 1].Trim(new char[] { ' ', '"' });
                        aug = gp[currTemplate.ICT_AUG - 1].Trim(new char[] { ' ', '"' });
                        sep = gp[currTemplate.ICT_SEP - 1].Trim(new char[] { ' ', '"' });
                        oct = gp[currTemplate.ICT_OCT - 1].Trim(new char[] { ' ', '"' });
                        nov = gp[currTemplate.ICT_NOV - 1].Trim(new char[] { ' ', '"' });
                        dec = gp[currTemplate.ICT_DEC - 1].Trim(new char[] { ' ', '"' });

                        if (currTemplate.ICT_ADDRESS == 0)
                        {
                            _address = null;
                        }
                        else
                        {
                            _address = gp[currTemplate.ICT_ADDRESS - 1].Trim(new char[] { ' ', '"' });
                        }

                        if (currTemplate.ICT_CITY == 0)
                        {
                            _city = null;
                        }
                        else
                        {
                            _city = gp[currTemplate.ICT_CITY - 1].Trim(new char[] { ' ', '"' });
                        }

                        if (currTemplate.ICT_STATE == 0)
                        {
                            _state = null;
                        }
                        else
                        {
                            _state = gp[currTemplate.ICT_STATE - 1].Trim(new char[] { ' ', '"' });
                        }

                        if (currTemplate.ICT_ZIP == 0)
                        {
                            _zip = null;
                        }
                        else
                        {
                            _zip = gp[currTemplate.ICT_ZIP - 1].Trim(new char[] { ' ', '"' }).ZeroPadZip();

                            if (_zip.Contains("USA"))
                            {
                                _zip = _zip.Replace("USA", "").Trim(new char[] { ' ', '"' });
                            }

                        }

                        if (currTemplate.ICT_ALL_12 == 0)
                        {
                            all12 = null;
                        }
                        else
                        {
                            all12 = gp[currTemplate.ICT_ALL_12 - 1].Trim(new char[] { ' ', '"' });
                        }

                        subscriber = gp[currTemplate.ICT_SUBSCRIBER - 1].Trim(new char[] { ' ', '"' });

                        if (String.Compare(currTemplate.ICT_SUBSCRIBER_FORMAT, "ssn", true) == 0)
                        {

                            subscriber = subscriber.Replace("-", "");
                            dependentLink = dependentLink.Replace("-", "");

                            if (String.Compare(ssn, dependentLink, true) == 0)
                            {
                                _subscriber = true;
                            }
                            else
                            {
                                _subscriber = false;
                            }

                            subscriber = AesEncryption.Encrypt(subscriber);

                            dependentLink = AesEncryption.Encrypt(dependentLink);

                        }
                        else
                        {
                            if (String.Compare(subscriber, currTemplate.ICT_SUBSCRIBER_FORMAT, true) == 0) { _subscriber = true; } else { _subscriber = false; }
                        }

                        if (String.Compare(jan, currTemplate.ICT_TRUE_FORMAT, true) == 0) { _jan = true; } else { _jan = false; }
                        if (String.Compare(feb, currTemplate.ICT_TRUE_FORMAT, true) == 0) { _feb = true; } else { _feb = false; }
                        if (String.Compare(mar, currTemplate.ICT_TRUE_FORMAT, true) == 0) { _mar = true; } else { _mar = false; }
                        if (String.Compare(apr, currTemplate.ICT_TRUE_FORMAT, true) == 0) { _apr = true; } else { _apr = false; }
                        if (String.Compare(may, currTemplate.ICT_TRUE_FORMAT, true) == 0) { _may = true; } else { _may = false; }
                        if (String.Compare(june, currTemplate.ICT_TRUE_FORMAT, true) == 0) { _june = true; } else { _june = false; }
                        if (String.Compare(july, currTemplate.ICT_TRUE_FORMAT, true) == 0) { _july = true; } else { _july = false; }
                        if (String.Compare(aug, currTemplate.ICT_TRUE_FORMAT, true) == 0) { _aug = true; } else { _aug = false; }
                        if (String.Compare(sep, currTemplate.ICT_TRUE_FORMAT, true) == 0) { _sep = true; } else { _sep = false; }
                        if (String.Compare(oct, currTemplate.ICT_TRUE_FORMAT, true) == 0) { _oct = true; } else { _oct = false; }
                        if (String.Compare(nov, currTemplate.ICT_TRUE_FORMAT, true) == 0) { _nov = true; } else { _nov = false; }
                        if (String.Compare(dec, currTemplate.ICT_TRUE_FORMAT, true) == 0) { _dec = true; } else { _dec = false; }

                        if (all12 == null && currTemplate.ICT_ALL_12_TRUE_FORMAT == null)
                        {

                            if (_jan == true && _feb == true && _mar == true && _apr == true && _may == true && _june == true && _july == true && _aug == true && _sep == true && _oct == true && _nov == true && _dec == true)
                            {
                                _all12 = true;
                            }
                            else
                            {
                                _all12 = false;
                            }

                        }
                        else
                        {

                            if (String.Compare(all12, currTemplate.ICT_ALL_12_TRUE_FORMAT, true) == 0)
                            {

                                _all12 = true;
                                _jan = true;
                                _feb = true;
                                _mar = true;
                                _apr = true;
                                _may = true;
                                _june = true;
                                _july = true;
                                _aug = true;
                                _sep = true;
                                _oct = true;
                                _nov = true;
                                _dec = true;

                            }
                            else
                            {
                                _all12 = false;
                            }

                        }

                        if (ssn != null)
                        {

                            if (ssn.Length > 0)
                            {
                                ssn = AesEncryption.Encrypt(ssn);
                            }

                        }

                        tempList.Add(
                                new insurance_coverage_I(
                                        0, 
                                        0,
                                        _employerID,
                                        0,
                                        taxYear,
                                        dependentLink,
                                        null,
                                        fname,
                                        mname,
                                        lname,
                                        ssn,
                                        dob,
                                        _all12,
                                        _jan,
                                        _feb,
                                        _mar,
                                        _apr,
                                        _may,
                                        _june,
                                        _july,
                                        _aug,
                                        _sep,
                                        _oct,
                                        _nov,
                                        _dec,
                                        all12,
                                        _subscriber,
                                        jan,
                                        feb,
                                        mar,
                                        apr,
                                        may,
                                        june,
                                        july,
                                        aug,
                                        sep,
                                        oct,
                                        nov,
                                        dec,
                                        subscriber,
                                        _address,
                                        _city,
                                        _state,
                                        0,
                                        _zip,
                                        0,
                                        _carrierID
                                    )
                            );

                        totalRecords += 1;

                    }
                    else
                    {

                        this.Log.Warn(String.Format("Found the wrong number of columns in insurance carrier import, expected {0} but found {1}.", gp.Count(), currTemplate.ICT_COLUMNS));

                        failedRecords += 1;

                        errorLines += count.ToString() + ", ";

                        break;

                    }

                }

                if (failedRecords == 0)
                {

                    int _batchID = EmployeeController.manufactureBatchID(_employerID, _modOn, _modBy);
                    count = 0;

                    foreach (insurance_coverage_I ici in tempList)
                    {
                        count++;

                        bool validImport = false;

                        validImport = insuranceController.insertNewInsuranceCarrierImportRow(_batchID, ici.IC_TAX_YEAR, ici.IC_EMPLOYER_ID, ici.IC_DEPENDENT_EMPLOYEE_LINK, ici.IC_FIRST_NAME, ici.IC_MIDDLE_NAME, ici.IC_LAST_NAME, ici.IC_SSN, ici.IC_DOB, ici.IC_JAN, ici.IC_FEB, ici.IC_MAR, ici.IC_APR, ici.IC_MAY, ici.IC_JUN, ici.IC_JUL, ici.IC_AUG, ici.IC_SEP, ici.IC_OCT, ici.IC_NOV, ici.IC_DEC, ici.IC_ALL_12, ici.IC_SUBSCRIBER, ici.IC_JAN_I, ici.IC_FEB_I, ici.IC_MAR_I, ici.IC_APR_I, ici.IC_MAY_I, ici.IC_JUN_I, ici.IC_JUL_I, ici.IC_AUG_I, ici.IC_SEP_I, ici.IC_OCT_I, ici.IC_NOV_I, ici.IC_DEC_I, ici.IC_ALL_12_I, ici.IC_SUBSCRIBER_I, ici.IC_ADDRESS_I, ici.IC_CITY_I, ici.IC_STATE_I, ici.IC_ZIP_I, ici.IC_CARRIER_ID);

                        if (validImport == false)
                        {

                            insuranceController.deleteInsucranceCarrierBatch(_batchID);

                            failedRecords += 1;

                            errorLines += count.ToString() + ", ";

                            break;

                        }

                    }

                }

            }
            catch (Exception exception)
            {

                this.Log.Warn("Suppressing errors.", exception);

                failedRecords += 1;

                errorLines += count.ToString();
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
            }

            if (failedRecords == 0)
            {

                if (System.IO.File.Exists(fullFilePath))
                {

                    try
                    {

                        new FileArchiverWrapper().ArchiveFile(fullFilePath, _employerID, "Processing File for Insurance Carier");
                        MpeWebMessage.Show();
                        LitMessage.Text = "The file has been SUCCESSFULLY been imported.";

                        loadFTPFiles();

                    }
                    catch (Exception exception)
                    {

                        this.Log.Warn("Suppressing errors.", exception);
                        MpeWebMessage.Show();

                        LitMessage.Text = "The file has been SUCCESSFULLY been imported, but could not be DELETED.";

                    }

                }

            }
            else
            {
                MpeWebMessage.Show();
                LitMessage.Text = "An error occurred while importing this specific file. There may be an error within the document that must be corrected before it can be imported.";

                LitMessage.Text += "Line: " + errorLines.ToString();

            }

        }
        else
        {

            MpeWebMessage.Show();

            LitMessage.Text = "You must select a DISTRICT, CARRIER and File. Please correct the red fields.";

        }

    }

    protected void BtnValidateEmployees_Click(object sender, EventArgs e)
    {

        bool validData = true;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {

            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            if (false == insuranceController.ValidateCurrentEmployee(_employerID))
            {

                MpeWebMessage.Show();

                LitMessage.Text = "There was an issue during Validation, please try again and contact IT.";
            }

        }
        else
        {

            MpeWebMessage.Show();
            LblFileUploadMessage.Text = "Please correct all red fields.";

            LitMessage.Text = "You must select an EMPLOYER before you can VALIDATE employees.";

        }

    }

    protected void BtnValidateDependents_Click(object sender, EventArgs e)
    {
        string modBy = LitUserName.Text;
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {

            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            if (false == insuranceController.validateCurrentEmployeeDependents(_employerID, modBy))
            {

                MpeWebMessage.Show();

                LitMessage.Text = "There was an issue during Validation, please try again and contact IT.";

            }

        }
        else
        {

            MpeWebMessage.Show();
            LblFileUploadMessage.Text = "Please correct all red fields.";

            LitMessage.Text = "You must select an EMPLOYER before you can VALIDATE dependents.";

        }

    }

    protected void BtnInvalidEmployees_Click(object sender, EventArgs e)
    {

        bool validData = true;
        DateTime _modOn = DateTime.Now;
        string _modBy = LitUserName.Text;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            insuranceController.createAlertsForMissingEmployees(_employerID, _modOn, _modBy);
        }
        else
        {
            MpeWebMessage.Show();
            LblFileUploadMessage.Text = "Please correct all red fields.";
            LitMessage.Text = "You must select an EMPLOYER before you can GENERATE employee alerts.";
        }
    }

    protected void BtnTransferRecords_Click(object sender, EventArgs e)
    {
        bool validData = true;
        string modBy = LitUserName.Text;
        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            insuranceController.transferInsuranceCarrierImportData(_employerID, modBy);
        }
        else
        {
            MpeWebMessage.Show();
            LblFileUploadMessage.Text = "Please correct all red fields.";
            LitMessage.Text = "You must select an EMPLOYER before you can TRANSFER valid records.";
        }
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
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
                new FileArchiverWrapper().ArchiveFile(filePath, int.Parse(DdlEmployer.SelectedItem.Value), "User Delete Insurance Carrier");
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

    protected void GvCurrentFiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/inscarrier"));
        List<FileInfo> fileList = directory.GetFiles().ToList<FileInfo>();
        GridViewRow row = (GridViewRow)GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hf = (HiddenField)row.FindControl("HfFilePath");
        Label lbl = (Label)row.FindControl("LblFileName");

        string filePath = hf.Value;
        string fileName = lbl.Text;
        char delimiter = '.';
        string[] fileType = fileName.Split(delimiter);

        PIILogger.LogPII(String.Format("Downloading inscarrier Upload File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", fileName, Request.UserHostAddress, LitUserName.Text));

        string appendText = "attachment; filename=" + fileName;
        Response.ContentType = "file/" + Path.GetExtension(fileName);
        Response.AppendHeader("Content-Disposition", appendText);
        Response.TransmitFile(filePath);
        Response.Flush();         
        Response.SuppressContent = true;                
        HttpContext.Current.ApplicationInstance.CompleteRequest();                      
        Response.End();
    }

    private ILog Log = LogManager.GetLogger(typeof(admin_ins_import));


}