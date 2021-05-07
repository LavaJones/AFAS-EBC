using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using log4net;

using Afas.AfComply.Domain;

public partial class securepages_transfer : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(securepages_transfer));

    protected override void PageLoadNonPostBack()
    {
        Session["ShowSSN"] = false;
    }

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        HfUserName.Value = user.User_Full_Name;
        HfDistrictID.Value = user.User_District_ID.ToString();
    }

    /// <summary>
    /// 60)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void BtnSaveFile_Click(object sender, EventArgs e)
    {
        string appendFileName = null;
        string _filePath = null;
        bool validData = true;
        employer _employer = (employer)Session["CurrentDistrict"];

        validData = FuFileTransfer.HasFile;
        validData = errorChecking.validateDropDownSelection(DdlFileType, validData);
        
        if (validData == true)
        {
            _filePath = getFilePath();
            if (_filePath == null)
            {
                Log.Warn("getFilePath returned null.");
                return;
            }

            appendFileName = getFileType();

            if (appendFileName != null)
            {
                
                String outboundFileNameNotUsedYet = String.Empty;

                FileProcessing.SaveFile(FuFileTransfer, _filePath, LblFileUploadMessage, appendFileName, out outboundFileNameNotUsedYet);
                FuFileTransfer.BackColor = System.Drawing.Color.White;
                loadFiles();
                appendFileName += "_" + FuFileTransfer.FileName;
                sendEmail(appendFileName);

                MpeWebMessage.Show();
                LitMessage.Text = "The file you selected has been uploaded. An email has been sent to notify the team.";
            }
            else
            {
                MpeWebMessage.Show();
                LitMessage.Text = "The File Type upload has not been setup by " + Branding.CompanyShortName + ", please contact " + Branding.CompanyShortName + " to setup the file upload structure.";
            }
        }
        else
        {
            FuFileTransfer.BackColor = System.Drawing.Color.Red;
            LblFileUploadMessage.Text = "Please select a file";
            MpeWebMessage.Show();
            LblFileUploadMessage.Text = "Please correct all red fields.";
            LitMessage.Text = "You must select a file. Use the browse button to find a file to upload.";
        }
    }


    private void loadFiles()
    {
        employer _employer = (employer)Session["CurrentDistrict"];
        List<FileInfo> tempList = new List<FileInfo>();
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(DdlFileType, validData);

        if (validData == true)
        {
            tempList = getFtpFiles();

            GvCurrentFiles.DataSource = tempList;
            GvCurrentFiles.DataBind();
        }
        else
        { }
        
    }

    private string getFileType()
    {
        employer _employer = (employer)Session["CurrentDistrict"];
        string fileType = DdlFileType.SelectedItem.Value;
        string fileName = null;

        switch (fileType)
        {
            case "PAY":
                fileName = _employer.EMPLOYER_IMPORT_PAYROLL;
                break;
            case "DEM":
                fileName = _employer.EMPLOYER_IMPORT_EMPLOYEE;
                break;
            case "GP":
                fileName = _employer.EMPLOYER_IMPORT_GP;
                break;
            case "HR":
                fileName = _employer.EMPLOYER_IMPORT_HR;
                break;
            case "EC":
                fileName = _employer.EMPLOYER_IMPORT_EC;
                break;
            case "IO":
                fileName = _employer.EMPLOYER_IMPORT_IO;
                break;
            case "IC":
                fileName = _employer.EMPLOYER_IMPORT_IC;
                break;
            case "PAY_MOD":
                fileName = _employer.EMPLOYER_IMPORT_PAY_MOD;
                break;
            default:
                break;
        }

        return fileName;

    }

    /// <summary>
    /// This will set the file path that is being viewed.
    /// </summary>
    /// <returns></returns>
    private string getFilePath()
    {
        employer _employer = (employer)Session["CurrentDistrict"];

        if (DdlFileType == null || DdlFileType.SelectedItem == null)
        {
            Log.Warn("There was an issue with DdlFileType.");
            
            return null;

        }
        
        string _filePath = null;
        string fileType = DdlFileType.SelectedItem.Value;

        Vendor empVendor = employerController.manufactureEmployerVendor(_employer.EMPLOYER_VENDOR_ID);

        if (fileType == "PAY" || fileType == "DEM")
        {
            if (empVendor.VENDOR_AUTO_UPLOAD == true)
            {
                _filePath = Server.MapPath("..\\ftps\\");
            }
            else
            {
                _filePath = Server.MapPath("..\\ftps\\rawdata\\");
            }
        }
        else
        {
            if (fileType == "GP")
            {
                _filePath = Server.MapPath("..\\ftps\\grosspay\\");
            }
            else if (fileType == "HR")
            {
                _filePath = Server.MapPath("..\\ftps\\hrstatus\\");
            }
            else if (fileType == "EC")
            {
                _filePath = Server.MapPath("..\\ftps\\eclass\\");
            }
            else if (fileType == "IO")
            {
                _filePath = Server.MapPath("..\\ftps\\insoffer\\");
            }
            else if (fileType == "IC")
            {
                _filePath = Server.MapPath("..\\ftps\\inscarrier\\");
            }
            else if (fileType == "PAY_MOD")
            {
                _filePath = Server.MapPath("..\\ftps\\paymod\\");
            }
        }

        return _filePath;
    }
    
    /// <summary>
    /// This will set the file path that is being viewed.
    /// </summary>
    /// <returns></returns>
    private List<FileInfo> getFtpFiles()
    {
        employer _employer = (employer)Session["CurrentDistrict"];
        List<FileInfo> _tempList = new List<FileInfo>();

        if (DdlFileType == null || DdlFileType.SelectedItem == null || DdlFileType.SelectedItem.Value == null)
        {
            return new List<FileInfo>();
        }

        string fileName = null;
        string fileType = DdlFileType.SelectedItem.Value;
        string _filePath = null;

        switch (fileType)
        {
            case "PAY":
                fileName = _employer.EMPLOYER_IMPORT_PAYROLL;
                break;
            case "DEM":
                fileName = _employer.EMPLOYER_IMPORT_EMPLOYEE;
                break;
            case "GP":
                fileName = _employer.EMPLOYER_IMPORT_GP;
                break;
            case "HR":
                fileName = _employer.EMPLOYER_IMPORT_HR;
                break;
            case "EC":
                fileName = _employer.EMPLOYER_IMPORT_EC;
                break;
            case "IO":
                fileName = _employer.EMPLOYER_IMPORT_IO;
                break;
            case "IC":
                fileName = _employer.EMPLOYER_IMPORT_IC;
                break;
            case "PAY_MOD":
                fileName = _employer.EMPLOYER_IMPORT_PAY_MOD;
                break;
            default:
                break;
        }        

        if (fileType == "PAY" || fileType == "DEM")
        {
            Vendor empVendor = employerController.manufactureEmployerVendor(_employer.EMPLOYER_VENDOR_ID);

            if (empVendor.VENDOR_AUTO_UPLOAD == true)
            {
                _filePath = Server.MapPath("..\\ftps\\");
                if (fileType == "PAY")
                {
                    _tempList = FileProcessing.getFtpFiles(_employer.EMPLOYER_IMPORT_PAYROLL);
                }
                else if (fileType == "DEM")
                {
                    _tempList = FileProcessing.getFtpFiles(_employer.EMPLOYER_IMPORT_EMPLOYEE);
                }
            }
            else
            {
                _filePath = Server.MapPath("..\\ftps\\rawdata\\");
                if (fileType == "PAY")
                {
                    _tempList = FileProcessing.getFtpRawFiles(_employer.EMPLOYER_IMPORT_PAYROLL);
                }
                else if (fileType == "DEM")
                {
                    _tempList = FileProcessing.getFtpRawFiles(_employer.EMPLOYER_IMPORT_EMPLOYEE);
                }
            }
        }
        else
        {
            if (fileType == "GP")
            {
                _filePath = Server.MapPath("..\\ftps\\grosspay\\");
                _tempList = FileProcessing.getFtpGpFiles(_employer.EMPLOYER_IMPORT_GP);
            }
            else if (fileType == "HR")
            {
                _filePath = Server.MapPath("..\\ftps\\hrstatus\\");
                _tempList = FileProcessing.getFtpHrFiles(_employer.EMPLOYER_IMPORT_HR);
            }
            else if (fileType == "EC")
            {
                _filePath = Server.MapPath("..\\ftps\\eclass\\");
                _tempList = FileProcessing.getFtpEcFiles(_employer.EMPLOYER_IMPORT_EC);
            }
            else if (fileType == "IO")
            {
                _filePath = Server.MapPath("..\\ftps\\insoffer\\");
                _tempList = FileProcessing.GetFtpInsuranceFiles(_employer.EMPLOYER_IMPORT_IO);
            }
            else if (fileType == "IC")
            {
                _filePath = Server.MapPath("..\\ftps\\inscarrier\\");
                _tempList = FileProcessing.getFtpInsuranceCarrierFiles(_employer.EMPLOYER_IMPORT_IC);
            }
            else if (fileType == "PAY_MOD")
            {
                _filePath = Server.MapPath("..\\ftps\\paymod\\");
                _tempList = FileProcessing.getFtpPayModFiles(_employer.EMPLOYER_IMPORT_PAY_MOD);
            }
        }

        return _tempList;
    }

    private void sendEmail(string _fileName)
    {
        employer currDist = (employer)Session["CurrentDistrict"];
        List<User> employerUsers = UserController.getDistrictUsers(currDist.EMPLOYER_ID);
        string _body = "A new file has been added to the import que." + _fileName;
        _body += "<br /><br />This file will be processed shortly.";
        string _subject = "Software Message";
        Email em = new Email();
        em.sendEmail(employerUsers, _subject, _body, true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GvCurrentFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        GridViewRow row = GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hfFilePath = (HiddenField)row.FindControl("HfFilePath");
        employer _employer = employerController.getEmployer(_employerID);

        string filePath = hfFilePath.Value;

        if (System.IO.File.Exists(filePath))
        {
            try
            {
                new FileArchiverWrapper().ArchiveFile(filePath, _employer.ResourceId, "User Delete Transfer page", _employer.EMPLOYER_ID);
                loadFiles();
            }
            catch (Exception exception)
            {
                this.Log.Warn("Suppressing errors.", exception);
            }
        }
    }


    protected void DdlFileType_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadFiles();
    }
}