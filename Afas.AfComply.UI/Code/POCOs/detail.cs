using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for detail
/// </summary>
public class detail
{
    private int id = 0;
    private string name = null;
    private int activityID = 0;
    private DateTime? sDate = null;
    private DateTime? eDate = null;
    private decimal every = 0;
    private int unitID = 0;
    private decimal hours = 0;

	public detail(int _id, string _name, int _activityID, DateTime? _sDate, DateTime? _eDate, decimal _every, int _unitID, decimal _hours)
	{
        this.id = _id;
        this.name = _name;
        this.activityID = _activityID;
        this.sDate = _sDate;
        this.eDate = _eDate;
        this.every = _every;
        this.unitID = _unitID;
        this.hours = _hours;
	}

    public int DETAIL_ID
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

    public string DETAIL_NAME
    {
        get
        {
            return this.name;
        }
        set
        {
            this.name = value;
        }
    }

    public int DETAIL_ACTIVITY_ID
    {
        get
        {
            return this.activityID;
        }
        set
        {
            this.activityID = value;
        }
    }

    public DateTime? DETAIL_START_DATE
    {
        get
        {
            return this.sDate;
        }
        set
        {
            this.sDate = value;
        }
    }

    public DateTime? DETAIL_END_DATE
    {
        get
        {
            return this.eDate;
        }
        set
        {
            this.eDate = value;
        }
    }

    public decimal DETAIL_EVERY
    {
        get
        {
            return this.every;
        }
        set
        {
            this.every = value;
        }
    }

    public int DETAIL_UNIT_ID
    {
        get
        {
            return this.unitID;
        }
        set
        {
            this.unitID = value;
        }
    }

    public decimal DETAIL_HOURS
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
}