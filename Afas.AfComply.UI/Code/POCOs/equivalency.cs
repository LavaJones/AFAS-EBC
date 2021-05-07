using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for equivlancy
/// </summary>
public class equivalency
{
    private int id = 0;                                               
    private int empID = 0;                                            
    private string name = null;                                       
    private int grossPayID = 0;                                            
    private decimal every = 0;                                                
    private int unitID = 0;                                                    
    private decimal credit = 0;                                                
    private DateTime? sdate = null;                                               
    private DateTime? edate = null;                                              
    private string notes = null;                                                
    private string modBy = null;                                             
    private DateTime modOn = System.DateTime.Now.AddYears(-20);                  
    private string history = null;                                              
    private bool active = false;                                              
    private int typeID = 0;                                                
    private string typeName = null;                                            
    private string unitName = null;                                           
    private int positionID = 0;                                                 
    private int activityID = 0;                                                
    private int detailID = 0;                                                   

    /// <summary>
    /// 01) Constructor for a new Equivalency Object. 
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_empID"></param>
    /// <param name="_name"></param>
    /// <param name="_extID"></param>
    /// <param name="_every"></param>
    /// <param name="_unitID"></param>
    /// <param name="_credit"></param>
    /// <param name="_sdate"></param>
    /// <param name="_edate"></param>
    /// <param name="_notes"></param>
    /// <param name="_modBy"></param>
    /// <param name="_modOn"></param>
    /// <param name="_history"></param>
    /// <param name="_active"></param>
    /// <param name="_typeID"></param>
    /// <param name="_typeName"></param>
    /// <param name="_unitName"></param>
    /// <param name="_positionID"></param>
    /// <param name="_activityID"></param>
    /// <param name="_detailID"></param>
	public equivalency(int _id, int _empID, string _name, int _gpID, decimal _every, int _unitID, decimal _credit, DateTime? _sdate, DateTime? _edate, string _notes, string _modBy, DateTime _modOn, string _history, bool _active, int _typeID, string _typeName, string _unitName, int _positionID, int _activityID, int _detailID)
	{
        this.id = _id;
        this.empID = _empID;
        this.name = _name;
        this.grossPayID = _gpID;
        this.every = _every;
        this.unitID = _unitID;
        this.credit = _credit;
        this.sdate = _sdate;
        this.edate = _edate;
        this.notes = _notes;
        this.modBy = _modBy;
        this.modOn = _modOn;
        this.history = _history;
        this.active = _active;
        this.typeID = _typeID;
        this.typeName = _typeName;
        this.unitName = _unitName;
        this.positionID = _positionID;
        this.activityID = _activityID;
        this.detailID = _detailID;
	}

    /// <summary>
    /// Getter/Setter for the Equivalency ID
    /// </summary>
    public int EQUIV_ID
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

    /// <summary>
    /// Getter/Setter for the Equivalency Name
    /// </summary>
    public string EQUIV_NAME
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

    /// <summary>
    /// Getter/Setter for the Equivalency Employer ID
    /// </summary>
    public int EQUIV_EMPLOYER_ID
    {
        get
        {
            return empID;
        }
        set
        {
            this.empID = value;
        }
    }

    /// <summary>
    /// Getter/Setter for the Equivalency External Gross Pay ID.
    /// </summary>
    public int EQUIV_GROSS_PAY_ID
    {
        get
        {
            return this.grossPayID;
        }
        set
        {
            this.grossPayID = value;
        }
    }

    /// <summary>
    /// Getter/Setter for the Equivalency equation for the first value of how many units. 
    /// </summary>
    public decimal EQUIV_EVERY
    {
        get
        {
            return every;
        }
        set
        {
            this.every = value;
        }
    }

    /// <summary>
    /// Getter/Setter for the Equivalency equation for the units selected.
    /// </summary>
    public int EQUIV_UNIT_ID
    {
        get
        {
            return this.unitID;
        }
        set
        {
            this.unitID = value;
        }
    }

    /// <summary>
    /// Getter/Setter for the Equivalency equation for the number of hours credited. 
    /// </summary>
    public decimal EQUIV_CREDIT
    {
        get
        {
            return this.credit;
        }
        set
        {
            this.credit = value;
        }
    }

    /// <summary>
    /// Getter/Setter for the Equivalency Start Date, if it is of type Date Range.
    /// </summary>
    public DateTime? EQUIV_S_DATE
    {
        get
        {
            return this.sdate;
        }
        set
        {
            this.sdate = value;
        }
    }

    /// <summary>
    /// Getter/Setter for the Equivalency End Date, if it is of type Date Range.
    /// </summary>
    public DateTime? EQUIV_E_DATE
    {
        get
        {
            return this.edate;
        }
        set
        {
            this.edate = value;
        }
    }

    public string EQUIV_NOTES
    {
        get
        {
            return this.notes;
        }
        set
        {
            this.notes = value;
        }
    }

    public string EQUIV_MOD_BY
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

    public DateTime EQUIV_MOD_ON
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


    public string EQUIV_HISTORY
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

    public bool EQUIV_ACTIVE
    {
        get
        {
            return this.active;
        }
        set
        {
            this.active = value;
        }
    }

    public int EQUIV_TYPE_ID
    {
        get
        {
            return this.typeID;
        }
        set
        {
            this.typeID = value;
        }
    }

    public string EQUIV_COMBINED
    {
        get
        {
            return "For every " + this.every.ToString() + " " + this.unitName + " add " + this.credit + " hours";
        }
        set
        { 
        
        }
    }

    public string EQUIV_TYPE_NAME
    {
        get
        {
            return this.typeName;
        }
        set
        {
            this.typeName = value;
        }
    }

    public string EQUIV_UNIT_NAME
    {
        get
        {
            return this.unitName;
        }
        set
        {
            this.unitName = value;
        }
    }

    public int EQUIV_POSITION_ID
    {
        get
        {
            return this.positionID;
        }
        set
        {
            this.positionID = value;
        }
    }

    public int EQUIV_ACTIVITY_ID
    {
        get
        {
            return this.activityID;
        }
        set
        {
            this.activityID = value;
        }
    }

    public int EQUIV_DETAIL_ID
    {
        get
        {
            return this.detailID;
        }
        set
        {
            this.detailID = value;
        }
    }
}