using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using log4net;

using Afas.AfComply.Domain;

using Afas.AfComply.UI.Code.Caching;
using Afas.Domain;

/// <summary>
/// Summary description for insuranceController
/// </summary>
public static class insuranceController
{

    private static ILog Log = LogManager.GetLogger(typeof(insuranceController));
    private static List<contribution> contributionList = new List<contribution>();
    public static List<contribution> getContributionTypes()
    {
        if (contributionList.Count > 0)
        {
            return contributionList;
        }
        else
        {
            insuranceFactory isf = new insuranceFactory();
            contributionList = isf.manufactureContributionList();
            return contributionList;
        }
    }

    public static List<insuranceType> GetInsuranceTypes(Boolean useCache)
    {

        if (useCache)
        {
            return CacheManager.InsuranceTypes();
        }
            
        return new insuranceFactory().ManufactureInsuranceTypeList();
    
    }

    public static List<insurance> manufactureInsuranceList(int _planyearID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.manufactureInsuranceList(_planyearID);

    }

    public static List<insuranceContribution> manufactureInsuranceContributionList(int _insuranceID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.manufactureInsuranceContributionList(_insuranceID);
    }

    public static insurance manufactureInsurancePlan(int _planyearID, string _name, decimal _cost, bool _minValue, bool _offSpouse, bool SpouseConditional, bool _offDependent, string _modBy, DateTime? _modOn, string _history, int _insuranceTypeID, bool _mec, bool _fullyPlusSelfInsured)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.manufactureInsurancePlan(_planyearID, _name, _cost, _minValue, _offSpouse, SpouseConditional, _offDependent, _modBy, _modOn, _history, _insuranceTypeID, _mec, _fullyPlusSelfInsured);
    }

    internal static bool BulkInsertOfferAndChange(DataTable processedItems)
    {

        return new insuranceFactory().BulkImportOffer(processedItems);

    }

    public static insuranceContribution manufactureInsuranceContribution(int _insuranceID, string _contributionID, int _classID, double _amount, string _modBy, DateTime? _modOn, string _history)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.manufactureInsuranceContribution(_insuranceID, _contributionID, _classID, _amount, _modBy, _modOn, _history);
    }

    public static bool updateInsurancePlan(int _insuranceID, int _planyearID, string _name, decimal _cost, bool _minValue, bool _offSpouse, bool SpouseConditional, bool _offDependent, string _modBy, DateTime? _modOn, string _history, int _insuranceTypeID, bool _mec, bool _fullyPlusSelfInsured)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.updateInsurancePlan(_insuranceID, _planyearID, _name, _cost, _minValue, _offSpouse, SpouseConditional, _offDependent, _modBy, _modOn, _history, _insuranceTypeID, _mec, _fullyPlusSelfInsured);
    }

    public static insurance getSingleInsurancePlan(int _insuranceID, List<insurance> _tempList)
    {
        insuranceShow ins = new insuranceShow();
        return ins.getSingleInsurancePlan(_insuranceID, _tempList);
    }

    public static bool deleteInsurancePlan(int _insuranceID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.deleteInsurancePlan(_insuranceID);
    }

    public static bool deleteInsuranceContribution(int _contributionID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.deleteInsuranceContribution(_contributionID);
    }

    public static insuranceContribution getSingleInsuranceContribution(int _insContributionID, List<insuranceContribution> _tempList)
    {
        insuranceShow ins = new insuranceShow();
        return ins.getSingleInsuranceContribution(_insContributionID, _tempList);
    }

    public static bool updateInsuranceContribution(int _contID, int _insuranceID, string _contributionID, int _classID, double _amount, string _modBy, DateTime? _modOn, string _history)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.updateInsuranceContribution(_contID, _insuranceID, _contributionID, _classID, _amount, _modBy, _modOn, _history);
    }

    public static bool updateInsuranceOffer(int _rowID, int? _insuranceID, int? _contributionID, double? _avgHours, bool? _offered, DateTime? _offeredDate, bool? _accepted, DateTime? _acceptedDate, DateTime _modOn, string _modBy, string _notes, string _history, DateTime? _effDate, double _hraFlex)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.updateInsuranceOffer(_rowID, _insuranceID, _contributionID, _avgHours, _offered, _offeredDate, _accepted, _acceptedDate, _modOn, _modBy, _notes, _history, _effDate, _hraFlex);
    }

    public static alert_insurance findSingleInsuranceOffer(int _planYearID, int _employeeID)
    {
         insuranceFactory inf = new insuranceFactory();
         return inf.findSingleInsuranceOffer(_planYearID, _employeeID);
    }

     /// <summary>
    /// This will generate a List of All insurance offers on a per employee basis.  
    /// </summary>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public static List<alert_insurance> findEmployeeInsuranceOffers(int _employeeID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.findEmployeeInsuranceOffers(_employeeID);
    }
    
     /// <summary>
    /// This will generate a List of All insurance offers on a per employee basis("employee_insurance_offer" and "employee_insurance_offer_archive").  
    /// </summary>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public static List<offered_Insurance> findAllInsurancesofferedToEmployee(int _employeeID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.findAllInsurancesofferedToEmployee(_employeeID);
    }
    

    /// <summary>
    /// This will generate a List of All insurance offer Change Events on a per employee basis.  
    /// </summary>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public static List<alert_insurance> findEmployeeInsuranceOfferChangeEvents(int _employeeID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.findEmployeeInsuranceOfferChangeEvents(_employeeID);
    }

    /// <summary>
    /// This will generate a List of All insurance offers and change events for an employer's planyear.  
    /// </summary>
    /// <param name="_employeeID">ID of the employer to use.</param>
    /// <param name="_planYearID">Id of the Plan Year to use.</param>
    /// <returns>A list of all Offers and Change Events</returns>
    public static List<alert_insurance> getAllInsuranceForEmployerPlanYear(int _employeeID, int _planYearID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.getAllInsuranceForEmployerPlanYear(_employeeID, _planYearID);
    }

    public static List<insurance> getAllActiveInsurancePlans(int _employerID, bool _alertsOnly)
    {
        insuranceShow ins = new insuranceShow();
        return ins.getAllActiveInsurancePlans(_employerID, _alertsOnly);
    }

    public static string generateInsuranceAlertFile(PlanYear planYear, employer _employer)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.generateInsuranceAlertFile(planYear, _employer);
    }

    public static string generateInsuranceAlertFile(List<alert_insurance> alerts, PlanYear planYear, employer _employer)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.generateInsuranceAlertFile(alerts, planYear, _employer);
    }

    public static string generateInsuranceAlertFileHRAFlex(List<alert_insurance> _tempList, employer _employer)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.generateInsuranceAlertFileHRAFlex(_tempList, _employer);
    }

    public static List<carrier> manufactureInsuranceCarriers()
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.manufactureInsuranceCarriers();
    }

    /// <summary>
    /// Return an Insurance Coverage object based on the insurance carrier report that was imported.
    /// </summary>
    /// <param name="_carrierID"></param>
    /// <returns></returns>
    public static insurance_coverage_template manufactureInsuranceCoverageTemplate(int _carrierID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.manufactureInsuranceCoverageTemplate(_carrierID);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_taxYear"></param>
    /// <param name="_employerID"></param>
    /// <param name="_dependentLink"></param>
    /// <param name="_fname"></param>
    /// <param name="_mname"></param>
    /// <param name="_lname"></param>
    /// <param name="_ssn"></param>
    /// <param name="_dob"></param>
    /// <param name="_jan"></param>
    /// <param name="_feb"></param>
    /// <param name="_mar"></param>
    /// <param name="_apr"></param>
    /// <param name="_may"></param>
    /// <param name="_jun"></param>
    /// <param name="_jul"></param>
    /// <param name="_aug"></param>
    /// <param name="_sep"></param>
    /// <param name="_oct"></param>
    /// <param name="_nov"></param>
    /// <param name="_dec"></param>
    /// <param name="_all12"></param>
    /// <returns></returns>
    public static bool insertNewInsuranceCarrierImportRow(int _batchID, int _taxYear, int _employerID, string _dependentLink, string _fname, string _mname, string _lname, string _ssn, DateTime? _dob, bool jan, bool feb, bool mar, bool apr, bool may, bool jun, bool jul, bool aug, bool sep, bool oct, bool nov, bool dec, bool all12, bool subscriber, string _jan, string _feb, string _mar, string _apr, string _may, string _jun, string _jul, string _aug, string _sep, string _oct, string _nov, string _dec, string _all12, string _subscriberi, string _address, string _city, string _state, string _zip, int _carrierID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.insertNewInsuranceCarrierImportRow(_batchID, _taxYear, _employerID, _dependentLink, _fname, _mname, _lname, _ssn, _dob, jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec, all12, subscriber, _jan, _feb, _mar, _apr, _may, _jun, _jul, _aug, _sep, _oct, _nov, _dec, _all12, _subscriberi, _address, _city, _state, _zip, _carrierID);
    }

    /// <summary>
    /// Delete all Insurance Carrier Coverage Rows by Batch ID.
    /// </summary>
    /// <param name="_batchID"></param>
    /// <returns></returns>
    public static bool deleteInsucranceCarrierBatch(int _batchID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.DeleteInsuranceCarrierBatch(_batchID);
    }

    /// <summary>
    /// Get all exisint Insurance Coverage Alerts or Unproccessed Data. 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <returns></returns>
    public static List<insurance_coverage_I> manufactureInsuranceCoverageAlerts(int _employerID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.manufactureInsuranceCoverageAlerts(_employerID);
    }

    public static bool InsertNewInsuranceCoverage(int _employerID, int _employeeID, int _planYearID, double _monthlyAvg, string _modBy, DateTime? _modOn, string _history)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.InsertNewInsuranceCoverage(_employerID, _employeeID, _planYearID, _monthlyAvg, _modBy, _modOn, _history);
    }

    /// <summary>
    /// Used for updating each EmployeeID and DependentID in the database for record. 
    /// </summary>
    /// <param name="_rowID"></param>
    /// <param name="_employeeID"></param>
    /// <param name="_dependentID"></param>
    /// <returns></returns>
    public static bool updateInsuranceCoverageAlert(int _rowID, int _employeeID, int _dependentID, string _ssn, DateTime? _dob, string _fname, string _lname, bool _subscriber)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.updateInsuranceCoverageAlert(_rowID, _employeeID, _dependentID, _ssn, _dob, _fname, _lname, _subscriber);
    }

    public static void crossReferanceInsuranceCarrierImportData(int _employerID, DateTime _modOn, string _modBy)
    {
     
        insuranceShow insSh = new insuranceShow();
        
        insSh.crossReferanceInsuranceCarrierImportData(_employerID, _modOn, _modBy);
    
    }

    /// <summary>
    /// Transfer Insurance Carrier Import Data from import table to real table. 
    /// </summary>
    /// <param name="_employerID"></param>
    public static void transferInsuranceCarrierImportData(int _employerID, string _modBy)
    {
        insuranceFactory inf = new insuranceFactory();
        inf.transferInsuranceCarrierImportData(_employerID, _modBy);
    }

    public static bool ValidateCurrentEmployee(int _employerID)
    {

        insuranceFactory inf = new insuranceFactory();
        
        return inf.ValidateCurrentEmployee(_employerID);
    
    }

    public static bool validateCurrentEmployeeDependents(int _employerID, string modBy)
    {

        insuranceFactory inf = new insuranceFactory();
        
        return inf.validateCurrentEmployeeDependents(_employerID, modBy);
    
    }

    public static void createAlertsForMissingEmployees(int _employerID, DateTime _modOn, string _modBy)
    {
        insuranceFactory inf = new insuranceFactory();
        inf.createAlertsForMissingEmployees(_employerID, _modOn, _modBy);
    }

    public static Boolean TransferInsuranceChangeEvent(
            int _rowID, 
            int? _insuranceID, 
            int? _contributionID, 
            double? _avgHours, 
            Boolean? _offered, 
            DateTime? _offeredDate, 
            Boolean? _accepted, 
            DateTime? _acceptedDate, 
            DateTime _modOn, 
            String _modBy, 
            String _notes, 
            String _history, 
            DateTime? _effDate, 
            double _hraFlex
        )
    {

        return new insuranceFactory().TransferInsuranceChangeEvent(
                _rowID, 
                _insuranceID, 
                _contributionID, 
                _avgHours, 
                _offered, 
                _offeredDate, 
                _accepted, 
                _acceptedDate, 
                _modOn, 
                _modBy, 
                _notes, 
                _history, 
                _effDate, 
                _hraFlex
            );
    
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_employeeID"></param>
    /// <param name="_taxYear"></param>
    /// <returns></returns>
    public static List<insurance_coverage> manufactureDependentInsuranceCoverage(int _employeeID, int _taxYear)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.manufactureDependentInsuranceCoverage(_employeeID, _taxYear);
    }

    public static List<insurance_coverage> manufactureAllInsuranceCoverage(int _employeeID, int _taxYear)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.manufactureAllInsuranceCoverage(_employeeID, _taxYear);
    }

    public static List<insurance_coverage> ManufactureEmployeeInsuranceCoverage(int _employeeID, int _taxYear, Boolean useCache)
    {

        if (useCache)
        {
            return CacheManager.EmployeeCoverageFromCarrierReport(_employeeID, _taxYear);
        }

        return new insuranceFactory().ManufactureEmployeeInsuranceCoverage(_employeeID, _taxYear);
    
    }

    /// <summary>
    /// DELETE a specific Insurance Carrier Import Row. 
    /// </summary>
    /// <param name="_rowID"></param>
    /// <param name="_modBy"></param>
    /// <param name="_modOn"></param>
    /// <returns></returns>
    public static bool deleteInsuranceCoverageAlert(int _rowID, string _modBy, DateTime _modOn)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.deleteInsuranceCoverageAlert(_rowID, _modBy, _modOn);
    }

    public static Boolean InsertNewInsuranceCoverageRow(
            int _taxYear,
            int _employeeID,
            int _employerID,
            int _dependentID,
            Boolean jan,
            Boolean feb,
            Boolean mar,
            Boolean apr,
            Boolean may,
            Boolean jun,
            Boolean jul,
            Boolean aug,
            Boolean sep,
            Boolean oct,
            Boolean nov,
            Boolean dec
        )
    {

        CacheManager.Irs1095AirCoverageInvalidate(_employeeID);

        CacheManager.Irs1095EmployeeMonthlyDetailInvalidate(_employeeID);

        return new insuranceFactory().InsertNewInsuranceCoverageRow(
                _taxYear,
                _employeeID,
                _employerID,
                _dependentID,
                jan,
                feb,
                mar,
                apr,
                may,
                jun,
                jul,
                aug,
                sep,
                oct,
                nov,
                dec
            );

    }

    public static CoveredIndividual InsertCoveredIndividual(
          int taxYear,
          int employeeID,
          int employerID,
          int dependentID,
          Boolean annual_coverage_indicator
       )
    {

        CacheManager.Irs1095AirCoverageInvalidate(employeeID);

        CacheManager.Irs1095EmployeeMonthlyDetailInvalidate(employeeID);

        return new insuranceFactory().InsertCoveredIndividual(
                  taxYear,
                  employeeID,
                  employerID,
                  dependentID,
                  annual_coverage_indicator
            );

    }

    public static Boolean InsertUpdateCoveredIndividuaMonthlyDetail(
         int coveredIndividualID,
            int employeeID,
            int employerID,
            int taxYear,
            int month,
            Boolean coveredIndicator
      )
    {

        return new insuranceFactory().InsertUpdateCoveredIndividuaMonthlyDetail(
            coveredIndividualID,
            employeeID,
            employerID,
            taxYear,
            month,
            coveredIndicator
            );

    }

    public static bool updateInsuranceCoverageRow(int _rowID, bool _jan, bool _feb, bool _mar, bool _apr, bool _may, bool _jun, bool _jul, bool _aug, bool _sep, bool _oct, bool _nov, bool _dec, string _history)
    {

        CacheManager.EmployeeInvalidate();

        insuranceFactory inf = new insuranceFactory();
        return inf.updateInsuranceCoverageRow(_rowID, _jan, _feb, _mar, _apr, _may, _jun, _jul, _aug, _sep, _oct, _nov, _dec, _history);
    }

    public static List<insurance_coverage> manufactureAllEditableInsuranceCoverage(int _employeeID, int _taxYear)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.manufactureAllEditableInsuranceCoverage(_employeeID, _taxYear);
    }

    /// <summary>
    /// Returns all of the editable coverage rows associated with the employeeID.
    /// </summary>
    public static IList<InsuranceCoverageEditable> ManufactureInsuranceCoverageEditable(int _employeeID, int _taxYear)
    {
        return new insuranceFactory().ManufactureInsuranceCoverageEditable(_employeeID, _taxYear);
    }

    public static bool deleteInsuranceCoverageRow(int _rowid)
    {
        CacheManager.EmployeeInvalidate();

        insuranceFactory inf = new insuranceFactory();
        return inf.deleteInsuranceCoverageRow(_rowid);
    
    }

    public static DataTable GetTableForCarrierExport() 
    {
        DataTable data = new DataTable();

        data.Columns.Add("FEIN", typeof(string));
        data.Columns.Add("Subscriber SSN", typeof(string));
        data.Columns.Add("Member", typeof(string));
        data.Columns.Add("Member SSN", typeof(string));
        data.Columns.Add("First Name", typeof(string));
        data.Columns.Add("Middle Name", typeof(string));
        data.Columns.Add("Last Name", typeof(string));
        data.Columns.Add("Suffix", typeof(string));
        data.Columns.Add("DOB", typeof(string));
        data.Columns.Add("JAN", typeof(string));
        data.Columns.Add("FEB", typeof(string));
        data.Columns.Add("MAR", typeof(string));
        data.Columns.Add("APR", typeof(string));
        data.Columns.Add("MAY", typeof(string));
        data.Columns.Add("JUN", typeof(string));
        data.Columns.Add("JUL", typeof(string));
        data.Columns.Add("AUG", typeof(string));
        data.Columns.Add("SEP", typeof(string));
        data.Columns.Add("OCT", typeof(string));
        data.Columns.Add("NOV", typeof(string));
        data.Columns.Add("DEC", typeof(string));
        data.Columns.Add("Total", typeof(string));

        return data;
    }

    public static bool AddExportDataToTable(DataTable addTo, insurance_coverage_I dataToAdd, string FEIN) 
    {
        try
        {

            DataRow row = addTo.NewRow();

            row["FEIN"] = FEIN;
            row["Subscriber SSN"] = dataToAdd.DependentLinkSSN_UnMasked;

            if (String.IsNullOrEmpty(dataToAdd.IC_SSN) && dataToAdd.DependentLinkSSN_UnMasked.IsNullOrEmpty())
            {

                row["Member"] = String.Empty;

            }
            else if(String.IsNullOrEmpty(dataToAdd.IC_SSN))
            {
                row["Member"] = "D";
                row["Member SSN"] = String.Empty;

            }
            else
            {

                row["Member"] = (dataToAdd.DependentLinkSSN_UnMasked.Equals(dataToAdd.IC_SSN)) ? "E" : "D";
                row["Member SSN"] = dataToAdd.IC_SSN;

            }
            row["First Name"] = dataToAdd.IC_FIRST_NAME;
            row["Middle Name"] = dataToAdd.IC_MIDDLE_NAME;
            row["Last Name"] = dataToAdd.IC_LAST_NAME;
            row["Suffix"] = "";
            row["DOB"] = dataToAdd.IC_DOB.parseDateToShortStringWithDbNull();
            row["JAN"] = dataToAdd.IC_JAN_I;
            row["FEB"] = dataToAdd.IC_FEB_I;
            row["MAR"] = dataToAdd.IC_MAR_I;
            row["APR"] = dataToAdd.IC_APR_I;
            row["MAY"] = dataToAdd.IC_MAY_I;
            row["JUN"] = dataToAdd.IC_JUN_I;
            row["JUL"] = dataToAdd.IC_JUL_I;
            row["AUG"] = dataToAdd.IC_AUG_I;
            row["SEP"] = dataToAdd.IC_SEP_I;
            row["OCT"] = dataToAdd.IC_OCT_I;
            row["NOV"] = dataToAdd.IC_NOV_I;
            row["DEC"] = dataToAdd.IC_DEC_I;
            row["Total"] = dataToAdd.IC_ALL_12_I;

            addTo.Rows.Add(row);

            return true;

        }
        catch (Exception ex) 
        {
            Log.Error("Exception while adding Row to Carrier Export.", ex);
        }

        return false;
    }



    /// <summary>
    /// This will migrate all insurance offers related to one employee and move them over to the correct employee.
    /// </summary>
    /// <param name="_rowID">Record ID</param>
    /// <param name="_employerID">Employer ID</param>
    /// <param name="_employeeIDold">Employee ID of the incorrect Employee</param>
    /// <param name="_employeeIDnew">Employee ID of the correct Employee</param>
    /// <returns></returns>
    public static bool migrateInsuranceOffers(int _rowID, int _employerID, int _employeeIDold, int _employeeIDnew, string _modBy, DateTime _modOn, string _history)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.migrateInsuranceOffers(_rowID, _employerID, _employeeIDold, _employeeIDnew, _modBy, _modOn, _history);
    }

    /// <summary>
    /// This will delete all insurance offers & Insurance Offer Change Events related to one employee.
    /// </summary>
    /// <param name="_rowID">Record ID</param>
    /// <param name="_employerID">Employer ID</param>
    /// <param name="_employee">Employee ID</param>
    /// <returns></returns>
    public static bool deleteInsuranceOffers(int _rowID, int _employerID, int _employeeID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.deleteInsuranceOffers(_rowID, _employerID, _employeeID);
    }

    /// <summary>
    /// Get all insurance coverage for a specific employee regardless of the tax year.
    /// </summary>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public static List<insurance_coverage> manufactureAllInsuranceCoverageForEmployee(int _employeeID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.manufactureAllInsuranceCoverageForEmployee(_employeeID);
    }

     /// <summary>
    /// Get all insurance coverage for a specific employee's dependents regardless of the tax year.
    /// </summary>
    /// <param name="_planYearID"></param>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public static List<insurance_coverage> manufactureAllInsuranceCoverageForEmployeeDependents(int _employeeID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.manufactureAllInsuranceCoverageForEmployeeDependents(_employeeID);
    }

    /// <summary>
    /// This will delete a single row in the insurance coverage table based on the row id passed in.
    /// </summary>
    /// <param name="_rowID">Record ID</param>
    /// <param name="_employee">Employee ID</param>
    /// <returns></returns>
    public static bool deleteInsuranceCoverageSingleRow(int _rowID, int _employeeID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.deleteInsuranceCoverageSingleRow(_rowID, _employeeID);
    }

    /// <summary>
    /// This will transfer a single insurance coverage row over to the correct employee. 
    /// This is used on the merge employee screen. 
    /// </summary>
    /// <param name="_rowID"></param>
    /// <param name="_employeeIDnew"></param>
    /// <param name="_employeeIDold"></param>
    /// <param name="_history"></param>
    /// <returns></returns>
    public static bool updateInsuranceCoverageRow(int _rowID, int _employeeIDnew, int _employeeIDold, string _history)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.updateInsuranceCoverageRow(_rowID, _employeeIDnew, _employeeIDold, _history);
    }

    /// <summary>
    /// Find a single Insurance Coverage object from a list of them. 
    /// </summary>
    /// <param name="rowID"></param>
    /// <param name="templist"></param>
    /// <returns></returns>
    public static insurance_coverage getSingleInsuranceCoverage(int rowID, List<insurance_coverage> templist)
    {
        insuranceShow ins = new insuranceShow();
        return ins.getSingleInsuranceCoverage(rowID, templist);
    }

    /// <summary>
    /// This will transfer all insurance coverage rows for a dependent of an employee over to the correct dependent/employee. 
    /// This is used on the merge employee screen. 
    /// </summary>
    /// <param name="_rowID"></param>
    /// <param name="_employeeIDnew"></param>
    /// <param name="_employeeIDold"></param>
    /// <param name="_history"></param>
    /// <returns></returns>
    public static bool updateInsuranceCoverageRowsDependent(int _rowID, int _employeeIDnew, int _employeeIDold, int _dependentIDnew, int _dependentIDold, string _history)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.updateInsuranceCoverageRowsDependent(_rowID, _employeeIDnew, _employeeIDold, _dependentIDnew, _dependentIDold, _history);
    }

    /// <summary>
    /// Get a list of dependent and employee editable coverage non tax year specific. 
    /// </summary>
    public static List<insurance_coverage> ManufactureInsuranceCoverageEditableWithNames(int _employeeID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.ManufactureInsuranceCoverageEditableWithNames(_employeeID);
    }

    /// <summary>
    /// Builds a list of Waiting Period Objects. 
    /// </summary>
    /// <returns></returns>
    public static List<waitingPeriod> manufactureWaitingPeriod()
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.manufactureWaitingPeriod();
    }

    public static List<insurance> getAllActiveInsurancePlansByPlanYear(int _planYearID)
    {
        insuranceShow ins = new insuranceShow();
        return ins.getAllActiveInsurancePlansByPlanYear(_planYearID);
    }

    public static bool deleteImportedInsuranceCarrierBatch(int _batchID, string _modBy, int _employerID)
    {
        insuranceFactory inf = new insuranceFactory();
        return inf.deleteImportedInsuranceCarrierBatch(_batchID, _modBy, _employerID);
    }
}