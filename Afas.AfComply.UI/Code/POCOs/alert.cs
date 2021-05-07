using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for alert
/// </summary>
public class alert
{
    private int id = 0;
    private string name = null;
    private int type_id = 0;
    private int employer_id = 0;
    private string tableName = null;
    private int count = 0;
    private string type_name = null;
    private string img_url = null;

	public alert(int _alertID, int _alertTypeID, string _alertName, int _employerID, string _tableName, int _count, string _typeName, string _imgurl)
	{
        this.id = _alertID;
        this.type_id = _alertTypeID;
        this.name = _alertName;
        this.employer_id = _employerID;
        this.tableName = _tableName;
        this.count = _count;
        this.type_name = _typeName;
        this.img_url = _imgurl;
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

    public int ALERT_TYPE_ID
    {
        get
        {
            return this.type_id;
        }
        set
        {
            this.type_id = value;
        }
    }

    public string ALERT_TYPE_NAME
    {
        get
        {
            return this.type_name;
        }
        set
        {
            this.type_name = value;
        }
    }

    public string ALERT_TYPE_IMG_URL
    {
        get
        {
            return this.img_url;
        }
        set
        {
            this.img_url = value;
        }
    }

    public string ALERT_COMB_NAME
    {
        get
        {
            return this.type_name + " - " + this.name;
        }
    }

    public int ALERT_EMPLOYER_ID
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

    public string ALERT_TABLE_NAME
    {
        get
        {
            return this.tableName;
        }
        set
        {
            this.tableName = value;
        }
    }

    public int ALERT_COUNT
    {
        get
        {
            return this.count;
        }
        set
        {
            this.count = value;
        }
    }

}