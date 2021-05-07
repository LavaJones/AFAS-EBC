using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ale
/// </summary>
public class status
{
    private int id = 0;
    private string name = null;

	public status(int _id, string _name)
	{
        this.id = _id;
        this.name = _name;
	}

    public int STATUS_ID
    {
        get {return this.id;}
        set { this.id = value; }
    }

    public string STATUS_NAME
    {
        get { return this.name; }
        set { this.name = value; }
    }

}