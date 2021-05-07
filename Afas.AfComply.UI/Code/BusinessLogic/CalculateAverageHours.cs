using Afas;
using Afas.AfComply.Domain;
using Afas.AfComply.Domain.POCO;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

/// <summary>
/// This class contains the functionality associated with calculating Average hour
/// </summary>
public class AverageHoursCalculator
{
    /// <summary>
    /// Default logger for this object 
    /// </summary>
    private readonly ILog Log;

    /// <summary>
    /// This is a tool to make writing performance logging easier by managing the Stopwatches and using standard log statements
    /// </summary>
    private readonly PerformanceTiming PerfTimer;

    /// <summary>
    /// Default constructor
    /// </summary>
    public AverageHoursCalculator()
    {
        Log = LogManager.GetLogger(typeof(AverageHoursCalculator));
        PerfTimer = new PerformanceTiming(typeof(AverageHoursCalculator), null, SystemSettings.UsePerformanceLog);
    }

    /// <summary>
    /// Run all the calculations for an employer.
    /// </summary>
    /// <param name="employerId">The Id of the employer to run</param>
    /// <returns>Success if true, false if a failure.</returns>
    public bool CalculateAveragesForEmployer(int employerId)
    {
        PerfTimer.MethodName = "CalculateAveragesForEmployer";
        PerfTimer.StartTimer("DB Data Pull", "CalculateAveragesForEmployer");

        #region pull Data from the DB and check it.

        List<Employee> employeeList = EmployeeController.manufactureEmployeeList(employerId);
        if (employeeList.Count == 0)
        {
            if (this.Log.IsInfoEnabled)
            {
                this.Log.Info("No Employees found, skiping calculation.");
            }

            return false;
        }

        List<BreakInService> Breaks = new BreakInServiceFactory().SelectAllBreaksInServiceForEmployer(employerId);

        List<PlanYear> planYears = PlanYear_Controller.getEmployerPlanYear(employerId);
        if (planYears.Count == 0)
        {
            if (this.Log.IsInfoEnabled)
            {
                this.Log.Info("No Plan Years found, skiping calculation.");
            }

            return false;
        }

        List<Measurement> Measurements = measurementController.manufactureMeasurementList(employerId);
        if (Measurements.Count == 0)
        {
            if (this.Log.IsInfoEnabled)
            {
                this.Log.Info("No MPs found, skiping calculation.");
            }

            return false;
        }

        List<Payroll> PayrollList = Payroll_Controller.getEmployerPayroll(employerId);
        if (PayrollList.Count == 0)
        {
            if (this.Log.IsInfoEnabled)
            {
                this.Log.Info("No Payroll found, skiping calculation.");
            }

            return false;
        }

        DateTime lastPayroll = (DateTime)PayrollList.OrderByDescending(pay => pay.PAY_EDATE).First().PAY_EDATE;

        Dictionary<int, List<Payroll>> PayrollPerEmployee = this.PrefilterPayroll(PayrollList);

        #endregion
        PerfTimer.LogTimeAndDispose("DB Data Pull", string.Empty, true);
        PerfTimer.StartTimer("All Calculations for Employer id: " + employerId, "CalculateAveragesForEmployer");
        PerfTimer.StartTimer("Create DataTables", "CalculateAveragesForEmployer");

        try
        {
            if (this.Log.IsInfoEnabled)
            {
                this.Log.Info(string.Format("Processing [{0}] employees in employer [{1}].", employeeList.Count, employerId));
            }

            DataTable AverageHoursTable = AverageHoursFactory.GetNewAverageHoursDataTable();
            DataTable EmployeeHours = EmployeeFactory.GetNewEmployeeHoursDataTable();

            PerfTimer.LogTimeAndDispose("Create DataTables");
            PerfTimer.StartLapTimer("Calculate All Employees", "CalculateAveragesForEmployer");

            #region Calculate All Employees Hours
            PerfTimer.StartLapTimerPaused("Hours Calculation", "CalculateAveragesForEmployer");
            PerfTimer.StartLapTimerPaused("Update Employee Object", "CalculateAveragesForEmployer");
            PerfTimer.StartLapTimer("Setup for Employee", "CalculateAveragesForEmployer");
                        
            foreach (Employee emp in employeeList)
            {
                
                #region Setup for this employee
                if (this.Log.IsDebugEnabled)
                {

                    this.Log.Debug(string.Format(
                    "Calculating Average for Emp: Id:[{0}], Class Id:[{1}], Type Id:[{2}], HR Status Id:[{3}], with current hours worked:[{4}].",
                    emp.EMPLOYEE_ID,
                    emp.EMPLOYEE_CLASS_ID,
                    emp.EMPLOYEE_TYPE_ID,
                    emp.EMPLOYEE_HR_STATUS_ID,
                    emp.EMPLOYEE_AVG_HOURS_WORKED));

                    this.Log.Debug(string.Format(
                        "employerId is [{0}]. employeeTypeId is [{1}]. _planYearID is [{2}]. _measPlanYearID is [{3}]. _limboPlanYearID is [{4}].",
                        employerId,
                        emp.EMPLOYEE_TYPE_ID,
                        emp.EMPLOYEE_PLAN_YEAR_ID,
                        emp.EMPLOYEE_PLAN_YEAR_ID_MEAS,
                        emp.EMPLOYEE_PLAN_YEAR_ID_LIMBO));
                }

                int PlanYearGroupId = (from plan in planYears where plan.PLAN_YEAR_ID == emp.EMPLOYEE_PLAN_YEAR_ID_MEAS select plan.PlanYearGroupId).First();

                if (false == PayrollPerEmployee.ContainsKey(emp.EMPLOYEE_ID))
                {
                    continue;
                }

                List<Payroll> EmployeePayroll = PayrollPerEmployee[emp.EMPLOYEE_ID];

                List<BreakInService> employeeBreaks = new List<BreakInService>();

                double _nhPlanYearWeeklyAvg = 0;

                #endregion
                
                PerfTimer.LapAndSwitchTimers("Setup for Employee", "Hours Calculation");

                #region Calculations for employee

                PerfTimer.StartLapTimerPaused("NH Calculations for Employee", "CalculateAveragesForEmployer");
                PerfTimer.StartLapTimerPaused("Standard Calculations for Employee", "CalculateAveragesForEmployer");
                PerfTimer.StartLapTimer("PlanYear Setup for Employee", "CalculateAveragesForEmployer");

                foreach (PlanYear year in planYears
                    .Where(plan => plan.PlanYearGroupId == PlanYearGroupId)
                    .OrderBy(plan => plan.PLAN_YEAR_START))
                {

                    #region Setup for this Employee + Planyear Calculation

                    Measurement mp_py = this.GetMeasurementPeriod(Measurements, year.PLAN_YEAR_ID, emp.EMPLOYEE_TYPE_ID);

                    if (mp_py == null)
                    {
                        if (this.Log.IsDebugEnabled)
                        {
                            this.Log.Debug(string.Format(
                            "Found Null [{0}] Measurement using; Plan Year ID: [{1}], EmployeeType Id: [{2}], Employer Id:[{3}].",
                            mp_py,
                            year.PLAN_YEAR_ID,
                            emp.EMPLOYEE_TYPE_ID,
                            employerId));
                        }

                        continue;
                    }

                    if (this.IsTerminated(emp, mp_py) || (emp.EMPLOYEE_HIRE_DATE >= mp_py.MEASUREMENT_END))
                    {

                        if (this.Log.IsDebugEnabled)
                        {
                            this.Log.Debug(string.Format(
                            "Skipping Measurement; IsTerminated: [{0}], Hire Date: [{1}], Measurement End: [{2}].",
                            this.IsTerminated(emp, mp_py),
                            emp.EMPLOYEE_HIRE_DATE,
                            mp_py.MEASUREMENT_END));
                        }

                        continue;
                    }

                    List<BreakInService> filteredBreaks = this.FilterBreaksToOneMp(Breaks, mp_py.MEASUREMENT_ID);

                    #endregion

                    PerfTimer.LapAndSwitchTimers("PlanYear Setup for Employee", "NH Calculations for Employee");


                    #region New Hire Initial Measurement Period Calculations

                    if (this.IsNewHire(emp, mp_py))
                    {
                        employeeBreaks.AddRange(filteredBreaks);

                        DateTime end = (DateTime)emp.EMPLOYEE_IMP_END;
                        if (end > mp_py.MEASUREMENT_END)
                        {
                            end = mp_py.MEASUREMENT_END;
                        }

                        DateTime start = EmployeeController.calculateIMPStartDate((DateTime)emp.EMPLOYEE_HIRE_DATE);

                        double trendTotalHours = 0;
                        DateTime trendEnd = this.GetTrendEndDate(start, end, lastPayroll);
                        double _nhTrendPlanYearWeeklyAvg = this.CalculateMeasurementPeriodWeeklyAverageHours(EmployeePayroll, employeeBreaks.Distinct().ToList(), start, trendEnd, out trendTotalHours, true);
                        double _nhTrendPlanYearMonthlyAvg = this.ConvertWeeksHoursToMonths(_nhTrendPlanYearWeeklyAvg);

                        DataRow rowNh = AverageHoursTable.NewRow();

                        rowNh["EmployeeId"] = emp.EMPLOYEE_ID.checkIntDBNull();
                        rowNh["MeasurementId"] = mp_py.MEASUREMENT_ID;
                        rowNh["WeeklyAverageHours"] = 0;
                        rowNh["MonthlyAverageHours"] = 0;
                        rowNh["TrendingWeeklyAverageHours"] = _nhTrendPlanYearWeeklyAvg.checkDecimalDBNull2();
                        rowNh["TrendingMonthlyAverageHours"] = _nhTrendPlanYearMonthlyAvg.checkDecimalDBNull2();
                        rowNh["TotalHours"] = trendTotalHours;
                        rowNh["IsNewHire"] = true;

                        if (emp.EMPLOYEE_IMP_END > mp_py.MEASUREMENT_END)
                        {
                            AverageHoursTable.Rows.Add(rowNh);

                            continue;
                        }
                        else
                        {
                            double totalNhHours = 0;

                            _nhPlanYearWeeklyAvg = this.CalculateMeasurementPeriodWeeklyAverageHours(EmployeePayroll, employeeBreaks.Distinct().ToList(), start, end, out totalNhHours, true);

                            double _nhPlanYearMonthlyAvg = this.ConvertWeeksHoursToMonths(_nhPlanYearWeeklyAvg);

                            rowNh["WeeklyAverageHours"] = _nhPlanYearWeeklyAvg.checkDecimalDBNull2();
                            rowNh["MonthlyAverageHours"] = _nhPlanYearMonthlyAvg.checkDecimalDBNull2();
                            rowNh["TotalHours"] = totalNhHours;

                            AverageHoursTable.Rows.Add(rowNh);
                        }
                    }
                    #endregion

                    PerfTimer.LapAndSwitchTimers("NH Calculations for Employee", "Standard Calculations for Employee");

                    #region Standard MP Calculations

                    if (this.Log.IsDebugEnabled)
                    {
                        this.Log.Debug(string.Format(
                        "Calculating for measurementPlan ID [{0}], Plan Year [{1}] EmployeeType [{2}]",
                        mp_py.MEASUREMENT_ID,
                        year.PLAN_YEAR_ID,
                        emp.EMPLOYEE_TYPE_ID));
                    }

                    double totalHours = 0;
                    double _planYearWeeklyAvg = 0;
                    double _projectedPlanYearWeeklyAvg = 0;

                    if (false == this.SkipCalculation(emp, mp_py))
                    {

                        DateTime start = mp_py.MEASUREMENT_START;
                        DateTime end = mp_py.MEASUREMENT_END;
                        
                        _planYearWeeklyAvg = this.CalculateMeasurementPeriodWeeklyAverageHours(EmployeePayroll, filteredBreaks, start, end, out totalHours);

                        end = this.GetTrendEndDate(mp_py, lastPayroll);
                        _projectedPlanYearWeeklyAvg = this.CalculateMeasurementPeriodWeeklyAverageHours(EmployeePayroll, filteredBreaks, start, end, out totalHours);
                        
                    }

                    DataRow row = AverageHoursTable.NewRow();

                    row["EmployeeId"] = emp.EMPLOYEE_ID.checkIntDBNull();
                    row["MeasurementId"] = mp_py.MEASUREMENT_ID.checkIntDBNull();
                    row["WeeklyAverageHours"] = _planYearWeeklyAvg.checkDecimalDBNull2();
                    row["MonthlyAverageHours"] = this.ConvertWeeksHoursToMonths(_planYearWeeklyAvg).checkDecimalDBNull2();
                    row["TrendingWeeklyAverageHours"] = _projectedPlanYearWeeklyAvg.checkDecimalDBNull2();
                    row["TrendingMonthlyAverageHours"] = this.ConvertWeeksHoursToMonths(_projectedPlanYearWeeklyAvg).checkDecimalDBNull2();
                    row["TotalHours"] = totalHours;
                    row["IsNewHire"] = false;

                    AverageHoursTable.Rows.Add(row);

                    #endregion

                    PerfTimer.LapAndSwitchTimers("Standard Calculations for Employee", "PlanYear Setup for Employee");

                }

                PerfTimer.LogAllLapsAndDispose("PlanYear Setup for Employee");
                PerfTimer.LogAllLapsAndDispose("NH Calculations for Employee");
                PerfTimer.LogAllLapsAndDispose("Standard Calculations for Employee");

                #endregion Calculations for employee

                PerfTimer.LapAndSwitchTimers("Hours Calculation", "Update Employee Object");

                #region Update the employee object values
                
                double _nhPlanYearAvg = 0;

                bool _planYearNH = this.IsNewHire(emp,
                    this.GetMeasurementPeriod(Measurements, emp.EMPLOYEE_PLAN_YEAR_ID, emp.EMPLOYEE_TYPE_ID));

                bool _measPlanYearNH = this.IsNewHire(emp,
                        this.GetMeasurementPeriod(Measurements, emp.EMPLOYEE_PLAN_YEAR_ID_MEAS, emp.EMPLOYEE_TYPE_ID));

                bool _limboPlanyYearNH = this.IsNewHire(emp,
                        this.GetMeasurementPeriod(Measurements, emp.EMPLOYEE_PLAN_YEAR_ID_LIMBO, emp.EMPLOYEE_TYPE_ID));

                if (_planYearNH == true || _measPlanYearNH == true || _limboPlanyYearNH == true)
                {

                    _nhPlanYearAvg = this.ConvertWeeksHoursToMonths(_nhPlanYearWeeklyAvg);

                }

                double _stabPlanYearAvg = 0;
                double _measPlanYearAvg = 0;
                double _adminPlanYearAvg = 0;
                if (emp.EMPLOYEE_PLAN_YEAR_ID > 0)
                {
                    Measurement meas = this.GetMeasurementPeriod(Measurements, emp.EMPLOYEE_PLAN_YEAR_ID, emp.EMPLOYEE_TYPE_ID);
                    if (null != meas)
                    {
                        _stabPlanYearAvg = this.GetMonthlyForEmployeeMP(AverageHoursTable, emp.EMPLOYEE_ID, meas.MEASUREMENT_ID);
                    }
                }
                if (emp.EMPLOYEE_PLAN_YEAR_ID_MEAS > 0)
                {
                    Measurement meas = this.GetMeasurementPeriod(Measurements, emp.EMPLOYEE_PLAN_YEAR_ID_MEAS, emp.EMPLOYEE_TYPE_ID);
                    if (null != meas)
                    {
                        _measPlanYearAvg = this.GetMonthlyForEmployeeMP(AverageHoursTable, emp.EMPLOYEE_ID, meas.MEASUREMENT_ID);
                    }
                }
                if (emp.EMPLOYEE_PLAN_YEAR_ID_LIMBO > 0)
                {
                    Measurement meas = this.GetMeasurementPeriod(Measurements, emp.EMPLOYEE_PLAN_YEAR_ID_LIMBO, emp.EMPLOYEE_TYPE_ID);
                    if (null != meas)
                    {
                        _adminPlanYearAvg = this.GetMonthlyForEmployeeMP(AverageHoursTable, emp.EMPLOYEE_ID, meas.MEASUREMENT_ID);
                    }
                }

                if (this.Log.IsDebugEnabled)
                {
                    this.Log.Debug(string.Format(
                        "Updating Employee emp.EMPLOYEE_ID:[{0}], _stabPlanYearAvg:[{1}], _limboPlanYearAvg:[{2}], _measPlanYearAvg:[{3}], _nhPlanYearAvg:[{4}].",
                        emp.EMPLOYEE_ID,
                        _stabPlanYearAvg,
                        _adminPlanYearAvg,
                        _measPlanYearAvg,
                        _nhPlanYearAvg));
                }

                DataRow employeeHours = EmployeeHours.NewRow();

                employeeHours["employee_id"] = emp.EMPLOYEE_ID.checkIntDBNull();
                employeeHours["pyAvg"] = _stabPlanYearAvg.checkDecimalDBNull2();
                employeeHours["lpyAvg"] = _adminPlanYearAvg.checkDecimalDBNull2();
                employeeHours["mpyAvg"] = _measPlanYearAvg.checkDecimalDBNull2();
                employeeHours["impAvg"] = _nhPlanYearAvg.checkDecimalDBNull2();

                EmployeeHours.Rows.Add(employeeHours);

                #endregion

                PerfTimer.LapAndSwitchTimers("Update Employee Object", "Setup for Employee");
                PerfTimer.LapAndLog("Calculate All Employees", "For Employee ID: "+ emp.EMPLOYEE_ID, true);                  

            }

            #endregion Calculate All Employees Hours

            PerfTimer.LogAllLapsAndDispose("Calculate All Employees", string.Empty, true);
            PerfTimer.StartTimer("Save To DB", "CalculateAveragesForEmployer");

            if ((AverageHoursTable.Rows.Count > 0 &&
                false == AverageHoursFactory.BulkUpsertAverageHours(AverageHoursTable, "Nightly Calculation", employerId))
                ||
                (EmployeeHours.Rows.Count > 0 &&
                false == EmployeeController.BulkUpdateEmployee(EmployeeHours))
                )
            {

                PerfTimer.LogTimeAndDispose("Save To DB", "Failed Writing data to DB", true);

                this.Log.Warn("Failed to Save Calculated Hours for Employer id: " + employerId);

                throw new ApplicationException("Failed to Save Calculated Hours for Employer id: " + employerId);

            }

            PerfTimer.LogTimeAndDispose("Save To DB", string.Empty, true);

        }
        catch (Exception exception)
        {

            this.Log.Error("Caught Exception during calculation of Employer id: [" + employerId + "].", exception);

            return false;

        }
        finally
        {

            PerfTimer.LogTimeAndDispose("All Calculations for Employer id: " + employerId);

        }

        return true;

    }

    /// <summary>
    /// Checks if the situation should skip the calculation for this employee/mp combo
    /// </summary>
    /// <param name="emp">The employee</param>
    /// <param name="mp_py">The Measuremnt Period</param>
    /// <returns>Returns true if it should be skipped.</returns>
    private bool SkipCalculation(Employee emp, Measurement mp_py)
    {
        if (mp_py != null && false == (emp.EMPLOYEE_HIRE_DATE >= mp_py.MEASUREMENT_START) && false == this.IsTerminated(emp, mp_py))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Gets the correct end dates for use in Trending calculations.
    /// </summary>
    /// <param name="mp_py">THe MP too get the Trending dates for.</param>
    /// <param name="lastPayroll">The date of the last payroll loaded.</param>
    /// <returns>The end date for use with trending.</returns>
    private DateTime GetTrendEndDate(Measurement mp_py, DateTime lastPayroll)
    {
        DateTime start = mp_py.MEASUREMENT_START;
        DateTime end = mp_py.MEASUREMENT_END;
        return this.GetTrendEndDate(start, end, lastPayroll);
    }

    /// <summary>
    /// Chooses what end date to use for the trending report
    /// </summary>
    /// <param name="start">The start of the trend time frame. (limits out of bounds issues)</param>
    /// <param name="end">The current end Date.</param>
    /// <param name="lastPayroll">The date of the last payroll end.</param>
    /// <returns>The end date for use in calculations.</returns>
    private DateTime GetTrendEndDate(DateTime start, DateTime end, DateTime lastPayroll)
    {
        if (end > lastPayroll && lastPayroll > start)
        {
            if (this.Log.IsDebugEnabled)
            {
                this.Log.Debug(string.Format("Trending shortened the Measurement from: [{0}] To: [{1}]", end, lastPayroll));
            }

            end = lastPayroll;
        }
        return end;
    }

    /// <summary>
    /// Calculate the Weekly Average Hours for this MP
    /// </summary>
    /// <param name="EmployeePayrollList">Payroll List, prefiltered to just this employee.</param>
    /// <param name="FilterBreaks">The Breaks in service, pre filtered to just this MP</param>
    /// <param name="start">Start Date to use.</param>
    /// <param name="end">End Date to use.</param>
    /// <param name="totalHours">Output of the total hours calculated.</param>
    /// <returns>The average Hours per week for this employee.</returns>
    private double CalculateMeasurementPeriodWeeklyAverageHours(List<Payroll> EmployeePayrollList, List<BreakInService> FilterBreaks, DateTime start, DateTime end, out double totalHours, bool newHire = false)
    {
        PerfTimer.StartTimer("Weekly Avg Hours Calc", "CalculateMeasurementPeriodWeeklyAverageHours");

        totalHours = 0.0;
        double _planYearAvg = 0;

        if (EmployeePayrollList.Count > 0)
        {
            PerfTimer.StartTimer("Filter By Dates", "CalculateMeasurementPeriodWeeklyAverageHours");

            List<Payroll> pay_pyList = new List<Payroll>();

            pay_pyList = this.FilterPayrollByDates(
                EmployeePayrollList,
                start,
                end);

            PerfTimer.LogTimeAndDispose("Filter By Dates");

            if (pay_pyList.Count > 0)
            {
                PerfTimer.StartTimer("Calc Total Hours", "CalculateMeasurementPeriodWeeklyAverageHours");

                totalHours = EmployeeShow.calculateTotalHoursWorked(
                    pay_pyList,
                    start,
                    end,
                    newHire);

                PerfTimer.LogTimeAndDispose("Calc Total Hours");

                if (totalHours > 0.0)
                {
                    PerfTimer.StartTimer("Calc With BIS", "CalculateMeasurementPeriodWeeklyAverageHours");

                    int weeksOfBreak = this.CalculateEmployeeWeeksOfBreaks(
                        FilterBreaks,
                        start,
                        end);

                    double MonthsInMp = 12.0;
                    MonthsInMp = (int)Math.Round((end - start).TotalDays / (365.0 / 12.0));

                    _planYearAvg = this.CalculateEmployeeAverageWeeklyHoursWithBreaks(totalHours, weeksOfBreak, MonthsInMp);

                    if (this.Log.IsDebugEnabled) 
                    {
                        this.Log.Debug(string.Format(
                            "mp_py: Calculate data if EMPLOYEE is ONGOING. Total Payroll List Count:[{0}], Total Hours: [{1}], Weeks of Break: [{2}], Calc Average Hours Month:[{3}].",
                            pay_pyList.Count,
                            totalHours,
                            weeksOfBreak,
                            _planYearAvg));
                    }

                    PerfTimer.LogTimeAndDispose("Calc With BIS");
                }

            }

        }

        PerfTimer.LogTimeAndDispose("Weekly Avg Hours Calc");

        return _planYearAvg;
    }

    /// <summary>
    /// Filter out any payroll that starts after the filter end or ends before the filter start
    /// </summary>
    /// <param name="filterStart">Filter out Payroll that ended before this date.</param>
    /// <param name="filterEnd">Filter out Payroll that started after this date.</param>
    /// <returns>THe Filtered Payroll list</returns>
    private List<Payroll> FilterPayrollByDates(List<Payroll> PayrollList, DateTime filterStart, DateTime filterEnd)
    {
        List<Payroll> result = (from pay in PayrollList
                                where
                                (pay.PAY_EDATE >= filterStart
                                && pay.PAY_SDATE <= filterEnd
                                && pay.PAY_SDATE <= pay.PAY_EDATE)
                                select pay).ToList();
        return result;
    }

    /// <summary>
    /// Do the filtering work upfront to limit the amount of work that must be done later on, this filters payroll into lists per employee.
    /// </summary>
    /// <param name="PayRollList">All Payroll to be filtered.</param>
    /// <returns>The dicitonary of Payroll filtered by employee Id.</returns>
    private Dictionary<int, List<Payroll>> PrefilterPayroll(List<Payroll> PayRollList)
    {
        PerfTimer.StartTimer("Filter Pay Data", "PrefilterPayroll");

        Dictionary<int, List<Payroll>> Filtered = new Dictionary<int, List<Payroll>>();

        foreach (Payroll pay in PayRollList)
        {
            if (pay.PAY_EDATE == null || pay.PAY_SDATE == null)
            {
                if (this.Log.IsInfoEnabled)
                {
                    this.Log.Info("Skipping payroll on calculation due to null dates. Pay Id: [" + pay.ROW_ID + "]");
                }
                continue;
            }

            if (false == Filtered.ContainsKey(pay.PAY_EMPLOYEE_ID))
            {
                Filtered.Add(pay.PAY_EMPLOYEE_ID, new List<Payroll>());
            }

            Filtered[pay.PAY_EMPLOYEE_ID].Add(pay);
        }

        PerfTimer.LogTimePerItemAndDispose("Filter Pay Data", PayRollList.Count);

        return Filtered;
    }

    /// <summary>
    /// Gets the correct MP from the Cache based on the plan year and employee type.
    /// </summary>
    /// <param name="Measurements">The cache of measurement periods</param>
    /// <param name="PlanId">The plan year to get the MP for.</param>
    /// <param name="EmployeeTypeId">THe employee Type for this MP</param>
    /// <returns>The MP or null if it was not found.</returns>
    private Measurement GetMeasurementPeriod(List<Measurement> Measurements, int PlanId, int EmployeeTypeId)
    {
        return (from meas in Measurements
                where meas.MEASUREMENT_PLAN_ID == PlanId
                    && meas.MEASUREMENT_EMPLOYEE_TYPE_ID == EmployeeTypeId
                select meas).FirstOrDefault();
    }

    /// <summary>
    /// Search through the finished employee data for the data
    /// </summary>
    /// <param name="AverageHoursTable">The data to search</param>
    /// <param name="EmployeeId">The id to search for.</param>
    /// <param name="MeasurementId">The Measurement Id to search for.</param>
    /// <returns>The average hours.</returns>
    private double GetMonthlyForEmployeeMP(DataTable AverageHoursTable, int EmployeeId, int MeasurementId)
    {
        DataRow[] dr = AverageHoursTable.Select("EmployeeId = " + EmployeeId.ToString() + " AND MeasurementId = " + MeasurementId.ToString() + " AND IsNewHire = false").ToArray();
        if (dr.Count() > 0 && dr.First()["MonthlyAverageHours"] != null)
        {
            return (double)dr.First()["MonthlyAverageHours"];
        }
        else
        {
            return 0.0;
        }
    }

    /// <summary>
    /// Filter the breaks list to only a single MP
    /// </summary>
    /// <param name="Breaks">List of all Breaks.</param>
    /// <param name="measurementId">MP Id to filter on.</param>
    /// <returns>Limited list of Breaks.</returns>
    private List<BreakInService> FilterBreaksToOneMp(List<BreakInService> Breaks, int measurementId)
    {
        List<BreakInService> Filtered = (from BreakInService bis in Breaks where bis.MeasurementId == measurementId select bis).ToList();
        return Filtered;
    }

    /// <summary>
    /// Calculate the weeks of break that apply to this time period (rounded)
    /// </summary>
    /// <param name="FilteredBreaks">The list of breaks (prefiltered)</param>
    /// <param name="lowerBounds">Lower bounds date, ignores all break value before this date.</param>
    /// <param name="upperBounds">Upper bounds date, ignores all break value after this date.</param>
    /// <returns>The Rounded up number of weeks of break.</returns>
    private int CalculateEmployeeWeeksOfBreaks(List<BreakInService> FilteredBreaks, DateTime lowerBounds, DateTime upperBounds)
    {
        int weeksOfBreaks = 0;

        if (FilteredBreaks.Count <= 0)
        {
            return weeksOfBreaks;
        }

        List<BreakInService> Composite = new List<BreakInService>();
        DateTime enddate = new DateTime();
        foreach (BreakInService breaks in FilteredBreaks.OrderBy(obj => obj.StartDate))
        {
            if (breaks.StartDate >= upperBounds || breaks.EndDate <= lowerBounds)
            {
                continue;
            }

            if (enddate >= breaks.StartDate) { continue; }

            BreakInService newComposite = new BreakInService
            {
                StartDate = breaks.StartDate
            };

            enddate = breaks.EndDate;
            foreach (BreakInService other in FilteredBreaks)
            {
                if (other.StartDate >= breaks.StartDate && other.StartDate < enddate && other.EndDate > enddate)
                {
                    enddate = other.EndDate;
                }
            }

            newComposite.EndDate = enddate;

            Composite.Add(newComposite);
        }

        foreach (BreakInService breaks in Composite)
        {
            DateTime StartDate = breaks.StartDate >= lowerBounds ? breaks.StartDate : lowerBounds;
            DateTime EndDate = breaks.EndDate <= upperBounds ? breaks.EndDate : upperBounds;
            weeksOfBreaks += (int)Math.Ceiling((EndDate - StartDate).TotalDays / 7.0);
        }

        return weeksOfBreaks;
    }

    /// <summary>
    /// Calculates the Average umber of weeks based on the months in the period.
    /// </summary>
    /// <param name="monthsInPeriod">The number of months to calculate weeks for.</param>
    /// <returns>The number of weeks</returns>
    private double CalculateWeeksInPeriod(double monthsInPeriod = 12.0)
    {
        return (52.0 / 12.0) * monthsInPeriod;
    }

    /// <summary>
    /// Actually calculate the average hours.
    /// </summary>
    /// <param name="sumOfHours">Total hours during the period.</param>
    /// <param name="weeksOfBreaks">Total number of weeks of break from during the period.</param>
    /// <param name="monthsInPeriod">Total months in period</param>
    /// <returns>The average weekly hours taking into account breaks in service and short MPs</returns>
    private double CalculateEmployeeAverageWeeklyHoursWithBreaks(double sumOfHours, int weeksOfBreaks, double monthsInPeriod = 12.0)
    {
        double weeksInPeriod = this.CalculateWeeksInPeriod(monthsInPeriod);

        double weeks = (weeksInPeriod - weeksOfBreaks) < (52.0 / 12.0) ? (52.0 / 12.0) : (weeksInPeriod - weeksOfBreaks);

        double oneWeekAvgHours = sumOfHours / weeks;

        if (this.Log.IsDebugEnabled)
        {
            this.Log.Debug("Calculated one Week Average: " + oneWeekAvgHours);
        }

        return oneWeekAvgHours;
    }

    /// <summary>
    /// Takes the average hours per week and converts it tho average hours per month 
    /// </summary>
    /// <param name="avergeWeeklyHours">The average hours per week.</param>
    /// <returns>Average hours per month.</returns>
    private double ConvertWeeksHoursToMonths(double avergeWeeklyHours)
    {
        return avergeWeeklyHours * (52.0 / 12.0);
    }

    /// <summary>
    /// CHecks if the Employee was in their IMP during this MP.
    /// </summary>
    /// <param name="emp">The employee to check</param>
    /// <param name="mp_py">The MP to check</param>
    /// <returns>True if the employe was in their IMP during any part of the MP.</returns>
    private bool IsNewHire(Employee emp, Measurement mp_py)
    {
        if (emp == null || mp_py == null)
        {
        
            return false;
        }

        return (emp.EMPLOYEE_HIRE_DATE >= mp_py.MEASUREMENT_START)
            && (emp.EMPLOYEE_HIRE_DATE <= mp_py.MEASUREMENT_END) ||
            (emp.EMPLOYEE_IMP_END >= mp_py.MEASUREMENT_START)
            && (emp.EMPLOYEE_IMP_END <= mp_py.MEASUREMENT_END);
    }

    /// <summary>
    /// Checks if the Employee was termiated before the start of the MP.
    /// </summary>
    /// <param name="emp">Employee to check</param>
    /// <param name="mp_py">MP to check</param>
    /// <returns>True if the Employee was not employed during the MP due to Termination.</returns>
    private bool IsTerminated(Employee emp, Measurement mp_py)
    {
        if (emp == null || mp_py == null || emp.EMPLOYEE_TERM_DATE == null)
        {
            return false;
        }

        return (emp.EMPLOYEE_TERM_DATE <= mp_py.MEASUREMENT_START);
    }
}