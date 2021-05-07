using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Data;

/// <summary>
/// Summary description for districtController
/// </summary>
public static class employerController
{

    private static ILog Log = LogManager.GetLogger(typeof(employerController));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_distID"></param>
    /// <returns></returns>
    public static employer getEmployer(int _employerID)
    {
        employerShow ds = new employerShow();
        return ds.GetEmployer(_employerID);
    }

    public static bool insertEmployerCalculation(int _employerID)
    {
        employerFactory ds = new employerFactory();
        return ds.insertEmployerCalculation(_employerID);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_d"></param>
    /// <param name="_img"></param>
    /// <returns></returns>
    public static bool loadDistrictLogo(employer _d, Image _img)
    {
        try
        {
            _img.ImageUrl = _d.EMPLOYER_LOGO;
            _img.AlternateText = _d.EMPLOYER_NAME;
            return true;
        }
        catch (NullReferenceException) 
        {
            
            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_distID"></param>
    /// <param name="_name"></param>
    /// <param name="_address"></param>
    /// <param name="_city"></param>
    /// <param name="_stateID"></param>
    /// <param name="_zip"></param>
    /// <param name="_contact1"></param>
    /// <param name="_email1"></param>
    /// <param name="_phone1"></param>
    /// <param name="_contact2"></param>
    /// <param name="_email2"></param>
    /// <param name="_phone2"></param>
    /// <returns></returns>
    public static bool updateEmployer(int _employerID, string _name, string _address, string _city, int _stateID, string _zip, string _imgPath, string _ein, int _empType, string _dbaName)
    {
        employerFactory df = new employerFactory();
        return df.updateEmployer(_employerID, _name, _address, _city, _stateID, _zip, _imgPath, _ein, _empType, _dbaName);
    }

    /// <summary>
    /// Stored Proc to update the IRS reporting toggle
    /// </summary>
    /// <param name="_employerID">Employer Id to toggle</param>
    /// <param name="IrsEnabled">Value to set as.</param>
    /// <returns>true is success, false is failure</returns>
    public static bool updateEmployer_IrsEnabled(int _employerID, bool IrsEnabled)
    {
        employerFactory df = new employerFactory();
        return df.updateEmployer_IrsEnabled(_employerID, IrsEnabled);
    }

    public static bool updateEmployer_Step(int _employerID, int taxYearId, int stepId)
    {
        employerFactory df = new employerFactory();
        return df.updateEmployer_Step(_employerID, taxYearId, stepId);
    }

    public static bool NukeEmployer(int _employerID) 
    {
        employerFactory df = new employerFactory();
        return df.NukeEmployer(_employerID);
    }

    public static bool NukeAirEmployer(int _employerID)
    {
        employerFactory df = new employerFactory();
        return df.NukeAirEmployer(_employerID);
    }

    public static bool ShortTransmit(int _employerID, int _taxYear)
    {
        employerFactory df = new employerFactory();
        return df.ShortTransmit(_employerID, _taxYear);
    }

    public static bool Transmit(int _employerID, int _taxYear)
    {
        employerFactory df = new employerFactory();
        return df.Transmit(_employerID, _taxYear);
    }
    public static bool PrepIRS(int _employerID, int _taxYear)
    {
        employerFactory df = new employerFactory();
        return df.PrepIRS(_employerID, _taxYear);
    }
    public static int newRegistration(Registration _re)
    {
        employerFactory df = new employerFactory();
        return df.newRegistration(_re);
    }

    public static List<employer> getAllEmployers()
    {
        employerShow es = new employerShow();
        return es.GetAllEmployers();
    }

    public static List<employer> getAll1095FinalizedEmployers()
    {
        employerShow es = new employerShow();
        return es.GetAll1095FinalizedEmployers();
    }
    public static System.Data.DataTable getEmployeeCountByMonth(int _employerID, DateTime _sdate, DateTime _edate)
    {
        employerShow es = new employerShow();
        return es.GetEmployeeCountByEmployerAndDateRange(_employerID, _sdate, _edate);
    }

    public static bool updateEmployerSetup(employer employer)
    {
        return updateEmployerSetup(employer.EMPLOYER_ID, employer.EMPLOYER_IEI, employer.EMPLOYER_IEC, employer.EMPLOYER_FTPEI, employer.EMPLOYER_FTPEC, 
            employer.EMPLOYER_IPI, employer.EMPLOYER_IPC, employer.EMPLOYER_FTPPI, employer.EMPLOYER_FTPPC, employer.EMPLOYER_IMPORT, 
            employer.EMPLOYER_AUTO_BILL, employer.EMPLOYER_AUTO_UPLOAD, employer.EMPLOYER_IMPORT_PAYROLL, employer.EMPLOYER_IMPORT_EMPLOYEE, 
            employer.EMPLOYER_IMPORT_GP, employer.EMPLOYER_IMPORT_HR, employer.EMPLOYER_VENDOR_ID, employer.EMPLOYER_IMPORT_EC, employer.EMPLOYER_IMPORT_IO, 
            employer.EMPLOYER_IMPORT_IC, employer.EMPLOYER_IMPORT_PAY_MOD);
    }

    public static bool updateEmployerSetup(int _employerID, string _iei, string _iec, string _ftpei, string _ftpec, string _ipi, string _ipc, string _ftppi, string _ftppc, string _ip, bool _billing, bool _fileUpload, string _paySU, string _demSU, string _gpSU, string _hrSU, int _vendorID, string _ecSU, string _ioSU, string _icSU, string _payMod)
    {
        employerFactory ef = new employerFactory();
        return ef.updateEmployerSetup(_employerID, _iei, _iec, _ftpei, _ftpec, _ipi, _ipc, _ftppi, _ftppc, _ip, _billing, _fileUpload, _paySU, _demSU, _gpSU, _hrSU, _vendorID, _ecSU, _ioSU, _icSU, _payMod);
    }

    public static bool UnApprove1095NeedingCorrection(int etytID)
    {

        employerFactory ef = new employerFactory();

        return ef.UnApprove1095NeedingCorrection(etytID);

    }

    public static Vendor manufactureEmployerVendor(int _vendorID)
    {
        employerFactory ef = new employerFactory();
        return ef.manufactureEmployerVendor(_vendorID);
    }

    public static List<Vendor> manufactureVendors()
    {
        employerFactory ef = new employerFactory();
        return ef.manufactureVendors();
    }

    public static List<employer> filterEmployerByVendor(int _vendorID, List<employer> _empList)
    {
        employerShow es = new employerShow();
        return es.FilterEmployerByVendor(_vendorID, _empList);
    }

    public static List<employer> getAllEmployersBilling()
    {
        employerShow es = new employerShow();
        return es.GetAllEmployersBilling();
    }

    public static List<employer> getAllEmployersAutoUpload()
    {
        employerShow es = new employerShow();
        return es.GetAllEmployersAutoUpload();
    }

    public static bool updateEmployerSUfee(int _employerID, bool _suFee)
    {
        employerFactory ef = new employerFactory();
        return ef.updateEmployerSUfee(_employerID, _suFee);
    }

    public static List<batch> manufactureBatchList(int _employerID)
    {
        employerFactory ef = new employerFactory();
        return ef.manufactureBatchList(_employerID);
    }

    public static List<batch> manufactureBatchListInsuranceCarrierImport(int _employerID)
    {
        employerFactory ef = new employerFactory();
        return ef.manufactureBatchListInsuranceCarrierImport(_employerID);
    }

    public static int getSummerHourRecordsCount(int _planYearID)
    {
        employerFactory ef = new employerFactory();
        return ef.getSummerHourRecordsCount(_planYearID);
    }

    /// <summary>
    /// Get any information for the Current Tax Year submission to the IRS that is collected during the question process. 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_taxYear"></param>
    /// <returns></returns>
    public static tax_year_submission manufactureTaxYearSubmission(int _employerID, int _taxYear)
    {
        employerFactory ef = new employerFactory();
        return ef.manufactureTaxYearSubmission(_employerID, _taxYear);
    }

    /// <summary>
    /// Insert or Update Tax Year IRS Approval
    /// </summary>
    public static bool updateInsertIrsSubmissionApproval(tax_year_submission _tys)
    {
        employerFactory ef = new employerFactory();
        return ef.updateInsertIrsSubmissionApproval(_tys);
    }

    /// <summary>
    /// Insert Employer Tax Year Transmission
    /// </summary>
    public static EmployerTaxYearTransmission insertUpdateEmployerTaxYearTransmission(EmployerTaxYearTransmission employerTaxYearTransmission)
    {
        employerFactory ef = new employerFactory();
        return ef.insertUpdateEmployerTaxYearTransmission(employerTaxYearTransmission);
    }

    public static List<Form1095CUpstreamDetail> getForm1095CUpstreamDetails(int employerId, int taxYearId, bool correctedInd, bool rejectedInd)
    {
        employerFactory ef = new employerFactory();
        return ef.getForm1095CUpstreamDetails(employerId, taxYearId, correctedInd, rejectedInd);
    }

    public static List<Employee_IRS> GetEmployeeWithEmployerInfo(int employerId, int taxYearId, bool getCoverageAndDependents = true)
    {
        employerFactory ef = new employerFactory();
        return ef.getEmployeeWithEmployerInfo(employerId, taxYearId, getCoverageAndDependents);
    }

    /// <summary>
    /// Insert Employer Tax Year Transmission
    /// </summary>
    public static EmployerTaxYearTransmission getEmployerTaxYearTransmissionByEmployerIdAndTaxYearId(int employerId, int taxYearId)
    {
        employerFactory ef = new employerFactory();
        return ef.getEmployerTaxYearTransmissionByEmployerIdAndTaxYearId(employerId, taxYearId);
    }

    public static EmployerTaxYearTransmissionStatus getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(int employerId, int taxYearId)
    {
        employerFactory ef = new employerFactory();
        return ef.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(employerId, taxYearId);
    }

    public static EmployerTaxYearTransmissionStatus getCurrentEmployerTaxYearTransmissionStatusByEmployerResourceIdAndTaxYearId(Guid employerResourceId, int taxYearId)
    {
        employerFactory ef = new employerFactory();
        return ef.getCurrentEmployerTaxYearTransmissionStatusByEmployerResourceIdAndTaxYearId(employerResourceId, taxYearId);
    }

    public static EmployerTaxYearTransmissionStatus getEmployerTaxYearTransmissionStatusById(int employerTransmissionTaxYearStatusId ){
         employerFactory ef = new employerFactory();
         return ef.getCurrentEmployerTaxYearTransmissionStatusById(employerTransmissionTaxYearStatusId);
    }

    public static EmployerTaxYearTransmissionStatus getCurrentEmployerTaxYearTransmissionStatusBeforeHalt(int employerId, int taxYearId)
    {
        employerFactory ef = new employerFactory();
        return ef.getCurrentEmployerTaxYearTransmissionStatusBeforeHalt(employerId, taxYearId);
    }

    public static List<IRSStatus> getIRSStatus(int employerId, int taxYearId)
    {
        employerFactory ef = new employerFactory();
        return ef.getIRSStatus(employerId, taxYearId);
    }

    /// <summary>
    /// Insert Employer Tax Year Transmission Status
    /// </summary>
    public static EmployerTaxYearTransmissionStatus insertUpdateEmployerTaxYearTransmissionStatus(EmployerTaxYearTransmissionStatus employerTaxYearTransmissionStatus)
    {
        employerFactory ef = new employerFactory();
        return ef.insertUpdateEmployerTaxYearTransmissionStatus(employerTaxYearTransmissionStatus);
    }

    public static Boolean insertUpdateTaxYear1095cCorrection(TaxYear1095CCorrection taxYear1095CCorrection)
    {
        employerFactory ef = new employerFactory();
        return ef.insertUpdateTaxYear1095cCorrection(taxYear1095CCorrection);
    }

    public static Boolean updateTaxYear1095cCorrectionCorrectedBit(TaxYear1095CCorrection taxYear1095CCorrection)
    {
        employerFactory ef = new employerFactory();
        return ef.updateTaxYear1095cCorrectionCorrectedBit(taxYear1095CCorrection);
    }

    public static Boolean updateTaxYear1095cCorrectionTransmittedBit(TaxYear1095CCorrection taxYear1095CCorrection)
    {
        employerFactory ef = new employerFactory();
        return ef.updateTaxYear1095cCorrectionTransmittedBit(taxYear1095CCorrection);
    }

    public static Boolean stageTaxYear1095cCorrection(int employerId, int taxYearId, String modifiedBy)
    {
        employerFactory ef = new employerFactory();
        return ef.stageTaxYear1095cCorrection(employerId, taxYearId, modifiedBy);
    }

    public static EmployerTaxYearTransmissionStatus endEmployerTaxYearTransmissionStatus(EmployerTaxYearTransmissionStatus employerTaxYearTransmissionStatus, string UserName)
    {
        employerTaxYearTransmissionStatus.EndDate = DateTime.Now;
        employerTaxYearTransmissionStatus.ModifiedBy = UserName;
        employerFactory ef = new employerFactory();
        return ef.insertUpdateEmployerTaxYearTransmissionStatus(employerTaxYearTransmissionStatus);
    }

    public static EmployerTaxYearTransmissionStatusQueue insertUpdateEmployerTaxYearTransmissionStatusQueue(EmployerTaxYearTransmissionStatusQueue employerTaxYearTransmissionStatusQueue)
    {
        employerFactory ef = new employerFactory();
        return ef.insertUpdateEmployerTaxYearTransmissionStatusQuene(employerTaxYearTransmissionStatusQueue);
    }

    public static List<EmployerTaxYearTransmissionStatusQueue> getEmployerTaxYearTransmissionStatusQueues(int batchSize)
    {
        employerFactory ef = new employerFactory();
        return ef.getEmployerTaxYearTransmissionStatusQueues(batchSize);
    }

    public static List<EmployerTaxYearTransmissionStatus> getEmployerTaxYearTransmissionStatusesByEmployerIdAndTaxYearId(int employerId, int taxYearId)
    {
        employerFactory ef = new employerFactory();
        return ef.getEmployerTaxYearTransmissionStatusesByEmployerIdAndTaxYearId(employerId, taxYearId);
    }

    public static List<EmployersCurrentTaxYearTransmissionStatus> getEmployersCurrentTaxYearTransmissionStatus(int taxYearId)
    {
        employerFactory ef = new employerFactory();
        return ef.getEmployersCurrentTaxYearTransmissionStatus(taxYearId);
    }

    public static List<Form1094CUpstreamDetail> getForm1094CUpstreamDetails(int taxYearId, int? employerId, bool corrected1094, bool corrected1095, bool rejectedInd)
    {
        employerFactory ef = new employerFactory();
        return ef.getForm1094CUpstreamDetails(taxYearId, employerId, corrected1094, corrected1095, rejectedInd);
    }

    public static DataTable ExportCSV(int employerId, int taxYearId)
    {
        employerFactory ef = new employerFactory();
        return ef.ExportCSV(employerId, taxYearId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_planYearID"></param>
    /// <param name="_generatePlanYearID"></param>
    /// <param name="_stabilityEndDate"></param>
    /// <param name="_hours"></param>
    /// <param name="_modBy"></param>
    /// <param name="_modOn"></param>
    /// <param name="_history"></param>
    /// <returns></returns>
    public static int generateMissingInsuranceAlerts(int _employerID, int _planYearID, int _generatePlanYearID, DateTime _stabilityEndDate, int _hours, string _modBy, DateTime _modOn, string _history)
    {
        employerFactory ef = new employerFactory();
        return ef.generateMissingInsuranceAlerts(_employerID, _planYearID, _generatePlanYearID, _stabilityEndDate, _hours, _modBy, _modOn, _history);
    }

    public static List<int> getTaxYears()
    {
        employerFactory ef = new employerFactory();
        return ef.getTaxYears();
    }

    /// <summary>
    /// Validates the Given Id against the list of possible choices to ensure it is correct
    /// </summary>
    /// <param name="tempEmployeeTypeList">Possible choices</param>
    /// <param name="_employeeTypeId">The Id Chosen</param>
    /// <returns>The Id Chosen or zero if invalid</returns>
    public static int validateEmployerEmployeeTypes(List<EmployeeType> tempEmployeeTypeList, int _employeeTypeId)
    {
        foreach (EmployeeType type in tempEmployeeTypeList) 
        {
            if (type.EMPLOYEE_TYPE_ID == _employeeTypeId) 
            {
                return _employeeTypeId;
            }
        }

        return 0;
    }
    
    public static bool BulkUpdatePrintedStatus(
            DataTable data,
            int taxYear)
    {
        employerFactory ef = new employerFactory();
        return ef.BulkUpdatePrintedStatus(data, taxYear);
    }

    public static List<object> GetPrintedCountPerEmployer(int taxYear)
    {
        employerFactory ef = new employerFactory();
        return ef.GetPrintedCountPerEmployer(taxYear);
    }

    public static List<object> GetNotPrintedCountPerEmployer(int taxYear)
    {
        employerFactory ef = new employerFactory();
        return ef.GetNotPrintedCountPerEmployer(taxYear);
    }

    public static List<object> GetNotTransmittedCountPerEmployer(int taxYear)
    {
        employerFactory ef = new employerFactory();
        return ef.GetNotTransmittedCountPerEmployer(taxYear);
    }

    public static header insertUpdateHeader(header header)
    {
        employerFactory ef = new employerFactory();
        return ef.insertUpdateHeader(header);
    }

    public static manifest insertUpdateManifest(manifest manifest)
    {
        employerFactory ef = new employerFactory();
        return ef.insertUpdateManifest(manifest);
    }

    public static _1094C insertUpdate1094C(_1094C _1094C)
    {
        employerFactory ef = new employerFactory();
        return ef.insertUpdate1094C(_1094C);
    }

    public static _1095C insertUpdate1095C(_1095C _1095C)
    {
        employerFactory ef = new employerFactory();
        return ef.insertUpdate1095C(_1095C);
    }

    public static status_request insertUpdateStatusRequest(status_request status_request)
    {
        employerFactory ef = new employerFactory();
        return ef.insertUpdateStatusRequest(status_request);
    }

    public static request_error insertUpdateRequestError(request_error request_error)
    {
        employerFactory ef = new employerFactory();
        return ef.insertUpdateRequestError(request_error);
    }

    public static List<long> GetTransmittedApprovedIds(int employerId)
    {

        employerFactory ef = new employerFactory();

        return ef.GetTransmittedApprovedIds(employerId);

    }

    /// <summary>
    /// Filter down the employer list by search text. 
    /// </summary>
    /// <param name="_searchText"></param>
    /// <param name="_employerList"></param>
    /// <returns></returns>
    public static List<employer> FilterEmployerBySearch(string _searchText, List<employer> _employerList)
    {
        employerShow es = new employerShow();
        return es.FilterEmployerBySearch(_searchText, _employerList);
    }

     /// <summary>
    /// Create 7/11/2017 Travis Wells: Updates the receipt ID in the new table, tax_year_employer_transmission.
    /// </summary>
    /// <param name="employerid"></param>
    /// <param name="taxyear"></param>
    /// <param name="uniqueTransmissionid"></param>
    /// <param name="receiptID"></param>
    /// <param name="modBy"></param>
    /// <param name="modOn"></param>
    /// <returns></returns>
    public static bool updateTaxYearEmployerTransmissionReceipt(int employerid, int taxyear, string uniqueTransmissionid, string receiptID, string modBy, DateTime modOn)
    {
        employerFactory ef = new employerFactory();
        return ef.updateTaxYearEmployerTransmissionReceipt(employerid, taxyear, uniqueTransmissionid, receiptID, modBy, modOn);
    }

    public static bool updateTaxYearEmployerTransmissionStatus(int employerid, int taxyear, string receiptID, string modBy, DateTime modOn, int transmissionStatusID, List<int> errantRecords, string ackFileName)
    {
        employerFactory ef = new employerFactory();
        return ef.updateTaxYearEmployerTransmissionStatus(employerid, taxyear, receiptID, modBy, modOn, transmissionStatusID, errantRecords, ackFileName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static List<TransmissionType> manufactureTransmissionType()
    {
        employerFactory ef = new employerFactory();
        return ef.manufactureTransmissionType();
    }

    public static DataTable GetEmployer1094CFilePath(int employerid)
    {
        employerFactory ef = new employerFactory();
        return ef.GetEmployer1094CFilePath(employerid);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employerid"></param>
    /// <returns></returns>
    public static DataTable GetEmployersInfo()
    {
        employerFactory ef = new employerFactory();
        return ef.GetEmployersInfo();
    }
    public static DataTable GetEmployerTransmitReport(DateTime startDate, DateTime endDate)
    {
        employerFactory ef = new employerFactory();
        return ef.GetEmployerTransmitReport(startDate, endDate);
    }
    public static DataTable GetTransmissionStatus()
    {
        employerFactory ef = new employerFactory();
        return ef.GetTransmissionStatus();
    }
    public static List<EmployersMeasurementPeriodDetails> getAllEmployersMeasurementPeriodDetails()
    {
        employerFactory ef = new employerFactory();
        return ef.getAllEmployersMeasurementPeriodDetails();
    }

    public static bool UpdateEmployeeMeasurementAverageHoursEntityStatus(int _employerID, string modifiedBy)
    {
        employerFactory ef = new employerFactory();
        return ef.UpdateEmployeeMeasurementAverageHoursEntityStatus(_employerID, modifiedBy);
    }

    public static List<ash> get4980HReliefCodes()
    {
        employerFactory ef = new employerFactory();
        return ef.get4980HReliefCodes();
    }

    public static List<entityStatus> getEntityStatusCodes()
    {
        employerFactory ef = new employerFactory();
        return ef.getEntityStatusCodes();
    }
    public static Boolean InsertUpdateEmployerConsultant(
       string _name,
       string _title,
       int _phoneNumber,
       int _employerId,
       string _crtBy
       )
    {

        return new employerFactory().InsertUpdateEmployerConsultant(_name, _title, _phoneNumber, _employerId, _crtBy);

    }

}