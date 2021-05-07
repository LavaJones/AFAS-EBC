using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Payroll_Controller
/// </summary>
public static class Payroll_Controller
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_batchID"></param>
    /// <param name="_employerID"></param>
    /// <param name="_fname"></param>
    /// <param name="_mname"></param>
    /// <param name="_lname"></param>
    /// <param name="_hours"></param>
    /// <param name="_sdate"></param>
    /// <param name="_edate"></param>
    /// <param name="_ssn"></param>
    /// <param name="_gpDesc"></param>
    /// <param name="_gpID"></param>
    /// <param name="_cdate"></param>
    /// <param name="_modBy"></param>
    /// <param name="_modOn"></param>
    /// <returns></returns>
    public static bool importPayroll(int _batchID, int _employerID, string _fname, string _mname, string _lname, string _hours, string _sdate, string _edate, string _ssn, string _gpDesc, string _gpID, string _cdate, string _modBy, DateTime _modOn, string _empExtID)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.importPayroll(_batchID, _employerID, _fname, _mname, _lname, _hours, _sdate, _edate, _ssn, _gpDesc, _gpID, _cdate, _modBy, _modOn, _empExtID);
    }

    public static bool BulkImportPayroll(DataTable payroll)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.BulkImportPayroll(payroll);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <returns></returns>
    public static List<Payroll_I> manufactureEmployerPayrollImportList(int _employerID, int? rowLimit = null)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.manufactureEmployerPayrollImportList(_employerID, rowLimit);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_batchID"></param>
    /// <returns></returns>
    public static bool deleteImportedPayrollBatch(int _batchID, string _modBy, DateTime _modOn)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.deleteImportedPayrollBatch(_batchID, _modBy, _modOn);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_rowID"></param>
    /// <returns></returns>
    public static bool deleteImportedPayrollRow(int _rowID)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.deleteImportedPayrollRow(_rowID);
    }

    public static bool deletePayroll(int _rowID, string _modBy, DateTime _modOn)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.deletePayroll(_rowID, _modBy, _modOn);
    }

    public static Boolean UpdateImportPayroll(
            int _rowID,
            int _employeeID,
            int _gpdID,
            decimal _hours,
            DateTime? _sdate,
            DateTime? _edate,
            DateTime? _cdate,
            String _modBy,
            DateTime _modOn
        )
    {

        Payroll_Factory pf = new Payroll_Factory();

        return pf.UpdateImportPayroll(_rowID, _employeeID, _gpdID, _hours, _sdate, _edate, _cdate, _modBy, _modOn);

    }

    public static Boolean BulkUpdateImportPayroll(DataTable payroll)
    {

        Payroll_Factory pf = new Payroll_Factory();

        return pf.BulkUpdateImportPayroll(payroll);

    }

    public static Boolean TransferPayroll(
            int _rowID,
            int _employerID,
            int _batchID,
            int _employeeID,
            int _gpID,
            decimal _hours,
            DateTime _sdate,
            DateTime _edate,
            DateTime _cdate,
            String _modBy,
            DateTime _modOn,
            String _history
        )
    {

        Payroll_Factory pf = new Payroll_Factory();

        return pf.TransferPayroll(_rowID, _employerID, _batchID, _employeeID, _gpID, _hours, _sdate, _edate, _cdate, _modBy, _modOn, _history);

    }

    public static Boolean BulkTransferPayroll(DataTable payroll, String _history)
    {
        Payroll_Factory pf = new Payroll_Factory();

        return pf.BulkTransferPayroll(payroll, _history);
    }    

    public static List<Payroll> getEmployeePayroll(int _employeeID2, DateTime _mStart, DateTime _mEnd)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.getEmployeePayroll(_employeeID2, _mStart, _mEnd);
    }

    public static List<Payroll> getEmployerPayroll(int _employerId)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.getEmployerPayroll(_employerId);
    }

    public static List<Payroll> getEmployerDuplicatePayroll(int _employerID)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.getEmployerDuplicatePayroll(_employerID);
    }

    public static Payroll_I getSinglePayroll_I(int _rowID, List<Payroll_I> _tempList)
    {
        Payroll_Show ps = new Payroll_Show();
        return ps.getSinglePayroll(_rowID, _tempList);
    }

    public static Payroll_E getSinglePayroll(int _rowID, List<Payroll_E> _tempList)
    {
        Payroll_Show ps = new Payroll_Show();
        return ps.getSinglePayroll(_rowID, _tempList);
    }

    public static Payroll getSinglePayroll(int _rowID, List<Payroll> _tempList)
    {
        Payroll_Show ps = new Payroll_Show();
        return ps.getSinglePayroll(_rowID, _tempList);
    }

    public static Payroll manufactureSinglePayroll(int _batchID, int _employeeID, int _employerID, int _gpdID, decimal _hours, DateTime? _sdate, DateTime? _edate, DateTime? _cdate, string _modBy, DateTime _modOn, string _history)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.ManufactureSinglePayroll(_batchID, _employeeID, _employerID, _gpdID, _hours, _sdate, _edate, _cdate, _modBy, _modOn, _history);
    }

    public static bool updatePayroll(int _rowID, int _employerID, int _employeeID, decimal _hours, DateTime? _sdate, DateTime? _edate, string _modBy, DateTime _modOn, string _history)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.updatePayroll(_rowID, _employerID, _employeeID, _hours, _sdate, _edate, _modBy, _modOn, _history);
    }

    public static string generateTextFile(List<Payroll_I> _tempList, employer _employer)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.generateTextFile(_tempList, _employer);
    }

    public static Boolean process_PAY_I_files(int _employerID, String _modBy, DateTime _modOn, String _filePath, String _fileName)
    {

        Payroll_Factory pf = new Payroll_Factory();

        return pf.process_PAY_I_files(_employerID, _modBy, _modOn, _filePath, _fileName);

    }

    public static void CrossReferenceData(int _employerID, String _modBy, DateTime _modOn)
    {

        Payroll_Factory pf = new Payroll_Factory();

        pf.CrossReferenceData(_employerID, _modBy, _modOn);

    }

    public static void TransferPayrollRecords(int _employerID, String _modBy, DateTime _modOn)
    {

        Payroll_Factory pf = new Payroll_Factory();

        pf.TransferPayrollRecords(_employerID, _modBy, _modOn);

    }

    public static List<Payroll> getEmployeePayrollSummerAverages(int _employeeID2, int _planYearID)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.getEmployeePayrollSummerAverages(_employeeID2, _planYearID);
    }

    public static bool deleteSummerAveragePayroll(int _rowID, string _modBy, DateTime _modOn)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.deleteSummerAveragePayroll(_rowID, _modBy, _modOn);
    }

    public static Payroll ManufacturePayrollSummerAverage(int _batchID, int _employeeID, int _employerID, int _gpdID, decimal _hours, DateTime? _sdate, DateTime? _edate, DateTime? _cdate, string _modBy, DateTime _modOn, string _history, int _planYearID)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.ManufacturePayrollSummerAverage(_batchID, _employeeID, _employerID, _gpdID, _hours, _sdate, _edate, _cdate, _modBy, _modOn, _history, _planYearID);
    }

    /// <summary>
    /// This function will return an integer list of Employee ID's who have a payroll record with the passed in
    /// Gross Pay ID. 
    /// </summary>
    /// <param name="_grossPayID"></param>
    /// <returns></returns>
    public static List<int> getEmployeeIDforPayrollGrossPayID(int _grossPayID)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.getEmployeeIDforPayrollGrossPayID(_grossPayID);
    }

    /// <summary>
    /// Return all applicable check dates.
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_msDate"></param>
    /// <returns></returns>
    public static List<string> getEmployerCheckDates(int _employerID, DateTime _msDate)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.getEmployerCheckDates(_employerID, _msDate);
    }


    public static List<Payroll_E> getPayrollbyBatchID(int _batchID, int _employerID)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.getPayrollbyBatchID(_batchID, _employerID);
    }

    public static string generateTextFile(List<Payroll_E> _tempList, employer _employer)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.generateTextFile(_tempList, _employer);
    }

    public static bool process_PAY_E_files(int _employerID, string _modBy, DateTime _modOn, string _filePath, string _fileName)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.process_PAY_E_files(_employerID, _modBy, _modOn, _filePath, _fileName);
    }


    public static DataTable GetNewBulkPayrollDataTable()
    {
        DataTable payroll = new DataTable();

        payroll.Columns.Add("rowid", typeof(int));
        payroll.Columns.Add("employerid", typeof(int));
        payroll.Columns.Add("batchid", typeof(int));
        payroll.Columns.Add("employee_id", typeof(int));
        payroll.Columns.Add("fname", typeof(string));
        payroll.Columns.Add("mname", typeof(string));
        payroll.Columns.Add("lname", typeof(string));
        payroll.Columns.Add("i_hours", typeof(string));
        payroll.Columns.Add("hours", typeof(decimal));
        payroll.Columns.Add("i_sdate", typeof(string));
        payroll.Columns.Add("sdate", typeof(DateTime));
        payroll.Columns.Add("i_edate", typeof(string));
        payroll.Columns.Add("edate", typeof(DateTime));
        payroll.Columns.Add("ssn", typeof(string));
        payroll.Columns.Add("gp_description", typeof(string));
        payroll.Columns.Add("gp_ext_id", typeof(string));
        payroll.Columns.Add("gp_id", typeof(int));
        payroll.Columns.Add("i_cdate", typeof(string));
        payroll.Columns.Add("cdate", typeof(DateTime));
        payroll.Columns.Add("modBy", typeof(string));
        payroll.Columns.Add("modOn", typeof(DateTime));
        payroll.Columns.Add("ext_employee_id", typeof(string));

        return payroll;
    }

    /// <summary>
    /// This function is used to migrate a payroll record from one employee to another. 
    /// </summary>
    /// <param name="_rowID"></param>
    /// <param name="_employerID"></param>
    /// <param name="_employeeID"></param>
    /// <param name="_modBy"></param>
    /// <param name="_modOn"></param>
    /// <param name="_history"></param>
    /// <returns></returns>
    public static bool migratePayrollSingle(int _rowID, int _employerID, int _employeeID, string _modBy, DateTime _modOn, string _history)
    {
        Payroll_Factory pf = new Payroll_Factory();
        return pf.migratePayrollSingle(_rowID, _employerID, _employeeID, _modBy, _modOn, _history);
    }
}