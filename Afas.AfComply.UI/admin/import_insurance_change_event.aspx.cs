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
using Afas.Domain;
using Afas.Application.Archiver;
using Afas.ImportConverter.Application;

public partial class import_insurance_change_event : Afas.AfComply.UI.admin.AdminPageBase
{

    private User loggedInUser = null;

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {

        Server.ScriptTimeout = 1800;

        loggedInUser = user;

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

            List<FileInfo> tempList = FileProcessing.GetFtpInsuranceChangeFiles(emp.EMPLOYER_IMPORT_IO);

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

        String filePath = Server.MapPath("..\\ftps\\InsuranceChangeEvent\\");
        String fileName = null;
        String fullFilePath = null;
        int _employerID = 0;
        Boolean validData = true;
        int failedRecords = 0;
        DateTime modOn = DateTime.Now;
        String modBy = LitUserName.Text;
        String message = "Insurance Offer Overview" + "<br />";
        User currUser = (User)Session["CurrentUser"];

        System.IO.StreamReader file = null;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlFileList, validData);
        IList<Employee> employees = null;
        IList<PlanYear> planYears = null;
        IList<insurance> insurances = null;
        DataTable SkippedRows = new DataTable();       

        if (validData == true)
        {

            fileName = DdlFileList.SelectedItem.Text;
            fullFilePath = filePath + fileName;

            ICsvFileDataTableBuilder Builder = DependencyInjection.GetCsvFileDataTableBuilder();
            DataTable ChangeEvents = Builder.CreateDataTableFromCsvFile(fullFilePath);

            foreach (DataColumn col in ChangeEvents.Columns)
            {
                SkippedRows.Columns.AddColumnIfMissing(col.ColumnName);
            }


            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

            employees = EmployeeController.manufactureEmployeeList(_employerID);
            planYears = PlanYear_Controller.getEmployerPlanYear(_employerID);
            insurances = insuranceController.getAllActiveInsurancePlans(_employerID, false);

            int lineCount = 0;
            try
            {

                foreach (DataRow row in ChangeEvents.Rows)
                {

                    lineCount++;

                    int EmployeeId = int.Parse(row["EMPLOYEE ID"].ToString());
                    int EmployerId = int.Parse(row["EMPLOYER ID"].ToString());
                    int PlanYearId = int.Parse(row["PLAN YEAR ID"].ToString());

                    String Name = row["NAME"].ToString();

                    if (EmployerId != _employerID)
                    {
                        throw new Exception("Employer ID Mismatch.");
                    }

                    Boolean Offered = (bool)row["OFFERED"].checkBoolNull2();
                    Boolean Accepted = (bool)row["ACCEPTED"].checkBoolNull2();
                    DateTime OfferedOn = (DateTime)row["OFFERED ON"].checkDateNull();
                    DateTime DecidedOn = (DateTime)row["ACCEPTED/DECLINED ON"].checkDateNull();
                    DateTime EffectiveOn = (DateTime)row["EFFECTIVE DATE"].checkDateNull();

                    if (EffectiveOn > new DateTime(2016, 12, 31))
                    {
                        continue;
                    }

                    alert_insurance insurance = insuranceController.findSingleInsuranceOffer(PlanYearId, EmployeeId);
                    if (null == insurance)
                    {

                        SkippedRows.Rows.Add(row.ItemArray);
                        continue;

                    }

                    if (Offered)
                    {

                        int InsuranceId = int.Parse(row["INSURANCE ID"].ToString());
                        int ContributionId = int.Parse(row["CONTRIBUTION ID"].ToString());

                        int ClassificationId = 0;
                        double AverageHours = 0.0;
                        double HraFlex = 0.0;

                        int.TryParse(row["CLASS ID"].ToString(), out ClassificationId);
                        double.TryParse(row["AVG HOURS"].ToString(), out AverageHours);
                        double.TryParse(row["HRA-Flex"].ToString(), out HraFlex);

                        bool success = false;
                        if (insurance.IALERT_EFFECTIVE_DATE == EffectiveOn)
                        {

                            this.Log.Warn("Found Insurance with same Effective Date, Overwriting.");

                            success = insuranceController.updateInsuranceOffer(
                                insurance.ROW_ID,
                                InsuranceId,
                                ContributionId,
                                AverageHours,
                                Offered,
                                OfferedOn,
                                Accepted,
                                DecidedOn,
                                DateTime.Now,
                                LitUserName.Text,
                                "Automatic Insurance Change Event",
                                insurance.IALERT_HISTORY + "\n\r Bulk Insurance Change Event",
                                EffectiveOn,
                                HraFlex);

                        }
                        else
                        {

                            success = insuranceController.TransferInsuranceChangeEvent(
                                insurance.ROW_ID,
                                InsuranceId,
                                ContributionId,
                                AverageHours,
                                Offered,
                                OfferedOn,
                                Accepted,
                                DecidedOn,
                                DateTime.Now,
                                LitUserName.Text,
                                "Automatic Insurance Change Event",
                                insurance.IALERT_HISTORY + "\n\r Bulk Insurance Change Event",
                                EffectiveOn,
                                HraFlex);

                        }

                        if (false == success)
                        {
                            this.Log.Error(String.Format("Failed To update Insurance Coverage for  ", EmployerId, PlanYearId, EmployeeId));
                            SkippedRows.Rows.Add(row.ItemArray);
                        }

                    }
                    else
                    {

                        Boolean success = false;

                        if (insurance.IALERT_OFFERED_ON == OfferedOn)
                        {

                            this.Log.Warn("Found Insurance with same Offer Date, Overwriting.");


                            success = insuranceController.updateInsuranceOffer(
                                insurance.ROW_ID,
                                null,
                                null,
                                insurance.EMPLOYEE_AVG_HOURS,
                                Offered,
                                OfferedOn,
                                Accepted,
                                DecidedOn,
                                DateTime.Now,
                                LitUserName.Text,
                                "Automatic Insurance Change Event",
                                insurance.IALERT_HISTORY + "\n\r Bulk Insurance Change Event",
                                EffectiveOn,
                                0.0);

                        }
                        else
                        {

                            success = insuranceController.TransferInsuranceChangeEvent(
                                    insurance.ROW_ID,
                                    null,
                                    null,
                                    insurance.EMPLOYEE_AVG_HOURS,
                                    Offered,
                                    OfferedOn,
                                    Accepted,
                                    DecidedOn,
                                    DateTime.Now,
                                    LitUserName.Text,
                                    "Automatic Insurance Change Event",
                                    insurance.IALERT_HISTORY + "\n\r Bulk Insurance Change Event",
                                    EffectiveOn,
                                    0.0);

                        }

                        if (false == success)
                        {
                            this.Log.Error(String.Format("Failed To update Insurance Change Event for  ", EmployerId, PlanYearId, EmployeeId));
                            SkippedRows.Rows.Add(row.ItemArray);
                        }

                    }

                }

            }
            catch (Exception exception)
            {

                Log.Warn("Suppressing errors.", exception);

                message += "An error occurred while importing this file, a required field is missing or in-correct on line: " + lineCount;

                failedRecords += 1;

            }

            if (failedRecords == 0)
            {

                if (SkippedRows.Rows.Count > 0)
                {
                    SkippedRows.WriteOutCsv(filePath + "_Failed_" + fileName);
                }

                if (System.IO.File.Exists(fullFilePath))
                {

                    try
                    {

                        new FileArchiverWrapper().ArchiveFile(fullFilePath, _employerID, "Processing File for Insurance Change Event");
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

                Email systemFailedEmail = new Email();
                systemFailedEmail.SendEmail(SystemSettings.ProcessingFailedAddress, "Message - Insurance Change Event Import Errors", message, false);

                loggedInUser = Session["CurrentUser"] as User;

                Email em = new Email();
                em.SendEmail(loggedInUser.User_Email, "Message - Insurance Change Event Import Errors", message, false);
                MpeWebMessage.Show();

                LitMessage.Text = "An error occurred while importing this specific file. Some records may have been updated, an email has been sent to you with the details of all the failed records. The error occurred on Line: " + lineCount.ToString();

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
                new FileArchiverWrapper().ArchiveFile(filePath, int.Parse(DdlEmployer.SelectedItem.Value), "User Delete Import Insurance Change Event");
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

        Boolean validData = true;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {

            if (FuGrossPayFile.HasFile)
            {

                Boolean validFile;

                int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                employer emp = employerController.getEmployer(_employerID);

                String _appendFile = emp.EMPLOYER_IMPORT_IO;
                String _filePath = Server.MapPath("..\\ftps\\InsuranceChangeEvent\\");

                String savedFileName = String.Empty;

                FileProcessing.SaveFile(FuGrossPayFile, _filePath, LblFileUploadMessage, _appendFile, out savedFileName);

                String archivePath = HttpContext.Current.Server.MapPath(Archive.ArchiveFolder);

                try
                {
                    FileArchiverWrapper archiver = new FileArchiverWrapper();
                    DataValidation.OfferFileIsForEmployer(savedFileName, emp.EMPLOYER_ID, emp.ResourceId, archiver, true);

                }
                catch (Exception exception)
                {

                    MpeWebMessage.Show();

                    LitMessage.Text = exception.Message;

                    if (LitMessage.Text.Contains("Incoming file"))
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

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void GvCurrentFiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/InsuranceChangeEvent"));
        List<FileInfo> fileList = directory.GetFiles().ToList<FileInfo>();
        GridViewRow row = (GridViewRow)GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hf = (HiddenField)row.FindControl("HfFilePath");
        Label lbl = (Label)row.FindControl("LblFileName");

        string filePath = hf.Value;
        string fileName = lbl.Text;
        char delimiter = '.';
        string[] fileType = fileName.Split(delimiter);

        PIILogger.LogPII(String.Format("Downloading InsuranceChangeEvent Upload File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", fileName, Request.UserHostAddress, LitUserName.Text));

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

    private ILog Log = LogManager.GetLogger(typeof(import_insurance_change_event));

}