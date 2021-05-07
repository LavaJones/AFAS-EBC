using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using log4net;

using Afas.AfComply.UI.Code.Caching;

/// <summary>
/// Summary description for airController
/// </summary>
public static class airController
{

    /// <summary>
    /// AIR - Connect to AIR DB and pull the OOC codes. 
    /// </summary>
    public static List<ooc> ManufactureOOCList(Boolean useCache)
    {

        if (useCache)
        {
            return CacheManager.OfferOfCoverageCodes();
        }

        return new airFactory().ManufactureOOCList();

    }

    /// <summary>
    /// AIR - Connect to AIR DB and pull the 4980H ASH codes.
    /// </summary>
    public static List<ash> ManufactureASHList(Boolean useCache)
    {

        if (useCache)
        {
            return CacheManager.AffordableSafeHarborCodes();
        }

        return new airFactory().ManufactureASHList();
    
    }

    /// <summary>
    /// AIR - Connect to AIR DB and pull Monthly Detail for Employee.
    /// </summary>
    public static List<monthlyDetail> ManufactureEmployeeMonthlyDetailList(int employeeId, Boolean useCache)
    {

        if (useCache)
        {
            return CacheManager.Irs1095EmployeeMonthlyDetail(employeeId);
        }

        return new airFactory().ManufactureEmployeeMonthlyDetailList(employeeId);
    
    }

    /// <summary>
    /// Update OOC, LCMP and ASH code of monthly detail record.
    /// Invalidates the cache for monthly detail and air coverage for the employeeid
    /// </summary>
    public static Boolean UpdateEmployeeMonthlyDetailList(
            int _employeeID,
            int _timeFrameID,
            int _employerID,
            String _ooc,
            decimal? _lcmp,
            String _ash,
            String _modBy,
            DateTime _modOn,
            decimal _hours,
            Boolean _enrolled,
            Boolean? _mec,
            int _monthlyStatusID,
            int _insuranceTypeID,
            Boolean _corrected
        )
    {

        CacheManager.Irs1095EmployeeMonthlyDetailInvalidate(_employeeID);
        
        CacheManager.Irs1095AirCoverageInvalidate(_employeeID);

        return new airFactory().UpdateEmployeeMonthlyDetailList(
                _employeeID,
                _timeFrameID,
                _employerID,
                _ooc,
                _lcmp,
                _ash,
                _modBy,
                _modOn,
                _hours,
                _enrolled,
                _mec,
                _monthlyStatusID,
                _insuranceTypeID,
                _corrected
            );
    
    }

    /// <summary>
    /// Update ModBy and ModOn for approval of monthly detail record.
    /// Invalidates the cache for monthly detail and air coverage for the employeeid
    /// </summary>
    public static void ApproveEmployeeMonthlyDetail(int _employeeID, int _timeFrameID, string _modBy, DateTime _modOn)
    {

        CacheManager.Irs1095EmployeeMonthlyDetailInvalidate(_employeeID);

        CacheManager.Irs1095AirCoverageInvalidate(_employeeID);

        new airFactory().ApproveEmployeeMonthlyDetail(_employeeID, _timeFrameID, _modBy, _modOn);
    
    }

    /// <summary>
    /// Get all Employee ID's who are flagged to recieve a 1095C form.
    /// </summary>
    public static List<int> GetEmployeesReceiving1095(int _employerID, int _taxYear, Boolean useCache)
    {

        if (useCache)
        {
            return CacheManager.Irs1095EmployeeIdsFlaggedFor1095(_employerID, _taxYear);
        }

        return new airFactory().GetEmployeesReceiving1095(_employerID, _taxYear);
    
    }

    /// <summary>
    /// Returns a list of integer values for Month ID's within a specific year.
    /// </summary>
    /// <returns></returns>
    public static List<int> manufactureTimeFrameList(int _taxYear, Boolean useCache)
    {
        
        if (useCache)
        {
            return CacheManager.TimeFrameIds(_taxYear);
        }
        
        return new airFactory().manufactureTimeFrameList(_taxYear);

    }

    public static List<airCoverage> GetAirCoverage(int _employeeID, Boolean useCache)
    {

        if (useCache)
        {
            return CacheManager.Irs1095AirCoverage(_employeeID);
        }

        return new airFactory().GetAirCoverage(_employeeID);
    
    }

    /// <summary>
    /// Updates the individual coverage with the passed data.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean UpdateIndividualCoverage(int _individualCoverageID, string _fname, string _lname, string _ssn, DateTime? _dob)
    {

        CacheManager.EmployeeInvalidate();

        return new airFactory().UpdateIndividualCoverage(_individualCoverageID, _fname, _lname, _ssn, _dob);
    
    }

    /// <summary>
    /// Run the 1095C proc post save.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean Recalculate1095Status(int _employerID, int _employeeID, int _taxYear)
    {

        CacheManager.EmployeeInvalidate();

        return new airFactory().Recalculate1095Status(_employerID, _employeeID, _taxYear);

    }

    /// <summary>
    /// Add the covered individual from the Part III area.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean AddCoveredIndividual(int rowId)
    {

        CacheManager.EmployeeInvalidate();

        return new airFactory().AddCoveredIndividual(rowId);

    }

    /// <summary>
    /// Remove the covered individual from the Part III area.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean RemoveCoveredIndividual(int rowId)
    {

        CacheManager.EmployeeInvalidate();

        return new airFactory().RemoveCoveredIndividual(rowId);

    }

    /// <summary>
    /// Run the short build for a specific Employee. 
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean runETL_ShortBuild(int _employerID, int _employeeID, int _taxYear)
    {

        CacheManager.EmployeeInvalidate();

        return new airFactory().runETL_ShortBuild(_employerID, _employeeID, _taxYear);
    
    }

    /// <summary>
    /// Run the etl build for a missing employee. 
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean runETL_Build_MissingEmployee(int _employerID, int _employeeID, int _taxYear)
    {

        CacheManager.EmployeeInvalidate();
        
        return new airFactory().runETL_Build_MissingEmployee(_employerID, _employeeID, _taxYear);
    
    }

    /// <summary>
    /// Get all Monthly Status ID's from AIR system.
    /// </summary>
    /// <returns></returns>
    public static List<status> ManufactureStatusList(Boolean useCache)
    {

        if (useCache)
        {
            return CacheManager.Status();
        }

        return new airFactory().ManufactureStatusList();
    
    }

    /// <summary>
    /// 2/17/2017: This will validate that an employee exists in the AIR tables for the selected Tax Year. 
    /// </summary>
    public static Boolean validateEmployeeTaxYearInAIR(int employeeId, int taxYear)
    {

        airFactory af = new airFactory();
        return af.validateEmployeeTaxYearInAIR(employeeId, taxYear);

    }


    /// <summary>
    /// Manufacture the error objects based off the XML file returned from the IRS.
    /// </summary>
    /// <param name="asr"></param>
    /// <returns></returns>
    public static List<airRequestError> manufactureAckErrorFiles(airStatusRequest asr, int employerID, string utID)
    {
        airFactory af = new airFactory();
        return af.manufactureAckErrorFiles(asr, employerID, utID);
    }


    /// <summary>
    /// Generate a .CSV file to export from ACT.
    /// </summary>
    /// <param name="_tempList"></param>
    /// <param name="_employer"></param>
    /// <returns></returns>
    public static string generateIrsSubmissionErrorFile(List<taxYearEmployeeTransmission> _tempList, employer _employer, int etytID)
    {
        airFactory af = new airFactory();
        return af.generateIrsSubmissionErrorFile(_tempList, _employer, etytID);
    }

    /// <summary>
    /// AIR - Builds submission status objects.
    /// </summary>
    /// <returns></returns>
    public static List<airSubmissionStatus> manufactureSubmissionStatuses()
    {
        airFactory af = new airFactory();
        return af.manufactureSubmissionStatuses();
    }

    public static int manufactureEmployerCorrectionId(int _employerID, int _year)
    {
        airFactory af = new airFactory();
        return af.manufactureEmployerCorrectionId(_employerID, _year);
    }
    public static int manufactureEmployer1094CorrectionId(int _employerID, int _year)
    {
        airFactory af = new airFactory();
        return af.manufactureEmployer1094CorrectionId(_employerID, _year);
    }
    /// <summary>
    /// 7/20/2017: New, this will need to be converted over to Entity Framework Select. 
    /// Create a list of taxYearEmployerTransmission Objects for a specific employer and tax year.
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_year"></param>
    /// <returns></returns>
    public static List<taxYearEmployerTransmission> manufactureEmployerTransmissions(int _employerID, int _year)
    {
        airFactory af = new airFactory();
        return af.manufactureEmployerTransmissions(_employerID, _year);
    }

    /// <summary>
    /// 7/20/2017: New, this will need to be converted over to Entity Framework Select. 
    /// Create a list of taxYearEmployeeTransmission Objects for a specific employee and tax year.
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_year"></param>
    /// <returns></returns>
    public static List<taxYearEmployeeTransmission> manufactureEmployeeTransmissions(int _transmissionID)
    {
        airFactory af = new airFactory();
        return af.manufactureEmployeeTransmissions(_transmissionID);
    }

    public static List<taxYearEmployeeTransmission> manufactureAllEmployeeTransmissions(int _transmissionID)
    {
        airFactory af = new airFactory();
        return af.manufactureAllEmployeeTransmissions(_transmissionID);
    }    

    private static ILog Log = log4net.LogManager.GetLogger(typeof(airController));

}