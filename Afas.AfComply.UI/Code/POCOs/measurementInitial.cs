using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for measurementInitial
/// </summary>
public class measurementInitial
{
    private int id = 0;
    private string name = null;
    private int months = 0;

	public measurementInitial(int _id, string _name, int _months)
	{
        this.id = _id;
        this.name = _name;
        this.months = _months;
	}

    public int INITIAL_ID
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

    public string INITIAL_NAME
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

    public int INITIAL_MONTHS
    {
        get
        {
            return this.months;
        }
        set
        {
            this.months = value;
        }
    }
}