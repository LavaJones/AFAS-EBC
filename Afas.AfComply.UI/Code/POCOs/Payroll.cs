using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Payroll
/// </summary>
public class Payroll
{
    protected int rowID = 0;
    protected int employerID = 0;
    protected int employeeID = 0;
    protected int batchID = 0;
    protected decimal hours = 0;
    protected DateTime? sdate;
    protected DateTime? edate;
    protected int gpID = 0;
    protected DateTime? cdate;
    protected string modBy = null;
    protected DateTime modOn;
    protected string desc = null;
    protected string history = null;

    public Payroll()
    { }

	public Payroll(int _rowID, int _employerID,int _batchID, int _employeeID, decimal _hours, DateTime? _sdate, DateTime? _edate, int _gpID, DateTime? _cdate, string _modBy, DateTime _modOn, string _desc, string _history)
	{
        this.rowID = _rowID;
        this.employerID = _employerID;
        this.employeeID = _employeeID;
        this.batchID = _batchID;
        this.hours = _hours;
        this.sdate = _sdate;
        this.edate = _edate;
        this.gpID = _gpID;
        this.cdate = _cdate;
        this.modBy = _modBy;
        this.modOn = _modOn;
        this.desc = _desc;
        this.history = _history;
	}

    public int ROW_ID
    {
        get
        {
            return this.rowID;
        }
        set
        {
            this.rowID = value;
        }
    }

    public int PAY_EMPLOYER_ID
    {
        get
        {
            return this.employerID;
        }
        set
        {
            this.employerID = value;
        }
    }


    public int PAY_EMPLOYEE_ID
    {
        get
        {
            return this.employeeID;
        }
        set
        {
            this.employeeID = value;
        }
    }

    public int PAY_BATCH_ID
    {
        get
        {
            return this.batchID;
        }
        set
        {
            this.batchID = value;
        }
    }

    public decimal PAY_HOURS
    {
        get
        {
            return this.hours;
        }
        set
        {
            this.hours = value;
        }
    }

    public DateTime? PAY_SDATE
    {
        get
        {
            return this.sdate;
        }
        set
        {
            this.sdate = value;
        }
    }

    public DateTime? PAY_EDATE
    {
        get
        {
            return this.edate;
        }
        set
        {
            this.edate = value;
        }
    }

    public int PAY_GP_ID
    {
        get
        {
            return this.gpID;
        }
        set
        {
            this.gpID = value;
        }
    }

    public string PAY_GP_NAME
    {
        get
        {
            return this.desc;
        }
        set
        {
            this.desc = value;
        }
    }

    public DateTime? PAY_CDATE
    {
        get
        {
            return this.cdate;
        }
        set
        {
            this.cdate = value;
        }
    }

    public string PAY_MOD_BY
    {
        get
        {
            return this.modBy;
        }
        set
        {
            this.modBy = value;
        }
    }

    public DateTime PAY_MOD_ON
    {
        get
        {
            return this.modOn;
        }
        set
        {
            this.modOn = value;
        }
    }

    public string PAY_HISTORY
    {
        get
        {
            return this.history;
        }
        set
        {
            this.history = value;
        }
    }
}