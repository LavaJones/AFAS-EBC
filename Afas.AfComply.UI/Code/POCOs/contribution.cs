using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for contribution
/// </summary>
public class contribution
{

    private string id = null;
    private string name = null;

	public contribution(string _id, string _name)
	{
        this.id = _id;
        this.name = _name;
	}

    public string CONT_ID
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

    public string CONT_NAME
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