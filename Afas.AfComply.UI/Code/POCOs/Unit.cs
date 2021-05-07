using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Unit
/// </summary>
public class Unit
{
    private int id = 0;
    private string name = null;

	public Unit(int _id, string _name)
	{
        this.id = _id;
        this.name = _name;
	}

    public int UNIT_ID
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

    public string UNIT_NAME
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