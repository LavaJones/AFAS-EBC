using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for report
/// </summary>
public class report
{
    private string name = null;
    private int id = 0;
  
	public report(string _name, int _id)
	{
        this.name = _name;
        this.id = _id;
	}

    public string REPORT_NAME
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

    public int REPORT_ID
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