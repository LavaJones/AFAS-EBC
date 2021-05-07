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

public partial class import_ins_import : Afas.AfComply.UI.admin.AdminPageBase
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

            List<FileInfo> tempList = FileProcessing.GetFtpInsuranceFiles(emp.EMPLOYER_IMPORT_IO);

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

        String filePath = Server.MapPath("..\\ftps\\insoffer\\");
        String fileName = null;
        String fullFilePath = null;
        int _employerID = 0;
        Boolean validData = true;
        int totalRecords = 0;
        int failedRecords = 0;
        String validationError = null;
        DateTime modOn = DateTime.Now;
        String modBy = LitUserName.Text;
        String message = "Insurance Offer Overview" + "<br />";
        User currUser = (User)Session["CurrentUser"];

        System.IO.StreamReader file = null;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlFileList, validData);
        IList<Employee> employees = null;
        IList<PlanYear> planYears = null;
        EmployeeType employeeType = null;
        Measurement measurementPeriod = null;
        IList<insurance> insurances = null;
        IList<insuranceContribution> insuranceContributions = null;

        if (validData == true)
        {

            fileName = DdlFileList.SelectedItem.Text;
            fullFilePath = filePath + fileName;
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

            employees = EmployeeController.manufactureEmployeeList(_employerID);
            planYears = PlanYear_Controller.getEmployerPlanYear(_employerID);
            insurances = insuranceController.getAllActiveInsurancePlans(_employerID, false);

            try
            {

                file = new System.IO.StreamReader(fullFilePath);
                String line = null;

                if (false == file.ReadLine().IsHeaderRow())
                {

                    file.Close();

                    file = new System.IO.StreamReader(fullFilePath);

                }

                while ((line = file.ReadLine()) != null)
                {

                    if (line.Trim() == null || line.Trim().Equals(String.Empty))
                        continue;

                    String[] io = CsvParse.SplitRow(line);

                    if (DataValidation.StringArrayContainsOnlyBlanks(io))
                    {
                        this.Log.Info(
                                String.Format("Skipping row for insurance processing in file {0}, all columns where blank.", fullFilePath)
                            );

                        continue;

                    }

                    validData = true;

                    if (io.Count() == 16)
                    {

                        int rowID = 0;
                        int employeeID = int.Parse(io[1]);
                        int employerID = int.Parse(io[2]);
                        int planYearID = int.Parse(io[3]);

                        if (false == int.TryParse(io[0], out rowID) || rowID == 0)
                        {
                            if (false == io[0].Equals("NEW-HIRE-COVERAGE"))
                            {
                                throw new Exception("The RowId [" + io[0] + "] was invalid.");
                            }

                            alert_insurance ai = insuranceController.findSingleInsuranceOffer(planYearID, employeeID);

                            if (ai != null)
                            {
                                rowID = ai.ROW_ID;
                                throw new Exception("Found unexpected new hire coverage Offer.");
                            }
                            else
                            {
                                bool success = insuranceController.InsertNewInsuranceCoverage(employerID, employeeID, planYearID, 0.0, LitUserName.Text, DateTime.Now, "Created by Import process.");
                                if (success == false)
                                {
                                    Log.Error(String.Format("Failed to insert Insurance coverage for new Hire using values; EmployerID: [{0}], EmployeeId: [{1}], PlanYearId: [{2}].", employerID, employeeID, planYearID));
                                }

                                ai = insuranceController.findSingleInsuranceOffer(planYearID, employeeID);

                                if (ai != null)
                                {
                                    rowID = ai.ROW_ID;
                                }
                                else
                                {
                                    throw new Exception("New Employee Covereage Insert failed.");
                                }
                            }
                        }

                        double? avgHours = double.Parse(io[7]);
                        double hraFlex = double.Parse(io[15]);

                        Employee employee = employees.FilterForEmployeeId(employeeID).SingleOrDefault();
                        PlanYear planYear = planYears.FilterForPlanYearId(planYearID).SingleOrDefault();

                        Measurement tempMeas = measurementController.getPlanYearMeasurement(employerID, planYear.PLAN_YEAR_ID, employee.EMPLOYEE_TYPE_ID);

                        Boolean? offered = Boolean.Parse(io[8]);
                        DateTime? offeredOn = null;
                        Boolean? accepted = null;
                        int insuranceID = 0;
                        int contributionID = 0;
                        DateTime? effectiveDate = null;
                        DateTime? acceptedOn = null;
                        DateTime _hireDate = employee.EMPLOYEE_HIRE_DATE.Value;

                        if (employee != null && planYear != null && employee.EMPLOYEE_TYPE_ID != 0 && tempMeas != null)
                        {

                            if (offered == true)
                            {

                                offeredOn = DateTime.Parse(io[9]);
                                accepted = Boolean.Parse(io[10]);
                                acceptedOn = DateTime.Parse(io[11]);
                                insuranceID = int.Parse(io[12]);
                                contributionID = int.Parse(io[13]);
                                effectiveDate = DateTime.Parse(io[14]);

                                insurance tempInsurance = insurances.FilterForInsuranceId(insuranceID).SingleOrDefault();
                                insuranceContributions = insuranceController.manufactureInsuranceContributionList(tempInsurance.INSURANCE_ID);
                                insuranceContribution tempIC = insuranceContributions.FilterForInsuranceContributionId(contributionID).SingleOrDefault();

                                if (tempInsurance != null && tempIC != null)
                                {

                                    validData = errorChecking.validateStringDate(((DateTime)offeredOn).ToShortDateString(), validData);
                                    validData = errorChecking.validateStringDate(((DateTime)acceptedOn).ToShortDateString(), validData);
                                    validData = errorChecking.validateStringDate(((DateTime)effectiveDate).ToShortDateString(), validData);

                                    if (validData == true)
                                    {

                                        Label lbl = new Label();
                                        TextBox txt = new TextBox();
                                        validData = errorChecking.validateInsuranceOfferDates((DateTime)offeredOn, tempMeas, (DateTime)acceptedOn, (DateTime)effectiveDate, validData, lbl, _hireDate, txt, txt, txt);

                                        if (validData == true)
                                        {
                                            totalRecords += 1;
                                        }
                                        else
                                        {
                                            message += employee.EMPLOYEE_FULL_NAME + " Offered On, Accepted On or Effective On dates do not have the correct time periods." + "<br />Details:<br />" + lbl.Text + "<hr />";
                                            validationError = "The Offered, Accepted or Effective date are incorrect.";
                                            failedRecords += 1;
                                        }

                                    }
                                    else
                                    {
                                        message += employee.EMPLOYEE_FULL_NAME + " Offered On, Accepted On or Effective On dates do not have the correct formatting.<br />";
                                        validationError = "The Offered On or Effective Date are incorrect. The offered on date must be greater than or equal to: " + tempMeas.MEASUREMENT_ADMIN_START.ToShortDateString() + ". The Effective must be greater than or equal to: " + tempMeas.MEASUREMENT_STAB_START.ToShortDateString();
                                        failedRecords += 1;
                                    }

                                }
                                else
                                {

                                    if (tempInsurance == null)
                                    {
                                        message += employee.EMPLOYEE_FULL_NAME + "The Insurance ID is wrong or missing. Line: " + totalRecords.ToString();
                                    }

                                    if (tempIC == null)
                                    {
                                        message += employee.EMPLOYEE_FULL_NAME + "The Insurance Contribution ID is wrong or missing. Line: " + totalRecords.ToString();
                                    }

                                    validationError = "The Insurance ID or Insurance Contribution ID is invalid.";

                                    failedRecords += 1;

                                }

                            }
                            else
                            {

                                offered = false;
                                offeredOn = null;
                                accepted = null;
                                acceptedOn = null;
                                insuranceID = 0;
                                contributionID = 0;
                                effectiveDate = null;

                                totalRecords += 1;

                            }

                            if (validData == true)
                            {

                                insuranceController.updateInsuranceOffer(
                                        rowID,
                                        insuranceID,
                                        contributionID,
                                        avgHours,
                                        offered,
                                        offeredOn,
                                        accepted,
                                        acceptedOn,
                                        modOn,
                                        modBy,
                                        "",
                                        "",
                                        effectiveDate,
                                        hraFlex
                                    );

                            }

                        }
                        else
                        {
                            if (insurances.Count < 1)
                            {
                                message += "The Insurance ID could not be found.";
                            }
                            if (planYear == null)
                            {
                                message += "The Plan Year ID could not be found.";
                            }
                            if (employee.EMPLOYEE_TYPE_ID == 0)
                            {
                                message += "The Employee Type ID could not be found.";
                            }
                            if (tempMeas == null)
                            {
                                message += "The Measurement Period ID could not be found.";
                            }
                            message += "The default ID values were changed. This file is now corrupted and should not be used! Please do not change values in the pre-filled columns.";
                            message += "<br />Please check line: " + totalRecords.ToString();
                            validationError = "The Employee ID or Plan Year ID are invalid.";

                            failedRecords += 1;

                            break;

                        }

                    }
                    else
                    {

                        this.Log.Warn(String.Format("Invalid column count at line {0}, expected 16, found {1}.", totalRecords, io.Count()));

                        failedRecords += 1;

                        break;

                    }

                }
            }
            catch (Exception exception)
            {

                Log.Warn("Suppressing errors.", exception);

                message += "An error occurred while importing this file, a required field is missing or in-correct on line: " + totalRecords.ToString();

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

                        new FileArchiverWrapper().ArchiveFile(fullFilePath, _employerID, "Processing File for Import Insurance");
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
                systemFailedEmail.SendEmail(SystemSettings.ProcessingFailedAddress, "Message - Insurance Import Errors", message, false);

                loggedInUser = Session["CurrentUser"] as User;

                Email em = new Email();
                em.SendEmail(loggedInUser.User_Email, "Message - Insurance Import Errors", message, false);

                MpeWebMessage.Show();
                LitMessage.Text = "An error occurred while importing this specific file. Some records may have been updated, an email has been sent to you with the details of all the failed records. The error occurred on Line: " + totalRecords.ToString();

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
                new FileArchiverWrapper().ArchiveFile(filePath, int.Parse(DdlEmployer.SelectedItem.Value), "User Delete Import Insurance");
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
                String _filePath = Server.MapPath("..\\ftps\\insoffer\\");

                String savedFileName = String.Empty;

                FileProcessing.SaveFile(FuGrossPayFile, _filePath, LblFileUploadMessage, _appendFile, out savedFileName);

                String archivePath = HttpContext.Current.Server.MapPath(Archive.ArchiveFolder);

                try
                {

                    DataValidation.OfferFileIsForEmployer(savedFileName, emp.EMPLOYER_ID, emp.ResourceId, new FileArchiverWrapper());

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
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/insoffer"));
        List<FileInfo> fileList = directory.GetFiles().ToList<FileInfo>();
        GridViewRow row = (GridViewRow)GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hf = (HiddenField)row.FindControl("HfFilePath");
        Label lbl = (Label)row.FindControl("LblFileName");

        string filePath = hf.Value;
        string fileName = lbl.Text;
        char delimiter = '.';
        string[] fileType = fileName.Split(delimiter);

        PIILogger.LogPII(String.Format("Downloading insoffer Upload File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", fileName, Request.UserHostAddress, LitUserName.Text));

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

    private ILog Log = LogManager.GetLogger(typeof(import_ins_import));

}