using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for alert_type
/// </summary>
public class alert_type
{
    private int id = 0;
    private string name = null;
    private string url = null;
    private string table = null;

	public alert_type(int _id, string _name, string _url, string _table)
	{
        this.id = _id;
        this.name = _name;
        this.url = _url;
        this.table = _table;
	}

    public int ALERT_ID
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

    public string ALERT_URL
    {
        get
        {
            return this.url;
        }
        set
        {
            this.url = value;
        }
    }

    public string ALERT_TABLE
    {
        get
        {
            return this.table;
        }
        set
        {
            this.table = value;
        }
    }

}