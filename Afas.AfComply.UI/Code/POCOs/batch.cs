using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for batch
/// </summary>
public class batch
{
    int id = 0;
    int employerID = 0;
    DateTime modOn;
    string modBy = null;
    DateTime? delOn = null;
    string delBy = null;

	public batch(int _id, int _employerID, DateTime _modOn, string _modBy, DateTime? _delOn, string _delBy)
	{
        this.id = _id;
        this.employerID = _employerID;
        this.modOn = _modOn;
        this.modBy = _modBy;
        this.delOn = _delOn;
        this.delBy = _delBy;
	}

    public int BATCH_ID
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

    public int EMPLOYER_ID
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

    public DateTime BATCH_MODON
    {
        get
        {
            return this.modOn;
        }
        set
        {
            this.modOn = value;
        }
    }

    public string BATCH_MODBY
    {
        get
        {
            return this.modBy;
        }
        set
        {
            this.modBy = value;
        }
    }

    public DateTime? BATCH_DELETED_ON
    {
        get
        {
            return this.delOn;
        }
        set
        {
            this.delOn = value;
        }
    }

    public string BATCH_DELETED_BY
    {
        get
        {
            return this.delBy;
        }
        set
        {
            this.delBy = value;
        }
    }
}