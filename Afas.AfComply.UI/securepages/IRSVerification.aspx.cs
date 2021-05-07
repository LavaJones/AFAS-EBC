using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Afas.AfComply.Domain;
using log4net;
using System.Data;

public partial class IRSVerification : Afas.AfComply.UI.securepages.SecurePageBase
{

    private ILog Log = LogManager.GetLogger(typeof(IRSVerification));

    private int EmployerId { get { return int.Parse(HfDistrictID.Value); } }
    private int TaxYearId { get { return Feature.PreviousReportingYear; } }

    protected override void PageLoadLoggedIn(User user, employer employer)
    {

        if (null == employer || false == employer.IrsEnabled)
        {
        
            Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [IRSVerification] which is is not yet enabled for them.");

            Response.Redirect("~/default.aspx?error=64", false);

            return;
        
        }

                LitUserName.Text = user.User_UserName;

        HfDistrictID.Value = user.User_District_ID.ToString();

        lblCompleteQuestionnaireStatus.Text = String.Format("Not Complete – must finish by {0} after completing Step 1", Branding.IrsDeadlineSetup);
        lblReviewAndApproveStatus.Text = String.Format("Not Complete – must finish by {0} after completing Step 2", Branding.IrsDeadlineCertify);

        lkTransmit.Visible = false;
        lkUnapproval.Visible = false;
        lkTransmissionStatus.Visible = false;

        var currentEmployerTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EmployerId, TaxYearId);
        if (currentEmployerTransmissionStatus != null)
        {

            if (currentEmployerTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.Halt)
            {
                lkTransmit.Visible = true;
            }

            switch (currentEmployerTransmissionStatus.TransmissionStatusId)
            {

                case TransmissionStatusEnum.Initiated:

                    if (currentEmployerTransmissionStatus.EndDate != null)
                    {
                        SetLabelToComplete(lblConfirmDataStatus);
                    }

                    break;

                case TransmissionStatusEnum.F1094_Collected:

                    SetLabelToComplete(lblConfirmDataStatus);
                    SetLabelToComplete(lblCompleteQuestionnaireStatus);
                    if (currentEmployerTransmissionStatus.EndDate != null)
                    {
                        lblEtlBuildProcess.Visible = true;
                    }

                    break;

                case TransmissionStatusEnum.ETL:

                    SetLabelToComplete(lblConfirmDataStatus);
                    SetLabelToComplete(lblCompleteQuestionnaireStatus);
                    if (currentEmployerTransmissionStatus.EndDate == null)
                    {
                        lblEtlBuildProcess.Visible = true;
                    }
                    else
                    {
                        EnableReviewInformationAndCodesLink();
                    }

                    break;

                case TransmissionStatusEnum.Review:

                    SetLabelToComplete(lblConfirmDataStatus);
                    SetLabelToComplete(lblCompleteQuestionnaireStatus);
                    EnableReviewInformationAndCodesLink();
                    if (currentEmployerTransmissionStatus.EndDate != null)
                    {
                        SetLabelToComplete(lblReviewAndApproveStatus);
                    }

                    break;

                default:

                    SetLabelToComplete(lblConfirmDataStatus);
                    SetLabelToComplete(lblCompleteQuestionnaireStatus);
                    SetLabelToComplete(lblReviewAndApproveStatus);
                    EnableReviewInformationAndCodesLink();

                    break;
            }

                lkViewPdfOfCompleted1095C.Visible = true;
            List<EmployerTaxYearTransmissionStatus> employerTaxYearTransmissionStatus = employerController.getEmployerTaxYearTransmissionStatusesByEmployerIdAndTaxYearId(EmployerId, TaxYearId);
            var statuses = employerTaxYearTransmissionStatus.FirstOrDefault(s => s.TransmissionStatusId == TransmissionStatusEnum.Transmit || s.TransmissionStatusId == TransmissionStatusEnum.Transmitted || s.TransmissionStatusId == TransmissionStatusEnum.ReTransmit || s.TransmissionStatusId == TransmissionStatusEnum.ReTransmitted || s.TransmissionStatusId == TransmissionStatusEnum.Accepted || s.TransmissionStatusId == TransmissionStatusEnum.Rejected || s.TransmissionStatusId == TransmissionStatusEnum.AcceptedWithErrors);

            if (statuses != null)
            {

                lkTransmissionStatus.Visible = true;
                litTransmissionStatus.Visible = false;

                lkUnapproval.Visible = false;
                litUnapproval.Visible = true;

            }

        }


    }

    private void SetLabelToComplete(Label lbl){
        lbl.Text = "Complete";
        lbl.ForeColor = System.Drawing.Color.Black;
    }

    private void EnableReviewInformationAndCodesLink()
    {

        lkReviewInformationAndCodes.Visible = true;
        litReviewInformationAndCodes.Visible = false;

        lkUnapproval.Visible = true;
        litUnapproval.Visible = false;
    
    }










    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

}