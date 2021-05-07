using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Measurement
/// </summary>
public class Measurement
{
    private int id = 0;
    private int employerID = 0;
    private int planyearID = 0;
    private int employeetypeID = 0;
    private int measurementtypeID = 0;
    private DateTime meas_start = System.DateTime.Now.AddYears(-50);
    private DateTime meas_end = System.DateTime.Now.AddYears(-50);
    private DateTime admin_end = System.DateTime.Now.AddYears(-50);
    private DateTime admin_start = System.DateTime.Now.AddYears(-50);
    private DateTime open_end = System.DateTime.Now.AddYears(-50);
    private DateTime open_start = System.DateTime.Now.AddYears(-50);
    private DateTime stab_end = System.DateTime.Now.AddYears(-50);
    private DateTime stab_start = System.DateTime.Now.AddYears(-50);
    private string notes = null;
    private string history = null;
    private DateTime modOn = System.DateTime.Now.AddYears(-50);
    private string modBy = null;
    private DateTime? swStart = null;
    private DateTime? swEnd = null;
    private DateTime? swStart2 = null;
    private DateTime? swEnd2 = null;
    private bool meas_complete = false;
    private bool admin_complete = false;
    private string meas_completed_by = null;
    private string admin_completed_by = null;
    private DateTime? meas_completed_on = null;
    private DateTime? admin_completed_on = null;

	public Measurement(int _id, int _employerID, int _planyearID, int _employeeTypeID, int _measurementTypeID, DateTime _meas_start, DateTime _meas_end, DateTime _admin_start, DateTime _admin_end, DateTime _open_start, DateTime _open_end, DateTime _stab_start, DateTime _stab_end, string _notes, string _history, DateTime _modOn, string _modBy, DateTime? _swStart, DateTime? _swEnd, DateTime? _swStart2, DateTime? _swEnd2, bool _measComp, bool _adminComp, string _measCompBy, string _adminCompBy, DateTime? _measCompOn, DateTime? _adminCompOn)
	{
        this.id = _id;
        this.employerID = _employerID;
        this.planyearID = _planyearID;
        this.employeetypeID = _employeeTypeID;
        this.measurementtypeID = _measurementTypeID;
        this.meas_start = _meas_start;
        this.meas_end = _meas_end;
        this.admin_start = _admin_start;
        this.admin_end = _admin_end;
        this.open_start = _open_start;
        this.open_end = _open_end;
        this.stab_start = _stab_start;
        this.stab_end = _stab_end;
        this.notes = _notes;
        this.history = _history;
        this.modOn = _modOn;
        this.modBy = _modBy;
        this.swStart = _swStart;
        this.swEnd = _swEnd;
        this.swStart2 = _swStart2;
        this.swEnd2 = _swEnd2;
        this.admin_complete = _adminComp;
        this.meas_complete = _measComp;
        this.admin_completed_by = _adminCompBy;
        this.meas_completed_by = _measCompBy;
        this.admin_completed_on = _adminCompOn;
        this.meas_completed_on = _measCompOn;
	}

    public int MEASUREMENT_ID
    {
        get
        {
            return this.id;
        }
        set
        {
            this.id = value;
        }
    }

    public int MEASUREMENT_EMPLOYER_ID
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

    public int MEASUREMENT_PLAN_ID
    {
        get
        {
            return this.planyearID;
        }
        set
        {
            this.planyearID = value;
        }
    }

    public int MEASUREMENT_EMPLOYEE_TYPE_ID
    {
        get
        {
            return this.employeetypeID;
        }
        set
        {
            this.employeetypeID = value;
        }
    }

    public int MEASUREMENT_TYPE_ID
    {
        get
        {
            return this.measurementtypeID;
        }
        set
        {
            this.measurementtypeID = value;
        }
    }

    public DateTime MEASUREMENT_START
    {
        get
        {
            return this.meas_start;
        }
        set
        {
            this.meas_start = value;
        }
    }

    public DateTime MEASUREMENT_END
    {
        get
        {
            return this.meas_end;
        }
        set
        {
            this.meas_end = value;
        }
    }

    public DateTime MEASUREMENT_ADMIN_START
    {
        get
        {
            return this.admin_start;
        }
        set
        {
            this.admin_start = value;
        }
    }

    public DateTime MEASUREMENT_ADMIN_END
    {
        get
        {
            return this.admin_end;
        }
        set
        {
            this.admin_end = value;
        }
    }

    public DateTime MEASUREMENT_OPEN_START
    {
        get
        {
            return this.open_start;
        }
        set
        {
            this.open_start = value;
        }
    }

    public DateTime MEASUREMENT_OPEN_END
    {
        get
        {
            return this.open_end;
        }
        set
        {
            this.open_end = value;
        }
    }

    public DateTime MEASUREMENT_STAB_START
    {
        get
        {
            return this.stab_start;
        }
        set
        {
            this.stab_start = value;
        }
    }

    public DateTime MEASUREMENT_STAB_END
    {
        get
        {
            return this.stab_end;
        }
        set
        {
            this.stab_end = value;
        }
    }

    public string MEASUREMENT_NOTES
    {
        get
        {
            return this.notes;
        }
        set
        {
            this.notes = value;
        }
    }

    public string MEASUREMENT_HISTORY
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

    public DateTime MEASUREMENT_MOD_ON
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

    public string MEASUREMENT_MOD_BY
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

    public DateTime? MEASUREMENT_MEAS_COMP_ON
    {
        get
        {
            return this.meas_completed_on;
        }
        set
        {
            this.meas_completed_on = value;
        }
    }

    public DateTime? MEASUREMENT_ADMIN_COMP_ON
    {
        get
        {
            return this.admin_completed_on;
        }
        set
        {
            this.admin_completed_on = value;
        }
    }

    public string MEASUREMENT_MEAS_COMP_BY
    {
        get
        {
            return this.meas_completed_by;
        }
        set
        {
            this.meas_completed_by = value;
        }
    }

    public string MEASUREMENT_ADMIN_COMP_BY
    {
        get
        {
            return this.admin_completed_by;
        }
        set
        {
            this.admin_completed_by = value;
        }
    }

    public bool MEASUREMENT_ADMIN_COMP
    {
        get
        {
            return this.admin_complete;
        }
        set
        {
            this.admin_complete = value;
        }
    }

    public bool MEASUREMENT_MEAS_COMP
    {
        get
        {
            return this.meas_complete;
        }
        set
        {
            this.meas_complete = value;
        }
    }
}