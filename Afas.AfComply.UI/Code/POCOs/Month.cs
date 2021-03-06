using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for months
/// </summary>
public class Month
{

    private int id = 0;
    private string name = null;

	public Month(int _id, string _name)
	{
        this.id = _id;
        this.name = _name;
	}

    public int MONTH_ID
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

    public string MONTH_NAME
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
}