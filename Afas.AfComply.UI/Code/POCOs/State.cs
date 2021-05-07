using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for State
/// </summary>
public class State
{

    private int id = 0;
    private string abbr = null;
    private string name = null;

	public State(int _id, string _abbr, string _name)
	{
        this.id = _id;
        this.abbr = _abbr;
        this.name = _name;
	}

    public int State_ID
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

    public string State_Abbr
    {
        get
        {
            return this.abbr;
        }
        set
        {
            this.abbr = value;
        }
    }

    public string State_Name
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

