using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Employee
/// </summary>
public class Employee_I : Employee
{
    /******************************************************************************************************************
    *******************  Imported Employee Attributes ***************************************************************** 
    ******************************************************************************************************************/
    private int row_id = 0;                                    
    private int hr_status_id = 0;                              
    private string hr_ext_id = null;                           
    private string hr_ext_description = null;                 
    private string stateAbb = null;                          
    private string i_hdate = null;                           
    private string i_cdate = null;                            
    private string i_tdate = null;                           
    private string i_dob = null;                            

	public Employee_I(int _rowID, int _measPlanYearID, int _employerID, string _ext_HR_ID, string _ext_HR_Desc, string _fname, string _mname, string _lname, string _address, string _city, string _stateAbb, string _zip, string _hdate, string _cdate, string _ssn, string _employee_ext_ID, string _tdate, string _dob)
	{
        this.planYearID_measurement = _measPlanYearID;
        this.row_id = _rowID;
        this.hr_ext_id = _ext_HR_ID;
        this.hr_ext_description = _ext_HR_Desc;
        this.employer_id = _employerID;
        this.fname = _fname;
        this.mname = _mname;
        this.lname = _lname;
        this.address = _address;
        this.city = _city;
        this.stateAbb = _stateAbb;
        this.zip = _zip;
        this.i_hdate = _hdate;
        this.i_cdate = _cdate;
        this.ssn = _ssn;
        this.employee_ext_id = _employee_ext_ID;
        this.i_tdate = _tdate;
        this.i_dob = _dob;
	}

    public int ROW_ID
    {
        get
        {
            return this.row_id;
        }
        set
        {
            this.row_id = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int EMPLOYEE_HR_STATUS_ID
    {
        get
        {
            return this.hr_status_id;
        }
        set
        {
            this.hr_status_id = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_HR_EXT_STATUS_ID
    {
        get
        {
            return this.hr_ext_id;
        }
        set
        {
            this.hr_ext_id = value;
        }
    }

    public string EMPLOYEE_HR_EXT_DESCRIPTION
    {
        get
        {
            return this.hr_ext_description;
        }
        set
        {
            this.hr_ext_description = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int EMPLOYEE_EMPLOYER_ID
    {
        get
        {
            return this.employer_id;
        }
        set
        {
            this.employer_id = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_STATE_ABB
    {
        get
        {
            return this.stateAbb;
        }
        set
        {
            this.stateAbb = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_I_HIRE_DATE
    {
        get
        {
            return this.i_hdate;
        }
        set
        {
            this.i_hdate = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_I_C_DATE
    {
        get
        {
            return this.i_cdate;
        }
        set
        {
            this.i_cdate = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_I_TERM_DATE
    {
        get
        {
            return this.i_tdate;
        }
        set
        {
            this.i_tdate = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_I_DOB
    {
        get
        {
            return this.i_dob;
        }
        set
        {
            this.i_dob = value;
        }
    }
}