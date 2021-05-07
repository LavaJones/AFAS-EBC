using Afas.AfComply.Domain;
using Afas.AfComply.Reporting.Application;
using Afas.AfComply.Reporting.Application.Services.LegacyServices;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Domain.LegacyData;
using Afas.AfComply.Reporting.Domain.Reporting;
using Afas.Domain;
using Afc.Core.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Afas.AfComply.Reporting.Domain.MonthlyDetails
{
    public static class _1095MonthlyDetails
    {
        private static ILog Log = LogManager.GetLogger(typeof(_1095MonthlyDetails));

        private static PerformanceTiming PerfTimer = new PerformanceTiming(typeof(_1095MonthlyDetails), null, SystemSettings.UsePerformanceLog);

        public static employee getEmployeeByResourceId(Guid ResourceId)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(_1095MonthlyDetails), "getEmployeeByResourceId", SystemSettings.UsePerformanceLog))
            {
                using (AcaEntities ctx = new AcaEntities())
                {
                    ctx.Database.CommandTimeout = 180;
                    return _1095MonthlyDetails.getEmployeeByResourceId(ResourceId, ctx);
                }
            }
        }

        private static employee getEmployeeByResourceId(Guid ResourceId, AcaEntities ctx)
        {
            return ctx.employees.Where(emp => emp.ResourceId == ResourceId).Single();
        }

        public static Employee1095summaryModel GetSingleEmployee1095summaryModel(Guid ResourceId, int EmployerId, int TaxYear, IUserEditPart2Service UserEditService, IUserReviewedService reviewService, ILegacyClassifictaionService LegacyClassifictaionService)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(_1095MonthlyDetails), "GetSingleEmployee1095summaryModel", SystemSettings.UsePerformanceLog))
            {
                using (AcaEntities ctx = new AcaEntities())
                {
                    ctx.Database.CommandTimeout = 180;
                    return _1095MonthlyDetails.GetSingleEmployee1095summaryModel(ResourceId, EmployerId, TaxYear, UserEditService, reviewService, LegacyClassifictaionService, ctx);
                }
            }
        }

        private static Employee1095summaryModel GetSingleEmployee1095summaryModel(Guid ResourceId, int EmployerId, int TaxYear, IUserEditPart2Service UserEditService, IUserReviewedService reviewService, ILegacyClassifictaionService LegacyClassifictaionService, AcaEntities ctx)
        {
            employee emp = _1095MonthlyDetails.getEmployeeByResourceId(ResourceId, ctx);

            List<Employee1095detailsPart2Model> part2s = _1095MonthlyDetails.getEmployeeMonthlyDetail(EmployerId, TaxYear, UserEditService, ctx, emp);

            List<Employee1095detailsPart3Model> part3s = _1095MonthlyDetails.getEmployeeInsuranceCoverage(emp.employee_id, TaxYear, ctx);

            bool reviewed = (reviewService.GetForEmployeeTaxYear(emp.employee_id, TaxYear).Count() > 0);

            string classification = ""; 

            return BuildModel(TaxYear, emp, part2s, part3s, reviewed, classification);
        }

        public static List<Employee1095summaryModel> GetAllEmployee1095summaryModel(int EmployerId, int TaxYear, IUserEditPart2Service UserEditService, ILegacyEmployeeService LegacyEmployeeService, IUserReviewedService reviewService, IFinalize1095Service finalService)
        {

            PerfTimer.StartTimer("Whole Method", "GetAllEmployee1095summaryModel");
            PerfTimer.StartTimer("Gathering Part 2 Info", "GetAllEmployee1095summaryModel");

            List<string> EmployeeIdIssue = new List<string>();
            List<string> Part2NotFound = new List<string>();
            List<string> ILBFound = new List<string>();

            List<Employee1095summaryModel> models = new List<Employee1095summaryModel>();
            
            HashSet<int> finalizedIds = new HashSet<int>(finalService.GetApproved1095sEmployeeIdsForEmployerTaxYear(EmployerId, TaxYear));

            Dictionary<int, List<Employee1095detailsPart2Model>> allPart2s = _1095MonthlyDetails.getEmployeeMonthlyDetailDic(EmployerId, TaxYear, UserEditService);

            PerfTimer.LogTimePerItemAndDispose("Gathering Part 2 Info", allPart2s.Count, "Gathered [" + allPart2s.Count + "] Part 2s from DB.", false);
            PerfTimer.StartTimer("Gathering Suplemental Info", "GetAllEmployee1095summaryModel");

            List<UserReviewed> revieweds = reviewService.GetForEmployerTaxYear(EmployerId, TaxYear).ToList();

            Dictionary<int, List<Employee1095detailsPart3Model>> AllPart3s = _1095MonthlyDetails.getAllEmployerInsuranceCoverage(EmployerId, TaxYear);

            PerfTimer.LogTimeAndDispose("Gathering Suplemental Info");
            PerfTimer.StartTimer("Linq sorting EmployeeIds", "GetAllEmployee1095summaryModel");

            List<int> employeeIds = allPart2s.Keys.ToList();

            employeeIds = (from int empId in employeeIds
                           where false == finalizedIds.Contains(empId)
                           select empId).ToList();

            HashSet<int> reviewedIds = new HashSet<int>(revieweds.Select(rev => rev.EmployeeId).Distinct().ToList());

            PerfTimer.LogTimeAndDispose("Linq sorting EmployeeIds");
            PerfTimer.StartTimer("Gathering Employees", "GetAllEmployee1095summaryModel");

            HashSet<int> hashEmployeeIds = new HashSet<int>(employeeIds);

            List<employee> allEmployees = LegacyEmployeeService.GetEmployeesForEmployer(EmployerId);

            List<employee> employees = new List<employee>();
            
            foreach (employee emp in allEmployees)
            {
                if (hashEmployeeIds.Contains(emp.employee_id))
                {
                    employees.Add(emp);
                }
                else
                {
                    if (false == finalizedIds.Contains(emp.employee_id))
                    {
                        EmployeeIdIssue.Add(emp.employee_id.ToString());
                    }
                }
            }

            PerfTimer.LogTimePerItemAndDispose("Gathering Employees", employees.Count);
            PerfTimer.StartTimer("Building EmployeeModels", "GetAllEmployee1095summaryModel");

            foreach (employee emp in employees)
            {

                List<Employee1095detailsPart3Model> part3s = new List<Employee1095detailsPart3Model>();
                if (true == AllPart3s.ContainsKey(emp.employee_id))
                {
                    part3s = AllPart3s[emp.employee_id];
                }

                List<Employee1095detailsPart2Model> part2s = new List<Employee1095detailsPart2Model>();
                if (allPart2s.ContainsKey(emp.employee_id))
                {
                    part2s = allPart2s[emp.employee_id].OrderBy(pt => pt.MonthId).ToList();

                    if (part2s.Count != 13)
                    {
                        ILBFound.Add(emp.employee_id.ToString());
                    }
                }
                else
                {
                    Part2NotFound.Add(emp.employee_id.ToString());
                }
                
                bool reviewed = reviewedIds.Contains(emp.employee_id);
                string classification = emp.classification_id.ToString();

                Employee1095summaryModel model = BuildModel(TaxYear, emp, part2s, part3s, reviewed, classification);

                models.Add(model);

            }

            models = models.OrderBy(item => item.LastName).ToList();

            PerfTimer.LogTimePerItemAndDispose("Building EmployeeModels", models.Count);

            if (EmployeeIdIssue.Count > 0)
            {
                Log.Error(string.Format("Employee Id Issue found on Employer [{0}] for Employee Ids: [{1}]", EmployerId, string.Join(", ", EmployeeIdIssue)));
            }
            if (Part2NotFound.Count > 0)
            {
                Log.Error(string.Format("No Part 2 Data found on Employer [{0}] for Employee Ids: [{1}]", EmployerId, string.Join(", ", Part2NotFound)));
            }
            if (ILBFound.Count > 0)
            {
                Log.Error(string.Format("Intentionally Left Blanks found on Employer [{0}] for Employee Ids: [{1}]", EmployerId, string.Join(", ", ILBFound)));
            }

            PerfTimer.LogTimeAndDispose("Whole Method", string.Empty, true);

            return models;

        }

        private static Employee1095summaryModel BuildModel(int TaxYear, employee emp, List<Employee1095detailsPart2Model> part2s, List<Employee1095detailsPart3Model> part3s, bool reviewed, string classification)
        {

            Employee1095summaryModel model = new Employee1095summaryModel
            {
                TaxYear = TaxYear,
                EmployeeMonthlyDetails = part2s,
                CoveredIndividuals = part3s,
                EmployeeID = emp.employee_id,
                EmployerID = emp.employer_id,
                EmployeeResourceId = emp.ResourceId,
                Address = emp.address,
                City = emp.city,
                EmployeeClass = classification,
                FirstName = emp.fName,
                HireDate = emp.hireDate,
                LastName = emp.lName,
                MiddleName = emp.mName
            };

            model.Receiving1095 = model.CoveredIndividuals.Count() > 0
                || model.EmployeeMonthlyDetails.Any(details => details.Receiving1095C == true)
                || model.EmployeeMonthlyDetails.Any(details => details.Enrolled == true && details.InsuranceType == "2");

            model.ResourceId = emp.ResourceId;
            model.ResourceStatus = "Active";
            model.Reviewed = reviewed;

            model.Ssn = emp.SSN;
            model.SsnHidden = emp.SSN_hidden;

            model.State = emp.state_id.ToString();
            model.TermDate = emp.terminationDate;
            model.Zip = int.Parse(emp.zip);
            model.ZipPlus4 = 0;


            return model;

        }


        public static List<Employee1095detailsPart2Model> getEmployeeMonthlyDetail(int employerId, int TaxYear, IUserEditPart2Service editsRepo, employee forEmployee = null)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(_1095MonthlyDetails), "getEmployeeMonthlyDetail", SystemSettings.UsePerformanceLog))
            {
                using (AcaEntities ctx = new AcaEntities())
                {
                    ctx.Database.CommandTimeout = 300;

                    return _1095MonthlyDetails.getEmployeeMonthlyDetail(employerId, TaxYear, editsRepo, ctx, forEmployee);
                }
            }
        }

        public static Dictionary<int, List<Employee1095detailsPart2Model>> getEmployeeMonthlyDetailDic(int employerId, int TaxYear, IUserEditPart2Service editsRepo, employee forEmployee = null)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(_1095MonthlyDetails), "getEmployeeMonthlyDetailDic", SystemSettings.UsePerformanceLog))
            {
                using (AcaEntities ctx = new AcaEntities())
                {
                    ctx.Database.CommandTimeout = 300;
                    return _1095MonthlyDetails.getEmployeeMonthlyDetailDic(employerId, TaxYear, editsRepo, ctx, forEmployee);
                }
            }
        }

        private static List<Employee1095detailsPart2Model> getEmployeeMonthlyDetail(
            int employerId,
            int TaxYear,
            IUserEditPart2Service editsRepo,
            AcaEntities ctx,
            employee forEmployee = null
            )
        {

            Dictionary<int, List<Employee1095detailsPart2Model>> filteredResult = _1095MonthlyDetails.getEmployeeMonthlyDetailDic(employerId, TaxYear, editsRepo, ctx, forEmployee);

            List<Employee1095detailsPart2Model> output = filteredResult.Values.ToList().SelectMany(x => x).ToList();

            return output;

        }

        public static Dictionary<int, List<Employee1095detailsPart2Model>> getEmployeeMonthlyDetailDic(
                int employerId,
                int TaxYear,
                IUserEditPart2Service editsRepo,
                AcaEntities ctx,
                employee forEmployee = null
            )
        {
            PerfTimer.MethodName = "getEmployeeMonthlyDetailDic";
            PerfTimer.StartTimer("Initial Data Pull Region", "getEmployeeMonthlyDetailDic");


            #region Initial Data Pull

            PerfTimer.StartTimer("Data Pull Section", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartTimer("Pull ctx.insurances", "getEmployeeMonthlyDetailDic");

            Dictionary<int, insurance> insurances = ctx.insurances.AsNoTracking().ToDictionary(item => item.insurance_id, item => item);

            PerfTimer.LogTimeAndDispose("Pull ctx.insurances");
            PerfTimer.StartTimer("Pull ctx.insurance_contribution", "getEmployeeMonthlyDetailDic");

            Dictionary<int, insurance_contribution> contributions = ctx.insurance_contribution.AsNoTracking().ToDictionary(item => item.ins_cont_id, item => item);

            PerfTimer.LogTimeAndDispose("Pull ctx.insurance_contribution");
            PerfTimer.StartTimer("Pull ctx.View_insurance_change_events", "getEmployeeMonthlyDetailDic");

            List<View_insurance_change_events> ices =
                (from changeEvent in ctx.View_insurance_change_events.AsNoTracking()
                 where changeEvent.employer_id.Value == employerId
                 orderby changeEvent.employee_id
                 , changeEvent.effectiveDate
                 , changeEvent.offered
                 select changeEvent).ToList();

            PerfTimer.LogTimeAndDispose("Pull ctx.View_insurance_change_events");
            PerfTimer.StartTimer("Pull ctx.plan_year", "getEmployeeMonthlyDetailDic");

            Dictionary<int, plan_year> planYearDic = ctx.plan_year.AsNoTracking().Where(s => s.employer_id == employerId).ToDictionary(py => py.plan_year_id, item => item);

            PerfTimer.LogTimeAndDispose("Pull ctx.plan_year");
            PerfTimer.StartTimer("Pull UserEditPart2", "getEmployeeMonthlyDetailDic");

            Dictionary<int, Dictionary<int, List<UserEditPart2>>> edits = editsRepo.GetForEmployerTaxYear(employerId, TaxYear);

            PerfTimer.LogTimeAndDispose("Pull UserEditPart2");
            PerfTimer.StartTimer("Forced fetch entities", "getEmployeeMonthlyDetailDic");

            List<View_air_replacement_EmployeeYearlyDetails> dataPull = null;

            DateTime This_TaxYearJan = new DateTime(TaxYear, 1, 1);
            DateTime Next_TaxYearJan = new DateTime(TaxYear + 1, 1, 1);
            
            if (null != forEmployee)
            {
                dataPull = (
                        from val in ctx.View_air_replacement_EmployeeYearlyDetails.AsNoTracking()
                        where (val.EmployeeId == forEmployee.employee_id
                              &&
                                  (val.tax_year == TaxYear
                                  ||
                                      ((true == val.IsNewHire)
                                        &&
                                        (val.initialMeasurmentEnd < Next_TaxYearJan
                                           &&
                                          val.InitialStabilityPeriodEndDate.Value >= This_TaxYearJan))))
                        select val
                    ).ToList();

            }
            else
            {
                dataPull = (
                        from val in ctx.View_air_replacement_EmployeeYearlyDetails.AsNoTracking()
                        where (val.employer_id == employerId
                              &&
                                  (val.tax_year == TaxYear
                                  || 
                                      ((true == val.IsNewHire)
                                        &&
                                        (val.initialMeasurmentEnd < Next_TaxYearJan
                                           &&
                                          val.InitialStabilityPeriodEndDate.Value >= This_TaxYearJan))))
                        select val
                    ).ToList();

            }

            IEnumerable<View_air_replacement_EmployeeYearlyDetails> dataQuery = dataPull;

            PerfTimer.LogTimePerItemAndDispose("Forced fetch entities", dataPull.Count);
            PerfTimer.LogTimePerItemAndDispose("Data Pull Section", dataPull.Count, "Fetched entities for employer: " + employerId, true);
            PerfTimer.StartTimer("Initial Processing Section", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartTimer("Processing insurance_change_events", "getEmployeeMonthlyDetailDic");

            Dictionary<int, IList<View_insurance_change_events_details>> cedDic = new Dictionary<int, IList<View_insurance_change_events_details>>();

            foreach (View_insurance_change_events ice in ices)
            {
                if (false == cedDic.ContainsKey(ice.employee_id))
                {
                    cedDic.Add(ice.employee_id, new List<View_insurance_change_events_details>());
                }

                View_insurance_change_events_details item = new View_insurance_change_events_details();

                insurance ins = new insurance();
                if (insurances.ContainsKey(ice.insurance_id ?? 0))
                {
                    ins = insurances[ice.insurance_id ?? 0];
                }

                insurance_contribution contribution = new insurance_contribution();
                if (contributions.ContainsKey(ice.ins_cont_id ?? 0))
                {
                    contribution = contributions[ice.ins_cont_id ?? 0];
                }

                item.accepted = ice.accepted;
                item.effectiveDate = ice.effectiveDate;
                item.employee_id = ice.employee_id;
                item.EmployerContribution = contribution.amount;
                item.EmployerContributionType = contribution.contribution_id;
                item.fullyPlusSelfInsured = ins.fullyPlusSelfInsured;
                item.hra_flex_contribution = ice.hra_flex_contribution;
                item.insurance_type_id = ins.insurance_type_id;
                item.Mec = ins.Mec;
                item.minValue = ins.minValue;
                item.monthlycost = ins.monthlycost;
                item.offDependent = ins.offDependent;
                item.offered = ice.offered;
                item.offSpouse = ins.offSpouse;
                item.plan_year_id = ice.plan_year_id;
                item.SpouseConditional = ins.SpouseConditional;

                if (false == cedDic[ice.employee_id].Contains(item))
                {
                    cedDic[ice.employee_id].Add(item);
                }
            }

            Dictionary<int, IList<View_insurance_change_events_details>> orderedCedDic = new Dictionary<int, IList<View_insurance_change_events_details>>();

            foreach (int key in cedDic.Keys)
            {
                orderedCedDic.Add(key, cedDic[key].OrderBy(item => item.effectiveDate).ToList());
            }

            PerfTimer.LogTimePerItemAndDispose("Processing insurance_change_events", ices.Count);
            PerfTimer.StartLapTimerPaused("EmployeeDetails Loop", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimer("Other Overhead", "getEmployeeMonthlyDetailDic");

            #region Build The Data out to 12 months

            List<View_air_replacement_EmployeeYearlyDetails> dataBuiltOut = new List<View_air_replacement_EmployeeYearlyDetails>();

            foreach (View_air_replacement_EmployeeYearlyDetails yearlyData in dataQuery)
            {
                if (yearlyData.MonthlyAverageHours == null)
                {
                    yearlyData.stability_start = new DateTime(TaxYear, 1, 1);
                    yearlyData.StartYear = TaxYear;
                    yearlyData.StartMonth = 1;
                    yearlyData.stability_end = new DateTime(TaxYear, 12, 31);
                    yearlyData.EndYear = TaxYear;
                    yearlyData.EndMonth = 12;
                }

                if (true == yearlyData.IsNewHire 
                    && 
                    null != yearlyData.InitialStabilityPeriodEndDate
                    &&
                        (yearlyData.MonthlyAverageHours != null 
                        && 
                        yearlyData.MonthlyAverageHours > 0 ))
                {
                    yearlyData.stability_start = yearlyData.InitialAdminEnd;
                    yearlyData.stability_end = yearlyData.InitialStabilityPeriodEndDate.Value;

                    yearlyData.StartMonth = yearlyData.stability_start.Month;
                    yearlyData.StartYear = yearlyData.stability_start.Year;
                    yearlyData.EndMonth = yearlyData.stability_end.Month;
                    yearlyData.EndYear = yearlyData.stability_end.Year;

                }

                if (yearlyData.stability_start > new DateTime(TaxYear, 12, 31) || yearlyData.stability_end < new DateTime(TaxYear, 1, 1))
                {
                    continue;
                }

                PerfTimer.LapAndSwitchTimers("Other Overhead", "EmployeeDetails Loop");

                for (int i = 1; i <= 12; i++)
                {
                    DateTime month = new DateTime(TaxYear, i, 1);
                    if (
                        yearlyData.hireDate > month
                        ||
                        yearlyData.terminationDate < month
                        )
                    {

                        View_air_replacement_EmployeeYearlyDetails blankMonth = new View_air_replacement_EmployeeYearlyDetails
                        {
                            month_id = i,
                            Ooc = null,
                            stability_end = yearlyData.stability_end,
                            stability_start = yearlyData.stability_start,
                            StartYear = TaxYear,
                            tax_year = TaxYear,
                            terminationDate = yearlyData.terminationDate,
                            WaitingPeriodID = yearlyData.WaitingPeriodID,
                            aca_status_id = yearlyData.aca_status_id,
                            EmployeeId = yearlyData.EmployeeId,
                            ash_code = null,
                            classification_id = yearlyData.classification_id,
                            employee_type_id = yearlyData.employee_type_id,
                            employer_id = yearlyData.employer_id,
                            StartMonth = null,
                            EndMonth = null,
                            EndYear = TaxYear,
                            hireDate = yearlyData.hireDate,
                            initialMeasurmentEnd = yearlyData.initialMeasurmentEnd,
                            InitialStabilityPeriodEndDate = yearlyData.InitialStabilityPeriodEndDate,
                            InitialStabilityPeriodMonths = yearlyData.InitialStabilityPeriodMonths,
                            plan_year_id = yearlyData.plan_year_id,
                            IsNewHire = false,
                            MeasurementId = yearlyData.MeasurementId,
                            MonthlyAverageHours = 0
                        };

                        dataBuiltOut.Add(blankMonth);

                    }
                    else if (((true == yearlyData.IsNewHire) 
                            && 
                            month <= yearlyData.InitialStabilityPeriodEndDate 
                            && 
                            month >= yearlyData.stability_start 
                            && 
                            month < yearlyData.stability_end)
                        ||
                            (false == (yearlyData.IsNewHire ?? false) 
                            && 
                            month >= yearlyData.stability_start 
                            && 
                            month < yearlyData.stability_end)
                        )
                    {

                        View_air_replacement_EmployeeYearlyDetails newMonth = yearlyData.Clone();

                        newMonth.month_id = i;

                        dataBuiltOut.Add(newMonth);

                    }
                }

                PerfTimer.LapAndSwitchTimers("EmployeeDetails Loop", "Other Overhead");

            }

            PerfTimer.LogTimePerItemAndDispose("Initial Processing Section", dataQuery.Count());
            PerfTimer.LogAllLapsAndDispose("EmployeeDetails Loop");
            PerfTimer.LogAllLapsAndDispose("Other Overhead");

            #endregion


            #endregion

            PerfTimer.LogTimeAndDispose("Initial Data Pull Region", string.Empty, true);
            PerfTimer.StartTimer("Initial Coding Region", "getEmployeeMonthlyDetailDic");

            #region Initial Coding

            Dictionary<int, List<Employee1095detailsPart2Model>> result = new Dictionary<int, List<Employee1095detailsPart2Model>>();

            PerfTimer.StartLapTimerPaused("Insurance Converage SubSection", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimerPaused("Get Insurance Data", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimerPaused("Order Insurance Data", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimerPaused("Insurance Month Data", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimerPaused("Set Month Data", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimerPaused("Monthly Values SubSection", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimerPaused("Get User Edit Data", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimerPaused("Get Line 14 Data", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimerPaused("Get Line 15 Data", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimerPaused("Get Line 16 Data", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimerPaused("Get Measured FT Data", "getEmployeeMonthlyDetailDic");

            PerfTimer.StartLapTimerPaused("Loop End SubSection", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimer("Loop Overhead SubSection", "getEmployeeMonthlyDetailDic");

            foreach (View_air_replacement_EmployeeYearlyDetails emp in dataBuiltOut)
            {
                try
                {

                    PerfTimer.LapAndSwitchTimers("Loop Overhead SubSection", "Insurance Converage SubSection");
                    PerfTimer.UnpauseLapTimer("Get Insurance Data");

                    Employee1095detailsPart2Model month = new Employee1095detailsPart2Model();

                    int planYearID = emp.plan_year_id;

                    IList<View_insurance_change_events_details> myChangeEventsByEmployee = new List<View_insurance_change_events_details>();
                    if (cedDic.ContainsKey(emp.EmployeeId))
                    {
                        myChangeEventsByEmployee = cedDic[emp.EmployeeId];
                    }

                    PerfTimer.LapAndSwitchTimers("Get Insurance Data", "Order Insurance Data");

                    View_insurance_change_events_details activeChangeEvent = null;


                    DateTime monthStartDate = new DateTime(TaxYear, emp.month_id, 1);
                    DateTime lastDayOfMonth = monthStartDate.AddMonths(1).AddDays(-1);

                    foreach (View_insurance_change_events_details ice in myChangeEventsByEmployee)
                    {
                        if (ice.effectiveDate > lastDayOfMonth)
                        {
                            break;
                        }

                        if (emp.terminationDate != null && emp.terminationDate < monthStartDate && ice.effectiveDate < emp.terminationDate)
                        {
                            continue;
                        }

                        plan_year that_plan_year = planYearDic[ice.plan_year_id];

                        if (ice.effectiveDate <= monthStartDate
                            &&
                            (activeChangeEvent == null || ice.effectiveDate > activeChangeEvent.effectiveDate)
                            &&
                            that_plan_year.endDate > monthStartDate)
                        {
                            activeChangeEvent = ice;
                        }

                    }

                    PerfTimer.LapAndSwitchTimers("Order Insurance Data", "Insurance Month Data");

                    if (activeChangeEvent != null)
                    {
                        month.InsuranceType = activeChangeEvent.insurance_type_id.ToString();

                        month.Offered = activeChangeEvent.offered ?? false;             

                        month.Enrolled = activeChangeEvent.accepted ?? false;             

                    }
                    else
                    {
                        month.Offered = false;
                        month.Enrolled = false;
                    }

                    if (emp.terminationDate != null && emp.terminationDate.Value < monthStartDate)
                    {
                        month.Offered = false;
                        month.Enrolled = false;
                    }

                    if (emp.hireDate > monthStartDate && emp.hireDate <= lastDayOfMonth)
                    {
                        month.Offered = false;
                        month.Enrolled = false;
                    }

                    PerfTimer.LapAndSwitchTimers("Insurance Month Data", "Set Month Data");

                    month.TaxYear = TaxYear;
                    month.MonthlyHours = (double)(emp.MonthlyAverageHours ?? 0m);
                    month.MonthId = emp.month_id;
                    month.AcaStatus = emp.aca_status_id ?? 0;
                    month.EmployeeId = emp.EmployeeId;

                    PerfTimer.LapAndPause("Set Month Data");
                    PerfTimer.LapAndSwitchTimers("Insurance Converage SubSection", "Monthly Values SubSection");
                    PerfTimer.UnpauseLapTimer("Get User Edit Data");

                    UserEditPart2 line14EditedValue = null;
                    UserEditPart2 line15EditedValue = null;
                    UserEditPart2 line16EditedValue = null;
                    UserEditPart2 receivingEditedValue = null;

                    if (edits.ContainsKey(month.EmployeeId))
                    {
                        var employeeEdits = edits[month.EmployeeId];

                        if (employeeEdits.ContainsKey(month.MonthId))
                        {
                            var monthEdits = employeeEdits[month.MonthId]
                                .OrderByDescending(item => item.CreatedDate)
                                .GroupBy(item => item.LineId)
                                .Select(item => item.FirstOrDefault())
                                .ToList();

                            foreach (var orderedEdits in monthEdits)
                            {
                                if (null == orderedEdits)
                                {
                                    continue;
                                }
                                if (14 == orderedEdits.LineId)
                                {
                                    line14EditedValue = orderedEdits;
                                }
                                if (15 == orderedEdits.LineId)
                                {
                                    line15EditedValue = orderedEdits;
                                }
                                if (16 == orderedEdits.LineId)
                                {
                                    line16EditedValue = orderedEdits;
                                }
                                if (0 == orderedEdits.LineId)
                                {
                                    receivingEditedValue = orderedEdits;
                                }
                            }
                        }
                    }

                    PerfTimer.LapAndSwitchTimers("Get User Edit Data", "Get Line 14 Data");

                    if (null == line14EditedValue)
                    {
                        month.Line14 = GetMecCode(TaxYear, emp, activeChangeEvent, month);
                    }
                    else
                    {
                        month.Line14 = line14EditedValue.NewValue;
                        month.UserEdited = true;
                    }

                    PerfTimer.LapAndSwitchTimers("Get Line 14 Data", "Get Line 15 Data");

                    if (month.Line14 == "1H")
                    {
                        month.Enrolled = false;
                        month.Offered = false;
                    }

                    if (null == line15EditedValue)
                    {
                        month.Line15 = GetMonthlyCost(activeChangeEvent, month);
                    }
                    else
                    {
                        month.Line15 = line15EditedValue.NewValue;
                        month.UserEdited = true;
                    }

                    PerfTimer.LapAndSwitchTimers("Get Line 15 Data", "Get Line 16 Data");

                    if (null == line16EditedValue)
                    {
                        month.Line16 = GetSafeHarborCode(TaxYear, emp, month);
                    }
                    else
                    {
                        month.Line16 = line16EditedValue.NewValue;
                        month.UserEdited = true;
                    }

                    PerfTimer.LapAndSwitchTimers("Get Line 16 Data", "Get Measured FT Data");

                    if (null == receivingEditedValue)
                    {
                        month.Receiving1095C = GetMeasuredFT(TaxYear, emp);
                    }
                    else
                    {
                        month.Receiving1095C = (receivingEditedValue.NewValue ?? "").ToUpper() == "TRUE";
                        month.UserEdited = true;
                    }

                    PerfTimer.LapAndPause("Get Measured FT Data");
                    PerfTimer.LapAndSwitchTimers("Monthly Values SubSection", "Loop End SubSection");

                    if (false == result.ContainsKey(month.EmployeeId))
                    {
                        result.Add(month.EmployeeId, new List<Employee1095detailsPart2Model>());
                    }

                    result[month.EmployeeId].Add(month);

                    PerfTimer.LapAndSwitchTimers("Loop End SubSection", "Loop Overhead SubSection");

                }
                catch (Exception ex)
                {

                    int id = emp.EmployeeId;         

                    Log.Warn(
                        string.Format(
                                "Suppressing exception for employeeId {0}.",
                                emp.EmployeeId),
                            ex
                        );
                }

            }

            PerfTimer.LogAllLapsAndDispose("Insurance Converage SubSection", string.Empty, true);
            PerfTimer.LogAllLapsAndDispose("Get Insurance Data");
            PerfTimer.LogAllLapsAndDispose("Order Insurance Data");
            PerfTimer.LogAllLapsAndDispose("Insurance Month Data");
            PerfTimer.LogAllLapsAndDispose("Set Month Data");

            PerfTimer.LogAllLapsAndDispose("Monthly Values SubSection", string.Empty, true);
            PerfTimer.LogAllLapsAndDispose("Get User Edit Data");
            PerfTimer.LogAllLapsAndDispose("Get Line 14 Data");
            PerfTimer.LogAllLapsAndDispose("Get Line 15 Data");
            PerfTimer.LogAllLapsAndDispose("Get Line 16 Data");
            PerfTimer.LogAllLapsAndDispose("Get Measured FT Data");

            PerfTimer.LogAllLapsAndDispose("Loop End SubSection");
            PerfTimer.LapLogAndDispose("Loop Overhead SubSection", true);

            #endregion

            PerfTimer.LogTimeAndDispose("Initial Coding Region", string.Empty, true);
            PerfTimer.StartTimer("Merge Duplicate Months Region", "getEmployeeMonthlyDetailDic");


            #region Merge Duplicate Months

            PerfTimer.StartLapTimerPaused("Duplicates Choosing Best Value", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimer("Duplicates Everything Else", "getEmployeeMonthlyDetailDic");

            Dictionary<int, List<Employee1095detailsPart2Model>> filteredResult = new Dictionary<int, List<Employee1095detailsPart2Model>>();

            foreach (int EmployeeKey in result.Keys)
            {
                try
                {
                    if (false == filteredResult.ContainsKey(EmployeeKey))
                    {
                        filteredResult.Add(EmployeeKey, new List<Employee1095detailsPart2Model>());
                    }

                    foreach (IEnumerable<Employee1095detailsPart2Model> items in result[EmployeeKey].GroupBy(val => val.MonthId))
                    {
                        PerfTimer.LapAndSwitchTimers("Duplicates Everything Else", "Duplicates Choosing Best Value");

                        double MaxHours = items.Max(val => val.MonthlyHours);
                        bool AnyReceiving = items.Any(val => val.Receiving1095C == true);

                        Employee1095detailsPart2Model choice = items
                            .OrderByDescending(val => val.Enrolled)
                            .ThenByDescending(val => val.Offered)
                            .ThenByDescending(val => val.Receiving1095C)
                            .ThenByDescending(val => val.MonthlyHours)
                            .ThenByDescending(val => val.ResourceId)
                            .First();
                        PerfTimer.LapAndSwitchTimers("Duplicates Choosing Best Value", "Duplicates Everything Else");

                        choice.MonthlyHours = MaxHours;
                        choice.Receiving1095C = AnyReceiving;

                        filteredResult[EmployeeKey].Add(choice);
                    }

                }
                catch (Exception ex)
                {
                    Log.Warn("Suppressing exception!", ex);
                }
            }

            PerfTimer.LogAllLapsAndDispose("Duplicates Choosing Best Value");
            PerfTimer.LapLogAndDispose("Duplicates Everything Else", true);

            #endregion

            PerfTimer.LogTimeAndDispose("Merge Duplicate Months Region", string.Empty, true);
            PerfTimer.StartTimer("1G Logic Region", "getEmployeeMonthlyDetailDic");

            #region 1G Logic

            PerfTimer.StartLapTimerPaused("Check if 1G", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimerPaused("Action if 1G", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimer("1G Overhead", "getEmployeeMonthlyDetailDic");

            foreach (int EmployeeKey in filteredResult.Keys)
            {
                bool is1G = false;

                PerfTimer.LapAndSwitchTimers("1G Overhead", "Check if 1G");

                foreach (Employee1095detailsPart2Model e in filteredResult[EmployeeKey])
                {
                    if (true == e.Enrolled &&
                        e.InsuranceType == "2")
                    {
                        is1G = true;
                    }

                    if (true == e.Receiving1095C                          
                        )
                    {
                        is1G = false;
                        break;
                    }

                }

                PerfTimer.LapAndSwitchTimers("Check if 1G", "Action if 1G");


                if (is1G)
                {
                    foreach (Employee1095detailsPart2Model e in filteredResult[EmployeeKey])
                    {
                        e.Line14 = "1G";
                        e.Line15 = string.Empty;
                        e.Line16 = string.Empty;
                    }
                }

                PerfTimer.LapAndSwitchTimers("Action if 1G", "1G Overhead");
            }

            PerfTimer.LogAllLapsAndDispose("Check if 1G");
            PerfTimer.LogAllLapsAndDispose("Action if 1G");
            PerfTimer.LapLogAndDispose("1G Overhead", true);

            #endregion

            PerfTimer.LogTimeAndDispose("1G Logic Region", string.Empty, true);
            PerfTimer.StartTimer("2D Logic Region", "getEmployeeMonthlyDetailDic");

            #region 2D Logic for Fulltime with No Offer

            PerfTimer.StartLapTimerPaused("Check if 2D", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimerPaused("Remove 2D", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimer("2D Overhead", "getEmployeeMonthlyDetailDic");

            foreach (int EmployeeKey in filteredResult.Keys)
            {
                List<Employee1095detailsPart2Model> tempList = filteredResult[EmployeeKey];

                PerfTimer.LapAndSwitchTimers("2D Overhead", "Check if 2D");

                bool remove2D = true;
                bool hit2D = false;
                foreach (Employee1095detailsPart2Model e in tempList)
                {
                    if (e.Line16 == "2D")
                    {
                        hit2D = true;
                    }

                    if (
                        e.Offered == true
                        ||
                        (hit2D
                        &&
                        false == e.Receiving1095C))                            
                    {

                        remove2D = false;
                        break;

                    }

                    if (e.Line16 == "2D")
                    {
                        e.Receiving1095C = false;
                    }
                }

                PerfTimer.LapAndSwitchTimers("Check if 2D", "Remove 2D");

                if (remove2D)
                {
                    foreach (Employee1095detailsPart2Model e in tempList)
                    {
                        if (e.Line16 == "2D")
                        {
                            e.Line16 = string.Empty;

                            e.Receiving1095C = e.Offered;

                        }
                    }
                }

                PerfTimer.LapAndSwitchTimers("Remove 2D", "2D Overhead");
            }

            PerfTimer.LogAllLapsAndDispose("Check if 2D");
            PerfTimer.LogAllLapsAndDispose("Remove 2D");
            PerfTimer.LapLogAndDispose("2D Overhead", true);

            #endregion

            PerfTimer.LogTimeAndDispose("2D Logic Region", string.Empty, true);
            PerfTimer.StartTimer("All 12 Region", "getEmployeeMonthlyDetailDic");

            #region All 12 Logic

            PerfTimer.StartLapTimerPaused("Examine All 12", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimerPaused("Clear All 12", "getEmployeeMonthlyDetailDic");
            PerfTimer.StartLapTimer("All 12 Overhead", "getEmployeeMonthlyDetailDic");

            foreach (int EmployeeKey in filteredResult.Keys)
            {

                if ((false == filteredResult.ContainsKey(EmployeeKey))
                    ||
                    (filteredResult[EmployeeKey].Any(x => x.MonthId == 0) == true))
                {
                    continue;
                }

                Employee1095detailsPart2Model all12month = new Employee1095detailsPart2Model
                {
                    TaxYear = TaxYear,
                    MonthId = 0,      
                    EmployeeId = EmployeeKey,
                    AcaStatus = 0,
                    MonthlyHours = 0,
                    Line14 = null,
                    Line15 = null,
                    Line16 = null,
                    Receiving1095C = null,
                    InsuranceType = string.Empty,
                    Enrolled = false,
                    Offered = false
                };

                bool line14Same = true;
                bool line15Same = true;
                bool line16Same = true;
                bool receivingSame = true;
                bool userEditedAny = false;
                bool enrolledSame = true;
                bool offeredSame = true;
                bool insuranceTypeSame = true;
                string line14 = null;
                string line15 = null;
                string line16 = null;
                bool receiving = false;
                bool enrolled = false;
                bool offered = false;
                string insuranceType = null;
                int row = 0;

                PerfTimer.LapAndSwitchTimers("All 12 Overhead", "Examine All 12");

                #region Examine All 12 Rows
                foreach (Employee1095detailsPart2Model edpm in filteredResult[EmployeeKey])
                {
                    if (row == 0)
                    {
                        line14 = edpm.Line14;
                        line15 = edpm.Line15;
                        line16 = edpm.Line16;
                        receiving = (edpm.Receiving1095C ?? false);
                        enrolled = edpm.Enrolled;
                        offered = edpm.Offered;
                        insuranceType = edpm.InsuranceType;
                        userEditedAny = edpm.UserEdited;
                    }
                    else
                    {
                        if (line14 != edpm.Line14)
                        {
                            line14Same = false;
                        }
                        if (line15 != edpm.Line15)
                        {
                            line15Same = false;
                        }
                        if (line16 != edpm.Line16)
                        {
                            line16Same = false;
                        }
                        if (receiving != edpm.Receiving1095C)
                        {
                            receivingSame = false;
                        }
                        if (offered != edpm.Offered)
                        {
                            offeredSame = false;
                        }
                        if (enrolled != edpm.Enrolled)
                        {
                            enrolledSame = false;
                        }
                        if (insuranceType != edpm.InsuranceType)
                        {
                            insuranceTypeSame = false;
                        }
                        if (true == edpm.UserEdited)
                        {
                            userEditedAny = edpm.UserEdited;
                        }

                    }

                    row += 1;
                }

                PerfTimer.LapAndSwitchTimers("Examine All 12", "Clear All 12");

                #endregion

                #region Clear the Other 12 Rows
                if (line14Same && line15Same && line16Same && receivingSame)
                {
                    all12month.Line14 = line14;
                    all12month.Line15 = line15;
                    all12month.Line16 = line16;
                    all12month.Receiving1095C = receiving;
                    all12month.UserEdited = userEditedAny;

                    if (offeredSame)
                    {
                        all12month.Offered = offered;
                    }
                    if (enrolledSame)
                    {
                        all12month.Enrolled = enrolled;
                    }
                    if (insuranceTypeSame)
                    {
                        all12month.InsuranceType = insuranceType;
                    }

                    foreach (Employee1095detailsPart2Model clear in filteredResult[EmployeeKey])
                    {
                        clear.Line14 = string.Empty;
                        clear.Line15 = string.Empty;
                        clear.Line16 = string.Empty;
                        clear.Receiving1095C = false;

                        if (offeredSame)
                        {
                            clear.Offered = false;
                        }
                        if (enrolledSame)
                        {
                            clear.Enrolled = false;
                        }
                        if (insuranceTypeSame)
                        {
                            clear.InsuranceType = string.Empty; ;
                        }

                    }

                }

                PerfTimer.LapAndSwitchTimers("Clear All 12", "All 12 Overhead");

                #endregion

                filteredResult[EmployeeKey].Add(all12month);


            }

            PerfTimer.LogAllLapsAndDispose("Examine All 12");
            PerfTimer.LogAllLapsAndDispose("Clear All 12");
            PerfTimer.LapLogAndDispose("All 12 Overhead", true);

            #endregion

            PerfTimer.LogTimeAndDispose("All 12 Region", string.Empty, true);

            return filteredResult;

        }
        
        private static bool? GetMeasuredFT( int TaxYear, View_air_replacement_EmployeeYearlyDetails emp )
        {

            try
            {
                DateTime firstDayOfMonth = new DateTime(TaxYear, emp.month_id, 1);
                DateTime lastDayOfMonth = new DateTime(TaxYear, emp.month_id, DateTime.DaysInMonth(TaxYear, emp.month_id));

                double monthlyAvgHours = (double)(emp.MonthlyAverageHours ?? 0m);

                bool isRehire = (emp.terminationDate != null
                    && emp.terminationDate != new DateTime()
                    && emp.terminationDate < emp.hireDate);

                if (emp.hireDate > lastDayOfMonth 
                    && false == isRehire)
                {

                    return false;

                }

                if (emp.terminationDate < firstDayOfMonth
                    && false == isRehire)
                {

                    return false;

                }

                if(isRehire 
                        && 
                    ((emp.terminationDate < firstDayOfMonth) 
                        && 
                    (emp.hireDate > lastDayOfMonth)))
                { 

                    return false;

                }

                if ((firstDayOfMonth < emp.hireDate && lastDayOfMonth >= emp.hireDate))
                {

                    return false;

                }

                if (monthlyAvgHours >= 130)
                {

                    return true;

                }

                if (emp.InitialAdminEnd >= firstDayOfMonth && emp.aca_status_id == 5)
                {

                    return true;

                }

            }
            catch (Exception ex)
            {
                Log.Warn("Caught an exception in GetMeasuredFT.", ex);
                return null;
            }

            return false;

        }

        private static string GetMecCode(int TaxYear, View_air_replacement_EmployeeYearlyDetails emp, View_insurance_change_events_details currentOffer, Employee1095detailsPart2Model month)
        {

            try
            {

                DateTime lastDayOfMonth = new DateTime(TaxYear, emp.month_id, DateTime.DaysInMonth(TaxYear, emp.month_id));
                DateTime firstDayOfMonth = new DateTime(TaxYear, emp.month_id, 1);
                bool isNewHireThisMonth = false;
                double monthlyAvgHours = (double)(emp.MonthlyAverageHours ?? 0m);

                if (emp.hireDate <= lastDayOfMonth)
                {
                    if ((emp.terminationDate != null && emp.terminationDate != new DateTime()) 
                        && (emp.terminationDate < firstDayOfMonth) 
                        && ((emp.hireDate <= emp.terminationDate) || (emp.hireDate > lastDayOfMonth)))                
                    {

                        month.MonthlyHours = 0;
                        month.Offered = false;
                        month.Enrolled = false;

                        return "1H";

                    }

                    if (firstDayOfMonth < emp.hireDate && lastDayOfMonth >= emp.hireDate)
                    {
                        isNewHireThisMonth = true;
                    }

                    bool? isFullTime = GetMeasuredFT(TaxYear, emp);

                    if (true == isFullTime)
                    {
                        if ((currentOffer != null) && (currentOffer.offered == true))
                        {
                            return GetLine14CodeForOffer(emp, currentOffer, isNewHireThisMonth);
                        }
                        else
                        {
                            return "1H";
                        }
                    }
                    else     
                    {
                        if ((currentOffer != null) && (currentOffer.offered == true))  
                        {
                            return GetLine14CodeForOffer(emp, currentOffer, isNewHireThisMonth);
                        }
                        else
                        {
                            return "1H";
                        }
                    }
                }
                else
                {
                    return "1H";
                }
            }
            catch (Exception ex)
            {
                Log.Warn("Caught an exception in GetMecCode.", ex);
                return string.Empty;
            }
        }

        private static string GetLine14CodeForOffer(View_air_replacement_EmployeeYearlyDetails emp, View_insurance_change_events_details currentOffer, bool isNewHireThisMonth)
        {
            string returnValue = string.Empty;

            if (currentOffer.Mec == false)
            {
                returnValue = "1H";
            }
            else if (isNewHireThisMonth == true)
            {
                returnValue = "1H";
            }   
            else if (currentOffer.minValue == false)
            {
                returnValue = "1F";
            }
            else if (currentOffer.minValue == true && currentOffer.Mec == true && currentOffer.SpouseConditional == true && currentOffer.offSpouse == false && currentOffer.offDependent == false)
            {
                returnValue = "1J";
            }
            else if (currentOffer.minValue == true && currentOffer.Mec == true && currentOffer.SpouseConditional == true && currentOffer.offSpouse == false && currentOffer.offDependent == true)
            {
                returnValue = "1K";
            }
            else if (currentOffer.minValue == true && currentOffer.Mec == true && (currentOffer.SpouseConditional == false || currentOffer.SpouseConditional == null) && currentOffer.offSpouse == true && currentOffer.offDependent == true)            
            {
                if (emp.Ooc != null)
                {
                    if (emp.Ooc.ToUpper() == "1A" || emp.Ooc.ToUpper() == "1E")
                    {
                        returnValue = emp.Ooc.ToUpper();
                    }
                    else
                    {
                        returnValue = "1E";
                    }
                }
                else
                {
                    returnValue = "1E";
                }
            }
            else if (currentOffer.minValue == true && currentOffer.Mec == true && (currentOffer.SpouseConditional == false || currentOffer.SpouseConditional == null) && currentOffer.offSpouse == true && currentOffer.offDependent == false)
            {
                returnValue = "1D";
            }
            else if (currentOffer.minValue == true && currentOffer.Mec == true && (currentOffer.SpouseConditional == false || currentOffer.SpouseConditional == null) && currentOffer.offSpouse == false && currentOffer.offDependent == true)
            {
                returnValue = "1C";
            }
            else if (currentOffer.minValue == true && currentOffer.Mec == true && (currentOffer.SpouseConditional == false || currentOffer.SpouseConditional == null) && currentOffer.offSpouse == false && currentOffer.offDependent == false)
            {
                returnValue = "1B";
            }

            return returnValue;
        }

        private static string GetMonthlyCost(View_insurance_change_events_details currentOffer, Employee1095detailsPart2Model month)
        {

            try
            {
                decimal adjustedCost = 0m;

                if (month.Line14 != "1H" && month.Line14 != "1A" && month.Line14 != "1G")
                {
                    if (currentOffer != null)
                    {
                        decimal cost = currentOffer.monthlycost ?? 0;
                        decimal contribution = currentOffer.EmployerContribution ?? 0;
                        decimal hra = currentOffer.hra_flex_contribution ?? 0;

                        if (currentOffer.EmployerContributionType == "$")
                        {
                            adjustedCost = (cost - contribution) - hra;
                            return Math.Max(0m, adjustedCost).ToString("F2");
                        }
                        else if (currentOffer.EmployerContributionType == "%")
                        {
                            adjustedCost = (cost - (cost * contribution)) - hra;
                            return Math.Max(0m, adjustedCost).ToString("F2");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Warn(string.Format("Cought exception in GetMonthlyCost", ex));
            }
            return string.Empty;

        }

        private static string GetSafeHarborCode(int TaxYear, View_air_replacement_EmployeeYearlyDetails emp, Employee1095detailsPart2Model month)
        {
            string returnValue = string.Empty;

            try
            {
                DateTime firstDayOfMonth = new DateTime(TaxYear, emp.month_id, 1);
                DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
                DateTime lastDayOfMonth = new DateTime(TaxYear, emp.month_id, DateTime.DaysInMonth(TaxYear, emp.month_id));

                if (emp.hireDate > lastDayOfMonth
                    && 
                    (emp.terminationDate == null       
                    || emp.hireDate <= emp.terminationDate        
                    || emp.terminationDate < firstDayOfMonth))        
                {
                    returnValue = "2A";
                } 
                else
                {
                    if (emp.terminationDate != null
                            && emp.terminationDate < firstDayOfMonth
                            && emp.hireDate <= emp.terminationDate)
                    {
                        returnValue = "2A";
                    }
                    else        
                    {
                        switch (month.Line14.ToUpperInvariant())
                        {
                            case "1A":         
                                if (month.Enrolled == true)
                                {
                                    returnValue = "2C";
                                }
                                else
                                {
                                    returnValue = string.Empty;
                                }
                                break;
                            case "1B":         
                                if (month.Enrolled == true)
                                {
                                    returnValue = "2C";
                                }
                                else
                                {
                                    returnValue = string.Empty;
                                }
                                break;
                            case "1C":        
                                if (month.Enrolled == true) { returnValue = "2C"; }
                                else
                                {
                                    returnValue = getSafeHarborCodeSub(emp, firstDayOfMonth);
                                }
                                break;
                            case "1D":
                                if (month.Enrolled == true) { returnValue = "2C"; }
                                else
                                {
                                    returnValue = getSafeHarborCodeSub(emp, firstDayOfMonth);
                                }
                                break;
                            case "1E":
                                if (month.Enrolled == true) { returnValue = "2C"; }
                                else
                                {
                                    returnValue = getSafeHarborCodeSub(emp, firstDayOfMonth);
                                }
                                break;
                            case "1F":
                                if (month.Enrolled == true) { returnValue = "2C"; }
                                else
                                {
                                    returnValue = getSafeHarborCodeSub(emp, firstDayOfMonth);
                                }
                                break;
                            case "1J":
                                if (month.Enrolled == true) { returnValue = "2C"; }
                                else
                                {
                                    returnValue = getSafeHarborCodeSub(emp, firstDayOfMonth);
                                }
                                break;
                            case "1K":
                                if (month.Enrolled == true) { returnValue = "2C"; }
                                else
                                {
                                    returnValue = getSafeHarborCodeSub(emp, firstDayOfMonth);
                                }
                                break;
                            case "1H":
                                if (emp.MonthlyAverageHours >= 130 
                                    || ((emp.InitialAdminEnd >= firstDayOfMonth && emp.aca_status_id == 5)))
                                {      

                                    DateTime waitingPeriodEnd = new DateTime();
                                    switch (emp.WaitingPeriodID)
                                    {
                                        case 1:
                                            waitingPeriodEnd = emp.hireDate;
                                            break;
                                        case 2:
                                            waitingPeriodEnd = new DateTime(emp.hireDate.Year, emp.hireDate.Month, 1).AddMonths(1);
                                            break;
                                        case 3:
                                            DateTime temp30 = emp.hireDate.AddDays(30);
                                            waitingPeriodEnd = new DateTime(temp30.Year, temp30.Month, 1).AddMonths(1);
                                            break;
                                        case 4:
                                            DateTime temp60 = emp.hireDate.AddDays(60);
                                            waitingPeriodEnd = new DateTime(temp60.Year, temp60.Month, 1).AddMonths(1);
                                            break;
                                        case 5:
                                            waitingPeriodEnd = emp.hireDate.AddDays(90);
                                            break;
                                        default:
                                            DateTime temp60d = emp.hireDate.AddDays(60);
                                            waitingPeriodEnd = new DateTime(temp60d.Year, temp60d.Month, 1).AddMonths(1);
                                            break;
                                    }

                                    if (waitingPeriodEnd > firstDayOfMonth
                                        && emp.hireDate <= lastDayOfMonth)
                                    {
                                        returnValue = "2D";
                                    }
                                    else
                                    {
                                        if (emp.ash_code != null)
                                        {
                                            if (emp.ash_code.ToUpper() == "2E")
                                            {
                                                returnValue = "2E";
                                            }
                                        }
                                        else
                                        {
                                            returnValue = string.Empty;
                                        }
                                    }
                                }
                                else
                                {
                                    if (emp.InitialAdminEnd > firstDayOfMonth
                                        && emp.hireDate <= lastDayOfMonth)
                                    {
                                        returnValue = "2D";
                                    }
                                    else
                                    {
                                        returnValue = "2B";
                                    }
                                }
                                break;
                            case "1G":
                                break;
                            default:
                                Log.Info("Unexpected default case: " + month.Line14);
                                break;
                        }

                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                Log.Warn(string.Format("GetSafeHarborCode caught Exception", ex));
            }

            return returnValue;
        }

        private static string getSafeHarborCodeSub(View_air_replacement_EmployeeYearlyDetails emp, DateTime firstDayOfMonth)
        {
            if (emp.MonthlyAverageHours >= 130 || (emp.hireDate <= firstDayOfMonth && emp.InitialStabilityPeriodEndDate >= firstDayOfMonth && emp.aca_status_id == 5))
            {
                if (emp.ash_code.IsNullOrEmpty())
                {
                    return string.Empty;
                }
                else
                {
                    return emp.ash_code.ToUpper();
                }
            }
            else
            {
                return "2B";
            }
        }

        #region Part III

        public static Dictionary<int, List<Employee1095detailsPart3Model>> getAllEmployerInsuranceCoverage(int employerId, int TaxYear)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(_1095MonthlyDetails), "getAllEmployerInsuranceCoverage", SystemSettings.UsePerformanceLog))
            {
                using (AcaEntities ctx = new AcaEntities())
                {
                    ctx.Database.CommandTimeout = 180;
                    return _1095MonthlyDetails.getAllEmployerInsuranceCoverage(ctx, employerId, TaxYear);
                }
            }
        }

        private static bool[] getEnrolments(View_full_insurance_coverage ic)
        {

            bool[] Enrolled = new bool[12];

            Enrolled[0] = (ic.jan ?? 0) != 0 ? true : false;
            Enrolled[1] = (ic.feb ?? 0) != 0 ? true : false;
            Enrolled[2] = (ic.mar ?? 0) != 0 ? true : false;
            Enrolled[3] = (ic.apr ?? 0) != 0 ? true : false;
            Enrolled[4] = (ic.may ?? 0) != 0 ? true : false;
            Enrolled[5] = (ic.jun ?? 0) != 0 ? true : false;
            Enrolled[6] = (ic.jul ?? 0) != 0 ? true : false;
            Enrolled[7] = (ic.aug ?? 0) != 0 ? true : false;
            Enrolled[8] = (ic.sep ?? 0) != 0 ? true : false;
            Enrolled[9] = (ic.oct ?? 0) != 0 ? true : false;
            Enrolled[10] = (ic.nov ?? 0) != 0 ? true : false;
            Enrolled[11] = (ic.dec ?? 0) != 0 ? true : false;

            return Enrolled;
        }

        private static bool[] getEnrolments(insurance_coverage ic)
        {
            bool[] Enrolled = new bool[12];
            Enrolled[0] = ic.jan;
            Enrolled[1] = ic.feb;
            Enrolled[2] = ic.mar;
            Enrolled[3] = ic.apr;
            Enrolled[4] = ic.may;
            Enrolled[5] = ic.jun;
            Enrolled[6] = ic.jul;
            Enrolled[7] = ic.aug;
            Enrolled[8] = ic.sep;
            Enrolled[9] = ic.oct;
            Enrolled[10] = ic.nov;
            Enrolled[11] = ic.dec;

            return Enrolled;
        }

        private static bool[] getEnrolments(insurance_coverage_editable ice)
        {

            bool[] Enrolled = new bool[12];

            Enrolled[0] = ice.Jan;
            Enrolled[1] = ice.Feb;
            Enrolled[2] = ice.Mar;
            Enrolled[3] = ice.Apr;
            Enrolled[4] = ice.May;
            Enrolled[5] = ice.Jun;
            Enrolled[6] = ice.Jul;
            Enrolled[7] = ice.Aug;
            Enrolled[8] = ice.Sept;
            Enrolled[9] = ice.Oct;
            Enrolled[10] = ice.Nov;
            Enrolled[11] = ice.Dec;

            return Enrolled;

        }

        private static Dictionary<int, List<Employee1095detailsPart3Model>> getAllEmployerInsuranceCoverage(AcaEntities ctx, int employerId, int TaxYear)
        {

            Dictionary<int, List<Employee1095detailsPart3Model>> DicPart3IC = new Dictionary<int, List<Employee1095detailsPart3Model>>();

            Log.Debug("Begin Building Part 3 data.");

            PerfTimer.StartTimer("Query ctx.insurance_coverage_editable", "getAllEmployerInsuranceCoverage");

            List<insurance_coverage_editable> EditedInsuranceCoverage = ctx.insurance_coverage_editable.Include("employee").Include("employee_dependents").AsNoTracking()
                .Where(ICE =>
                    ICE.employer_id == employerId
                    && ICE.tax_year == TaxYear
                    && ICE.EntityStatusID == (int)EntityStatusEnum.Active)
                    .OrderByDescending(ice => ice.ModifiedDate)
                .ToList();

            PerfTimer.LogTimePerItemAndDispose("Query ctx.insurance_coverage_editable", EditedInsuranceCoverage.Count());
            PerfTimer.StartTimer("Query ctx.View_full_insurance_coverage", "getAllEmployerInsuranceCoverage");

            List<View_full_insurance_coverage> fullInsuranceCoverage = (from part3 in ctx.View_full_insurance_coverage.AsNoTracking()
                                                                        where part3.tax_year == TaxYear
                                                                           && part3.employer_id == employerId
                                                                        orderby part3.dependent_id
                                                                        select part3).ToList();

            PerfTimer.LogTimePerItemAndDispose("Query ctx.View_full_insurance_coverage", fullInsuranceCoverage.Count());
            PerfTimer.StartTimer("Build Insurance Dictionary from Editable", "getAllEmployerInsuranceCoverage");

            foreach (insurance_coverage_editable ice in EditedInsuranceCoverage.OrderBy(item => item.employee_id))
            {
                Employee1095detailsPart3Model EmpOrDefIC = new Employee1095detailsPart3Model();

                if (false == DicPart3IC.ContainsKey(ice.employee_id))
                {
                    DicPart3IC.Add(ice.employee_id, new List<Employee1095detailsPart3Model>());
                }

                if (string.IsNullOrEmpty(ice.dependent_id?.ToString()))
                {
                    EmpOrDefIC.FirstName = ice.employee.fName;
                    EmpOrDefIC.LastName = ice.employee.lName;
                    EmpOrDefIC.MiddleName = ice.employee.mName;
                    EmpOrDefIC.Dob = ice.employee.dob;
                    EmpOrDefIC.SsnHidden = AesEncryption.Decrypt(ice.employee.ssn).Masked_SSN();
                    EmpOrDefIC.Enrolled = getEnrolments(ice);
                    EmpOrDefIC.EmployeeID = ice.employee_id;

                }
                else
                {
                    EmpOrDefIC.FirstName = ice.employee_dependents.fName;
                    EmpOrDefIC.LastName = ice.employee_dependents.lName;
                    EmpOrDefIC.MiddleName = ice.employee_dependents.mName;
                    EmpOrDefIC.Dob = ice.employee_dependents.dob;
                    EmpOrDefIC.SsnHidden = AesEncryption.Decrypt(ice.employee_dependents.ssn);
                    EmpOrDefIC.Enrolled = getEnrolments(ice);
                    EmpOrDefIC.EmployeeID = ice.employee_dependents.employee_id;
                    EmpOrDefIC.DependantID = (int)ice.employee_dependents.dependent_id;

                }

                EmpOrDefIC.TaxYear = TaxYear;

                DicPart3IC[ice.employee_id].Add(EmpOrDefIC);

                fullInsuranceCoverage.RemoveAll(coverage =>
                    coverage.employee_id == ice.employee_id
                    && coverage.dependent_id == ice.dependent_id
                    && coverage.tax_year == ice.tax_year);

            }

            PerfTimer.LogTimePerItemAndDispose("Build Insurance Dictionary from Editable", EditedInsuranceCoverage.Count());
            PerfTimer.StartTimer("Build Insurance Dictionary from Imported", "getAllEmployerInsuranceCoverage");

            foreach (View_full_insurance_coverage ic in fullInsuranceCoverage.OrderBy(item => item.employee_id))
            {

                Employee1095detailsPart3Model EmpOrDefIC = new Employee1095detailsPart3Model();

                if (false == DicPart3IC.ContainsKey(ic.employee_id))
                {
                    DicPart3IC.Add(ic.employee_id, new List<Employee1095detailsPart3Model>());
                }

                if (string.IsNullOrEmpty(ic.dependent_id.ToString()))
                {
                    EmpOrDefIC.FirstName = ic.employeeFName;
                    EmpOrDefIC.LastName = ic.employeeLName;
                    EmpOrDefIC.MiddleName = ic.employeeMName;
                    EmpOrDefIC.Dob = ic.employeeDob;
                    EmpOrDefIC.SsnHidden = AesEncryption.Decrypt(ic.employeeSsn).Masked_SSN();
                    EmpOrDefIC.Enrolled = getEnrolments(ic);
                    EmpOrDefIC.EmployeeID = ic.employee_id;
                }
                else
                {
                    EmpOrDefIC.FirstName = ic.dependentFName;
                    EmpOrDefIC.LastName = ic.dependentLName;
                    EmpOrDefIC.MiddleName = ic.dependentMName;
                    EmpOrDefIC.Dob = ic.dependentDob;
                    EmpOrDefIC.SsnHidden = AesEncryption.Decrypt(ic.dependentSsn);
                    EmpOrDefIC.Enrolled = getEnrolments(ic);
                    EmpOrDefIC.EmployeeID = ic.employee_id;
                    EmpOrDefIC.DependantID = (int)ic.dependent_id;
                }

                EmpOrDefIC.TaxYear = TaxYear;

                DicPart3IC[ic.employee_id].Add(EmpOrDefIC);

            }

            PerfTimer.LogTimePerItemAndDispose("Build Insurance Dictionary from Imported", fullInsuranceCoverage.Count());

            return DicPart3IC;
        }

        public static List<Employee1095detailsPart3Model> getEmployeeInsuranceCoverage(int employeeID, int TaxYear)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(_1095MonthlyDetails), "getEmployeeInsuranceCoverage", SystemSettings.UsePerformanceLog))
            {
                using (AcaEntities ctx = new AcaEntities())
                {
                    ctx.Database.CommandTimeout = 180;
                    return _1095MonthlyDetails.getEmployeeInsuranceCoverage(employeeID, TaxYear, ctx);
                }
            }
        }

        private static List<Employee1095detailsPart3Model> getEmployeeInsuranceCoverage(int employeeID, int TaxYear, AcaEntities ctx)
        {

            List<Employee1095detailsPart3Model> Part3IC = new List<Employee1095detailsPart3Model>();

            ctx.Database.CommandTimeout = 180;
            PerfTimer.StartTimer("Query ctx.insurance_coverage_editable", "getEmployeeInsuranceCoverage");

            List<insurance_coverage_editable> EditedInsuranceCoverage = ctx.insurance_coverage_editable.Include("employee").Include("employee_dependents").AsNoTracking()
                .Where(ICE =>
                    ICE.employee_id == employeeID
                    && ICE.EntityStatusID == (int)EntityStatusEnum.Active
                    && ICE.tax_year == TaxYear)
                .ToList();

            PerfTimer.LogTimePerItemAndDispose("Query ctx.insurance_coverage_editable", EditedInsuranceCoverage.Count());
            PerfTimer.StartTimer("Query ctx.View_full_insurance_coverage", "getEmployeeInsuranceCoverage");

            List<View_full_insurance_coverage> fullInsuranceCoverage = (from part3 in ctx.View_full_insurance_coverage.AsNoTracking()
                                                                        where part3.tax_year == TaxYear
                                                                           && part3.employee_id == employeeID
                                                                        orderby part3.dependent_id
                                                                        select part3).ToList();

            PerfTimer.LogTimePerItemAndDispose("Query ctx.View_full_insurance_coverage", fullInsuranceCoverage.Count());
            PerfTimer.StartTimer("Build Insurance Dictionary from Editable", "getEmployeeInsuranceCoverage");

            foreach (insurance_coverage_editable ice in EditedInsuranceCoverage.OrderBy(item => item.employee_id))
            {
                Employee1095detailsPart3Model EmpOrDefIC = new Employee1095detailsPart3Model();

                if (string.IsNullOrEmpty(ice.dependent_id?.ToString()))
                {
                    EmpOrDefIC.FirstName = ice.employee.fName;
                    EmpOrDefIC.LastName = ice.employee.lName;
                    EmpOrDefIC.MiddleName = ice.employee.mName;
                    EmpOrDefIC.Dob = ice.employee.dob;
                    EmpOrDefIC.SsnHidden = AesEncryption.Decrypt(ice.employee.ssn).Masked_SSN();
                    EmpOrDefIC.Enrolled = getEnrolments(ice);
                    EmpOrDefIC.EmployeeID = ice.employee_id;

                }
                else
                {
                    EmpOrDefIC.FirstName = ice.employee_dependents.fName;
                    EmpOrDefIC.LastName = ice.employee_dependents.lName;
                    EmpOrDefIC.MiddleName = ice.employee_dependents.mName;
                    EmpOrDefIC.Dob = ice.employee_dependents.dob;
                    EmpOrDefIC.SsnHidden = AesEncryption.Decrypt(ice.employee_dependents.ssn);
                    EmpOrDefIC.Enrolled = getEnrolments(ice);
                    EmpOrDefIC.EmployeeID = ice.employee_dependents.employee_id;
                    EmpOrDefIC.DependantID = (int)ice.employee_dependents.dependent_id;

                }

                EmpOrDefIC.TaxYear = TaxYear;

                Part3IC.Add(EmpOrDefIC);

                fullInsuranceCoverage.RemoveAll(coverage =>
                    coverage.employee_id == ice.employee_id
                    && coverage.dependent_id == ice.dependent_id
                    && coverage.tax_year == ice.tax_year);

            }

            PerfTimer.LogTimePerItemAndDispose("Build Insurance Dictionary from Editable", EditedInsuranceCoverage.Count());
            PerfTimer.StartTimer("Build Insurance Dictionary from Imported", "getEmployeeInsuranceCoverage");

            foreach (View_full_insurance_coverage ic in fullInsuranceCoverage)
            {

                Employee1095detailsPart3Model EmpOrDefIC = new Employee1095detailsPart3Model();
                if (string.IsNullOrEmpty(ic.dependent_id.ToString()))
                {
                    employee empee = ctx.employees.AsNoTracking().Where(ee => ee.employee_id == employeeID).Single();
                    EmpOrDefIC.FirstName = empee.fName;
                    EmpOrDefIC.LastName = empee.lName;
                    EmpOrDefIC.MiddleName = empee.mName;
                    EmpOrDefIC.Dob = empee.dob;
                    EmpOrDefIC.SsnHidden = empee.SSN_hidden;
                    EmpOrDefIC.Enrolled = getEnrolments(ic);
                    EmpOrDefIC.EmployeeID = empee.employee_id;
                }
                else
                {
                    employee_dependents dependent = ctx.employee_dependents.AsNoTracking().Where(ee => ee.dependent_id == ic.dependent_id).Single();
                    EmpOrDefIC.FirstName = dependent.fName;
                    EmpOrDefIC.LastName = dependent.lName;
                    EmpOrDefIC.MiddleName = dependent.mName;
                    EmpOrDefIC.Dob = dependent.dob;
                    EmpOrDefIC.SsnHidden = AesEncryption.Decrypt(dependent.ssn);
                    EmpOrDefIC.Enrolled = getEnrolments(ic);
                    EmpOrDefIC.DependantID = dependent.dependent_id;
                    EmpOrDefIC.EmployeeID = dependent.employee_id;
                }

                EmpOrDefIC.TaxYear = TaxYear;

                Part3IC.Add(EmpOrDefIC);
            }

            PerfTimer.LogTimePerItemAndDispose("Build Insurance Dictionary from Imported", fullInsuranceCoverage.Count());

            return Part3IC;
        }
        private static Employee1095detailsPart3Model ConvertViewAirReplacementPartIIIDetailsToEmployee1095detailsPart3Model(View_air_replacement_PartIII_details VARP3, int taxYear, int employeeID, AcaEntities ctx)
        {
            Employee1095detailsPart3Model EmpOrDefIC = new Employee1095detailsPart3Model
            {
                TaxYear = taxYear,
                Enrolled = getEnrolmentsfromVARP3DB(VARP3)
            };
            if (string.IsNullOrEmpty(VARP3.dependent_id.ToString()))
            {
                employee empee = ctx.employees.AsNoTracking().Where(ee => ee.employee_id == employeeID).Single();
                EmpOrDefIC.FirstName = empee.fName;
                EmpOrDefIC.LastName = empee.lName;
                EmpOrDefIC.MiddleName = empee.mName;
                EmpOrDefIC.Dob = empee.dob.Value.Date;
                EmpOrDefIC.SsnHidden = empee.SSN_hidden;
                EmpOrDefIC.SSN = empee.SSN;
                EmpOrDefIC.EmployeeID = empee.employee_id;

            }
            else
            {
                employee_dependents dependent = ctx.employee_dependents.AsNoTracking().Where(ee => ee.dependent_id == VARP3.dependent_id).Single();
                EmpOrDefIC.FirstName = dependent.fName;
                EmpOrDefIC.LastName = dependent.lName;
                EmpOrDefIC.MiddleName = dependent.mName;
                EmpOrDefIC.Dob = dependent.dob;
                EmpOrDefIC.SsnHidden = string.IsNullOrEmpty(dependent.ssn) ? "" : AfComply.Domain.AesEncryption.Decrypt(dependent.ssn);
                EmpOrDefIC.SSN = string.IsNullOrEmpty(dependent.ssn) ? "" : AfComply.Domain.AesEncryption.Decrypt(dependent.ssn);
                EmpOrDefIC.DependantID = dependent.dependent_id;
                EmpOrDefIC.EmployeeID = employeeID;


            }

            EmpOrDefIC.TaxYear = VARP3.tax_year;
            return EmpOrDefIC;
        }

        private static List<insurance_coverage> getEmployeeInsuranceCoverageFromICTable(int employee_id, int Taxyear)
        {
            using (AcaEntities ctx = new AcaEntities())
            {
                ctx.Database.CommandTimeout = 180;
                return _1095MonthlyDetails.getEmployeeInsuranceCoverageFromICTable(employee_id, Taxyear, ctx);
            }
        }

        private static List<insurance_coverage> getEmployeeInsuranceCoverageFromICTable(int employee_id, int Taxyear, AcaEntities ctx)
        {
            return ctx.insurance_coverage.Where(ic => ic.employee_id == employee_id && ic.tax_year == Taxyear && ic.EntityStatusID == (int)EntityStatusEnum.Active).ToList();
        }

        public static void UpdatePart3InsuranceCoverage(Employee1095detailsPart3Model Part3IC, string user)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(_1095MonthlyDetails), "UpdatePart3InsuranceCoverage", SystemSettings.UsePerformanceLog))
            {
                using (AcaEntities ctx = new AcaEntities())
                {
                    using (System.Data.Entity.DbContextTransaction scope = ctx.Database.BeginTransaction())
                    {
                        ctx.Database.CommandTimeout = 180;
                        _1095MonthlyDetails.UpdatePart3InsuranceCoverage(Part3IC, user, ctx);
                        scope.Commit();
                    }
                }
            }
        }

        private static void UpdatePart3InsuranceCoverage(Employee1095detailsPart3Model Part3IC, string user, AcaEntities ctx)
        {
            try
            {
                Log.Debug("Updating Part3 in 1095MonthlyDetails:");
                Log.Debug(string.Format("Part3 Update:EmployeeID: {0},ResourceID: {1}, DependantID: {2}, Enrolments: ",
                                         Part3IC.EmployeeID,
                                         Part3IC.ResourceId,
                                         Part3IC.DependantID
                                     )
                             );
                List<insurance_coverage> icl = getInsuranceCoverage(Part3IC, ctx);

                List<insurance_coverage_editable> icel = getInsuranceCoverageEditable(Part3IC, ctx);
                Log.Debug("Updating Part3 in 1095MonthlyDetails: records Insurance Coverage count is" + icl.Count);
                Log.Debug("Updating Part3 in 1095MonthlyDetails: records Insurance Coverage Editable count is" + icel.Count);

                employee employee = ctx.employees.Where(empee => empee.employee_id == Part3IC.EmployeeID).SingleOrDefault();

                if (icel.Count == 0)
                {
                    insurance_coverage_editable newICE = new insurance_coverage_editable
                    {
                        ResourceId = Guid.NewGuid(),
                        employee_id = Part3IC.EmployeeID
                    };
                    if (Part3IC.DependantID != 0)
                    { newICE.dependent_id = Part3IC.DependantID; }
                    newICE.ModifiedBy = user;
                    newICE.tax_year = Part3IC.TaxYear;
                    newICE.ModifiedDate = DateTime.Now;
                    newICE.employer_id = employee.employer_id;
                    newICE.CreatedBy = user;
                    newICE.CreatedDate = DateTime.Now;
                    newICE.EntityStatusID = (int)EntityStatusEnum.Active;
                    setEnrolmentsToDB(newICE, Part3IC.Enrolled);
                    ctx.insurance_coverage_editable.Add(newICE);

                }
                else
                {        
                    foreach (insurance_coverage_editable ice in icel)
                    {
                        setEnrolmentsToDB(ice, Part3IC.Enrolled);
                        ctx.insurance_coverage_editable.Attach(ice);
                        ctx.Entry(ice).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                if (Part3IC.DependantID != 0)
                {
                    int depID = Part3IC.DependantID;
                    employee_dependents dependent = ctx.employee_dependents.Where(ee => ee.dependent_id == depID && ee.EntityStatusID == (int)EntityStatusEnum.Active).Single();
                    dependent.fName = Part3IC.FirstName;
                    dependent.lName = Part3IC.LastName;
                    dependent.mName = Part3IC.MiddleName;
                    dependent.ssn = AfComply.Domain.AesEncryption.Encrypt(string.IsNullOrEmpty(Part3IC.SSN) ? Part3IC.SsnHidden : Part3IC.SSN);

                    dependent.dob = Part3IC.Dob;
                    dependent.ModifiedBy = user;
                    dependent.ModifiedDate = DateTime.Now;
                    dependent.employee_id = Part3IC.EmployeeID;
                    ctx.employee_dependents.Attach(dependent);
                    ctx.Entry(dependent).State = System.Data.Entity.EntityState.Modified;
                }

                foreach (insurance_coverage_editable ice in icel)
                {
                    setEnrolmentsToDB(ice, Part3IC.Enrolled);
                    ctx.Entry(ice).State = System.Data.Entity.EntityState.Modified;
                }

                SaveContext(ctx);
                Log.Info("Updated Part3");

            }
            catch (Exception ex)
            {
                Log.Error("Errors during updating Part3 Insrance Coverage in 1095MD ", ex);
            }
        }

        public static void DeletePart3InsuranceCoverage(Employee1095detailsPart3Model Part3IC, string user)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(_1095MonthlyDetails), "DeletePart3InsuranceCoverage", SystemSettings.UsePerformanceLog))
            {
                using (AcaEntities ctx = new AcaEntities())
                {
                    ctx.Database.CommandTimeout = 180;
                    _1095MonthlyDetails.DeletePart3InsuranceCoverage(Part3IC, user, ctx);
                }
            }
        }

        private static void DeletePart3InsuranceCoverage(Employee1095detailsPart3Model Part3IC, string user, AcaEntities ctx)
        {

            List<insurance_coverage> icl = getInsuranceCoverage(Part3IC, ctx);
            List<insurance_coverage_editable> icel = getInsuranceCoverageEditable(Part3IC, ctx);
            Log.Info(string.Format("Part3 Deleting from 1095MD:EmployeeID: {0},ResourceID: {1}, TaxYear{2}, DependantID: {3}",
                                          Part3IC.EmployeeID,
                                          Part3IC.ResourceId,
                                          Part3IC.TaxYear,
                                          Part3IC.DependantID
                                      )
                              );
            if (Part3IC.DependantID == 0)
            {
                employee empee = ctx.employees.Where(ee => ee.employee_id == Part3IC.EmployeeID).Single();
                List<insurance_coverage> employeeDependents = ctx.insurance_coverage.AsNoTracking().Where(IC => IC.employee_id == Part3IC.EmployeeID && IC.tax_year == Part3IC.TaxYear && IC.EntityStatusID == (int)EntityStatusEnum.Active).ToList();
                if (employeeDependents.Count > 1)
                {
                }
                else
                {
                    InactiveAllRecords(icl, icel, ctx, user);
                }
            }
            else
            {
                List<employee_dependents> Dependents = ctx.employee_dependents.AsNoTracking().Where(D => D.dependent_id == Part3IC.DependantID && D.EntityStatusID == (int)EntityStatusEnum.Active).ToList();
                foreach (employee_dependents ED in Dependents)
                {
                    InactiveDependant(ED, ctx, user);
                }

                InactiveAllRecords(icl, icel, ctx, user);
            }

            ctx.SaveChanges();

        }

        private static void InactiveAllRecords(List<insurance_coverage> icl, List<insurance_coverage_editable> icel, AcaEntities ctx, string user)
        {
            foreach (insurance_coverage ic in icl)
            {
                InactiveRecordInInsuranceCoverage(ic, ctx, user);
            }

            if (icel.Count != 0)
            {
                foreach (insurance_coverage_editable ice in icel)
                {
                    InactiveRecordInInsuranceCoverageEditable(ice, ctx, user);
                }
            }
        }

        private static void InactiveDependant(employee_dependents ED, AcaEntities ctx, string user)
        {
            ED.EntityStatusID = (int)EntityStatusEnum.Inactive;
            ED.ModifiedDate = DateTime.Now;
            ED.ModifiedBy = user;
            ctx.employee_dependents.Attach(ED);
            ctx.Entry(ED).State = System.Data.Entity.EntityState.Modified;
        }

        private static void InactiveRecordInInsuranceCoverage(insurance_coverage ic, AcaEntities ctx, string user)
        {
            ic.EntityStatusID = (int)EntityStatusEnum.Inactive;
            ic.ModifiedDate = DateTime.Now;
            ic.ModifiedBy = user;
            ctx.insurance_coverage.Attach(ic);
            ctx.Entry(ic).State = System.Data.Entity.EntityState.Modified;
        }

        private static void InactiveRecordInInsuranceCoverageEditable(insurance_coverage_editable ice, AcaEntities ctx, string user)
        {
            ice.EntityStatusID = (int)EntityStatusEnum.Inactive;
            ice.ModifiedDate = DateTime.Now;
            ice.ModifiedBy = user;
            ctx.insurance_coverage_editable.Attach(ice);
            ctx.Entry(ice).State = System.Data.Entity.EntityState.Modified;
        }

        private static Employee1095detailsPart3Model ConvertInsuranceCoverageToEmployee1095detailsPart3Model(insurance_coverage ic, int TaxYear, int employeeID, AcaEntities ctx)
        {
            Employee1095detailsPart3Model EmpOrDefIC = new Employee1095detailsPart3Model
            {
                InsuranceCoverageRowID = ic.row_id,
                TaxYear = TaxYear,
                Enrolled = getEnrolmentsfromDB(ic)
            };
            if (string.IsNullOrEmpty(ic.dependent_id.ToString()))
            {

                employee empee = ctx.employees.AsNoTracking().Where(ee => ee.employee_id == employeeID).Single();
                EmpOrDefIC.FirstName = empee.fName;
                EmpOrDefIC.LastName = empee.lName;
                EmpOrDefIC.MiddleName = empee.mName;
                EmpOrDefIC.Dob = empee.dob.Value.Date;
                EmpOrDefIC.SsnHidden = empee.SSN_hidden;
                EmpOrDefIC.SSN = empee.SSN;
                EmpOrDefIC.EmployeeID = empee.employee_id;
                EmpOrDefIC.ResourceId = ic.ResourceId;

            }
            else
            {

                employee_dependents dependent = ctx.employee_dependents.AsNoTracking().Where(ee => ee.dependent_id == ic.dependent_id).Single();
                EmpOrDefIC.FirstName = dependent.fName;
                EmpOrDefIC.LastName = dependent.lName;
                EmpOrDefIC.MiddleName = dependent.mName;
                EmpOrDefIC.Dob = dependent.dob;
                EmpOrDefIC.SsnHidden = AesEncryption.Decrypt(dependent.ssn);
                EmpOrDefIC.DependantID = dependent.dependent_id;
                EmpOrDefIC.EmployeeID = employeeID;
                EmpOrDefIC.ResourceId = ic.ResourceId;

            }

            EmpOrDefIC.TaxYear = TaxYear;
            return EmpOrDefIC;
        }

        public static Employee1095detailsPart3Model AddPart3InsuranceCoverage(Employee1095detailsPart3Model Part3IC, Guid EmployeeResourceId, string user)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(_1095MonthlyDetails), "AddPart3InsuranceCoverage", SystemSettings.UsePerformanceLog))
            {
                using (AcaEntities ctx = new AcaEntities())
                {
                    ctx.Database.CommandTimeout = 180;
                    return _1095MonthlyDetails.AddPart3InsuranceCoverage(Part3IC, EmployeeResourceId, user, ctx);
                }
            }
        }

        private static Employee1095detailsPart3Model AddPart3InsuranceCoverage(Employee1095detailsPart3Model Part3IC, Guid EmployeeResourceId, string user, AcaEntities ctx)
        {

            try
            {

                employee employee = ctx.employees.Where(empee => empee.ResourceId == EmployeeResourceId).Single();
                List<insurance_coverage> icl = getEmployeeInsuranceCoverageFromICTable(employee.employee_id, Part3IC.TaxYear);
                insurance_coverage ic = new insurance_coverage
                {
                    dependent_id = null
                };
                if (icl.Count >= 1)
                {
                    ic.dependent_id = SaveDapendant(Part3IC, employee.employee_id, user);

                }

                ic.employee_id = employee.employee_id;
                ic.tax_year = Part3IC.TaxYear;
                ic.EntityStatusID = (int)EntityStatusEnum.Active; ;
                ic.ModifiedBy = user;
                ic.CreatedBy = user;
                ic.ModifiedDate = DateTime.Now;
                ic.CreatedDate = DateTime.Now;
                ic.carrier_id = 13;
                ic.ResourceId = Guid.NewGuid();
                setEnrolmentsToDB(ic, Part3IC.Enrolled);
                ctx.insurance_coverage.Add(ic);
                ctx.SaveChanges();

                return ConvertInsuranceCoverageToEmployee1095detailsPart3Model(ic, Part3IC.TaxYear, employee.employee_id, ctx);

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                StringBuilder message = new StringBuilder();
                foreach (System.Data.Entity.Validation.DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (System.Data.Entity.Validation.DbValidationError validationError in validationErrors.ValidationErrors)
                    {
                        message.Append(string.Format("{0}:{1}",
                           validationErrors.Entry.Entity.ToString(),
                           validationError.ErrorMessage));
                    }
                    raise = new InvalidOperationException(message.ToString(), raise);
                }
                throw raise;
            }
        }

        private static Guid GetGuide(Guid ICEresourceId, Guid ICresourceId)

        {
            if (ICEresourceId.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                return ICresourceId;
            }
            else
            {
                return ICEresourceId;
            }
        }

        private static int SaveDapendant(Employee1095detailsPart3Model Part3IC, int employeeID, string user)
        {
            using (AcaEntities ctx = new AcaEntities())
            {
                ctx.Database.CommandTimeout = 180;
                return SaveDapendant(Part3IC, employeeID, user, ctx);
            }
        }

        private static int SaveDapendant(Employee1095detailsPart3Model Part3IC, int employeeID, string user, AcaEntities ctx)
        {

            try
            {

                employee_dependents Dependant = new employee_dependents
                {
                    fName = Part3IC.FirstName,
                    lName = Part3IC.LastName,
                    mName = Part3IC.MiddleName,
                    ssn = string.IsNullOrEmpty(Part3IC.SSN) ? "" : AfComply.Domain.AesEncryption.Encrypt(Part3IC.SSN),
                    dob = Part3IC.Dob,
                    employee_id = employeeID,
                    EntityStatusID = (int)EntityStatusEnum.Active,
                    CreatedBy = user,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = user,
                    ModifiedDate = DateTime.Now,
                    ResourceId = Guid.NewGuid()
                };
                ctx.employee_dependents.Add(Dependant);
                ctx.SaveChanges();
                return Dependant.dependent_id;

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (System.Data.Entity.Validation.DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (System.Data.Entity.Validation.DbValidationError validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

        }

        private static List<insurance_coverage_editable> getInsuranceCoverageEditable(Employee1095detailsPart3Model Part3IC, AcaEntities ctx)
        {
            List<insurance_coverage_editable> ice = new List<insurance_coverage_editable>();
            if (Part3IC.DependantID == 0)
            {

                ice = ctx.insurance_coverage_editable.AsNoTracking().Where(ICE => ICE.employee_id == Part3IC.EmployeeID && ICE.EntityStatusID == (int)EntityStatusEnum.Active && ICE.dependent_id == null && ICE.tax_year == Part3IC.TaxYear).ToList();
            }
            else
            {

                ice = ctx.insurance_coverage_editable.AsNoTracking().Where(ICE => ICE.employee_id == Part3IC.EmployeeID && ICE.dependent_id == Part3IC.DependantID && ICE.EntityStatusID == (int)EntityStatusEnum.Active && ICE.tax_year == Part3IC.TaxYear).ToList();

            }
            return ice;
        }

        private static List<insurance_coverage> getInsuranceCoverage(Employee1095detailsPart3Model Part3IC, AcaEntities ctx)
        {
            Log.Debug("Part3 Update: LN2243 method");
            List<insurance_coverage> ic = new List<insurance_coverage>();

            if (Part3IC.DependantID == 0)
            {

                ic = ctx.insurance_coverage.AsNoTracking().Where(IC => IC.employee_id == Part3IC.EmployeeID && IC.EntityStatusID == (int)EntityStatusEnum.Active && IC.dependent_id == null && IC.tax_year == Part3IC.TaxYear).ToList();

                Log.Debug("Part3 Update: getInsuranceCoverage. EMPLOYEE");

            }
            else
            {

                ic = ctx.insurance_coverage.AsNoTracking().Where(IC => IC.employee_id == Part3IC.EmployeeID && IC.dependent_id == Part3IC.DependantID && IC.EntityStatusID == (int)EntityStatusEnum.Active && IC.tax_year == Part3IC.TaxYear).ToList();
                Log.Debug("Part3 Update: getInsuranceCoverage. DEPENDANT");

            }

            return ic;
        }

        private static void setEnrolmentsToDB(insurance_coverage_editable ic, bool[] enrolled)
        {
            ic.Jan = enrolled[0];
            ic.Feb = enrolled[1];
            ic.Mar = enrolled[2];
            ic.Apr = enrolled[3];
            ic.May = enrolled[4];
            ic.Jun = enrolled[5];
            ic.Jul = enrolled[6];
            ic.Aug = enrolled[7];
            ic.Sept = enrolled[8];
            ic.Oct = enrolled[9];
            ic.Nov = enrolled[10];
            ic.Dec = enrolled[11];
        }

        private static void setEnrolmentsToDB(insurance_coverage ic, bool[] enrolled)
        {
            ic.jan = enrolled[0];
            ic.feb = enrolled[1];
            ic.mar = enrolled[2];
            ic.apr = enrolled[3];
            ic.may = enrolled[4];
            ic.jun = enrolled[5];
            ic.jul = enrolled[6];
            ic.aug = enrolled[7];
            ic.sep = enrolled[8];
            ic.oct = enrolled[9];
            ic.nov = enrolled[10];
            ic.dec = enrolled[11];
        }

        private static bool[] getEnrolmentsfromDB(insurance_coverage ic)
        {
            bool[] Enrolled = new bool[12];
            Enrolled[0] = ic.jan;
            Enrolled[1] = ic.feb;
            Enrolled[2] = ic.mar;
            Enrolled[3] = ic.apr;
            Enrolled[4] = ic.may;
            Enrolled[5] = ic.jun;
            Enrolled[6] = ic.jul;
            Enrolled[7] = ic.aug;
            Enrolled[8] = ic.sep;
            Enrolled[9] = ic.oct;
            Enrolled[10] = ic.nov;
            Enrolled[11] = ic.dec;

            return Enrolled;
        }
        private static bool[] getEnrolmentsfromVARP3DB(View_air_replacement_PartIII_details ic)
        {
            bool[] Enrolled = new bool[12];
            Enrolled[0] = ic.jan == 1 ? true : false;
            Enrolled[1] = ic.feb == 1 ? true : false;
            Enrolled[2] = ic.mar == 1 ? true : false;
            Enrolled[3] = ic.apr == 1 ? true : false;
            Enrolled[4] = ic.may == 1 ? true : false;
            Enrolled[5] = ic.jun == 1 ? true : false;
            Enrolled[6] = ic.jul == 1 ? true : false;
            Enrolled[7] = ic.aug == 1 ? true : false;
            Enrolled[8] = ic.sep == 1 ? true : false;
            Enrolled[9] = ic.oct == 1 ? true : false;
            Enrolled[10] = ic.nov == 1 ? true : false;
            Enrolled[11] = ic.dec == 1 ? true : false;

            return Enrolled;
        }
        #endregion

        private static void SaveContext(AcaEntities ctx)
        {
            try
            {
                ctx.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Exception raise = ex;
                foreach (System.Data.Entity.Validation.DbEntityValidationResult validationErrors in ex.EntityValidationErrors)
                {
                    foreach (System.Data.Entity.Validation.DbValidationError validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                Log.Error("Errors during updating Part3 Insrance Coverage in 1095MD SaveContext method ln:2321", ex);

            }
        }

    }

}