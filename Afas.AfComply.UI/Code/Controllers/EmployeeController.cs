using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

using System.IO;

using Afas.AfComply.Domain;

using Afas.AfComply.UI.Code.Caching;

/// <summary>
/// Summary description for EmployeeController
/// </summary>
public static class EmployeeController
{

    /// <summary>
    /// Addes a new employee to the live employee table.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Employee ManufactureEmployee(
            int _employeeTypeID,
            int _hrStatusID,
            int _employerID,
            String _fname,
            String _mname,
            String _lname,
            String _address,
            String _city,
            int _stateid,
            String _zip,
            DateTime _hireDate,
            DateTime? _currDate,
            String _ssn,
            String _extID,
            DateTime? _termDate,
            DateTime? _dob,
            DateTime _imEnd,
            int _planYearID,
            DateTime _modOn,
            String _modBy,
            int _classID,
            int _actStatusID
        )
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().ManufactureEmployee(
                _employeeTypeID,
                _hrStatusID,
                _employerID,
                _fname,
                _mname,
                _lname,
                _address,
                _city,
                _stateid,
                _zip,
                _hireDate,
                _currDate,
                _ssn,
                _extID,
                _termDate,
                _dob,
                _imEnd,
                _planYearID,
                _modOn,
                _modBy,
                _classID,
                _actStatusID
            );

    }

    public static tax_year_1095c_correction_exception InsertUpadateTaxYear1095cCorrectionException(tax_year_1095c_correction_exception tax_year_1095c_correction_exception)
    {
        EmployeeFactory ef = new EmployeeFactory();
        return ef.InsertUpadateTaxYear1095cCorrectionException(tax_year_1095c_correction_exception);
    }

    public static tax_year_1095c_correction_exception getTaxYear1095cCorrectionException(int tax_year, int employer_id, int employee_id)
    {
        EmployeeFactory ef = new EmployeeFactory();
        return ef.getTaxYear1095cCorrectionException(tax_year,employer_id,employee_id);
    }

    public static List<Employee_E> manufactureEmployeeExportList(int _employerID)
    {
        EmployeeFactory ef = new EmployeeFactory();
        return ef.manufactureEmployeeExportList(_employerID);
    }

    public static List<Employee> manufactureEmployeeList(int _employerID)
    {
        EmployeeFactory ef = new EmployeeFactory();
        return ef.manufactureEmployeeList(_employerID);
    }
    public static List<Employee> SearchEmployee(int _employerID, string fName, string lName, string mName)
    {
        EmployeeFactory ef = new EmployeeFactory();
        return ef.SearchEmployee(_employerID, fName, lName, mName);
    }


    /// <summary>
    /// Returns the list of employees listed in the carrier report.
    /// </summary>
    public static List<int> GetEmployeesInInsuranceCarrierImport(int _employerID, int _taxYear, Boolean useCache)
    {

        if (useCache)
        {
            return CacheManager.EmployeeIdsInCarrierReport(_employerID, _taxYear);
        }

        return new EmployeeFactory().GetEmployeesInInsuranceCarrierImport(_employerID, _taxYear);

    }

    public static List<Employee_I> manufactureImportEmployeeList(int _employerID, int? rowLimit = null)
    {
        EmployeeFactory ef = new EmployeeFactory();
        return ef.manufactureImportEmployeeList(_employerID, rowLimit);
    }

    /// <summary>
    /// Import the passed in employee.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean ImportEmployee(
            int _batchid,
            String _hrStatusID,
            String _hrDescription,
            int _employerID,
            String _fname,
            String _mname,
            String _lname,
            String _address,
            String _city,
            String _stateid,
            String _zip,
            String _hireDate,
            String _currDate,
            String _ssn,
            String _extID,
            String _termDate,
            String _dob
        )
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().importEmployee(
                _batchid,
                _hrStatusID,
                _hrDescription,
                _employerID,
                _fname,
                _mname,
                _lname,
                _address,
                _city,
                _stateid,
                _zip,
                _hireDate,
                _currDate,
                _ssn,
                _extID,
                _termDate,
                _dob
            );

    }

    /// <summary>
    /// Bulk imports the employees in the passed dataTable.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean BulkImportEmployee(int _batchid, DataTable employees)
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().BulkImportEmployee(_batchid, employees);

    }

    /// <summary>
    /// Bulk imports the employees in the passed dataTable.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean BulkImportFullEmployee(int _batchid, DataTable employees)
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().BulkImportFullEmployee(_batchid, employees);

    }

    public static Boolean BulkInsertTaxYear1095cCorrection(List<TaxYear1095CCorrection> taxYear1095CCorrections)
    {
        return new EmployeeFactory().BulkInsertTaxYear1095cCorrection(taxYear1095CCorrections);
    }

    public static Boolean BulkDeleteTaxYear1095cApprovals(List<TaxYear1095CApproval> taxYear1095CApprovals)
    {
        return new EmployeeFactory().BulkDeleteTaxYear1095cApprovals(taxYear1095CApprovals);    
    }

    public static Boolean BulkInactivateTaxYear1095cCorrections(int employerId, String modifiedBy)
    {
        return new EmployeeFactory().BulkInactivateTaxYear1095cCorrections(employerId, modifiedBy);
    }

    public static Boolean CheckIfTaxYear1095cCorrectionRecord(int employee_Id, int tax_year)
    {
        return new EmployeeFactory().CheckIfTaxYear1095cCorrectionRecord(employee_Id, tax_year);
    }

    public static int getDateTimeComplete_Percent(DateTime _sDate, DateTime _eDate)
    {
        EmployeeShow es = new EmployeeShow();
        return es.getDateTimeComplete_Percent(_sDate, _eDate);
    }

    /// <summary>
    /// Calculate IMP start date as the first, first of the month the employee is employed 
    /// </summary>
    /// <param name="hireDate">The raw hire date.</param>
    /// <returns>The first, first of the month the employee is employed. </returns>
    public static DateTime calculateIMPStartDate(DateTime hireDate)
    {
        if (hireDate.Day == 1)
        {
            return hireDate;
        }

        DateTime tempdate = new DateTime(hireDate.Year, hireDate.Month, 1);
        return tempdate.AddMonths(1);

    }

    /// <summary>
    /// Calculate IMP end date based on the hire date and the length of the IMP
    /// </summary>
    /// <param name="hireDate">The date of Hire</param>
    /// <param name="impLength">The length of the IMP in months.</param>
    /// <returns>The End date of the IMP</returns>
    public static DateTime calculateIMPEndDate(DateTime hireDate, int impLength)
    {

        if (impLength > 12 || impLength < 3)
        {
            throw new ArgumentOutOfRangeException("IMP length must be between 3 and 12 months.");
        }

        DateTime impStart = calculateIMPStartDate(hireDate);

        DateTime impEnd = impStart.AddMonths(impLength).AddDays(-1);

        return impEnd;

    }

    /// <summary>
    /// Updates the passed information for the requested employee.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean UpdateEmployeeSNN(
            int _employeeID,
            DateTime _modOn,
            String _modBy,
            String _ssn,
            int _hrSatusID,
            int _classID,
            int _acaStatusID
        )
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().updateEmployeeSNN(_employeeID, _modOn, _modBy, _ssn, _hrSatusID, _classID, _acaStatusID);

    }

    /// <summary>
    /// Updates the passed information for the requested employee.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean UpdateEmployee(Employee employee, String modBy)
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().updateEmployee(employee, modBy);

    }

    /// <summary>
    /// Updates the Employee Info in aca and air
    /// </summary>
    /// <returns>Success or failure</returns>
    public static bool UpdateEmployeeInfo_ACA_AIR(int _employeeID, string FirstName, string MiddleName, string LastName, string StateId, string Address, string City, string Zip, string ssn, int ModBy)
    {
        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().UpdateEmployeeInfo_ACA_AIR(_employeeID, FirstName, MiddleName, LastName, StateId, Address, City, Zip, ssn, ModBy);
    }

    /// <summary>
    /// Updates the pased import employee's information.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean UpdateImportEmployee(
            int _rowID,
            int _employeeTypeID,
            int _hrStatusID,
            String _hrStatusExt,
            String _hrStatusDesc,
            int _planyearID,
            int _stateid,
            DateTime? _hireDate,
            String _hireDateI,
            DateTime? _cDate,
            DateTime? _termDate,
            String _termDateI,
            DateTime? _dob,
            String _dobI,
            DateTime? _impEnd,
            int _acaStatusID,
            int _classID
        )
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().updateImportEmployee(
                _rowID,
                _employeeTypeID,
                _hrStatusID,
                _hrStatusExt,
                _hrStatusDesc,
                _planyearID,
                _stateid,
                _hireDate,
                _hireDateI,
                _cDate,
                _termDate,
                _termDateI,
                _dob,
                _dobI,
                _impEnd,
                _acaStatusID,
                _classID
            );

    }

    /// <summary>
    /// Bulk imports the employees in the passed dataTable.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    /// <param name="employees"></param>
    /// <returns></returns>
    public static Boolean BulkUpdateImportEmployee(DataTable employees)
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().BulkUpdateImportEmployee(employees);

    }

    /// <summary>
    /// Transfers the passed employee from the import table to the live employee table.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Employee TransferImportedEmployee(
            int _employeeID,
            int _rowID,
            int _employeeTypeID,
            int _hrStatusID,
            int _employerID,
            String _fname,
            String _mname,
            String _lname,
            String _address,
            String _city,
            int _stateid,
            String _zip,
            DateTime? _hireDate,
            DateTime? _cDate,
            String _ssn,
            String _extID,
            DateTime? _termDate,
            DateTime? _dob,
            DateTime? _impEnd,
            int _planYearID,
            int _planYearID_limbo,
            int _planYarID_meas,
            DateTime _modOn,
            String _modBy,
            Boolean _offer,
            int _offerPlanYearID,
            int _classID,
            int _acaStatusID
        )
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().TransferImportedEmployee(
                _employeeID,
                _rowID,
                _employeeTypeID,
                _hrStatusID,
                _employerID,
                _fname,
                _mname,
                _lname,
                _address,
                _city,
                _stateid,
                _zip,
                _hireDate,
                _cDate,
                _ssn,
                _extID,
                _termDate,
                _dob,
                _impEnd,
                _planYearID,
                _planYearID_limbo,
                _planYarID_meas,
                _modOn,
                _modBy,
                _offer,
                _offerPlanYearID,
                _classID,
                _acaStatusID
            );

    }

    public static int manufactureBatchID(int _employerID, DateTime _modOn, string _modBy)
    {
        EmployeeFactory ef = new EmployeeFactory();
        return ef.manufactureBatchID(_employerID, _modOn, _modBy);
    }

    public static Employee_I findEmployee(List<Employee_I> _empList, int _rowID)
    {
        EmployeeShow es = new EmployeeShow();
        return es.findEmployee(_empList, _rowID);
    }

    /// <summary>
    /// Deletes a failed batch import.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean DeleteFailedBatch(int _batchID)
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().DeleteFailedBatch(_batchID);

    }

    public static bool deleteImportedEmployee(int _rowID)
    {
        EmployeeFactory ef = new EmployeeFactory();
        return ef.deleteImportedEmployee(_rowID);
    }

    /// <summary>
    /// This is used to validate if the employee already exists in the database. 
    /// </summary>
    /// <param name="_emp"></param>
    /// <param name="_ssn"></param>
    /// <returns></returns>
    public static Employee validateExistingEmployee(List<Employee> _emp, string _ssn)
    {
        EmployeeShow es = new EmployeeShow();
        return es.validateExistingEmployee(_emp, _ssn);
    }

    public static Employee findEmployee(List<Employee> _empList, int _employeeID)
    {
        EmployeeShow es = new EmployeeShow();
        return es.findEmployee(_empList, _employeeID);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_employeeTypeID"></param>
    /// <param name="_planYearID"></param>
    /// <param name="_hdate"></param>
    /// <param name="measTypeID"></param>
    /// <param name="_tempList"></param>
    /// <returns></returns>
    public static bool calculateIMP(int _employerID, int _employeeTypeID, int _planYearID, DateTime _hdate, int _measTypeID)
    {
        EmployeeShow es = new EmployeeShow();
        return es.calculateIMP(_employerID, _employeeTypeID, _planYearID, _hdate, _measTypeID);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_employeeTypeID"></param>
    /// <param name="_planYearID"></param>
    /// <param name="_hdate"></param>
    /// <param name="_measTypeID"></param>
    /// <returns></returns>
    public static bool calculateTMP(int _employerID, int _employeeTypeID, int _planYearID, DateTime _hdate, int _measTypeID)
    {
        EmployeeShow es = new EmployeeShow();
        return es.calculateTMP(_employerID, _employeeTypeID, _planYearID, _hdate, _measTypeID);
    }

    public static string calculateBarColor(double _hours)
    {
        EmployeeShow es = new EmployeeShow();
        return es.calculateBarColor(_hours);
    }

    /// <summary>
    /// This static function will create an EmployeeFactory instance and the process all data needed to 
    /// roll over all required Employee's Measurement Period Plan Year. 
    /// Invalidates any/all of the employee information in the cache.
    /// Invalidates any/all of the employer information in the cache.
    /// </summary>
    public static Boolean UpdateEmployeePlanYearPeriodMeasurement(
            int _employerID,
            int _employerTypeID,
            int _planYearID,
            int _newPlayYearID,
            DateTime _modOn,
            String _modBy
        )
    {

        CacheManager.EmployeeInvalidate();

        CacheManager.EmployerInvalidate();

        return new EmployeeFactory().updateEmployeePlanYearPeriod_Measurement(_employerID, _employerTypeID, _planYearID, _newPlayYearID, _modOn, _modBy);

    }

    /// <summary>
    /// This function is used for UnRolling the Measurement Period. 
    /// Invalidates any/all of the employee information in the cache.
    /// Invalidates any/all of the employer information in the cache.
    /// </summary>
    public static Boolean RollBackEmployeePlanYearPeriod_Measurement(
            int _employerID,
            int _employerTypeID,
            int _planYearID,
            int _newPlayYearID,
            DateTime _modOn,
            String _modBy
        )
    {

        CacheManager.EmployeeInvalidate();

        CacheManager.EmployerInvalidate();

        return new EmployeeFactory().RollBackEmployeePlanYearPeriod_Measurement(_employerID, _employerTypeID, _planYearID, _newPlayYearID, _modOn, _modBy);

    }

    /// <summary>
    /// Imports the records from the file into the employee import table.
    /// Invalidates any/all of the employee information in the cache.
    /// Invalidates any/all of the employer information in the cache.
    /// </summary>
    public static Boolean ProcessDemographicImportFiles(
            int _employerID,
            String _modBy,
            DateTime _modOn,
            String _filePath,
            String _fileName
        )
    {

        CacheManager.EmployeeInvalidate();

        CacheManager.EmployerInvalidate();

        return new EmployeeFactory().process_DEM_I_files(_employerID, _modBy, _modOn, _filePath, _fileName);

    }

    /// <summary>
    /// Link the data in the import table to any existing records in the live employee table.
    /// Invalidates any/all of the employee information in the cache.
    /// Invalidates any/all of the employer information in the cache.
    /// </summary>
    public static Boolean CrossReferenceDemographicImportTableData(
            int _employerID,
            int _planYearID,
            int _employeeClassID,
            int _acaStatusID,
            int _employeeTypeId
        )
    {

        CacheManager.EmployeeInvalidate();

        CacheManager.EmployerInvalidate();

        return new EmployeeFactory().CrossReferenceData_DEM_I_data(_employerID, _planYearID, _employeeClassID, _acaStatusID, _employeeTypeId);

    }

    /// <summary>
    /// Transfer's any and all valid employee data from the import table to the live tables.
    /// Invalidates any/all of the employee information in the cache.
    /// Invalidates any/all of the employer information in the cache.
    /// </summary>
    public static Boolean TransferDemographicImportTableData(
            int _employerID,
            String _modBy,
            Boolean _initialImport,
            Boolean _ignoreNewHire
        )
    {

        CacheManager.EmployeeInvalidate();

        CacheManager.EmployerInvalidate();

        return new EmployeeFactory().TransferDemographicImportTableData(_employerID, _modBy, _initialImport, _ignoreNewHire);

    }

    /// <summary>
    /// Updates the (hours?) for the employee.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean UpdateEmployeeAvgMonthlyHours(
            int _employeeID,
            double _pyAvg,
            double _lpyAvg,
            double _mpyAvg,
            double _impAvg
        )
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().updateEmployeeAvgMonthlyHours(_employeeID, _pyAvg, _lpyAvg, _mpyAvg, _impAvg);

    }

    /// <summary>
    /// Bulk update employee data from the passed dataTable.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public static Boolean BulkUpdateEmployee(DataTable dataTable)
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().BulkUpdateEmployee(dataTable);

    }

    /// <summary>
    /// Updates the employee's (plan year?)
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean updateEmployeePlanYearMeasId(
            int _employeeID,
            int _planYearID,
            DateTime _modOn,
            String _modBy
        )
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().updateEmployeePlanYearMeasId(_employeeID, _planYearID, _modOn, _modBy);

    }

    /// <summary>
    /// Rolls the employer/employerType/planYearId into the adminstrative period.
    /// Invalidates any/all of the employee information in the cache.
    /// Invalidates any/all of the employer information in the cache.
    /// </summary>
    public static Boolean RolloverAdministrativePeriod(
            int _employerID,
            int _employerTypeID,
            int _planYearID,
            DateTime _modOn,
            String _modBy
        )
    {

        CacheManager.EmployeeInvalidate();

        CacheManager.EmployerInvalidate();

        return new EmployeeFactory().RolloverAdministrativePeriod(_employerID, _employerTypeID, _planYearID, _modOn, _modBy);

    }

    public static string generateTextFile(List<Employee_E> _tempList, employer _employer)
    {
        EmployeeFactory ef = new EmployeeFactory();
        return ef.generateTextFile(_tempList, _employer);
    }

    public static void WriteEmployeeDataToStream(List<Employee_E> _tempList, StreamWriter sw)
    {
        EmployeeFactory ef = new EmployeeFactory();
        ef.WriteEmployeeDataToStream(_tempList, sw);
    }

    /// <summary>
    /// Updates and employee's class and ACA Status.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean UpdateEmployeeClassAcaStatus(
            int _employerID,
            int _employeeID,
            int _classID,
            int _acaID,
            String _modBy,
            DateTime _modOn
        )
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().updateEmployeeClassAcaStatus(_employerID, _employeeID, _classID, _acaID, _modBy, _modOn);

    }

    /// <summary>
    /// Deletes the employee.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean DeleteEmployee(int employerID, int employeeID)
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().deleteEmployee(employerID, employeeID);

    }

    public static Employee findSingleEmployee(int _employeeID)
    {
        EmployeeFactory ef = new EmployeeFactory();
        return ef.findSingleEmployee(_employeeID);
    }

    /// <summary>
    /// Get an Employees Dependents
    /// </summary>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public static List<Dependent> manufactureEmployeeDependentList(int _employeeID)
    {
        EmployeeFactory ef = new EmployeeFactory();
        return ef.manufactureEmployeeDependentList(_employeeID);
    }

    /// <summary>
    /// Update or Insert EMPLOYEE Dependent Information.
    /// </summary>
    /// <param name="_employeeID"></param>
    /// <param name="_fname"></param>
    /// <param name="_mname"></param>
    /// <param name="_lname"></param>
    /// <param name="_ssn"></param>
    /// <param name="_dob"></param>
    /// <returns></returns>
    public static Dependent updateEmployeeDependent(int _dependentID, int _employeeID, string _fname, string _mname, string _lname, string _ssn, DateTime? _dob, string modBy, int entityStatusID)
    {

        EmployeeFactory ef = new EmployeeFactory();
        return ef.updateEmployeeDependent(_dependentID, _employeeID, _fname, _mname, _lname, _ssn, _dob, modBy, entityStatusID);
    }

    /// <summary>
    /// Triggers a copying of dependent data from ACA to AIR databases.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean AddDependentCoverage(int taxYear)
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().addDependentCoverage(taxYear);

    }

    /// <summary>
    /// Deletes the employee's dependent.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean DeleteEmployeeDependent(int dependentID, int employeeID)
    {

        CacheManager.Irs1095AirCoverageInvalidate(employeeID);

        CacheManager.Irs1095EmployeeMonthlyDetail(employeeID);

        return new EmployeeFactory().deleteEmployeeDependent(dependentID, employeeID);

    }

    /// <summary>
    /// Insert new Tax Year Approval.
    /// Invalidates any/all of the employee information in the cache.
    /// </summary>
    public static Boolean InsertTaxYear1095Approval(
            int _taxYear,
            int _employeeID,
            int _employerID,
            String _modBy,
            DateTime _modOn,
            Boolean _1095C
        )
    {

        CacheManager.EmployeeInvalidate();

        return new EmployeeFactory().InsertTaxYear1095Approval(_taxYear, _employeeID, _employerID, _modBy, _modOn, _1095C);

    }

    public static List<Employee> RemainingEmployeesNeeding1095Approval(int employerID, int taxYear, Boolean useCache)
    {

        if (useCache)
        {
            return CacheManager.RemainingEmployeesNeeding1095Approval(employerID, taxYear);
        }

        return new EmployeeFactory().RemainingEmployeesNeeding1095Approval(employerID, taxYear);

    }

    /// <summary>
    /// Gets a list of all Employees who have not had their 1095 data edited/approved.
    /// </summary>
    public static List<Employee> EmployeesPending1095Approval(int employerID, int taxYear, Boolean useCache)
    {

        if (useCache)
        {
            return CacheManager.EmployeesPending1095Review(employerID, taxYear);
        }

        return new EmployeeFactory().EmployeesPending1095Approval(employerID, taxYear);

    }

    public static List<Employee> EmployeesPending1095Corrections(int employerID, int taxYear, Boolean useCache)
    {
        if (useCache)
        {
            return CacheManager.EmployeesPending1095Corrections(employerID, taxYear);
        }

        return new EmployeeFactory().EmployeesPending1095Corrections(employerID, taxYear);
    }

    public static List<Employee> ManufactureEmployeeList1095Finalized(int _employerID, int _taxYear)
    {
        return new EmployeeFactory().ManufactureEmployeeList1095Finalized(_employerID, _taxYear);
    }

    public static bool updateEmployeeLineIII_DOB(int _employeeID, DateTime _modOn, string _modBy, DateTime _dob, string _fname, string _lname)
    {

        CacheManager.Irs1095AirCoverageInvalidate(_employeeID);

        CacheManager.Irs1095EmployeeMonthlyDetail(_employeeID);

        EmployeeFactory ef = new EmployeeFactory();
        return ef.updateEmployeeLineIII_DOB(_employeeID, _modOn, _modBy, _dob, _fname, _lname);
    }

    public static bool updateEmployeeLineIII_SSN(int _employeeID, DateTime _modOn, string _modBy, string _ssn, string _fname, string _lname)
    {

        CacheManager.Irs1095AirCoverageInvalidate(_employeeID);

        CacheManager.Irs1095EmployeeMonthlyDetail(_employeeID);

        EmployeeFactory ef = new EmployeeFactory();
        return ef.updateEmployeeLineIII_SSN(_employeeID, _modOn, _modBy, _ssn, _fname, _lname);
    }

    public static bool deleteEmployee1095cApproval(int _employeeID, int _employerID, int _taxyear)
    {

        CacheManager.EmployeeInvalidate();

        EmployeeFactory ef = new EmployeeFactory();
        return ef.deleteEmployee1095cApproval(_employeeID, _employerID, _taxyear);
    }

    public static DataTable GetNewImportEmployeeDataTable()
    {
        DataTable employees = new DataTable();

        employees.Columns.Add("rowid", typeof(int));
        employees.Columns.Add("employeeTypeID", typeof(int));
        employees.Columns.Add("hr_status_id", typeof(int));
        employees.Columns.Add("hr_status_ext_id", typeof(String));
        employees.Columns.Add("hr_description", typeof(String));
        employees.Columns.Add("employerID", typeof(int));
        employees.Columns.Add("planYearID", typeof(int));
        employees.Columns.Add("fName", typeof(String));
        employees.Columns.Add("mName", typeof(String));
        employees.Columns.Add("lName", typeof(String));
        employees.Columns.Add("address", typeof(String));
        employees.Columns.Add("city", typeof(String));
        employees.Columns.Add("stateID", typeof(String));
        employees.Columns.Add("stateAbb", typeof(string));
        employees.Columns.Add("zip", typeof(String));
        employees.Columns.Add("hDate", typeof(DateTime));
        employees.Columns.Add("i_hDate", typeof(String));
        employees.Columns.Add("cDate", typeof(DateTime));
        employees.Columns.Add("i_cDate", typeof(String));
        employees.Columns.Add("ssn", typeof(String));
        employees.Columns.Add("ext_employee_id", typeof(String));
        employees.Columns.Add("tDate", typeof(DateTime));
        employees.Columns.Add("i_tDate", typeof(String));
        employees.Columns.Add("dob", typeof(DateTime));
        employees.Columns.Add("i_dob", typeof(String));
        employees.Columns.Add("impEnd", typeof(DateTime));
        employees.Columns.Add("batchid", typeof(int));
        employees.Columns.Add("aca_status_id", typeof(int));
        employees.Columns.Add("class_id", typeof(int));

        return employees;
    }

    public static void AddEmployeeIUpdateToDataTable(DataTable employees, Employee_I employee)
    {
        DataRow row = employees.NewRow();

        row["rowid"] = employee.ROW_ID;
        row["employeeTypeID"] = employee.EMPLOYEE_TYPE_ID.checkIntDBNull();
        row["HR_status_id"] = employee.EMPLOYEE_HR_STATUS_ID.checkIntDBNull();
        row["hr_status_ext_id"] = employee.EMPLOYEE_HR_EXT_STATUS_ID.TruncateLength(50).checkForDBNull();
        row["hr_description"] = employee.EMPLOYEE_HR_EXT_DESCRIPTION.TruncateLength(50).checkForDBNull();
        row["planYearID"] = employee.EMPLOYEE_PLAN_YEAR_ID_MEAS.checkIntDBNull();
        row["stateID"] = employee.EMPLOYEE_STATE_ID.checkIntDBNull();
        row["hDate"] = employee.EMPLOYEE_HIRE_DATE.checkDateDBNull();
        row["i_hDate"] = employee.EMPLOYEE_I_HIRE_DATE.TruncateLength(8).checkForDBNull();
        row["cDate"] = employee.EMPLOYEE_C_DATE.checkDateDBNull();
        row["tDate"] = employee.EMPLOYEE_TERM_DATE.checkDateDBNull();
        row["i_tDate"] = employee.EMPLOYEE_I_TERM_DATE.TruncateLength(8).checkForDBNull();
        row["dob"] = employee.EMPLOYEE_DOB.checkDateDBNull();
        row["i_dob"] = employee.EMPLOYEE_I_DOB.TruncateLength(8).checkForDBNull();
        row["impEnd"] = employee.EMPLOYEE_IMP_END.checkDateDBNull();
        row["class_id"] = employee.EMPLOYEE_CLASS_ID.checkIntDBNull();
        row["aca_status_id"] = employee.EMPLOYEE_ACT_STATUS_ID.checkIntDBNull();

        employees.Rows.Add(row);
    }

    public static DataTable GetInsCarrierExport(IList<Employee_E> Employees, string EmployerFein, int EmployerId)
    {

        List<hrStatus> hrStatuses = hrStatus_Controller.manufactureHRStatusList(EmployerId);
        List<classification_aca> actStatus = classificationController.getACAstatusList();
        List<classification> classifications = classificationController.ManufactureEmployerClassificationList(EmployerId, true);
        List<EmployeeType> employeeTypes = EmployeeTypeController.getEmployeeTypes(EmployerId);

        DataTable export = new DataTable();

        export.Columns.Add("FEIN", typeof(String));
        export.Columns.Add("Subscriber SSN", typeof(String));
        export.Columns.Add("Member", typeof(String));
        export.Columns.Add("Member SSN", typeof(String));
        export.Columns.Add("First Name", typeof(String));
        export.Columns.Add("Middle Name", typeof(String));
        export.Columns.Add("Last Name", typeof(String));
        export.Columns.Add("Suffix", typeof(String));
        export.Columns.Add("DOB", typeof(String));
        export.Columns.Add("JAN", typeof(String));
        export.Columns.Add("FEB", typeof(String));
        export.Columns.Add("MAR", typeof(String));
        export.Columns.Add("APR", typeof(String));
        export.Columns.Add("MAY", typeof(String));
        export.Columns.Add("JUN", typeof(String));
        export.Columns.Add("JUL", typeof(String));
        export.Columns.Add("AUG", typeof(String));
        export.Columns.Add("SEP", typeof(String));
        export.Columns.Add("OCT", typeof(String));
        export.Columns.Add("NOV", typeof(String));
        export.Columns.Add("DEC", typeof(String));
        export.Columns.Add("Total", typeof(String));
        export.Columns.Add("Hire Date", typeof(String));
        export.Columns.Add("Change Date", typeof(String));
        export.Columns.Add("Rehire Date", typeof(String));
        export.Columns.Add("Termination Date", typeof(String));
        export.Columns.Add("Employee #", typeof(String));
        export.Columns.Add("HR Status Code", typeof(String));
        export.Columns.Add("HR Status Description", typeof(String));
        export.Columns.Add("ACA Status", typeof(String));
        export.Columns.Add("Employee Class", typeof(String));
        export.Columns.Add("Employee Type", typeof(String));

        foreach (Employee_E employee in Employees)
        {

            DataRow row = export.NewRow();

            row["FEIN"] = EmployerFein;
            row["Subscriber SSN"] = employee.Employee_SSN_Visible;
            row["Member"] = "E";
            row["Member SSN"] = employee.Employee_SSN_Visible;
            row["First Name"] = employee.EMPLOYEE_FIRST_NAME;
            row["Middle Name"] = employee.EMPLOYEE_MIDDLE_NAME;
            row["Last Name"] = employee.EMPLOYEE_LAST_NAME;
            row["Suffix"] = "";
            row["DOB"] = employee.EMPLOYEE_DOB.ToShortDate();

            row["JAN"] = String.Empty;
            row["FEB"] = String.Empty;
            row["MAR"] = String.Empty;
            row["APR"] = String.Empty;
            row["MAY"] = String.Empty;
            row["JUN"] = String.Empty;
            row["JUL"] = String.Empty;
            row["AUG"] = String.Empty;
            row["SEP"] = String.Empty;
            row["OCT"] = String.Empty;
            row["NOV"] = String.Empty;
            row["DEC"] = String.Empty;
            row["Total"] = String.Empty;

            row["Hire Date"] = employee.EMPLOYEE_HIRE_DATE.ToShortDate();
            row["Change Date"] = employee.EMPLOYEE_C_DATE.ToShortDate();
            row["Rehire Date"] = String.Empty;
            row["Termination Date"] = employee.EMPLOYEE_TERM_DATE.ToShortDate();
            row["Employee #"] = employee.EMPLOYEE_EXT_ID;
            row["HR Status Code"] = GetHrStatusName(employee.EMPLOYEE_HR_STATUS_ID, hrStatuses);
            row["HR Status Description"] = employee.EX_HR_STATUS_NAME;
            row["ACA Status"] = GetAcaStatusName(employee.EMPLOYEE_ACT_STATUS_ID, actStatus);
            row["Employee Class"] = GetClassificationName(employee.EMPLOYEE_CLASS_ID, classifications);
            row["Employee Type"] = GetEmployeeTypeName(employee.EMPLOYEE_TYPE_ID, employeeTypes);

            export.Rows.Add(row);

        }

        return export;

    }

    public static DataTable GetEmployeesCheTable(IList<Employee_E> Employees, string EmployerFein, int EmployerId)
    {
         
        List<hrStatus> hrStatuses = hrStatus_Controller.manufactureHRStatusList(EmployerId);
        List<classification_aca> actStatus = classificationController.getACAstatusList();
        List<classification> classifications = classificationController.ManufactureEmployerClassificationList(EmployerId, true);
        List<EmployeeType> employeeTypes = EmployeeTypeController.getEmployeeTypes(EmployerId);

        DataTable export = new DataTable();

        export.Columns.Add("FEIN", typeof(string));
        export.Columns.Add("SSN", typeof(string));
        export.Columns.Add("First Name", typeof(string));
        export.Columns.Add("Middle Name", typeof(string));
        export.Columns.Add("Last Name", typeof(string));
        export.Columns.Add("Suffix", typeof(string));
        export.Columns.Add("Street Address", typeof(string));
        export.Columns.Add("City Name", typeof(string));
        export.Columns.Add("State Code", typeof(string));
        export.Columns.Add("Zip Code", typeof(string));
        export.Columns.Add("Zip+4", typeof(string));
        export.Columns.Add("Country Name", typeof(string));
        export.Columns.Add("Hire Date", typeof(string));
        export.Columns.Add("Change Date", typeof(string));
        export.Columns.Add("Rehire Date", typeof(string));
        export.Columns.Add("Termination Date", typeof(string));
        export.Columns.Add("Employee #", typeof(string));
        export.Columns.Add("HR Status Code", typeof(string));
        export.Columns.Add("HR Status Description", typeof(string));
        export.Columns.Add("DOB", typeof(string));
        export.Columns.Add("ACA Status", typeof(string));
        export.Columns.Add("Employee Class", typeof(string));
        export.Columns.Add("Employee Type", typeof(string));
        export.Columns.Add("Custom Salary", typeof(string));
        export.Columns.Add("Custom Pay Rate", typeof(string));
        export.Columns.Add("Custom Pay Type", typeof(string));

        foreach (Employee_E employee in Employees)
        {

            DataRow row = export.NewRow();

            row["FEIN"] = EmployerFein;
            row["SSN"] = employee.Employee_SSN_Visible;
            row["First Name"] = employee.EMPLOYEE_FIRST_NAME;
            row["Middle Name"] = employee.EMPLOYEE_MIDDLE_NAME;
            row["Last Name"] = employee.EMPLOYEE_LAST_NAME;
            row["Suffix"] = "";
            row["Street Address"] = employee.EMPLOYEE_ADDRESS;
            row["City Name"] = employee.EMPLOYEE_CITY;

            row["State Code"] = GetStateAbreviation(employee.EMPLOYEE_STATE_ID);

            row["Zip Code"] = employee.EMPLOYEE_ZIP;
            row["Zip+4"] = "";
            row["Country Name"] = "USA";
            row["Hire Date"] = employee.EMPLOYEE_HIRE_DATE.ToShortDate();
            row["Change Date"] = employee.EMPLOYEE_C_DATE.ToShortDate();
            row["Rehire Date"] = "";
            row["Termination Date"] = employee.EMPLOYEE_TERM_DATE.ToShortDate();
            row["Employee #"] = employee.EMPLOYEE_EXT_ID;

            row["HR Status Code"] = GetHrStatusName(employee.EMPLOYEE_HR_STATUS_ID, hrStatuses);

            row["HR Status Description"] = employee.EX_HR_STATUS_NAME;
            row["DOB"] = employee.EMPLOYEE_DOB.ToShortDate();

            row["ACA Status"] = GetAcaStatusName(employee.EMPLOYEE_ACT_STATUS_ID, actStatus);

            row["Employee Class"] = GetClassificationName(employee.EMPLOYEE_CLASS_ID, classifications);

            row["Employee Type"] = GetEmployeeTypeName(employee.EMPLOYEE_TYPE_ID, employeeTypes);

            row["Custom Salary"] = "";
            row["Custom Pay Rate"] = "";
            row["Custom Pay Type"] = "";

            export.Rows.Add(row);

        }

        return export;

    }

    private static string GetStateAbreviation(int stateId)
    {
        State state = StateController.findState(stateId);
        if (null != state)
        {
            return state.State_Abbr;
        }
        else
        {
            return "";
        }
    }

    private static string GetHrStatusName(int id, IList<hrStatus> hrStatuses)
    {
        hrStatus result = hrStatuses.Where(status => status.HR_STATUS_ID == id).FirstOrDefault();
        if (result != null)
        {
            return result.HR_STATUS_NAME;
        }
        else
        {
            return "";
        }
    }

    private static string GetAcaStatusName(int id, IList<classification_aca> actStatus)
    {
        classification_aca result = actStatus.Where(status => status.ACA_STATUS_ID == id).FirstOrDefault();
        if (result != null)
        {
            return result.ACA_STATUS_NAME;
        }
        else
        {
            return "";
        }
    }

    private static string GetClassificationName(int id, IList<classification> classifications)
    {
        classification result = classifications.Where(status => status.CLASS_ID == id).FirstOrDefault();
        if (result != null)
        {
            return result.CLASS_DESC;
        }
        else
        {
            return "";
        }
    }

    private static string GetEmployeeTypeName(int id, IList<EmployeeType> employeeTypes)
    {
        EmployeeType result = employeeTypes.Where(status => status.EMPLOYEE_TYPE_ID == id).FirstOrDefault();
        if (result != null)
        {
            return result.EMPLOYEE_TYPE_NAME;
        }
        else
        {
            return "";
        }
    }

      /// <summary>
    /// Search a list of Dependent objects for a single dependent. 
    /// </summary>
    /// <param name="dependentList"></param>
    /// <param name="_dependentID"></param>
    /// <returns></returns>
    public static Dependent findSingleDependent(List<Dependent> dependentList, int _dependentID)
    {
        EmployeeShow es = new EmployeeShow();
        return es.findSingleDependent(dependentList, _dependentID);
    }


     /// <summary>
    /// Moves a dependent from Employee to Another.
    /// </summary>
    /// <returns></returns>
    public static bool migrateEmployeeDependent(int _dependentID, int _employeeIDold, int _employeeIDnew)
    {
        EmployeeFactory ef = new EmployeeFactory();
        return ef.migrateEmployeeDependent(_dependentID, _employeeIDold, _employeeIDnew);
    }

    /// <summary>
    /// This will completely remove an employee from the ACA database. This is used on the employee merge screen. 
    /// </summary>
    /// <param name="employeeID"></param>
    /// <returns></returns>
    public static Boolean nukeEmployeeFromACA(int employeeID)
    {
        EmployeeFactory ef = new EmployeeFactory();
        return ef.nukeEmployeeFromACA(employeeID);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tyet"></param>
    /// <returns></returns>
    public static bool insertTaxYearEmployerTransmission(List<taxYearEmployerTransmission> tyet)
    {
        employerFactory ef = new employerFactory();
        return ef.insertTaxYearEmployerTransmission(tyet);
    }


    public static void InsertSingleTaxYearEmployerTransmission(int taxYearId, int employerId, string transmissionTypeCd, string uniqueTransmissionId, string SubmissionId, int transmissionStatusCodeId, string originalReceiptId, string originalUniqueSubmissionId, string user_UserName, string short10941095FileName, string shortManifestFileName)
    {

        employerFactory ef = new employerFactory();
        ef.InsertSingleTaxYearEmployerTransmission(taxYearId, employerId, transmissionTypeCd, uniqueTransmissionId, SubmissionId, transmissionStatusCodeId, originalReceiptId, originalUniqueSubmissionId, user_UserName, short10941095FileName, shortManifestFileName);

    }

}