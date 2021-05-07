using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Diagnostics;
using System.IO;

public partial class admin_export_employers : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_export_employers));

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

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {
        LitUserName.Text = user.User_UserName;
        CASSPrintFileGenerator.PopulateFormsDropDownList(DdlForm);
    }

    protected void DdlFilterForms_SelectedIndexChanged(object sender, EventArgs e)
    {
        var employers = employerController.getAllEmployers();
        CASSPrintFileGenerator.PopulateEmployersDropDownList(DdlFilterEmployers, employers);
    }

    protected void DdlFilterEmployers_SelectedIndexChanged(object sender, EventArgs e)
    {
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

    protected void btnPrintFile_Click(object sender, EventArgs e)
    {

        EmployerTaxYearTransmissionStatus currentEployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EmployerId, TaxYearId);

        if (currentEployerTaxYearTransmissionStatus == null)
        {
            lblMsg.Text = CASSPrintFileGenerator.NoEmployerTaxTransmissionErrorMessage;
            return;
        }
        
        TransmissionStatusEnum currentEmployerTaxYearTransmissionStatusId = TransmissionStatusEnum.CASSRecieved;

        List<Employee_IRS> tempList;
        if (currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.CompanyApproved)
        {
            tempList = employerController.GetEmployeeWithEmployerInfo(EmployerId, TaxYearId, false);
        }
        else if (currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.CASSRecieved)
        {
            tempList = employerController.GetEmployeeWithEmployerInfo(EmployerId, TaxYearId);
        }
        else
        {
            MpeWebMessage.Show();
            LitMessage.Text = CASSPrintFileGenerator.PrintCASSErrorMessage;
            return;
        }

        var form = DdlForm.SelectedItem.Text.Replace(" ", "");

        var file_name = CASSPrintFileGenerator.FileName(form, tempList.FirstOrDefault());

        if (String.IsNullOrEmpty(file_name))
        {
            lblMsg.Text = CASSPrintFileGenerator.NoRecordsFoundErrorMessage;
            return;
        }

        if (currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.CompanyApproved)
        {
            var folder_path = string.Format(@"{0}ftps\CASS", Server.MapPath("~/"));

            string csv = CASSPrintFileGenerator.GenerateCassCSVContent(form,tempList);

            lblMsg.Text = CASSPrintFileGenerator.WriteCSVContentToFile(folder_path, file_name, csv, EmployerId);

            User user = ((User)Session["CurrentUser"]);

            var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                   currentEployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                   TransmissionStatusEnum.CASSGenerated,
                   user.User_UserName
               );

            newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);

        }
        
        if (currentEmployerTaxYearTransmissionStatusId == TransmissionStatusEnum.CASSRecieved)
        {
            var folder_path = string.Format(@"{0}ftps\Print", Server.MapPath("~/"));
            
            string csv = CASSPrintFileGenerator.GeneratePrintCSVContent(false,form, tempList, this.Log);

            lblMsg.Text = CASSPrintFileGenerator.WriteCSVContentToFile(folder_path, file_name, csv, EmployerId);

            User user = ((User)Session["CurrentUser"]);

            var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                   currentEployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                   TransmissionStatusEnum.Print,
                   user.User_UserName
               );

            newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);

        }


    }

}