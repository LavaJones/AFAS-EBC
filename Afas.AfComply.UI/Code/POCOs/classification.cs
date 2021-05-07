using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for classification
/// </summary>
public class classification
{
    private int id = 0;
    private int employer_id = 0;
    private string descriptiom = null;
    private string affordabilityCode = null;
    private DateTime modOn;
    private string modBy = null;
    private string history = null;
    private int waitingPeriodID = 0;
    private DateTime createdOn;
    private string createdBy = null;
    private int entityStatusID = 0;
    private string ooc = null;

	public classification(int _id, int _employerID, string _desc, string _affCode, DateTime _modOn, string _modBy, string _history, int _watingPeriodID, DateTime _createdOn, string _createdBy, int _entityStatusID, string _ooc)
	{
        this.id = _id;
        this.employer_id = _employerID;
        this.descriptiom = _desc;
        this.affordabilityCode = _affCode;
        this.modOn = _modOn;
        this.modBy = _modBy;
        this.history = _history;
        this.waitingPeriodID = _watingPeriodID;
        this.createdBy = _createdBy;
        this.createdOn = _createdOn;
        this.entityStatusID = _entityStatusID;
        this.ooc = _ooc;
	}

    public int CLASS_ID
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

    public int CLASS_EMPLOYER_ID
    {
        get
        {
            return this.employer_id;
        }
        set
        {
            this.employer_id = value;
        }
    }

    public string CLASS_DESC
    {
        get
        {
            return this.descriptiom;
        }
        set
        {
            this.descriptiom = value;
        }
    }

    public string CLASS_AFFORDABILITY_CODE
    {
        get
        {
            return this.affordabilityCode;
        }
        set
        {
            this.affordabilityCode = value;
        }
    }

    public DateTime CLASS_MOD_ON
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

    public string CLASS_MOD_BY
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

    public string CLASS_HISTORY
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

    public int CLASS_WAITING_PERIOD_ID
    {
        get { return this.waitingPeriodID; }
        set { this.waitingPeriodID = value; }
    }

    public DateTime CLASS_CREATED_ON
    {
        get { return this.createdOn; }
        set { this.createdOn = value; }
    }

    public string CLASS_CREATED_BY
    {
        get { return this.createdBy; }
        set { this.createdBy = value; }
    }

    public int CLASS_ENTITY_STATUS
    {
        get { return this.entityStatusID; }
        set { this.entityStatusID = value; }
    }

    public string CLASS_DEFAULT_OOC
    {
        get { return this.ooc; }
        set { this.ooc = value; }
    }
}