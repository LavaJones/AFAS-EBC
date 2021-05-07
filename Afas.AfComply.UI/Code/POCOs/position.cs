using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for position
/// </summary>
public class position
{
    private int id = 0;
    private string name = null;

	public position(int _id, string _name)
	{
        this.id = _id;
        this.name = _name;
	}

    public int POSITION_ID
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

    public string POSITION_NAME
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