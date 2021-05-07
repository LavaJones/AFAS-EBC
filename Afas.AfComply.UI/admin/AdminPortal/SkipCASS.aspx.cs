using Afas.AfComply.UI.Areas;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Afas.AfComply.UI.admin.AdminPortal
{
    
    public partial class SkipCASS : Afas.AfComply.UI.admin.AdminPageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(SkipCASS));
        private int TaxYearId
        {
            get
            {
                int taxYearId = 0;
                int.TryParse(DdlTaxYear.SelectedValue, out taxYearId);
                return taxYearId;
            }
        }

        private int EmployerId
        {
            get
            {
                int employerId = 0;
                int.TryParse(DdlFilterEmployers.SelectedItem.Value, out employerId);
                return employerId;
            }
        }

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            CASSPrintFileGenerator.PopulateFormsDropDownList(DdlForm);
        }

        protected void DdlForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            var employers = employerController.getAllEmployers();
            CASSPrintFileGenerator.PopulateEmployersDropDownList(DdlFilterEmployers, employers);
        }

        protected void DdlFilterEmployers_SelectedIndexChanged(object sender, EventArgs e)
        {

            lblMsg.Text = "";
            if (CASSPrintFileGenerator.ValidateDdlFilterEmployersSelectedItem(DdlFilterEmployers, DdlTaxYear, MpeWebMessage, LitMessage))
            {
                loadTaxYears();
            }
        }

        private void loadTaxYears()
        {
            var taxYears = employerController.getTaxYears();
            CASSPrintFileGenerator.PopulateTaxYearDropDownList(DdlTaxYear, taxYears);
        }
        protected void BtnPrint_Click(object sender, EventArgs e)
        {

            
            if (chkConfirm.Checked)
            {
                //EmployerTaxYearTransmissionStatus currentEployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EmployerId, TaxYearId);
                //if (currentEployerTaxYearTransmissionStatus == null)
                //{
                //    lblMsg.Text = CASSPrintFileGenerator.NoEmployerTaxTransmissionErrorMessage;
                //    return;
                //}
                //TransmissionStatusEnum currentEmployerTaxYearTransmissionStatusId = currentEployerTaxYearTransmissionStatus.TransmissionStatusId;

                List<Employee_IRS> tempList;
                //if (currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.CompanyApproved || currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.SkipCASS || currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.Halt || currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.Print || currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.Printed || currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.Transmit)
                //{
                    tempList = employerController.GetEmployeeWithEmployerInfo(EmployerId, TaxYearId);
                //}
                //else
                //{
                //    MpeWebMessage.Show();
                //    LitMessage.Text = CASSPrintFileGenerator.SkipCASSErrorMessage;
                //    return;
                //}

                var form = DdlForm.SelectedItem.Text.Replace(" ", "");

                var file_name = CASSPrintFileGenerator.FileName(form, tempList.FirstOrDefault());

                if (String.IsNullOrEmpty(file_name))
                {
                    lblMsg.Text = CASSPrintFileGenerator.NoRecordsFoundErrorMessage;
                    return;
                }

                //var folder_path = string.Format(@"{0}ftps\Print", Server.MapPath("~/"));
                //var folder_path = new Uri(Server.MapPath("~/ftps/Print")).LocalPath;
                var folder_path = HttpContext.Current.Server.MapPath("~/ftps/Print/");

                string csv = CASSPrintFileGenerator.GeneratePrintCSVContent(chbCorrected.Checked, form, tempList, this.Log);

                lblMsg.Text = CASSPrintFileGenerator.WriteCSVContentToFile(folder_path, file_name, csv, EmployerId);

                User user = ((User)Session["CurrentUser"]);

                //if (currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.CompanyApproved || currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.Halt || currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.Print || currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.Printed || currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.SkipCASS || currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.Transmit || currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.Accepted || currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.AcceptedWithErrors || currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.Rejected)
                //{
                //    var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                //          currentEployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                //          TransmissionStatusEnum.SkipCASS,
                //          user.User_UserName,
                //          DateTime.Now
                //      );

                //    newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);
                //}


            }
            else
            {
                MpeWebMessage.Show();
                LitMessage.Text = "You need to check the confirm box!";
            }
        }

    }
}
