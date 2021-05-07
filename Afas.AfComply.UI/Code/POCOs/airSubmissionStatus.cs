using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for airRequestError
/// </summary>
public class airSubmissionStatus
{
    private int id = 0;
    private string name = null;

	public airSubmissionStatus(int _id, string _name)
	{
        this.id = _id;
        this.name = _name;
	}

    /// <summary>
    /// AIR Submission status ID
    /// </summary>
    public int STATUS_ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    /// <summary>
    /// AIR Submission status Name. 
    /// </summary>
    public string STATUS_NAME
    {
        get { return this.name; }
        set { this.name = value; }
    }

  
}