using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Afas.AfComply.Domain.POCO;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.AfComply.Domain;
using Afas.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class TrendingDataExport : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(DownloadEmployerFile));
        private IList<employer> Employers { get; set; }


        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the EditEmployer page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=44", false);
            }
            else
            {
                LoadEmployers();
            }
        }


        private void LoadEmployers()
        {
            Employers = employerController.getAllEmployers();
            DdlEmployer.DataSource = Employers;
            DdlEmployer.DataTextField = "EMPLOYER_NAME";
            DdlEmployer.DataValueField = "EMPLOYER_ID";
            DdlEmployer.Items.Add("ALL Employers");
            DdlEmployer.DataBind();
            DdlEmployer.Items.Add("ALL Employers");
            DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;

        }

        protected void BtnExportToCSV_Click(object sender, EventArgs e)
        {
            DataTable Allexport = CreateExportDataTable();
            Employers = employerController.getAllEmployers();
            if (DdlEmployer.SelectedItem.Text == "ALL Employers")
            {
                foreach (employer empr in Employers)
                {
                    Allexport.Merge(GetEmployersData(empr));

                }
            }
            else
            {
                int employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                employer empr = employerController.getEmployer(employerID);
                Allexport = GetEmployersData(empr);
            }

            DownloadEmployerData(Allexport);
        }

        private void DownloadEmployerData(DataTable allexport)
        {
            string fileName = DdlEmployer.SelectedItem.Text + "Report_" + DateTime.Now.ToShortDateString();
            string body = DdlEmployer.SelectedItem.Text + " employee Hours Report has been downloaded by " + ((Literal)Master.FindControl("LitUserName")).Text + " at " + DateTime.Now.ToString();
            PIILogger.LogPII(String.Format("Employee Class Export Download: {0} -- Row Count:[{1}], IP:[{2}], User Id:[{3}]", body, allexport.Rows.Count, Request.UserHostAddress, ((Literal)Master.FindControl("LitUserName")).ID));
            fileName = fileName.Replace('/', '_');
            // Convert to a CSV string and download that as the file
            // Next 4 lines of Code from internet : http://stackoverflow.com/questions/1746701/export-datatable-to-excel-file
            string attachment = "attachment; filename=" + fileName.CleanFileName() + ".csv";
            Response.ClearContent();
            Response.BufferOutput = false;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(allexport.GetAsCsv());
            // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
            Response.Flush(); // Sends all currently buffered output to the client.
            Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            Response.End();
        }

        private DataTable GetEmployersData(employer empr)
        {
            DataTable EmployerData = CreateExportDataTable();

            List<PlanYear> planYears = PlanYear_Controller.getEmployerPlanYear(empr.EMPLOYER_ID);
            foreach (PlanYear PL in planYears)
            {
                EmployerData.Merge(GetEmployersDataForPlanYear(PL, empr));
            }
            return EmployerData;
        }

        private DataTable GetEmployersDataForPlanYear(PlanYear PY, employer empr)
        {
            DataTable EmployersDataForPlanYear = CreateExportDataTable();
            try
            {
                List<hrStatus> status = hrStatus_Controller.manufactureHRStatusList(empr.EMPLOYER_ID);
                List<classification_aca> acaStatus = classificationController.getACAstatusList();
                List<Employee> employees = EmployeeController.manufactureEmployeeList(empr.EMPLOYER_ID);
                List<Employee> distinctEmployees = employees.Distinct().ToList();
                List<Measurement> measurements = measurementController.manufactureMeasurementList(empr.EMPLOYER_ID);
                measurements = measurements.Where(meas => meas.MEASUREMENT_PLAN_ID == PY.PLAN_YEAR_ID).ToList();
                IEnumerable<AverageHours> averages = GetAverages(measurements);
                foreach (AverageHours hour in averages)
                {
                    Employee employee = distinctEmployees.Where(emp => emp.EMPLOYEE_ID == hour.EmployeeId).FirstOrDefault();
                    DataTable r = getrecord(empr, hour, employee, status, acaStatus, PY);
                    if (r.Rows.Count > 0)
                    {
                        EmployersDataForPlanYear.Merge(r);
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Warn("Error Durring Export using DataTable.", exception);
            }

            return EmployersDataForPlanYear;
        }

        private DataTable getrecord(employer empr, AverageHours hour, Employee employee, List<hrStatus> status, List<classification_aca> acaStatus, PlanYear PY)
        {
            DataTable EmployersDataForPlanYear = CreateExportDataTable();
            DataRow DataRow = EmployersDataForPlanYear.NewRow();
            try
            {

                if (employee == null)
                {
                    Log.Warn("Found average hours but did not find employee. Employee Id: " + hour.EmployeeId);
                }
                else
                {

                    DataRow["FEIN"] = empr.EMPLOYER_EIN;
                    DataRow["Employer Name"] = empr.EMPLOYER_NAME;

                    DataRow["Employee Name"] = employee.EMPLOYEE_FULL_NAME;
                    DataRow["Employee #"] = employee.EMPLOYEE_EXT_ID;
                    DataRow["IMP End Date"] = ((DateTime)employee.EMPLOYEE_IMP_END).ToShortDateString(); ;


                    DataRow["HR Status"] = (from hrStatus stat in status
                                            where stat.HR_STATUS_ID == employee.EMPLOYEE_HR_STATUS_ID
                                            select stat.HR_STATUS_NAME).FirstOrDefault();

                    DataRow["ACA Status"] = (from classification_aca aca in acaStatus
                                             where aca.ACA_STATUS_ID == employee.EMPLOYEE_ACT_STATUS_ID
                                             select aca.ACA_STATUS_NAME).FirstOrDefault();
                    DataRow["Hire Date"] = ((DateTime)employee.EMPLOYEE_HIRE_DATE).ToShortDateString();

                    if (null != employee.EMPLOYEE_TERM_DATE)
                    {
                        DataRow["Termination Date"] = ((DateTime)employee.EMPLOYEE_TERM_DATE).ToShortDateString();
                    }

                    DataRow["Plan Year"] = PY.PLAN_YEAR_DESCRIPTION;

                    DataRow["Trending Monthly"] = string.Format("{0:N2}", Math.Round(hour.TrendingMonthlyAverageHours, 2));


                    DataRow["Total Hours"] = string.Format("{0:N2}", Math.Round(hour.TotalHours, 2));


                    EmployersDataForPlanYear.Rows.Add(DataRow);

                }
            }
            catch (Exception exception)
            {
                Log.Warn("Error Durring Export using DataTable.", exception);
            }
            return EmployersDataForPlanYear;
        }

        private IEnumerable<AverageHours> GetAverages(List<Measurement> measurements)
        {
            IEnumerable<AverageHours> averages = new List<AverageHours>();
            foreach (Measurement measurement in measurements)
            {
                averages = averages.Concat(AverageHoursFactory.GetAllAverageHoursForMeasurementId(measurement.MEASUREMENT_ID));
            }
            averages = averages.Where(item => item.IsNewHire == true).ToList();
            return averages;

        }

        protected DataTable CreateExportDataTable()
        {
            DataTable export = new DataTable();
            export.Columns.Add("FEIN", typeof(string));
            export.Columns.Add("Employer Name", typeof(string));
            export.Columns.Add("Employee Name", typeof(string));
            export.Columns.Add("Employee #", typeof(string));
            export.Columns.Add("HR Status", typeof(string));
            export.Columns.Add("ACA Status", typeof(string));
            export.Columns.Add("Hire Date", typeof(string));
            export.Columns.Add("Termination Date", typeof(string));
            export.Columns.Add("IMP End Date", typeof(string));
            export.Columns.Add("Plan Year", typeof(string));
            export.Columns.Add("Trending Monthly", typeof(string));
            export.Columns.Add("Total Hours", typeof(string));
            return export;

        }

    }

}