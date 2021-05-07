using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

/// <summary>
/// Summary description for Registration
/// </summary>
public class Registration
{
    private ILog Log = LogManager.GetLogger(typeof(Registration));

    private int employerTypeID = 0;
    private string employerName = null;
    private string ein = null;
    private string address = null;
    private string city = null;
    private int stateID = 0;
    private string zip = null;
    private string planName = null;
    private int renewalMonth = 0;
    private bool SecondPlan = false;
    private string planName2 = null;
    private int renewalMonth2 = 0;

    private string b_address = null;
    private string b_city = null;
    private int b_state = 0;
    private string b_zip = null;

    private User adminUser = null;
    private User billUser = null;

    private DateTime modOn = System.DateTime.Now;       
    private string modBy = "Registration";              
    private string history = null;                      
    private int payroll_Vendor = 0;
    private string dbaName = null;

    public Registration(int _empTypeID, string _empName, string _ein, string _address, string _city, int _stateID, string _zip, string _pn1, int _rMonth1, bool _twoPlans, string _pn2, int _rMonth2, string _baddress, string _bcity, int _bstateID, string _bzip, User _admin, User _billing, string _dbaName)
	{
        this.employerTypeID = _empTypeID;
        this.employerName = _empName;
        this.ein = _ein;
        this.address = _address;
        this.city = _city;
        this.stateID = _stateID;
        this.zip = _zip;
        this.planName = _pn1;
        this.renewalMonth = _rMonth1;
        this.SecondPlan = _twoPlans;
        this.planName2 = _pn2;
        this.renewalMonth2 = _rMonth2;
        this.b_address = _baddress;
        this.b_city = _bcity;
        this.b_state = _bstateID;
        this.b_zip = _bzip;
        this.history = "Created on: " + modOn.ToString() + " by " + modBy;
        this.adminUser = _admin;
        this.billUser = _billing;
        this.dbaName = _dbaName;
	}

    public int REG_EMP_ID
    {
        get
        {
            return this.employerTypeID;
        }
        set
        {
            this.employerTypeID = value;
        }
    }

    public string REG_EMP_NAME
    {
        get
        {
            return this.employerName;
        }
        set
        {
            this.employerName = value;
        }
    }

    public string REG_EIN
    {
        get
        {
            return this.ein;
        }
        set
        {
            this.ein = value;
        }
    }

    public string REG_ADDRESS
    {
        get
        {
            return this.address;
        }
        set
        {
            this.address = value;
        }
    }

    public string REG_CITY
    {
        get
        {
            return this.city;
        }
        set
        {
            this.city = value;
        }
    }

    public int REG_STATE_ID
    {
        get
        {
            return this.stateID;
        }
        set
        {
            this.stateID = value;
        }
    }

    public string REG_ZIP
    {
        get
        {
            return this.zip;
        }
        set
        {
            this.zip = value;
        }
    }

    public string REG_PLANNAME1
    {
        get
        {
            return this.planName;
        }
        set
        {
            this.planName = value;
        }
    }

    public int REG_RENEWAL_MONTH
    {
        get
        {
            return this.renewalMonth;
        }
        set
        {
            this.renewalMonth = value;
        }
    }

    public DateTime? REG_RENEWAL_DATE
    {
        get
        {
            try
            {
                return DateTime.Parse(renewalMonth.ToString() + "/1/2015");
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                return null;
            }
        }
    }

    public bool REG_TWO_PLANS
    {
        get
        {
            return this.SecondPlan;
        }
        set
        {
            this.SecondPlan = value;
        }
    }

    public string REG_PLANNAME2
    {
        get
        {
            return this.planName2;
        }
        set
        {
            this.planName2 = value;
        }
    }

    public int REG_RENEWAL_MONTH2
    {
        get
        {
            return this.renewalMonth2;
        }
        set
        {
            this.renewalMonth2 = value;
        }
    }

    public DateTime? REG_RENEWAL_DATE2
    {
        get
        {
            if (this.SecondPlan == true)
            {
                try
                {
                    return DateTime.Parse(renewalMonth2.ToString() + "/1/2015");
                }
                catch (Exception exception)
                {
                    Log.Warn("Suppressing errors.", exception);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

    public User REG_ADMIN_USER
    {
        get
        {
            return this.adminUser;
        }
        set
        {
            this.adminUser = value;
        }
    }

    public User REG_BILL_USER
    {
        get
        {
            return this.billUser;
        }
        set
        {
            this.billUser = value;
        }
    }

   

    public string REG_BILL_ADDRESS
    {
        get
        {
            return this.b_address;
        }
        set
        {
            this.b_address = value;
        }
    }

    public string REG_BILL_CITY
    {
        get
        {
            return this.b_city;
        }
        set
        {
            this.b_city = value;
        }
    }

    public int REG_BILL_STATE
    {
        get
        {
            return this.b_state;
        }
        set
        {
            this.b_state = value;
        }
    }

    public string REG_BILL_ZIP
    {
        get
        {
            return this.b_zip;
        }
        set
        {
            this.b_zip = value;
        }
    }

    public DateTime REG_MOD_ON
    {
        get
        {
            return this.modOn;
        }
    }

    public string REG_MOD_BY
    {
        get
        {
            return this.modBy;
        }
    }

    public string REG_HISTORY
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

    public int Payroll_Vendor 
    {
        get { return this.payroll_Vendor; }
        set { this.payroll_Vendor = value; }
    }

    public string DbaName 
    {
        get 
        {
            return dbaName;
        }
        set 
        {
            if (value == REG_EMP_NAME) 
            { 
                dbaName = null; 
            }
            else
            {
                dbaName = value;
            }
        }
    }
}