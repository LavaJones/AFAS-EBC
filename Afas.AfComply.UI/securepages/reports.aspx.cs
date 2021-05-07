using Afas.AfComply.Domain;
using Afas.AfComply.Domain.POCO;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class securepages_reports : Afas.AfComply.UI.securepages.SecurePageBase
{

    protected override void PageLoadLoggedIn(User user, employer currDist)
    {

        this.HfUserName.Value = user.User_UserName;

        this.HfDistrictID.Value = user.User_District_ID.ToString();

        this.loadReports();
        this.loadPlanYears(currDist.EMPLOYER_ID);

        if (this.Session["SelectedPlanYear"] != null)
        {

            string planYearID = this.Session["SelectedPlanYear"].ToString();
            errorChecking.setDropDownList(this.DdlPlanYear, int.Parse(planYearID));

        }

        if (this.Request.QueryString["code"] != "" && this.Request.QueryString["code"] != null)
        {

            int code = System.Convert.ToInt16(this.Request.QueryString["code"]);
            this.GvReports.SelectedIndex = code;
            this.loadReportData();

        }
        else
        {

            this.GvReports.SelectedIndex = -1;
            this.PnlOngoing.Visible = false;
            this.PnlPayorPlay.Visible = false;

        }

    }


    private void loadPlanYears(int _employerID)
    {

        this.DdlPlanYear.DataSource = PlanYear_Controller.getEmployerPlanYear(_employerID);
        this.DdlPlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
        this.DdlPlanYear.DataValueField = "PLAN_YEAR_ID";
        this.DdlPlanYear.DataBind();

    }

    private void loadReports()
    {

        demo d = new demo();
        this.GvReports.DataSource = d.getReports().Distinct().ToList();
        this.GvReports.DataBind();

    }

    protected void GvReports_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (this.DdlPlanYear != null && this.DdlPlanYear.SelectedItem != null && this.DdlPlanYear.SelectedItem.Value != null)
        {

            this.Session["SelectedPlanYear"] = this.DdlPlanYear.SelectedItem.Value.ToString();

            this.loadReportData();

        }

    }

    private void loadReportData()
    {

        User currUser = (User)this.Session["CurrentUser"];
        int _employerID = currUser.User_District_ID;

        try
        {

            HiddenField hfID = (HiddenField)this.GvReports.SelectedRow.FindControl("HfReportID");
            int selValue = System.Convert.ToInt32(hfID.Value);
            switch (selValue)
            {
                case 0:
                    this.ShowGraph(true, false);
                    return;
                case 1:
                    this.ShowGraph(false, false);
                    return;
                case 2:
                    this.ShowGraph(true, true);
                    return;
                case 3:
                    this.ShowGraph(false, true);
                    return;
                default:
                    break;
            }

            try
            {
                bool? newHire = (bool?)this.Session["IsNewHireReport"];
                bool? trending = (bool?)this.Session["IsTrendingReport"];
                if (newHire != null && trending != null)
                {
                    this.ShowGraph((bool)newHire, (bool)trending);
                    return;
                }
            }
            catch (Exception anotherException)
            {
                this.Log.Warn("Suppressing errors.", anotherException);
            }

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);
        }

    }

    private void ShowGraph(bool newHire, bool trending)
    {

        this.Session["IsTrendingReport"] = trending;
        this.Session["IsNewHireReport"] = newHire;

        this.PnlOngoing.Visible = true;
        this.PnlPayorPlay.Visible = false;

        DataTable dtGraph = this.showTrendReport(newHire, trending);
        int max = this.getMax(dtGraph);
        this.Chart.DataSource = dtGraph;
        this.Chart.ChartAreas[0].AxisY.Maximum = max;
        this.Chart.ChartAreas[0].AxisY.Interval = max / 10;
        this.Chart.ChartAreas[0].AxisY.Minimum = 0;
        this.Chart.Series[0].YValueMembers = "Count";
        this.Chart.Series[0].XValueMember = "Type";
        this.Chart.DataBind();

        int i = 0;
        foreach (System.Web.UI.DataVisualization.Charting.DataPoint item in this.Chart.Series[0].Points)
        {
            if (i == 0)
            {
                item.Color = System.Drawing.Color.Green;
                item.Url = "r_details.aspx?rpt=0";
                item.ToolTip = "Click to view all Employees who are currently averaging 130 or more hours per month.";
            }
            else if (i == 1)
            {
                item.Color = System.Drawing.Color.Yellow;
                item.Url = "r_details.aspx?rpt=1";
                item.ToolTip = "Click to view all Employees who are currently averaging 100 to 129.99 hours per month.";
            }
            else
            {
                item.Color = System.Drawing.Color.Red;
                item.Url = "r_details.aspx?rpt=2";
                item.ToolTip = "Click to view all Employees who are currently averaging less than 100 hours per month.";
            }

            i += 1;
        }
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Logout.aspx", false);
    }

    private int getMax(System.Data.DataTable _dt)
    {
        int maxValue = 0;

        if (_dt != null)
        {
            foreach (DataRow dr in _dt.Rows)
            {
                int tempValue = int.Parse(dr[0].ToString());
                if (tempValue > maxValue)
                {
                    maxValue = tempValue;
                }
            }
        }

        if (maxValue == 0)
        {
            maxValue = 100;
        }

        return maxValue;
    }

    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {
        if (this.DdlPlanYear == null || this.DdlPlanYear.SelectedItem == null || this.HfDistrictID == null || this.HfDistrictID.Value == null)
        {   
            return;
        }

        int employerID = int.Parse(this.HfDistrictID.Value);
        int planYearID = int.Parse(this.DdlPlanYear.SelectedItem.Value);

        bool trending = (bool)(this.Session["IsTrendingReport"] ?? false);
        bool newHire = (bool)(this.Session["IsNewHireReport"] ?? false);

        employer currDist = (employer)this.Session["CurrentDistrict"];
        User currUser = (User)this.Session["CurrentUser"];

        DataTable export = new DataTable();

        try
        {
            export.Columns.Add("FEIN", typeof(string));
            export.Columns.Add("Employee Name", typeof(string));
            export.Columns.Add("Employee #", typeof(string));
            export.Columns.Add("HR Status", typeof(string));
            export.Columns.Add("ACA Status", typeof(string));
            export.Columns.Add("Term Date", typeof(string));
            export.Columns.Add("Hire Date", typeof(string));

            if (newHire)
            {
                export.Columns.Add("IMP End Date", typeof(string));
            }

            if (trending)
            {
                export.Columns.Add("Trending Monthly", typeof(string));
            }
            else
            {
                export.Columns.Add("Average Monthly", typeof(string));
            }

            export.Columns.Add("Total Hours", typeof(string));

            List<Measurement> measurements = measurementController.manufactureMeasurementList(employerID);
            measurements = measurements.Where(meas => meas.MEASUREMENT_PLAN_ID == planYearID).ToList();

            IEnumerable<AverageHours> averages = new List<AverageHours>();
            foreach (Measurement measurement in measurements)
            {
                averages = averages.Concat(AverageHoursFactory.GetAllAverageHoursForMeasurementId(measurement.MEASUREMENT_ID));
            }
            averages = averages.Where(item => item.IsNewHire == newHire).ToList();

            List<hrStatus> status = hrStatus_Controller.manufactureHRStatusList(currDist.EMPLOYER_ID);
            List<classification_aca> acaStatus = classificationController.getACAstatusList();
            List<Employee> employees = EmployeeController.manufactureEmployeeList(employerID);
            List<Employee> distinctEmployees = employees.Distinct().ToList();
            foreach (AverageHours hour in averages)
            {
                DataRow row = export.NewRow();
                Employee employee = distinctEmployees.Where(emp => emp.EMPLOYEE_ID == hour.EmployeeId).FirstOrDefault();

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
                    row["Trending Monthly"] = string.Format("{0:N2}", Math.Round(hour.TrendingMonthlyAverageHours, 2));
                }
                else
                {
                    row["Average Monthly"] = string.Format("{0:N2}", Math.Round(hour.MonthlyAverageHours, 2));
                }

                row["Total Hours"] = string.Format("{0:N2}", Math.Round(hour.TotalHours, 2));

                export.Rows.Add(row);
            }
        }
        catch (Exception exception)
        {
            this.Log.Warn("Error Durring Export using DataTable.", exception);
        }
        string fileName = "";
        if (newHire)
        {
            fileName += "NewHire_";
        }
        if (trending)
        {
            fileName += "Trend_";
        }

        fileName += "Report_" + DateTime.Now.ToShortDateString();

        string body = "A " + currDist.EMPLOYER_NAME + " employee Hours Report has been downloaded by " + this.HfUserName.Value + " at " + DateTime.Now.ToString();
        PIILogger.LogPII(string.Format("Employee Class Export Download: {0} -- Row Count:[{1}], IP:[{2}], User Id:[{3}]", body, export.Rows.Count, this.Request.UserHostAddress, currUser.User_ID));

        fileName = fileName.Replace('/', '_');

        string attachment = "attachment; filename=" + fileName.CleanFileName() + ".csv";
        this.Response.ClearContent();
        this.Response.BufferOutput = false;
        this.Response.AddHeader("content-disposition", attachment);
        this.Response.ContentType = "application/vnd.ms-excel";

        this.Response.Write(export.GetAsCsv());

        this.Response.Flush();         
        this.Response.SuppressContent = true;                
        HttpContext.Current.ApplicationInstance.CompleteRequest();                      
        this.Response.End();

    }

    private DataTable showTrendReport(bool newHire, bool trending)
    {

        DataTable dataTable = new DataTable();
        dataTable.Clear();
        dataTable.Columns.Add("Count");
        dataTable.Columns.Add("Type");

        if (this.DdlPlanYear == null || this.DdlPlanYear.SelectedItem == null || this.HfDistrictID == null || this.HfDistrictID.Value == null)
        {   
            return dataTable;
        }

        int employerID = int.Parse(this.HfDistrictID.Value);
        int planYearID = int.Parse(this.DdlPlanYear.SelectedItem.Value);

        int onTrack = 0;
        int caution = 0;
        int notOnTrack = 0;

        List<Measurement> measurements = measurementController.manufactureMeasurementList(employerID);

        if (this.Log.IsInfoEnabled)
        {
            this.Log.Info(string.Format("Found {0} measurements for employer {1}.", measurements.Count, employerID));
        }

        List<Measurement> currentPlanYearMeasurements = measurements.Where(meas => meas.MEASUREMENT_PLAN_ID == planYearID).ToList();

        if (this.Log.IsInfoEnabled)
        {
            this.Log.Info(string.Format("Found {0} measurements for current planyear {1} for employer {2}.", currentPlanYearMeasurements.Count, planYearID, employerID));
        }

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

        if (this.Log.IsInfoEnabled)
        {
            this.Log.Info(string.Format("Filtered currentHourSet to {0} from {1} based on newHire = {2}.", currentHourSet.Count(), average.Count(), newHire));
        }

        if (trending)
        {

            this.TrendingDisclaimer.Visible = true;

            onTrack = currentHourSet.FilterForOntrackTrending().Count();
            caution = currentHourSet.FilterForCautionTrending().Count();
            notOnTrack = currentHourSet.Count() - onTrack - caution;

        }
        else
        {

            this.TrendingDisclaimer.Visible = false;

            onTrack = currentHourSet.FilterForOntrackMonthly().Count();
            caution = currentHourSet.FilterForCautionMonthly().Count();
            notOnTrack = currentHourSet.Count() - onTrack - caution;

        }

        this.TxtOngoingOnTrack.Text = onTrack.ToString();
        this.TxtOngoingNotOnTrack.Text = notOnTrack.ToString();
        this.TxtOngoingCaution.Text = caution.ToString();

        DataRow row1 = dataTable.NewRow();
        row1["Count"] = onTrack.ToString();
        row1["Type"] = "On-Track";
        dataTable.Rows.Add(row1);

        DataRow row2 = dataTable.NewRow();
        row2["Count"] = caution.ToString();
        row2["Type"] = "Caution";
        dataTable.Rows.Add(row2);

        DataRow row3 = dataTable.NewRow();
        row3["Count"] = notOnTrack.ToString();
        row3["Type"] = "Not On-Track";
        dataTable.Rows.Add(row3);

        return dataTable;

    }

    protected void DdlPlanYear_SelectedIndexChanged(object sender, EventArgs eventArgs)
    {

        this.Session["SelectedPlanYear"] = this.DdlPlanYear.SelectedItem.Value.ToString();

        this.loadReportData();

    }

    private ILog Log = LogManager.GetLogger(typeof(securepages_reports));

}