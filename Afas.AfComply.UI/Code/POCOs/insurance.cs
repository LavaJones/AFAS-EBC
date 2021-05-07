using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for insurance
/// </summary>
public class insurance
{
    private int id = 0;
    private int planyearID = 0;
    private string name = null;
    private decimal cost = 0;
    private bool minValue = false;
    private bool offSpouse = false;
    private bool offDependents = false;
    private string modBy = null;
    private DateTime? modOn = null;
    private string history = null;
    private int insuranceTypeID = 0;
    private string resourceID = null;
    private bool mec = false;
    private string createdBy = null;
    private DateTime? createdOn = null;
    private int entityStatusID = 0;
    private bool fullyAndSelf = false;

    public insurance(int _id, int _planyearID, string _name, decimal _cost, bool _minValue, bool _offSpouse, 
        bool spouseConditional, bool _offDependents, string _modBy, DateTime? _modOn, string _history, int _insuranceTypeID, bool _mec, string _createdBy, DateTime? _createdOn, int _entityStatusID, bool _fullyAndSelf)
	{
        this.id = _id;
        this.planyearID = _planyearID;
        this.name = _name;
        this.cost = _cost;
        this.minValue = _minValue;
        this.offSpouse = _offSpouse;
        this.SpouseConditional = spouseConditional;
        this.offDependents = _offDependents;
        this.modBy = _modBy;
        this.modOn = _modOn;
        this.history = _history;
        this.insuranceTypeID = _insuranceTypeID;
        this.mec = _mec;
        this.createdBy = _createdBy;
        this.createdOn = _createdOn;
        this.entityStatusID = _entityStatusID;
        this.fullyAndSelf = _fullyAndSelf;
	}

    public int INSURANCE_ID
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

    public int INSURANCE_PLAN_YEAR_ID
    {
        get
        {
            return this.planyearID;
        }
        set
        {
            this.planyearID = value;
        }
    }

    public string INSURANCE_NAME
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

    public decimal INSURANCE_COST
    {
        get
        {
            return this.cost;
        }
        set
        {
            this.cost = value;
        }
    }

    public string INSURANCE_COMBO
    {
        get
        {
            return this.name + " - " + this.cost.ToString("c");
        }
    }

    public bool INSURANCE_MIN_VALUE
    {
        get
        {
            return this.minValue;
        }
        set
        {
            this.minValue = value;
        }
    }

    public bool INSURANCE_OFF_SPOUSE
    {
        get
        {
            return this.offSpouse;
        }
        set
        {
            this.offSpouse = value;
        }
    }

    public bool INSURANCE_OFF_DEPENDENTS
    {
        get
        {
            return this.offDependents;
        }
        set
        {
            this.offDependents = value;
        }
    }

    public string INSURANCE_HISTORY
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

    public int INSURANCE_TYPE_ID
    {
        get
        {
            return this.insuranceTypeID;
        }
        set
        {
            this.insuranceTypeID = value;
        }
    }

    public bool SpouseConditional { get; set; }

    public bool INSURANCE_MEC
    {
        get { return this.mec; }
        set { this.mec = value; }
    }

    public int INSURANCE_ENTITY_STATUS
    {
        get { return this.entityStatusID; }
        set { this.entityStatusID = value; }
    }

    public bool INSURANCE_FULLY_AND_SELF
    {
        get { return this.fullyAndSelf; }
        set { this.fullyAndSelf = value; }
    }
}