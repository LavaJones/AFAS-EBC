using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for hrStatus
/// </summary>
public class hrStatus
{
    private int id = 0;
    private int employerID = 0;
    private string externalID = null;
    private string name = null;

	public hrStatus(int _id, int _employerID, string _externalID, string _name)
	{
        this.id = _id;
        this.employerID = _employerID;
        this.externalID = _externalID;
        this.name = _name;
	}

    public int HR_STATUS_ID
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

    public int HR_STATUS_EMPLOYER_ID
    {
        get
        {
            return this.employerID;
        }
        set
        {
            this.employerID = value;
        }
    }

    public string HR_STATUS_EXTERNAL_ID
    {
        get
        {
            return this.externalID;
        }
        set
        {
            this.externalID = value;
        }
    }

    public string HR_STATUS_NAME
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