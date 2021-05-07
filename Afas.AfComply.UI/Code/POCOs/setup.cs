using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for setup
/// </summary>
public class setup
{
    private string name = null;
    private int id = 0;

	public setup(string _name, int _id)
	{
        this.name = _name;
        this.id = _id;
	}

    public string SETUP_NAME
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

    public int SETUP_ID
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
}