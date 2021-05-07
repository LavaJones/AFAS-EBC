using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for insuranceContribution
/// </summary>
public class insuranceContribution
{
    private int id = 0;
    private int insuranceID = 0;
    private string contributionID = null;     
    private int classID = 0;
    private string className = null;
    private double amount = 0;
    private string modBy = null;
    private DateTime? modOn = null;
    private string history = null;

    public insuranceContribution(int _id, int _insuranceID, string _contributionID, int _classID, double _amount, string _modBy, DateTime? _modOn, string _history, string _className)
	{
        this.id = _id;
        this.insuranceID = _insuranceID;
        this.contributionID = _contributionID;
        this.classID = _classID;
        this.amount = _amount;
        this.modBy = _modBy;
        this.modOn = _modOn;
        this.history = _history;
        this.className = _className;
	}

    public int INS_CONT_ID
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

    public int INS_CONT_INSURANCE_ID
    {
        get
        {
            return this.insuranceID;
        }
        set
        {
            this.insuranceID = value;
        }
    }

    public string INS_CONT_CONTRIBUTION_ID
    {
        get
        {
            return this.contributionID;
        }
        set
        {
            this.contributionID = value;
        }
    }

    public int INS_CONT_CLASSID
    {
        get
        {
            return this.classID;
        }
        set
        {
            this.classID = value;
        }
    }

    public string INS_CONT_CLASSNAME
    {
        get
        {
            return this.className;
        }
        set
        {
            this.className = value;
        }
    }

    public double INS_CONT_AMOUNT
    {
        get
        {
            return this.amount;
        }
        set
        {
            this.amount = value;
        }
    }

    public string INS_CONT_MODBY
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

    public DateTime? INS_CONT_MODON
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

    public string INS_CONT_HISTORY
    {
        get
        {
            return this.history;
        }
        set
        {
            this.history = value;
        }
    }

    public string INS_DESC
    {
        get
        {
            return this.className + " - " + this.contributionID.ToString() + this.amount.ToString();
        }
    }
}