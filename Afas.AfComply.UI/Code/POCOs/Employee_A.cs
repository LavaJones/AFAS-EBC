using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Insurance_Alert
/// </summary>
public class Employee_A : Employee
{
    private bool qualified = false;
    private DateTime? offered = null;
    private bool accepted = false;
    private DateTime modOn;
    private string modBy = null;
    private string notes = null;
    private string history = null;

	public Employee_A(int _employeeID, int _employeeTypeID, int _hrStatusID, int _employerID, string _fname, string _mname, string _lname, string _address, string _city, int _stateID, string _zip, DateTime _hdate, DateTime? _cdate, string _ssn, string _extEmployeeID, DateTime? _tdate, DateTime? _dob, DateTime _impEnd, int _pyID_curr, int _pyID_limbo, int _pyID_meas)
	{
        this.employee_id = _employeeID;
        this.employee_type_id = _employeeTypeID;
        this.hr_status_id = _hrStatusID;
        this.employer_id = _employerID;
        this.hr_status_id = _hrStatusID;
        this.fname = _fname;
        this.mname = _mname;
        this.lname = _lname;
        this.address = _address;
        this.city = _city;
        this.stateID = _stateID;
        this.zip = _zip;
        this.hdate = _hdate;
        this.cdate = _cdate;
        this.ssn = _ssn;
        this.employee_ext_id = _extEmployeeID;
        this.tdate = _tdate;
        this.dob = _dob;
        this.impEnd = _impEnd;
        this.planYearID = _pyID_curr;
        this.planYearID_limbo = _pyID_limbo;
        this.planYearID_measurement = _pyID_meas;
	}

}