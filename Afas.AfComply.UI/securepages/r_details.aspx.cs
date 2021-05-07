using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

using log4net;

using Afas.AfComply.Domain;
using Afas.AfComply.Domain.POCO;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.Domain;

public partial class securepages_r_details : Afas.AfComply.UI.securepages.SecurePageBase
{

    protected override void PageLoadLoggedIn(User user, employer employer)
    {

        HfUserName.Value = user.User_UserName;
       HfDistrictID.Value = user.User_District_ID.ToString();

        try
        {

            List<Employee> employees = EmployeeController.manufactureEmployeeList(user.User_District_ID);

            DataTable data = new DataTable();

            int code = System.Convert.ToInt16(Request.QueryString["rpt"]);

            GvEmployeeList.DataSource = GetReportData();
            GvEmployeeList.DataBind();

        }
        catch (Exception exception)
        {
            this.Log.Warn("Suppressing errors.", exception);
        }

    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {

        employer currDist = (employer)Session["CurrentDistrict"];
        User currUser = (User)Session["CurrentUser"];
        bool newHire = (bool)(Session["IsNewHireReport"] ?? false);
        bool trending = (bool)(Session["IsTrendingReport"] ?? false);

        DataTable export = GetReportData();

        string fileName = "";
        if (newHire)
        {
            fileName += "NewHire_";
        }

        if (trending)
        {
            fileName += "Trend_";
        }

        int code = System.Convert.ToInt16(Request.QueryString["rpt"]);

        if (code == 0)
        {
            fileName += "OnTrack_";
        }
        else if (code == 1)
        {
            fileName += "Caution_";
        }
        else if (code == 2)
        {
            fileName += "NotOnTrack_";
        }
        else
        {
            this.Log.Warn("Reporting got called with invalid parameter.");
        }

        fileName += "Report_" + DateTime.Now.ToShortDateString();

        fileName = fileName.Replace('/', '_');

        String body = "A " + currDist.EMPLOYER_NAME + " employee Hours Report has been downloaded by " + HfUserName.Value + " at " + DateTime.Now.ToString();

        PIILogger.LogPII(String.Format("Employee Hours Export Download: {0} -- Row Count:[{1}], IP:[{2}], User Id:[{3}]", body, export.Rows.Count, Request.UserHostAddress, currUser.User_ID));

        string attachment = "attachment; filename=" + fileName.CleanFileName() + ".csv";
        Response.ClearContent();
        Response.BufferOutput = false;
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/vnd.ms-excel";

        Response.Write(export.GetAsCsv());

        Response.Flush();         
        Response.SuppressContent = true;                
        HttpContext.Current.ApplicationInstance.CompleteRequest();                      
        Response.End();

    }

    protected void GvEmployeeList_OnSorting(object sender, GridViewSortEventArgs e)
    {

        DataTable dataTable = GetReportData();

        if (dataTable != null)
        {

            DataView dataView = dataTable.DefaultView;

            if (e.SortDirection.ToString() == "Ascending")
            {
                dataView.Sort = e.SortExpression + " ASC";
            }
            else if (e.SortDirection.ToString() == "Descending")
            {
                dataView.Sort = e.SortExpression + " DESC";
            }
            else
            {
                dataView.Sort = e.SortExpression + " " + e.SortDirection;
            }

            GvEmployeeList.DataSource = dataTable;
            GvEmployeeList.DataBind();

        }

    }

    private DataTable GetReportData()
    {

        employer currDist = (employer)Session["CurrentDistrict"];
        Boolean trending = (Boolean)(Session["IsTrendingReport"] ?? false);
        Boolean newHire = (Boolean)(Session["IsNewHireReport"] ?? false);
        DataTable export = new DataTable();

        export.Columns.Add("FEIN", typeof(String));
        export.Columns.Add("Employee Name", typeof(String));
        export.Columns.Add("Employee #", typeof(String));
        export.Columns.Add("HR Status", typeof(String));
        export.Columns.Add("ACA Status", typeof(String));
        export.Columns.Add("Term Date", typeof(String));
        export.Columns.Add("Hire Date", typeof(String));

        if (newHire)
        {
            export.Columns.Add("IMP End Date", typeof(string));
        }

        if (trending)
        {
            export.Columns.Add("Trending Monthly", typeof(String));
        }
        else
        {
            export.Columns.Add("Average Monthly", typeof(String));
        }

        export.Columns.Add("Total Hours", typeof(String));

        List<AverageHours> averages = getAverages();

        List<hrStatus> status = hrStatus_Controller.manufactureHRStatusList(currDist.EMPLOYER_ID);
        List<classification_aca> acaStatus = classificationController.getACAstatusList();
        List<Employee> employees = EmployeeController.manufactureEmployeeList(currDist.EMPLOYER_ID);

        foreach (AverageHours hour in averages)
        {

            DataRow row = export.NewRow();

            Employee employee = employees.Where(emp => emp.EMPLOYEE_ID == hour.EmployeeId).FirstOrDefault();
            if (employee == null)
            {

                this.Log.Warn("Found average hours but did not find employee. Employee Id: " + hour.EmployeeId);

                continue;

            }

            row["FEIN"] = currDist.EMPLOYER_EIN;
            row["Employee Name"] = employee.EMPLOYEE_FULL_NAME;
            row["Employee #"] = employee.EMPLOYEE_EXT_ID;

            row["HR Status"] = (from hrStatus stat in status
                                where stat.HR_STATUS_ID == employee.EMPLOYEE_HR_STATUS_ID
                                select stat.HR_STATUS_NAME).FirstOrDefault();

            row["ACA Status"] = (from classification_aca aca in acaStatus
                                 where aca.ACA_STATUS_ID == employee.EMPLOYEE_ACT_STATUS_ID
                                 select aca.ACA_STATUS_NAME).FirstOrDefault();

            row["Hire Date"] = ((DateTime)employee.EMPLOYEE_HIRE_DATE).ToShortDateString();

            if (null != employee.EMPLOYEE_TERM_DATE)
            {
                row["Term Date"] = ((DateTime)employee.EMPLOYEE_TERM_DATE).ToShortDateString();
            }

            if (newHire)
            {
                row["IMP End Date"] = ((DateTime)employee.EMPLOYEE_IMP_END).ToShortDateString();
            }

            if (trending)
            {
                row["Trending Monthly"] = String.Format("{0:N2}", Math.Round(hour.TrendingMonthlyAverageHours, 2));
            }
            else
            {
                row["Average Monthly"] = String.Format("{0:N2}", Math.Round(hour.MonthlyAverageHours, 2));
            }

            row["Total Hours"] = String.Format("{0:N2}", Math.Round(hour.TotalHours, 2));

            export.Rows.Add(row);

        }

        export.DefaultView.Sort = "Employee Name";

        return export;

    }

    private List<AverageHours> getAverages()
    {

        Boolean trending = (Boolean)(Session["IsTrendingReport"] ?? false);
        Boolean newHire = (Boolean)(Session["IsNewHireReport"] ?? false);

        employer currDist = (employer)Session["CurrentDistrict"];
        int planYearID = 0;
        if (false == int.TryParse(Session["SelectedPlanYear"].ToString(), out planYearID))
        {
            return new List<AverageHours>();
        }

        List<Measurement> measurements = measurementController.manufactureMeasurementList(currDist.EMPLOYER_ID);
        if (this.Log.IsInfoEnabled) this.Log.Info(String.Format("Found {0} measurements for employer {1}.", measurements.Count, currDist.EMPLOYER_ID));

        List<Measurement> currentPlanYearMeasurements = measurements.Where(meas => meas.MEASUREMENT_PLAN_ID == planYearID).ToList();
        if (this.Log.IsInfoEnabled) this.Log.Info(String.Format("Found {0} measurements for current planyear {1} for employer {2}.", currentPlanYearMeasurements.Count, planYearID, currDist.EMPLOYER_ID));

        IEnumerable<AverageHours> average = new List<AverageHours>();
        foreach (Measurement measurement in currentPlanYearMeasurements)
        {
            average = average.Concat(AverageHoursFactory.GetAllAverageHoursForMeasurementId(measurement.MEASUREMENT_ID));
        }

        IList<AverageHours> currentHourSet;
        if (newHire)
        {
            currentHourSet = average.ToList().FilterForAnIntialMeasurementPeriod().ToList();
        }
        else
        {
            currentHourSet = average.ToList().FilterForAnOngoingMeasurementPeriod().ToList();
        }
        if (this.Log.IsInfoEnabled) this.Log.Info(String.Format("Filtered currentHourSet to {0} from {1} based on newHire = {2}.", currentHourSet.Count(), average.Count(), newHire));

        List<AverageHours> averages = new List<AverageHours>();

        int code = System.Convert.ToInt16(Request.QueryString["rpt"]);
        if (trending)
        {

            TrendingDisclaimer.Visible = true;

            if (code == 0)
            {
                averages = currentHourSet.FilterForOntrackTrending().ToList();
            }
            else if (code == 1)
            {
                averages = currentHourSet.FilterForCautionTrending().ToList();
            }
            else if (code == 2)
            {
                averages = currentHourSet.FilterForNotOntrackTrending().ToList();
            }
            else
            {
                this.Log.Warn("Reporting got called with invalid parameter.");
            }

        }
        else
        {

            TrendingDisclaimer.Visible = false;

            if (code == 0)
            {
                averages = currentHourSet.FilterForOntrackMonthly().ToList();
            }
            else if (code == 1)
            {
                averages = currentHourSet.FilterForCautionMonthly().ToList();
            }
            else if (code == 2)
            {
                averages = currentHourSet.FilterForNotOntrackMonthly().ToList();
            }
            else
            {
                this.Log.Warn("Reporting got called with invalid parameter.");
            }

        }

        return averages;

    }

    protected void GvEmployeeList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        GvEmployeeList.PageIndex = e.NewPageIndex;
        GvEmployeeList.DataSource = GetReportData();
        GvEmployeeList.DataBind();

    }

    protected void LbBack_Click(object sender, EventArgs e)
    {

        Boolean trending = (Boolean)(Session["IsTrendingReport"] ?? false);
        Boolean newHire = (Boolean)(Session["IsNewHireReport"] ?? false);

        String url = "r_reporting.aspx";

        if (newHire)
        {

            if (trending)
            {
                url += "?code=2";
            }
            else
            {
                url += "?code=0";
            }

        }
        else
        {

            if (trending)
            {
                url += "?code=3";
            }
            else
            {
                url += "?code=1";
            }

        }

        Response.Redirect(url, false);

    }

    private ILog Log = LogManager.GetLogger(typeof(securepages_r_details));

}