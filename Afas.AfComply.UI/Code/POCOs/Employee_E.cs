using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Employee_E
/// </summary>
public class Employee_E : Employee
{
    string className = null;
    string hrStatusName = null;
    string acaStatusName = null;

	public Employee_E(int _employeeID, int _employeeTypeID, int _hrStatusID, int _employerID, string _fname, string _mname, string _lname, string _address, string _city, int _stateID, string _zip, DateTime? _hdate, DateTime? _cdate, string _ssn, string _extEmployeeID, DateTime? _tdate, DateTime? _dob, DateTime? _impEnd, int _pyID_curr, int _pyID_limbo, int _pyID_meas, double _pyAvg, double _pyAvgLimbo, double _pyAvgMeas, double _pyAvgInit, int _classID, int _actID, string _className, string _hrStatusName, string _acaStatusName)
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
        this.planYearStabilityAvgHours = _pyAvg;
        this.planYearAdminAvgHours = _pyAvgLimbo;
        this.planYearMeasAvgHours = _pyAvgMeas;
        this.planYearInitAvgHours = _pyAvgInit;
        this.classificationID = _classID;
        this.actStatusID = _actID;
        this.className = _className;
        this.hrStatusName = _hrStatusName;
        this.acaStatusName = _acaStatusName;
	}

    public string EX_CLASS_NAME
    {
        get
        {
            return this.className;
        }
        set
        {
            this.className = value;
        }
    }

    public string EX_HR_STATUS_NAME
    {
        get
        {
            return this.hrStatusName;
        }
        set
        {
            this.hrStatusName = value;
        }
    }

    public string EX_ACA_NAME
    {
        get
        {
            return this.acaStatusName;
        }
        set
        {
            this.acaStatusName = value;
        }
    }
}