using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for employer_type
/// </summary>
public class employer_type
{
    private int id = 0;
    private string name = null;
    private bool active = true;

	public employer_type(int _id, string _name)
	{
        this.id = _id;
        this.name = _name;
	}

    public int EMPLOYER_TYPE_ID
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

    public string EMPLOYER_TYPE_NAME
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

    public bool EMPLOYER_TYPE_ACTIVE
    {
        get
        {
            return this.active;
        }
        set
        {
            this.active = value;
        }
    }
}