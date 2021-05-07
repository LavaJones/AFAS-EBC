using Afas.AfComply.Domain;
using Afas.AfComply.Reporting.Application;
using Afas.AfComply.Reporting.Core.Request;
using Afas.AfComply.Reporting.Core.Response;
using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.LegacyData;
using Afas.AfComply.UI.App_Start;
using Afas.Domain;
using Afc.Core.Application;
using Afc.Marketing;
using AutoMapper;
using Ionic.Zip;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EmployerIRSTransmission : Afas.AfComply.UI.admin.AdminPageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(EmployerIRSTransmission));

        private int TaxYearId
        {
            get
            {

                int taxYearId = 0;
                int.TryParse(this.DdlTaxYear.SelectedValue, out taxYearId);
                return taxYearId;

            }

        }

        private int EmployerId
        {
            get
            {

                int employerId = 0;
                int.TryParse(this.DdlFilterEmployers.SelectedItem.Value, out employerId);
                return employerId;

            }
        }

        private int CorrectedTaxYearTransmissionId
        {
            get
            {

                int correctedTransmissionId = 0;
                int.TryParse(this.DdlReceiptID.SelectedItem.Value, out correctedTransmissionId);


                return correctedTransmissionId;

            }
        }
        private int CorrectedIncrementId
        {
            get
            {
                int CorrectInd = airController.manufactureEmployerCorrectionId(this.EmployerId, this.TaxYearId);

                return CorrectInd;
            }
        }
        private int CorrectedIncrementId1094C
        {
            get
            {
                int Correct1094Ind = airController.manufactureEmployer1094CorrectionId(this.EmployerId, this.TaxYearId);

                return Correct1094Ind;
            }
        }

        private int CorrectedSubmissionId
        {
            get
            {

                List<taxYearEmployerTransmission> filteredList = airController.manufactureEmployerTransmissions(this.EmployerId, this.TaxYearId);

                string subId = filteredList.First(Item => Item.tax_year_employer_transmissionID == this.CorrectedTaxYearTransmissionId).UniqueSubmissionId;

                int SubmissionId = 0;
                int.TryParse(subId, out SubmissionId);

                return SubmissionId;

            }
        }

        private string CorrectedReciptId
        {
            get
            {

                if (this.DdlReceiptID.SelectedItem.Text.IsNullOrEmpty() || this.DdlReceiptID.SelectedItem.Text.Equals("Select"))
                {
                    return null;
                }

                return this.DdlReceiptID.SelectedItem.Text;
            }
        }

        private string SecurityHeaderTemplate { get { return this.ReturnTemplate("SecurityHeader"); } }
        private string ACATransmitterManifestTemplate { get { return this.ReturnTemplate("ACATransmitterManifest"); } }
        private string Form109495CTransmittalUpstreamTemplate { get { return this.ReturnTemplate("Form109495CTransmittalUpstream"); } }
        private string Form1094CUpstreamDetailTemplate { get { return this.ReturnTemplate("Form1094CUpstreamDetail"); } }
        private string GovtEntityEmployerInfoGrpTemplate { get { return this.ReturnTemplate("GovtEntityEmployerInfoGrp"); } }
        private string ALEMemberInformationGrpTemplate { get { return this.ReturnTemplate("ALEMemberInformationGrp"); } }
        private string Form1095CUpstreamDetailTemplate { get { return this.ReturnTemplate("Form1095CUpstreamDetail"); } }
        private string CorrectedRecordInfoGrpTemplate { get { return this.ReturnTemplate("CorrectedRecordInfoGrp"); } }
        private string CorrectedSubmissionInfoGrpTemplate { get { return this.ReturnTemplate("CorrectedSubmissionInfoGrp"); } }
        private string CoveredIndividualGrpTemplate { get { return this.ReturnTemplate("CoveredIndividualGrp"); } }
        private string CoveredIndividualMonthlyIndGrpTemplate { get { return this.ReturnTemplate("CoveredIndividualMonthlyIndGrp"); } }
        private string MonthlyOfferCoverageGrpTemplate { get { return this.ReturnTemplate("MonthlyOfferCoverageGrp"); } }
        private string MonthlyEmployeeRequiredContriGrpTemplate { get { return this.ReturnTemplate("MonthlyEmployeeRequiredContriGrp"); } }
        private string MonthlySafeHarborGrpTemplate { get { return this.ReturnTemplate("MonthlySafeHarborGrp"); } }
        private string OtherALEMembersGrpTemplate { get { return this.ReturnTemplate("OtherALEMembersGrp"); } }

        private int resetCount = 0;

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            this.Server.ScriptTimeout = Transmitter.Timeout * 3;

            List<employer> employers = employerController.getAllEmployers();
            this.PopulateEmployersDropDownList(this.DdlFilterEmployers, employers);

            List<int> taxYears = employerController.getTaxYears();
            CASSPrintFileGenerator.PopulateTaxYearDropDownList(this.DdlTaxYear, taxYears);

            this.loadTransmissionTypes();
        }

        private void PopulateEmployersDropDownList(DropDownList ddlFilterEmployers, List<employer> employers)
        {

            this.DdlFilterEmployers.DataSource = employers;
            this.DdlFilterEmployers.DataTextField = "EMPLOYER_NAME_And_EIN";
            this.DdlFilterEmployers.DataValueField = "EMPLOYER_ID";
            this.DdlFilterEmployers.DataBind();

            this.DdlFilterEmployers.Items.Add("Select");
            this.DdlFilterEmployers.SelectedIndex = this.DdlFilterEmployers.Items.Count - 1;
            if (this.resetCount > 0) { this.Response.Redirect(this.Request.RawUrl); }
            this.resetCount = 1;

        }

        private void loadTransmissionTypes()
        {
            this.DdlTransmissionType.DataSource = employerController.manufactureTransmissionType();

            this.DdlTransmissionType.DataTextField = "name";
            this.DdlTransmissionType.DataValueField = "transmissionID";
            this.DdlTransmissionType.DataBind();

            this.DdlTransmissionType.Items.Add("Select");
            this.DdlTransmissionType.SelectedIndex = this.DdlTransmissionType.Items.Count - 1;

        }

        protected void btnTransmit_Click(object sender, EventArgs e)
        {
            string selectText = "Select";
            if (this.DdlTaxYear.SelectedItem.Text == selectText)
            {
                this.MpeWebMessage.Show();
                this.LitMessage.Text = "You need to Select a TAX YEAR!";
                return;
            }
            else
            {
            }

        }

        private bool _1094Checked { get { return this.rblCorrections.SelectedValue == "1094C"; } }

        private bool _1095Checked { get { return this.rblCorrections.SelectedValue == "1095C"; } }

        private static String getEmployeeMedicalMonth(int ID, AcaEntities ctx)
        {
            DateTime MedicalMonth;
            MedicalMonth = (from emp in ctx.employees
                            join pyear in ctx.plan_year
                            on emp.plan_year_id equals pyear.plan_year_id
                            where emp.employee_id == ID
                            select pyear.startDate).SingleOrDefault();
            return MedicalMonth.Date.Month.ToString();
        }

        protected void btnGenerateACAForms_Click(object sender, EventArgs e)
        {

            bool validData = true;
            bool originalSubmissionComplete = false;
            int employerID = 0;
            int taxYear = 0;

            validData = errorChecking.validateDropDownSelection(this.DdlFilterEmployers, validData);
            validData = errorChecking.validateDropDownSelection(this.DdlTaxYear, validData);


            if (validData == true)
            {
                int.TryParse(this.DdlFilterEmployers.SelectedItem.Value, out employerID);
                int.TryParse(this.DdlTaxYear.SelectedItem.Value, out taxYear);
                originalSubmissionComplete = false; 

                if (this.DdlTransmissionType.SelectedValue == "R")
                {
                    if (this.RbTransmissionReplacement.Checked == false && this.RbSubmissionReplacement.Checked == false)
                    {
                        validData = false;
                        this.MpeWebMessage.Show();
                        this.LitMessage.Text = "Please select the type of replacement file that needs to be generated!";
                    }
                    else
                    {
                        validData = errorChecking.validateDropDownSelection(this.DdlOriginalSubmissionData, validData);
                        if (validData == false)
                        {
                            validData = false;
                            this.MpeWebMessage.Show();
                            this.LitMessage.Text = "Please select the Receipt ID that is being replaced!";
                        }
                    }
                }
                else if (this.DdlTransmissionType.SelectedValue == "C")
                {
                    if (this._1094Checked == false && this._1095Checked == false)
                    {
                        validData = false;
                        this.MpeWebMessage.Show();
                        this.LitMessage.Text = "You need to check at least one of the boxes for 1094 or 1095!";
                    }
                }
                else if (this.DdlTransmissionType.SelectedValue == "O")
                {
                    if (originalSubmissionComplete == true)
                    {
                        validData = false;
                        this.MpeWebMessage.Show();
                        this.LitMessage.Text = "An Original Transmission has already been filed with the IRS, please choose another option.";
                    }
                }

                if (validData == true) { this.GenerateACAForms(); }
            }
            else
            {
                this.MpeWebMessage.Show();
                this.LitMessage.Text = "Please correct any red highlight fields.";
            }

        }

        private void loadReceipts(int _employerID, int _year)
        {
            List<taxYearEmployerTransmission> filteredList = airController.manufactureEmployerTransmissions(_employerID, _year);

            filteredList = filteredList.Where(item => false == item.ReceiptID.IsNullOrEmpty()).ToList();

            this.DdlReceiptID.DataSource = filteredList;
            this.DdlReceiptID.DataTextField = "ReceiptID";
            this.DdlReceiptID.DataValueField = "tax_year_employer_transmissionID";
            this.DdlReceiptID.DataBind();

            this.DdlReceiptID.Items.Add("Select");
            this.DdlReceiptID.SelectedIndex = this.DdlReceiptID.Items.Count - 1;

        }


        private void GenerateACAForms()
        {
            this.lblMsg.Text = string.Empty;
            User User = (User)this.Session["CurrentUser"];
            DateTime currDate = DateTime.Now;
            string folder_path = HttpContext.Current.Server.MapPath("~/ftps/Transmit/");
            string bulk10941095FileName = null;
            string short10941095FileName = "~/ftps/Transmit/";
            string manifestFilename = null;
            string shortManifestFileName = "~/ftps/Transmit/";

            bool rejectedSubmissionInd = this.RbSubmissionReplacement.Checked;
            bool rejectedTransmissionInd = this.RbTransmissionReplacement.Checked;

            #region SecurityHeader
            string originalReceiptID = null;
            if (this.DdlTransmissionType.SelectedItem.Value == "R") { originalReceiptID = this.DdlOriginalSubmissionData.SelectedItem.Text; }

            SecurityHeader ACATransmitterManifest = new SecurityHeader()
            {
                PaymentYr = this.TaxYearId.ToString(),
                PriorYearDataInd = this.chbPriorYearInd.Checked ? "1" : "0",
                TransmissionTypeCd = this.DdlTransmissionType.SelectedItem.Value,
                ResourceId = Guid.NewGuid(),
                OriginalReceiptId = originalReceiptID
            };
            #endregion

            #region 1094UpstreamDetails
            if (this.EmployerId == 0)
            {
                List<Form1094CUpstreamDetail> form1094CUpstreamDetails = employerController.getForm1094CUpstreamDetails(this.TaxYearId, null, false, false, false);
                foreach (Form1094CUpstreamDetail form1094CUpstreamDetail in form1094CUpstreamDetails)
                {

                    EmployerTaxYearTransmissionStatus employerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(this.EmployerId, this.TaxYearId);
                    if (employerTaxYearTransmissionStatus.TransmissionStatusId != TransmissionStatusEnum.Halt)
                    {
                        ACATransmitterManifest.Form1094CUpstreamDetails.Add(form1094CUpstreamDetail);
                    }

                }

            }
            else
            {


                EmployerTaxYearRequest request = new EmployerTaxYearRequest();
                request.EmployerId = EmployerId;
                request.TaxYear = TaxYearId;
                request.Requester = User.User_UserName;


                var apiHelper = ContainerActivator._container.Resolve<IApiHelper>();
                UIMessageResponse message = apiHelper.Send<UIMessageResponse, EmployerTaxYearRequest>("Finalize1094-1094summary", request);

                EmployerTaxYearTransmissionStatus employerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(this.EmployerId, this.TaxYearId);

                if (employerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.Halt)
                {
                    this.MpeWebMessage.Show();
                    this.LitMessage.Text = "This Employer Is In Halt Status";
                    return;
                }

                IFinalize1094Service service1094 = ContainerActivator._container.Resolve<IFinalize1094Service>();
                service1094.Context = ContainerActivator._container.Resolve<ITransactionContext>();
                List<Approved1094FinalPart1> fromDB1094 = service1094.GetApproved1094sForEmployerTaxYear(this.EmployerId, this.TaxYearId);

                fromDB1094 = fromDB1094.OrderByDescending(item => item.ModifiedDate).ToList();
                fromDB1094 = new List<Approved1094FinalPart1> { fromDB1094.First() };
                ACATransmitterManifest.Form1094CUpstreamDetails = Mapper.Map<List<Approved1094FinalPart1>, List<Form1094CUpstreamDetail>>(fromDB1094);

                Form1094CUpstreamDetail This1094Form = ACATransmitterManifest.Form1094CUpstreamDetails.First();

                IFinalize1095Service service1095 = ContainerActivator._container.Resolve<IFinalize1095Service>();
                service1095.Context = ContainerActivator._container.Resolve<ITransactionContext>();
                List<Approved1095Final> fromDB1095 = service1095.GetApproved1095sForEmployerTaxYear(this.EmployerId, this.TaxYearId);

                fromDB1095 = fromDB1095.Where(item => item.Receiving1095 == true).ToList();

                This1094Form.QualifyingOfferMethodInd = fromDB1095.Any(item => item.part2s.Any(p2 => p2.Line14 == "1A")).BoolToOneZero();

                if (ACATransmitterManifest.TransmissionTypeCd == "C")
                {
                    HashSet<long> ApprovedTransmitted = new HashSet<long>(employerController.GetTransmittedApprovedIds(this.EmployerId));

                    fromDB1095 = fromDB1095.Where(item => false == ApprovedTransmitted.Contains(item.ID)).ToList();

                }

                Dictionary<int, Employee> employees = EmployeeController.manufactureEmployeeList(this.EmployerId).ToDictionary(item => item.EMPLOYEE_ID);
                foreach (Approved1095Final form in fromDB1095)
                {
                    Employee ee = employees[form.EmployeeID];
                    if (ee != null)
                    {
                        form.SSN = ee.Employee_SSN_Visible;
                        foreach (Approved1095FinalPart3 cov in form.part3s)
                        {
                            if (cov.DependantID == 0)
                            {
                                cov.SSN = ee.Employee_SSN_Visible;
                            }
                        }
                    }
                }

                if (ACATransmitterManifest.TransmissionTypeCd == "C" && this._1094Checked)
                {

                    This1094Form.Form1095CUpstreamDetails = new List<Form1095CUpstreamDetail>();

                    This1094Form.Form1095CAttachedCnt = "0";

                }
                else
                {

                    This1094Form.Form1095CUpstreamDetails = Mapper.Map<List<Approved1095Final>, List<Form1095CUpstreamDetail>>(fromDB1095);

                    This1094Form.Form1095CAttachedCnt = This1094Form.Form1095CUpstreamDetails.Count.ToString();

                }

                string CorrectedUniqueSubmissionId = "";
                List<taxYearEmployeeTransmission> tyetList = new List<taxYearEmployeeTransmission>();
                if (ACATransmitterManifest.TransmissionTypeCd == "C")
                {

                    tyetList = airController.manufactureAllEmployeeTransmissions(this.CorrectedTaxYearTransmissionId);

                    int addition = 1;
                    if (this._1094Checked)
                    {
                        addition = 2;
                        This1094Form.CorrectedInd = "1";
                    }

                    if (this.DdlReceiptID.SelectedItem.Text.IsNullOrEmpty() || this.DdlReceiptID.SelectedItem.Text.Equals("Select"))
                    {
                        this.MpeWebMessage.Show();
                        this.LitMessage.Text = "Please Select a transmission Recipt Id that this transmission is correcting.";
                        return;
                    }

                    This1094Form.SubmissionId = (this.CorrectedSubmissionId + addition).ToString();
                    if (this._1095Checked)
                    {
                        if (this.CorrectedIncrementId >= 2 && this.CorrectedIncrementId1094C >= 1)
                        { CorrectedUniqueSubmissionId = this.CorrectedReciptId + '|' + (this.CorrectedIncrementId - addition); }
                        else
                        {
                            CorrectedUniqueSubmissionId = this.CorrectedReciptId + '|' + this.CorrectedSubmissionId;
                        }
                    }
                    else
                    { CorrectedUniqueSubmissionId = this.CorrectedReciptId + '|' + this.CorrectedSubmissionId; }

                    if (this._1094Checked)
                    {
                        This1094Form.CorrectedSubmissionInfoGrp = new CorrectedSubmissionInfoGrp()
                        {

                            CorrectedUniqueSubmissionId = this.CorrectedReciptId + '|' + this.CorrectedSubmissionId,
                            BusinessNameLine1Txt = This1094Form.BusinessNameLine1Txt,
                            BusinessNameLine2Txt = This1094Form.BusinessNameLine2Txt,
                            CorrectedSubmissionPayerTIN = This1094Form.EmployerEIN
 
                        };
                    }
                }

                List<Form1095CUpstreamDetail> toRemove = new List<Form1095CUpstreamDetail>();
                string MedMonthly;
                foreach (Form1095CUpstreamDetail detail in This1094Form.Form1095CUpstreamDetails)
                {
                    using (AcaEntities ctx = new AcaEntities())
                    {
                        ctx.Database.CommandTimeout = 180;
                        MedMonthly = getEmployeeMedicalMonth(detail.employee_id, ctx);
                    }
                    detail.StartMonthNumberCd = MedMonthly.ZeroPad(2);
                    detail.ALEContactPhoneNum = This1094Form.ContactPhoneNum;                       

                    if (ACATransmitterManifest.TransmissionTypeCd == "C")
                    {

                        taxYearEmployeeTransmission empTrans = tyetList.Where(Item => Item.employee_id == detail.employee_id).SingleOrDefault();
                        if (empTrans != null)
                        {
                            int oldRecordId = empTrans.RecordID;

                            detail.CorrectedInd = "1";
                            detail.CorrectedRecordInfoGrp = new CorrectedRecordInfoGrp()
                            {

                                CorrectedUniqueRecordId = CorrectedUniqueSubmissionId + "|" + oldRecordId,
                                PersonFirstNm = detail.OtherCompletePersonFirstNm,
                                PersonLastNm = detail.OtherCompletePersonLastNm

                            };
                        }
                        else
                        {
                            toRemove.Add(detail);
                        }
                    }

                }

                foreach (Form1095CUpstreamDetail remove in toRemove)
                {
                    This1094Form.Form1095CUpstreamDetails.Remove(remove);
                }

                int i = 1;
                foreach (Form1095CUpstreamDetail detail in This1094Form.Form1095CUpstreamDetails)
                {
                    detail.RecordId = i.ToString();
                    i++;
                }
            }

            if (ACATransmitterManifest.Form1094CUpstreamDetails.FirstOrDefault() == null)
            {
                this.lblMsg.Text = "This Employer has no 1094Forms!";
                return;
            }

            #endregion

            #region Build 109495 XML File for Submission
            string Form1094CUpstreamDetailText = this.GenerateForm1094CUpstreamDetails(ACATransmitterManifest.Form1094CUpstreamDetails);
            ACATransmitterManifest.SetCountValues();
            ACATransmitterManifest.EndTimestampAndSetFileName();

            string Form109495CTransmittalUpstreamText = this.Form109495CTransmittalUpstreamTemplate.Replace("@@Form1094CUpstreamDetail@@", Form1094CUpstreamDetailText);
            Form1094CUpstreamDetailText = Regex.Replace(Form1094CUpstreamDetailText, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);

            XmlDocument xmlFormDoc = new XmlDocument();
            xmlFormDoc.LoadXml(Form109495CTransmittalUpstreamText);

            bulk10941095FileName = string.Format(@"{0}/{1}", folder_path, ACATransmitterManifest.DocumentSystemFileNm);

            short10941095FileName += ACATransmitterManifest.DocumentSystemFileNm;

            xmlFormDoc.Save(bulk10941095FileName);

            ACATransmitterManifest.SetAttachmentByteSizeNumAndChecksumAugmentationNum(bulk10941095FileName);

            #endregion

            #region Create the Header
            header header = new header()
            {
                transmitter_control_code = Transmitter.TransmitterControlCode,
                unique_transmission_id = ACATransmitterManifest.UniqueTransmissionId,
                transmission_timestamp = Convert.ToDateTime(ACATransmitterManifest.Timestamp)
            };

            header = employerController.insertUpdateHeader(header);
            #endregion

            #region Build the Manifest XML File for Submission.
            string ACATransmitterManifestText = this.ReplacePropertyValues(this.ACATransmitterManifestTemplate, ACATransmitterManifest);
            ACATransmitterManifestText = Regex.Replace(ACATransmitterManifestText, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);

            XmlDocument xmlManifestDoc = new XmlDocument();
            xmlManifestDoc.LoadXml(ACATransmitterManifestText);
            manifestFilename = string.Format(@"{0}/Manifest_{1}", folder_path, ACATransmitterManifest.DocumentSystemFileNm);
            xmlManifestDoc.Save(manifestFilename);
            shortManifestFileName += "Manifest_" + ACATransmitterManifest.DocumentSystemFileNm;
            Form1094CUpstreamDetail form_1094 = ACATransmitterManifest.Form1094CUpstreamDetails.First();
            #endregion

            #region Save manifest to the database
            #endregion

            #region Save 1094C to the database.
            #endregion

            #region Save the _1095 data to the databse.
            List<taxYearEmployerTransmission> employeeTransmissions = new List<taxYearEmployerTransmission>();
            if (this._1094Checked)
            {

                EmployeeController.InsertSingleTaxYearEmployerTransmission(
                           this.TaxYearId,
                           this.EmployerId,
                           ACATransmitterManifest.TransmissionTypeCd,
                           ACATransmitterManifest.UniqueTransmissionId,
                           form_1094.SubmissionId,
                           4,
                           ACATransmitterManifest.OriginalReceiptId,
                           form_1094.OriginalUniqueSubmissionId,
                           User.User_UserName,
                           short10941095FileName,
                           shortManifestFileName);

            }
            else
            {



                foreach (Form1095CUpstreamDetail form_1095 in form_1094.Form1095CUpstreamDetails)
                {
                    #region Obsolete Code When the Change Occures
                    #endregion
                    taxYearEmployerTransmission tyet = new taxYearEmployerTransmission()
                    {
                        tax_year_employer_transmissionID = 0,
                        tax_year = TaxYearId,
                        employee_id = form_1095.employee_id,
                        employer_id = form_1095.employer_id,
                        ResourceId = Guid.NewGuid(),
                        TransmissionType = ACATransmitterManifest.TransmissionTypeCd,
                        UniqueTransmissionId = ACATransmitterManifest.UniqueTransmissionId,
                        ReceiptID = null,
                        UniqueSubmissionId = form_1094.SubmissionId,
                        RecordID = form_1095.RecordId.ToInteger().Value,
                        transmission_status_code_id = 4,                                                 
                        OriginalReceiptId = ACATransmitterManifest.OriginalReceiptId,                                       
                        OriginalUniqueSubmissionId = form_1094.OriginalUniqueSubmissionId,                  
                        EntityStatusId = 1,                                                              
                        createdBy = User.User_UserName,
                        createdOn = currDate,
                        ModifiedBy = User.User_UserName,
                        ModifiedDate = currDate,
                        BulkFile = short10941095FileName,
                        manifestFile = shortManifestFileName
                    };
                    employeeTransmissions.Add(tyet);

                }

                EmployeeController.insertTaxYearEmployerTransmission(employeeTransmissions);
            }
            #endregion

            #region Log Info
            PIILogger.LogPII(string.Format("User {0} Downloaded IRS Transmission Files {1} and {2}",
                User.User_UserName, manifestFilename, bulk10941095FileName));
            #endregion

            #region Update Transmission Status
            EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(this.EmployerId, this.TaxYearId);
            employerController.endEmployerTaxYearTransmissionStatus(currentEmployerTaxYearTransmissionStatus, User.User_UserName);
            if (currentEmployerTaxYearTransmissionStatus != null)
            {

                TransmissionStatusEnum transmissionStatus;
                if (this.DdlTransmissionType.SelectedValue == "C")
                {
                    if (this._1095Checked)
                    {
                        transmissionStatus = TransmissionStatusEnum.ReTransmitted;
                    }
                    else
                    {
                        transmissionStatus = TransmissionStatusEnum.ReTransmit;
                    }
                }
                else
                {
                    transmissionStatus = TransmissionStatusEnum.Transmit;
                }

                EmployerTaxYearTransmissionStatus newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                        currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                        transmissionStatus,
                        User.User_UserName,
                        DateTime.Now
                    );

                newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);

                if (newEmployerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.ReTransmitted && this._1095Checked)
                {
                    if (form_1094.Form1095CUpstreamDetails.Count <= 0)
                    {
                        this.lblMsg.Text = "This Employer has no 1095Forms ready for retransmit!";

                        return;
                    }

                    foreach (Form1095CUpstreamDetail form_1095 in form_1094.Form1095CUpstreamDetails)
                    {

                        TaxYear1095CCorrection taxYear1095CCorrection = new TaxYear1095CCorrection()
                        {
                            tax_year = TaxYearId,
                            employee_id = form_1095.employee_id,
                            Transmitted = true,
                            ModifiedBy = User.User_UserName
                        };

                        employerController.updateTaxYear1095cCorrectionTransmittedBit(taxYear1095CCorrection);
                    }
                }
            }
            #endregion

            this.pnlRejection.Visible = false;
            this.pnlOriginalReceiptId.Visible = false;
            this.DdlOriginalSubmissionData.SelectedIndex = this.DdlOriginalSubmissionData.Items.Count - 1;

            #region Zip the Bulk and Manifest Files for the user to download.
            using (ZipFile zip = new ZipFile())
            {
                string zip_directory = "Xml Files";
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName(zip_directory);
                zip.AddFile(manifestFilename, zip_directory);
                zip.AddFile(bulk10941095FileName, zip_directory);

                this.Response.Clear();
                this.Response.BufferOutput = false;
                string zipName = string.Format("{0}_IRS_Transmission_{1}", ACATransmitterManifest.Form1094CUpstreamDetails.First().BusinessNameLine1Txt, ACATransmitterManifest.FullTimestamp);
                this.Response.ContentType = "application/zip";
                this.Response.AddHeader("content-disposition", "attachment; filename=" + zipName.CleanFileName() + ".zip");

                zip.Save(this.Response.OutputStream);

                this.Response.Flush();         
                this.Response.SuppressContent = true;                
                HttpContext.Current.ApplicationInstance.CompleteRequest();                      
                this.Response.End();
            }
            #endregion
        }

        private string ReturnTemplate(string textFile)
        {
            string templateFolderPath = HttpContext.Current.Server.MapPath("~/Code/XMLTemplates/");
            string templatePath = string.Format(@"{0}/{1}.txt", templateFolderPath, textFile);
            FileStream fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
            return streamReader.ReadToEnd();
        }

        private List<string> propertyHeaders { get { return new List<string>() { "Govt", "Yearly", "OtherALE", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" }; } }

        private string ReplacePropertyValues<T>(string template, T obj) where T : class, new()
        {
            string text = template;
            string propertyName = string.Empty;
            try
            {
                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    if (property.PropertyType == typeof(string))
                    {
                        propertyName = property.Name;
                        string value = (property.GetValue(obj) == null) ? string.Empty : property.GetValue(obj).ToString().Trim();
                        string replacingString = string.Empty;
                        if (string.IsNullOrEmpty(value))
                        {
                            replacingString = string.Format("<{0}>@@{0}@@</{0}>", propertyName);
                            text = text.Replace(replacingString, value);

                            replacingString = string.Format("<irs:{0}>@@{0}@@</irs:{0}>", propertyName);
                            text = text.Replace(replacingString, value);

                            replacingString = string.Format("<urn:{0}>@@{0}@@</urn:{0}>", propertyName);
                            text = text.Replace(replacingString, value);

                            replacingString = string.Format("<urn1:{0}>@@{0}@@</urn1:{0}>", propertyName);
                            text = text.Replace(replacingString, value);

                            foreach (string propertyHeader in this.propertyHeaders)
                            {
                                if (propertyName.Contains(propertyHeader))
                                {
                                    string propertyTag = propertyName.Replace(propertyHeader, "");

                                    replacingString = string.Format("<{0}>@@{1}@@</{0}>", propertyTag, propertyName);
                                    text = text.Replace(replacingString, value);

                                    replacingString = string.Format("<irs:{0}>@@{1}@@</irs:{0}>", propertyTag, propertyName);
                                    text = text.Replace(replacingString, value);

                                    replacingString = string.Format("<urn:{0}>@@{1}@@</urn:{0}>", propertyTag, propertyName);
                                    text = text.Replace(replacingString, value);

                                    replacingString = string.Format("<urn1:{0}>@@{1}@@</urn1:{0}>", propertyTag, propertyName);
                                }
                            }

                        }
                        else
                        {
                            replacingString = string.Format("@@{0}@@", propertyName);
                            text = text.Replace(replacingString, Transmitter.EscapeXMLValue(value));
                        }

                    }
                }
                return text;
            }
            catch (Exception ex)
            {
                this.Log.Error(string.Format("Exception in template {0} for property {1}: {2}", template, propertyName, ex.Message));
                return string.Empty;
            }

        }

        private string GenerateForm1094CUpstreamDetails(List<Form1094CUpstreamDetail> form1094CUpstreamDetails)
        {
            string Form1094CUpstreamDetailText = string.Empty;

            foreach (Form1094CUpstreamDetail form1094CUpstreamDetail in form1094CUpstreamDetails)
            {
                bool validData = true;
                validData = errorChecking.validateDropDownSelection(this.DdlOriginalSubmissionData, validData);

                if (this.DdlTransmissionType.SelectedValue == "R" && validData == true)
                {
                    form1094CUpstreamDetail.OriginalUniqueSubmissionId = this.DdlOriginalSubmissionData.SelectedItem.Value;
                    string submissionId = form1094CUpstreamDetail.OriginalUniqueSubmissionId.Substring(form1094CUpstreamDetail.OriginalUniqueSubmissionId.LastIndexOf('|') + 1);
                    form1094CUpstreamDetail.SubmissionId = submissionId;
                }


                if (this.DdlTransmissionType.SelectedValue == "O")
                {
                    string submissionId = "1";
                    form1094CUpstreamDetail.SubmissionId = submissionId;

                }
                string currentForm1094CUpstreamDetail = this.ReplacePropertyValues(this.Form1094CUpstreamDetailTemplate, form1094CUpstreamDetail);

                string correctedSubmissionInfoGrpReplacingText = (form1094CUpstreamDetail.CorrectedSubmissionInfoGrp == null) ? "" : this.ReplacePropertyValues(this.CorrectedSubmissionInfoGrpTemplate, form1094CUpstreamDetail.CorrectedSubmissionInfoGrp);
                currentForm1094CUpstreamDetail = currentForm1094CUpstreamDetail.Replace("@@CorrectedSubmissionInfoGrp@@", correctedSubmissionInfoGrpReplacingText);

                string Form1095CUpstreamDetailText = string.Empty;
                foreach (Form1095CUpstreamDetail form1095CUpstreamDetail in form1094CUpstreamDetail.Form1095CUpstreamDetails)
                {
                    string currentForm1095CUpstreamDetail = this.ReplacePropertyValues(this.Form1095CUpstreamDetailTemplate, form1095CUpstreamDetail);

                    string correctedRecordInfoGrpReplacingText = (form1095CUpstreamDetail.CorrectedRecordInfoGrp == null) ? "" : this.ReplacePropertyValues(this.CorrectedRecordInfoGrpTemplate, form1095CUpstreamDetail.CorrectedRecordInfoGrp);
                    currentForm1095CUpstreamDetail = currentForm1095CUpstreamDetail.Replace("@@CorrectedRecordInfoGrp@@", correctedRecordInfoGrpReplacingText);

                    string monthlyOfferCoverageGrpReplacingText = (form1095CUpstreamDetail.MonthlyOfferCoverageGrp == null) ? "" : this.ReplacePropertyValues(this.MonthlyOfferCoverageGrpTemplate, form1095CUpstreamDetail.MonthlyOfferCoverageGrp);
                    currentForm1095CUpstreamDetail = currentForm1095CUpstreamDetail.Replace("@@MonthlyOfferCoverageGrp@@", monthlyOfferCoverageGrpReplacingText);

                    string MonthlyEmployeeRequiredContriGrpReplacingText = (form1095CUpstreamDetail.MonthlyEmployeeRequiredContriGrp == null) ? "" : this.ReplacePropertyValues(this.MonthlyEmployeeRequiredContriGrpTemplate, form1095CUpstreamDetail.MonthlyEmployeeRequiredContriGrp);
                    currentForm1095CUpstreamDetail = currentForm1095CUpstreamDetail.Replace("@@MonthlyEmployeeRequiredContriGrp@@", MonthlyEmployeeRequiredContriGrpReplacingText);

                    string monthlySafeHarborGrpReplacingText = (form1095CUpstreamDetail.MonthlySafeHarborGrp == null) ? "" : this.ReplacePropertyValues(this.MonthlySafeHarborGrpTemplate, form1095CUpstreamDetail.MonthlySafeHarborGrp);
                    currentForm1095CUpstreamDetail = currentForm1095CUpstreamDetail.Replace("@@MonthlySafeHarborGrp@@", monthlySafeHarborGrpReplacingText);

                    string CoveredIndividualGrpsText = string.Empty;
                    if (form1095CUpstreamDetail.CoveredIndividualGrps != null)
                    {
                        foreach (CoveredIndividualGrp coveredIndividualGrp in form1095CUpstreamDetail.CoveredIndividualGrps)
                        {
                            string currentCoveredIndividualGrp = this.ReplacePropertyValues(this.CoveredIndividualGrpTemplate, coveredIndividualGrp);
                            string coveredIndividualMonthlyIndGrpReplacingText = (coveredIndividualGrp.CoveredIndividualMonthlyIndGrp == null) ? "" : this.ReplacePropertyValues(this.CoveredIndividualMonthlyIndGrpTemplate, coveredIndividualGrp.CoveredIndividualMonthlyIndGrp);
                            currentCoveredIndividualGrp = currentCoveredIndividualGrp.Replace("@@CoveredIndividualMonthlyIndGrp@@", coveredIndividualMonthlyIndGrpReplacingText);
                            CoveredIndividualGrpsText += currentCoveredIndividualGrp;
                        }
                    }

                    currentForm1095CUpstreamDetail = currentForm1095CUpstreamDetail.Replace("@@CoveredIndividualGrp@@", CoveredIndividualGrpsText);

                    currentForm1095CUpstreamDetail = currentForm1095CUpstreamDetail.Replace("<MonthlyEmployeeRequiredContriGrp></MonthlyEmployeeRequiredContriGrp>", "");
                    currentForm1095CUpstreamDetail = currentForm1095CUpstreamDetail.Replace("<MonthlySafeHarborGrp></MonthlySafeHarborGrp>", "");
                    Form1095CUpstreamDetailText += currentForm1095CUpstreamDetail;
                    Form1095CUpstreamDetailText += "\r\n";

                }

                string govtEntityEmployerInfoGrpReplacingText = (form1094CUpstreamDetail.GovtEntityEmployerInfoGrp == null) ? "" : this.ReplacePropertyValues(this.GovtEntityEmployerInfoGrpTemplate, form1094CUpstreamDetail.GovtEntityEmployerInfoGrp);
                currentForm1094CUpstreamDetail = currentForm1094CUpstreamDetail.Replace("@@GovtEntityEmployerInfoGrp@@", govtEntityEmployerInfoGrpReplacingText);

                string aleMemberInformationGrpReplacingText = (form1094CUpstreamDetail.ALEMemberInformationGrp == null) ? "" : this.ReplacePropertyValues(this.ALEMemberInformationGrpTemplate, form1094CUpstreamDetail.ALEMemberInformationGrp);
                currentForm1094CUpstreamDetail = currentForm1094CUpstreamDetail.Replace("@@ALEMemberInformationGrp@@", aleMemberInformationGrpReplacingText);

                string OtherALEMembersGrpText = string.Empty;
                if (form1094CUpstreamDetail.OtherALEMembersGrps != null)
                {
                    foreach (OtherALEMembersGrp otherALEMembersGrp in form1094CUpstreamDetail.OtherALEMembersGrps)
                    {
                        string currentOtherALEMembersGrp = this.ReplacePropertyValues(this.OtherALEMembersGrpTemplate, otherALEMembersGrp);
                        OtherALEMembersGrpText += currentOtherALEMembersGrp;
                        OtherALEMembersGrpText += "\r\n";
                    }
                }
                currentForm1094CUpstreamDetail = currentForm1094CUpstreamDetail.Replace("@@OtherALEMembersGrp@@", OtherALEMembersGrpText);

                currentForm1094CUpstreamDetail = currentForm1094CUpstreamDetail.Replace("@@Form1095CUpstreamDetail@@", Form1095CUpstreamDetailText);
                Form1094CUpstreamDetailText += currentForm1094CUpstreamDetail;
                Form1094CUpstreamDetailText += "\r\n";

            }

            return Form1094CUpstreamDetailText;

        }

        protected void DdlTransmissionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DdlTransmissionType.SelectedValue == "C")
            {
                this.pnlCorrection.Visible = true;
                this.pnlRejection.Visible = false;
                this.pnlOriginalReceiptId.Visible = false;

                this.loadReceipts(this.EmployerId, this.TaxYearId);

            }


            if (this.DdlTransmissionType.SelectedValue == "R")
            {
                this.pnlCorrection.Visible = false;
                this.pnlRejection.Visible = true;
                this.pnlOriginalReceiptId.Visible = true;
            }

            if (this.DdlTransmissionType.SelectedValue == "O")
            {
                this.pnlCorrection.Visible = false;
                this.pnlRejection.Visible = false;
                this.pnlOriginalReceiptId.Visible = false;
            }
        }



        protected void RbTransmissionReplacement_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RbTransmissionReplacement.Checked == true)
            {
                this.loadTransmissionReceipts();
            }
        }

        protected void RbSubmissionReplacement_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RbSubmissionReplacement.Checked == true)
            {
                this.loadTransmissionReceipts();
            }
        }

        private void loadTransmissionReceipts()
        {
            bool validData = true;
            int taxYear = 0;
            int employerID = 0;

            validData = errorChecking.validateDropDownSelection(this.DdlTaxYear, validData);
            validData = errorChecking.validateDropDownSelection(this.DdlFilterEmployers, validData);
            if (validData == true)
            {
                int.TryParse(this.DdlTaxYear.SelectedItem.Text, out taxYear);
                int.TryParse(this.DdlFilterEmployers.SelectedItem.Value, out employerID);
                List<taxYearEmployerTransmission> tyetList = airController.manufactureEmployerTransmissions(employerID, taxYear);
                List<taxYearEmployerTransmission> tyetFilteredList = new List<taxYearEmployerTransmission>();

                this.DdlOriginalSubmissionData.DataSource = tyetList;
                this.DdlOriginalSubmissionData.DataTextField = "ReceiptID";
                this.DdlOriginalSubmissionData.DataValueField = "getUSID";
                this.DdlOriginalSubmissionData.DataBind();

                this.DdlOriginalSubmissionData.Items.Add("Select");
                this.DdlOriginalSubmissionData.SelectedIndex = this.DdlOriginalSubmissionData.Items.Count - 1;
            }
            else
            {
                this.MpeWebMessage.Show();
                this.LitMessage.Text = "Please select the employer and tax year!";
                this.DdlOriginalSubmissionData.Items.Clear();
            }
        }

        protected void DdlReceiptID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string temp = this.CorrectedReciptId + '|' + this.CorrectedSubmissionId;
            this.Log.Debug(temp);

        }
    }
}