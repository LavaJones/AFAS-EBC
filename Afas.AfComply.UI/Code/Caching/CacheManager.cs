using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.SessionState;

using log4net;

namespace Afas.AfComply.UI.Code.Caching
{

    public static class CacheManager
    {

        /// <summary>
        /// Returns the Affordable Safe Harbor Codes. Universal across all employers.
        /// </summary>
        public static List<ash> AffordableSafeHarborCodes()
        {
            if (IsSessionNull) return airController.ManufactureASHList(false);

            String cacheKey = String.Format("{0}", CacheKeys.Generic.AffordableSafeHarborCodes);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = airController.ManufactureASHList(false);

            }

            return Session[cacheKey] as List<ash>;

        }

        /// <summary>
        /// Invalidates the cached Affordable Safe Harbor Codes
        /// </summary>
        public static void AffordableSafeHarborCodesInvalidate()
        {
            if (IsSessionNull) return;

            String cacheKey = String.Format("{0}", CacheKeys.Generic.AffordableSafeHarborCodes);

            Log.DebugFormat("Invalidating cache for key: {0}.", cacheKey);

            Session[cacheKey] = null;

        }

        /// <summary>
        /// Returns the employee classifications for a specific employer.
        /// </summary>
        public static List<classification> EmployeeClassifications(int employerId)
        {
            if (IsSessionNull) return classificationController.ManufactureEmployerClassificationList(employerId, false);

            String cacheKey = String.Format("{0}_{1}", CacheKeys.Employer.EmployeeClassifications, employerId);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = classificationController.ManufactureEmployerClassificationList(employerId, false);

            }

            return Session[cacheKey] as List<classification>;

        }

        public static void EmployeeClassificationsInvalidate(int employerId)
        {
            if (IsSessionNull) return;

            String cacheKey = String.Format("{0}_{1}", CacheKeys.Employer.EmployeeClassifications, employerId);

            Log.DebugFormat("Invalidating cache for key: {0}.", cacheKey);

            Session[cacheKey] = null;

        }

        /// <summary>
        /// Returns the list of employee coverage information from the carrier report.
        /// </summary>
        public static List<insurance_coverage> EmployeeCoverageFromCarrierReport(int employeeId, int taxYear)
        {
            if (IsSessionNull) return insuranceController.ManufactureEmployeeInsuranceCoverage(employeeId, taxYear, false);

            String cacheKey = String.Format("{0}_{1}_{2}", CacheKeys.Employee.EmployeeCoverageFromCarrierReport, employeeId, taxYear);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = insuranceController.ManufactureEmployeeInsuranceCoverage(employeeId, taxYear, false);

            }

            return Session[cacheKey] as List<insurance_coverage>;

        }

        /// <summary>
        /// Invalidates all items associated with the employee coverage information in the carrier report.
        /// </summary>
        public static void EmployeeCoverageFromCarrierReportInvalidate(int employeeId, int taxYear)
        {
            if (IsSessionNull) return;

            String cacheKey = String.Format("{0}_{1}_{2}", CacheKeys.Employee.EmployeeCoverageFromCarrierReport, employeeId, taxYear);

            Log.DebugFormat("Invalidating cache for key: {0}.", cacheKey);

            Session[cacheKey] = null;

        }

        /// <summary>
        /// Returns the employees listed in the carrier report.
        /// </summary>
        public static List<int> EmployeeIdsInCarrierReport(int employerId, int taxYear)
        {
            if (IsSessionNull) return EmployeeController.GetEmployeesInInsuranceCarrierImport(employerId, taxYear, false);

            String cacheKey = String.Format("{0}_{1}_{2}", CacheKeys.Employee.EmployeeIdsInCarrierReport, employerId, taxYear);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = EmployeeController.GetEmployeesInInsuranceCarrierImport(employerId, taxYear, false);

            }

            return Session[cacheKey] as List<int>;

        }

        /// <summary>
        /// Invalidates all items associated with the employees in the carrier report.
        /// </summary>
        public static void EmployeeIdsInCarrierReportInvalidate(int employerId, int taxYear)
        {
            if (IsSessionNull) return;

            String cacheKey = String.Format("{0}_{1}_{2}", CacheKeys.Employee.EmployeeIdsInCarrierReport, employerId, taxYear);

            Log.DebugFormat("Invalidating cache for key: {0}.", cacheKey);

            Session[cacheKey] = null;

        }


        public static List<Employee> RemainingEmployeesNeeding1095Approval(int employerId, int taxYear)
        {
            if (IsSessionNull) return EmployeeController.RemainingEmployeesNeeding1095Approval(employerId, taxYear, false);

            String cacheKey = String.Format("{0}_{1}_{2}", CacheKeys.Employee.RemainingEmployeesNeeding1095Approval, employerId, taxYear);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = EmployeeController.RemainingEmployeesNeeding1095Approval(employerId, taxYear, false);

            }

            return Session[cacheKey] as List<Employee>;

        }

        /// <summary>
        /// Invalidates all items associated with the 1095 pending review list.
        /// </summary>
        public static void RemainingEmployeesNeeding1095ApprovalInvalidate(int employerId, int taxYear)
        {
            if (IsSessionNull) return;

            String cacheKey = String.Format("{0}_{1}_{2}", CacheKeys.Employee.RemainingEmployeesNeeding1095Approval, employerId, taxYear);

            Log.DebugFormat("Invalidating cache for key: {0}.", cacheKey);

            Session[cacheKey] = null;

        }


        /// <summary>
        /// Returns the employees still waiting on a 1095 review.
        /// </summary>
        public static List<Employee> EmployeesPending1095Review(int employerId, int taxYear)
        {
            if (IsSessionNull) return EmployeeController.EmployeesPending1095Approval(employerId, taxYear, false);

            String cacheKey = String.Format("{0}_{1}_{2}", CacheKeys.Employee.EmployeesPending1095Review, employerId, taxYear);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = EmployeeController.EmployeesPending1095Approval(employerId, taxYear, false);

            }

            return Session[cacheKey] as List<Employee>;

        }

        public static List<Employee> EmployeesPending1095Corrections(int employerId, int taxYear)
        {
            if (IsSessionNull) return EmployeeController.EmployeesPending1095Corrections(employerId, taxYear, false);

            String cacheKey = String.Format("{0}_{1}_{2}", CacheKeys.Employee.EmployeesPending1095Corrections, employerId, taxYear);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = EmployeeController.EmployeesPending1095Corrections(employerId, taxYear, false);

            }

            return Session[cacheKey] as List<Employee>;

        }

        /// <summary>
        /// Invalidates all items associated with the 1095 pending review list.
        /// </summary>
        public static void EmployeesPending1095ReviewInvalidate(int employerId, int taxYear)
        {
            if (IsSessionNull) return;

            String cacheKey = String.Format("{0}_{1}_{2}", CacheKeys.Employee.EmployeesPending1095Review, employerId, taxYear);

            Log.DebugFormat("Invalidating cache for key: {0}.", cacheKey);

            Session[cacheKey] = null;

        }

        /// <summary>
        /// Invalidates all items associated with any employee that is currently cached.
        /// </summary>
        public static void EmployeeInvalidate()
        {
            if (IsSessionNull) return;

            IList<String> keys = (from String key in Session.Keys
                                  where key.StartsWith(CacheKeys.Employee.CacheKeyHeader)
                                  select key).ToList();

            foreach (String key in keys)
            {

                Log.DebugFormat("Invalidating cache for key: {0}.", key);
                
                Session[key] = null;
            
            }

        }

        /// <summary>
        /// Invalidates all items associated with any employer that is currently cached.
        /// </summary>
        public static void EmployerInvalidate()
        {
            if (IsSessionNull) return;

            IList<String> keys = (from String key in Session.Keys
                                  where key.StartsWith(CacheKeys.Employer.CacheKeyHeader)
                                  select key).ToList();

            foreach (String key in keys)
            {

                Log.DebugFormat("Invalidating cache for key: {0}.", key);
                
                Session[key] = null;
            
            }

        }

        /// <summary>
        /// Returns the Insurance Types.
        /// </summary>
        public static List<insuranceType> InsuranceTypes()
        {
            if (IsSessionNull) return insuranceController.GetInsuranceTypes(false);

            String cacheKey = String.Format("{0}", CacheKeys.Generic.InsuranceTypes);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = insuranceController.GetInsuranceTypes(false);

            }

            return Session[cacheKey] as List<insuranceType>;

        }

        /// <summary>
        /// Invalidates the cached insurance types.
        /// </summary>
        public static void InsuranceTypesInvalidate()
        {
            if (IsSessionNull) return;

            String cacheKey = String.Format("{0}", CacheKeys.Generic.InsuranceTypes);

            Log.DebugFormat("Invalidating cache for key: {0}.", cacheKey);

            Session[cacheKey] = null;

        }

        /// <summary>
        /// Returns the Employees IDs that have been flagged in the appr yearly table to receive a 1095 form.
        /// </summary>
        public static List<int> Irs1095EmployeeIdsFlaggedFor1095(int employerId, int taxYear)
        {
            if (IsSessionNull) return airController.GetEmployeesReceiving1095(employerId, taxYear, false);

            String cacheKey = String.Format("{0}_{1}_{2}", CacheKeys.Employee.EmployeeIdsFlaggedFor1095, employerId, taxYear);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = airController.GetEmployeesReceiving1095(employerId, taxYear, false);

            }

            return Session[cacheKey] as List<int>;

        }

        /// <summary>
        /// Invalidates the cached employee ids that have been flagged for a 1095
        /// </summary>
        public static void Irs1095EmployeeIdsFlaggedFor1095Invalidate(int employerId, int taxYear)
        {
            if (IsSessionNull) return;

            String cacheKey = String.Format("{0}_{1}_{2}", CacheKeys.Employee.EmployeeIdsFlaggedFor1095, employerId, taxYear);

            Log.DebugFormat("Invalidating cache for key: {0}.", cacheKey);

            Session[cacheKey] = null;

        }

        /// <summary>
        /// Returns the air coverage for a specific employeeId
        /// </summary>
        public static List<airCoverage> Irs1095AirCoverage(int employeeId)
        {
            if (IsSessionNull) return airController.GetAirCoverage(employeeId, false);

            String cacheKey = String.Format("{0}_{1}", CacheKeys.Employee.EmployeeAirCoverage, employeeId);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = airController.GetAirCoverage(employeeId, false);

            }

            return Session[cacheKey] as List<airCoverage>;

        }

        /// <summary>
        /// Invalidates the cached AirCoverage for the passed employeeId
        /// </summary>
        public static void Irs1095AirCoverageInvalidate(int employeeId)
        {
            if (IsSessionNull) return;

            String cacheKey = String.Format("{0}_{1}", CacheKeys.Employee.EmployeeAirCoverage, employeeId);

            Log.DebugFormat("Invalidating cache for key: {0}.", cacheKey);

            Session[cacheKey] = null;

        }

        /// <summary>
        /// Returns the EmployeeMonthlyDetailList for a specific employeeId.
        /// </summary>
        public static List<monthlyDetail> Irs1095EmployeeMonthlyDetail(int employeeId)
        {
            if (IsSessionNull) return airController.ManufactureEmployeeMonthlyDetailList(employeeId, false);

            String cacheKey = String.Format("{0}_{1}", CacheKeys.Employee.EmployeeMonthlyDetailList, employeeId);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = airController.ManufactureEmployeeMonthlyDetailList(employeeId, false);

            }

            return Session[cacheKey] as List<monthlyDetail>;

        }

        /// <summary>
        /// Invalidates the cached EmployeeMonthlyDetail for the passed employeeId
        /// </summary>
        public static void Irs1095EmployeeMonthlyDetailInvalidate(int employeeId)
        {
            if (IsSessionNull) return;

            String cacheKey = String.Format("{0}_{1}", CacheKeys.Employee.EmployeeMonthlyDetailList, employeeId);

            Log.DebugFormat("Invalidating cache for key: {0}.", cacheKey);

            Session[cacheKey] = null;

        }

        /// <summary>
        /// Returns the Offer Of Coverage codes. Universal across all employers.
        /// </summary>
        public static List<ooc> OfferOfCoverageCodes()
        {
            if (IsSessionNull) return airController.ManufactureOOCList(false);

            String cacheKey = String.Format("{0}", CacheKeys.Generic.OfferOfCoverageCodes);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = airController.ManufactureOOCList(false);
            
            }

            return Session[cacheKey] as List<ooc>;

        }

        /// <summary>
        /// Invalidates the cached Offer of Coverage codes.
        /// </summary>
        public static void OfferOfCoverageCodesInvalidate()
        {
            if (IsSessionNull) return;

            String cacheKey = String.Format("{0}", CacheKeys.Generic.OfferOfCoverageCodes);

            Log.DebugFormat("Invalidating cache for key: {0}.", cacheKey);

            Session[cacheKey] = null;

        }

        /// <summary>
        /// Returns the Time Frames for a Tax Year.
        /// </summary>
        public static List<status> Status()
        {
            if (IsSessionNull) return airController.ManufactureStatusList(false);

            String cacheKey = String.Format("{0}", CacheKeys.Generic.StatusList);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = airController.ManufactureStatusList(false);

            }

            return Session[cacheKey] as List<status>;

        }

        /// <summary>
        /// Invalidates the cached status.
        /// </summary>
        public static void StatusInvalidate(int taxYear)
        {
            if (IsSessionNull) return;

            String cacheKey = String.Format("{0}_{1}", CacheKeys.Generic.AirTimeFrameIds, taxYear);

            Log.DebugFormat("Invalidating cache for key: {0}.", cacheKey);

            Session[cacheKey] = null;

        }

        /// <summary>
        /// Returns the Time Frames for a Tax Year.
        /// </summary>
        public static List<int> TimeFrameIds(int taxYear)
        {
            if (IsSessionNull) return airController.manufactureTimeFrameList(taxYear, false);

            String cacheKey = String.Format("{0}_{1}", CacheKeys.Generic.AirTimeFrameIds, taxYear);

            if (Session[cacheKey] == null)
            {

                Log.DebugFormat("Setting cache value for key: {0}.", cacheKey);

                Session[cacheKey] = airController.manufactureTimeFrameList(taxYear, false);

            }

            return Session[cacheKey] as List<int>;

        }

        /// <summary>
        /// Invalidates the cached time frames.
        /// </summary>
        public static void TimeFrameIdsInvalidate(int taxYear)
        {
            if (IsSessionNull) return;

            String cacheKey = String.Format("{0}_{1}", CacheKeys.Generic.AirTimeFrameIds, taxYear);

            Log.DebugFormat("Invalidating cache for key: {0}.", cacheKey);

            Session[cacheKey] = null;

        }

        internal static bool IsSessionNull { get { return HttpContext.Current.Session == null; } }

        internal static HttpSessionState Session { get { return HttpContext.Current.Session; } }

        internal static ILog Log = log4net.LogManager.GetLogger(typeof(CacheManager));

    }

}