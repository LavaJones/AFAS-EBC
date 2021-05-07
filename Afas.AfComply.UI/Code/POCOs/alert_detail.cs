using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for alert_detail
/// </summary>
public class alert_detail
{

    private int id = 0;
    private string name = null;
    private string employee = null;
    private string warningURL = null;
    private bool error = true;
    private string employeeLink = null;
    private string endDate = null;
    private string projDate = null;

	public alert_detail(string _employee, string _name, string _endDate, string _warningURL, string _projDate)
	{
        this.name = _name;
        this.employee = _employee;
        this.endDate = _endDate;
        this.warningURL = _warningURL;
        this.projDate = _projDate;
	}

    public string ALERT_NAME
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

    public string ALERT_EMP
    {
        get
        {
            return this.employee;
        }
        set
        {
            this.employee = value;
        }
    }

    public bool ALERT_ERROR
    {
        get
        {
            return this.error;
        }
        set
        {
            this.error = value;
        }
    }

    public string ALERT_ENDDATE
    {
        get
        {
            return this.endDate;
        }
        set
        {
            this.endDate = value;
        }
    }

    public string ALERT_URL
    {
        get
        {
            return this.warningURL;
        }
        set
        {
            this.warningURL = value;
        }
    }

    public string ALERT_PROJ
    {
        get
        {
            return this.projDate;
        }
        set
        {
            this.projDate = value;
        }
    }

}