using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for insuranceType
/// </summary>
public class insuranceType
{
    private int id = 0;
    private string name = null;

	public insuranceType(int _id, string _name)
	{
        this.id = _id;
        this.name = _name;
	}

    public int INSURANCE_TYPE_ID
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

    public string INSURANCE_TYPE_NAME
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