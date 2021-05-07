using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using log4net;

/// <summary>
/// Summary description for User
/// </summary>
public class User
{

    /// <summary>
    /// Constructor for a new USER object. 
    /// Purpose:
    ///     - This contains all the information for a specific user.
    /// </summary>
    public User(
            int _id, 
            String _fname, 
            String _lname, 
            String _email, 
            String _phone, 
            String _username, 
            String _password, 
            int _districtID, 
            Boolean _powerUser, 
            Boolean _active, 
            DateTime? _lastMod, 
            String _lastModBy, 
            Boolean _resetRequired, 
            Boolean _billing, 
            Boolean _irsContact, 
            Boolean _floater, 
            Guid _resourceId = new Guid()
        )
	{

       
        this.id = _id;
        this.fname = _fname;
        this.lname = _lname;
        this.email = _email;
        this.phone = _phone;
        this.username = _username;
        this.password = _password;
        this.district_id = _districtID;
        this.powerUser = _powerUser;
        this.activeUser = _active;
        this.lastMod = _lastMod;
        this.lastModBy = _lastModBy;
        this.resetRequired = _resetRequired;
        this.recieveBilling = _billing;
        this.irsContact = _irsContact;
        this.ebcFloater = _floater;
        this.resourceId = _resourceId;
	}

    /// <summary>
    /// 
    /// </summary>
    public int User_ID
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
    /// 
    /// </summary>
    public string User_First_Name
    {
        get
        {
            return this.fname;
        }
        set
        {
            this.fname = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string User_Last_Name
    {
        get
        {
            return this.lname;
        }
        set
        {
            this.lname = value;
        }
    }

    public string User_Full_Name
    {
        get
        {
            return this.fname + " " + this.lname;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string User_Email
    {
        get
        {
            
            // inline hack to track down a null reference in production.
            if (String.IsNullOrEmpty(this.email))
            {
                
                ILog log = LogManager.GetLogger(typeof(User));

                log.Error(String.Format("Found a null email address for user record: {0}, returning the processing failed address instead.", this.id));

                return SystemSettings.ProcessingFailedAddress;

            }

            return this.email.ToLower();
        
        }
        set
        {
            this.email = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string User_Phone
    {
        get
        {
            return this.phone;
        }
        set
        {
            this.phone = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string User_UserName
    {
        get
        {
            return this.username;
        }
        set
        {
            this.username = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string User_Password
    {
        get
        {
            return this.password;
        }
        set
        {
            this.password = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool User_Admin
    {
        get
        {
            return this.admin;
        }
        set
        {
            this.admin = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int User_District_ID
    {
        get
        {
            return this.district_id;
        }
        set
        {
            this.district_id = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool User_Power
    {
        get
        {
            return this.powerUser;
        }
        set
        {
            this.powerUser = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool USER_ACTIVE
    {
        get
        {
            return this.activeUser;
        }
        set
        {
            this.activeUser = value;
        }
    }

    public string LAST_MOD_BY
    {
        get
        {
            return this.lastModBy;
        }
        set
        {
            this.lastModBy = value;
        }
    }

    public DateTime? LAST_MOD
    {
        get
        {
            return this.lastMod;
        }
        set
        {
            this.lastMod = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool User_PWD_RESET
    {
        get
        {
            return this.resetRequired;
        }
        set
        {
            this.resetRequired = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool User_Billing
    {
        get
        {
            return this.recieveBilling;
        }
        set
        {
            this.recieveBilling = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool User_IRS_CONTACT
    {
        get
        {
            return this.irsContact;
        }
        set
        {
            this.irsContact = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool User_Floater
    {
        get
        {
            return this.ebcFloater;
        }
        set
        {
            this.ebcFloater = value;
        }
    }

    /// <summary>
    /// A Unique Guid that is used to identify this user
    /// </summary>
    public Guid ResourceId 
    {
        get 
        { 
            return this.resourceId; 
        }
        set 
        {
            this.resourceId = value;
        }
    }

    private int id = 0;                     //01) User ID, generated by the database. 
    private string fname = null;            //02) Users first name.
    private string lname = null;            //03) Users last name.
    private string email = null;            //04) Users email address.
    private string phone = null;            //05) Users phone number.
    private string username = null;         //06) Users user name. 
    private string password = null;         //07) Users hashed password. 
    private bool admin = false;             //08) Users administrative rights.
    private int district_id = 0;            //09) Users district ID.
    private bool powerUser = false;         //10) User has more ability to change things. 
    private bool activeUser = false;        //11) User is active or not.
    private DateTime? lastMod = null;       //12) User was last modified on this date. 
    private string lastModBy = null;        //13) User was last modified by.
    private bool resetRequired = false;     //14) User needs to reset password the next time they login. 
    private bool recieveBilling = false;    //15) User will recieve emailed invoices if set to true. 
    private bool irsContact = true;          //16) User will be the designated contact for IRS reporting.
    private bool ebcFloater = false;        //17) User is used for floating between Clients accounts.
    private Guid resourceId = new Guid();   //Guid used to uniquely identify this user. 

}