using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class demo
{
    public demo()
    {

    }

    public List<report> getReports()
    {

        List<report> tempList = new List<report>();

        report rpt1 = new report("New Hire Average Hours", 0);
        report rpt2 = new report("Ongoing Average Hours", 1);
        report rpt3 = new report("Trending New Hire Average Hours", 2);
        report rpt4 = new report("Trending Ongoing Average Hours", 3);

        tempList.Add(rpt1);
        tempList.Add(rpt2);
        tempList.Add(rpt3);
        tempList.Add(rpt4);

        return tempList;

    }

    public List<setup> getSetupLinks()
    {

        List<setup> tempList = new List<setup>();

        setup s1 = new setup("Initial Measurement Period", 1);
        setup s2 = new setup("Ongoing Measurement Period", 2);
        setup s4 = new setup("Define Equivalencies", 4);
        setup s5 = new setup("Users/Billing", 5);
        setup s6 = new setup("Profile", 6);
        setup s7 = new setup("Stability Period", 7);
        setup s8 = new setup("Medical Plan", 8);
        setup s9 = new setup("Alerts", 9);
        setup s10 = new setup("Pay Description Filter", 10);
        setup s11 = new setup("Employee Classification", 11);
        setup s12 = new setup("Medical Plan Contributions", 12);

        tempList.Add(s1);
        tempList.Add(s2);
        tempList.Add(s4);
        tempList.Add(s5);
        tempList.Add(s6);
        tempList.Add(s7);
        tempList.Add(s8);
        tempList.Add(s12);
        tempList.Add(s9);
        tempList.Add(s10);
        tempList.Add(s11);

        return tempList;

    }

    public static string getAdminLinks()
    {

    
        String adminLinks = null;

        adminLinks = "<ul>";
        adminLinks += "<li><a href='default.aspx'>Home</a></li>";
        adminLinks += "<li><a href='#'>Views</a>";
        adminLinks += "<ul>";
        adminLinks += "<li><a href='view_employers.aspx'>Employer Status</a></li>";
        adminLinks += "<li><a href='view_employers_meas_status.aspx'>Employer Measurement Period Status</a></li>";
        adminLinks += "<li><a href='view_payroll_import.aspx'>Payroll Import Alerts</a></li>";
        adminLinks += "<li><a href='view_employer_employees.aspx'>Employee Import Alerts</a></li>";
        adminLinks += "<li><a href='export_employers.aspx'>Employer Export</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/TrendingDataExport.aspx'>Trending Data Export</a></li> ";
        adminLinks += "<li><a href='employers_certified.aspx'>Employers Certified</a></li>";
        adminLinks += "<li><a href='Employee_Insurance_Offer.aspx'>Employee Insurance Offer</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/EmployersCurrentTransmissionTaxYearStatus.aspx'>Employers Current Transmission Tax Year Status</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/EmployerTransmissionTaxYearStatuses.aspx'>Employer Transmission Tax Year Statuses</a></li>";
        adminLinks += "<li><a href='/admin/ReportPortalStatus.aspx'>Report Portal Status</a></li>";
        adminLinks += "</ul>";
        adminLinks += "</li>";
        adminLinks += "<li><a href='view_payroll_import.aspx'>Import</a>";
        adminLinks += "<ul>";

        if (Feature.BulkConverterEnabled)
        {
            adminLinks += "<li><a href='/admin/AdminPortal/ImportConverter.aspx'>Bulk Data Convert</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/LegacyConverter.aspx'>Legacy Format Converter</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/OfferMissingEmployees.aspx'>Offer File Missing Employees</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/ConfirmNewHires.aspx'>Confirm New Hires</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/ClearOfferAlerts.aspx'>Clear Offer Alerts</a></li>";
            adminLinks += "<li><a href='/admin/import_offer.aspx'>Import Offer File</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/RunCalculations.aspx'>Run Nightly Calculations</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/InactiveEmployeeAveHours.aspx'>Inactive Employee Average Hours</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/employee_classification_insurance.aspx'>Employee Classification Insurance</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/DOB_import.aspx'>Import DOB</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/CarrierAlertExport.aspx'>Export Carrier Alerts</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/DownloadEmployerFile.aspx'>Download Employer File</a></li>";
        
        }

        adminLinks += "<li><a href='import_payroll.aspx'>Payroll</a></li>";
        adminLinks += "<li><a href='import_payroll_batch_modification.aspx'>Payroll Batch Mod</a></li>";
        adminLinks += "<li><a href='import_employee.aspx'>Employee</a></li>";
        adminLinks += "<li><a href='import_employee_che.aspx'>Employee Che format</a></li>";
        adminLinks += "<li><a href='import_grosspay.aspx'>Pay Code</a></li>";
        adminLinks += "<li><a href='import_hrstatus.aspx'>HR Status</a></li>";
        adminLinks += "<li><a href='import_employee_class.aspx'>Employee Class</a></li>";
        adminLinks += "<li><a href='batch_management.aspx'>Batch Management</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/batch_management_insurance_carrier_imports.aspx'>Batch Management Insurance Carrier</a></li>";
        adminLinks += "<li><a href='import_insurance.aspx'>Insurance Offer</a></li>";
        adminLinks += "<li><a href='import_insurance_change_event.aspx'>Insurance Offer Change Event</a></li>";
        adminLinks += "<li><a href='import_insurance_discrepancy.aspx'>Insurance Offer Discrepancies</a></li>";
        adminLinks += "<li><a href='import_insurance_carrier.aspx'>Insurance Carrier Report</a></li>";
        adminLinks += "</ul>";
        adminLinks += "</li>";

        if (Feature.BulkConverterEnabled)
        {
            adminLinks += "<li><a href='#'>Setup</a>";
            adminLinks += "<ul>";
            adminLinks += "<li><a href='/admin/AdminPortal/NewEmployer.aspx'>Add New Employer</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/EmployeeType.aspx' runat='server'>Employee Types</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/EmployeeClassifications.aspx' runat='server'>Classifications</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/EmployerPlanYear.aspx' runat='server'>Plan Years</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/EmployerMeasurementPeriods.aspx' runat='server'>Measurement Periods</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/PlanYearGroup.aspx' runat='server'>Plan Year Groups</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/ClonePlanYear.aspx'>Clone A PlanYear</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/EmployerImp.aspx' runat='server'>Initial Measurement</a></li>";
            adminLinks += "<li><a href='/admin/AdminPortal/EditEmployees.aspx' runat='server'>Edit Employees</a></li>";
            adminLinks += "</ul>";
            adminLinks += "</li>";
        }

        adminLinks += "<li><a href='#'>Plan Year</a>";
        adminLinks += "<ul>";
        adminLinks += "<li><a href='admin_py_rollover_prep.aspx'>Measurement Period Prep</a></li>";
        adminLinks += "<li><a href='admin_py_rollover_multi_prep.aspx'>Dual Measurement Period Prep</a></li>";
        adminLinks += "<li><a href='admin_py_rollover.aspx'>Plan Year</a></li>";
        adminLinks += "<li><a href='AdminAuto_py_rollover.aspx'>Auto Rollover from Admin to Stability</a></li>";
        adminLinks += "</ul>";
        adminLinks += "</li>";
        adminLinks += "<li><a href='#'>IRS</a>";
        adminLinks += "<ul>";
        adminLinks += "<li><a href='/admin/AdminPortal/ToggleIRS.aspx'>Enable IRS Reporting Menu for a Client</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/IRSStaging1095.aspx'>Transfer 1095 Information</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/DisableReview.aspx'>Disable IRS Review</a></li>";
        adminLinks += "<li><a href='/admin/employers_certified.aspx'>Company Approval to proceed to CASS/Print</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/SkipCASS.aspx'>Authorize skipping the CASS Process and Go to Print</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/IRS1095PDFStaging.aspx'>Import Printed 1095's</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/EmployersCurrentPdfStatus.aspx'>Employer Printed PDF Count</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/EmployersCurrentNotPrintedPdf.aspx'>Employer Unprinted PDF Count</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/EmployersCurrentNotTransmit.aspx'>Employer Untransmitted Count</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/EmployersCurrentTransmissionTaxYearStatus.aspx'>Current Transmission Status For All Employers</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/EmployersCurrentIRSTaxYearStatus.aspx'>Employers Current IRS Tax Year Status</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/EmployerTransmissionTaxYearStatuses.aspx'>Detailed History of an Employer's Transmission Status</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/EmployerIRSTransmission.aspx'>Transmit 1094/95C Forms to IRS</a></li>";
        adminLinks += "<li><a href='/admin/AdminPortal/StageForCorrectionRetransmission.aspx'>Stage For Correction Retransmission</a></li>";
        adminLinks += "<li><a href='/admin/admin_10941095_receipt_import.aspx'>Record 1094/95C Transmission Receipt ID</a></li>";
        adminLinks += "<li><a href='/admin/admin_10941095_ack_import.aspx'>Record 1094/95C Transmission Status</a></li>";
        adminLinks += "</ul>";
        adminLinks += "</li>";

        adminLinks += "<li><a href='#'>Verification</a>";
        adminLinks += "<ul>";
        adminLinks += "<li><a href='payroll_duplicate_checker.aspx'>Duplicate Payroll</a></li>";
        adminLinks += "<li><a href='insurance_alert_generator.aspx'>Generate Missing Insurance Alerts</a>";
        adminLinks += "<li><a href='AdminPortal/mergeDuplicateEmployee.aspx'>Merge Duplicate Employees</a>";
        adminLinks += "</ul>";
        adminLinks += "</li>";
        adminLinks += "<li><a href='#'>Formatting</a>";
        adminLinks += "<ul>";
        adminLinks += "<li><a href='formatting_adp_payroll.aspx'>ADP Payroll</a></li>";
        adminLinks += "<li><a href='formatting_adp_demographic.aspx'>ADP Dem</a></li>";
        adminLinks += "<li><a href='formatting_skyward_demographic.aspx'>Sky Dem</a></li>";
        adminLinks += "<li><a href='formatting_skyward_payroll.aspx'>Sky Payroll</a></li>";
        adminLinks += "<li><a href='formatting_smartHR_payroll.aspx'>SHR Payroll</a></li>";
        adminLinks += "<li><a href='formatting_smartHR_demographic.aspx'>SHR Dem</a></li>";
        adminLinks += "</ul>";
        adminLinks += "</li>";
        adminLinks += "</ul>";

        return adminLinks;

    }


    public static string getLeftLinks(bool IrsEnabled)
    {

        String links = "";
        links += "<div class='v_menu'>";
        
       
        
        links += "<a href='transfer.aspx'>File Import</a>";
        
     
        
        links += "<a href='view_payroll_import.aspx'>Payroll Alert Export</a>";
        links += "<a href='view_employee_import.aspx'>Employee Alert Export</a>";
       
      
        
        links += "<a href='export_employees.aspx'>Employee Status/Class</a>";
        links += "<a href='export_payroll_batch.aspx' onclick=''>Payroll Batches</a>";
        links += "<a href='mass_insurance_offering.aspx'>Group Medical Coverage Assignment</a>";
        
        

        if (IrsEnabled)
        {   

           
            links += "<a href='/Reporting/Verification'>Status & Action Portal</a>";
            
        
        }

        links += "</div>";

        return links;

    }

}