using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ooc
/// </summary>
public class ooc
{
    private string id = null;
    private string description = null;

	public ooc(string _id, string _notes)
	{
        this.id = _id;
        this.description = _notes;
	}

    public string OOC_ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    public string OOC_DESCRIPTION
    {
        get { return this.description; }
        set { this.description = value; }
    }
}