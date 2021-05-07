using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Afas.AfComply.Domain;
using Afas.Domain;

/// <summary>
/// Summary description for District
/// </summary>
public class employer
{
    private int id = 0;
    private string name = null;
    private string address = null;
    private string city = null;
    private int state_id = 0;
    private string zip = null;
    private string logo = null;
    private string b_address = null;
    private string b_city = null;
    private int b_state_id = 0;
    private string b_zip = null;
    private int emp_type_id = 0;
    private string ein = null;
    private int initID = 0;
    private string import_demo = null;
    private string import_payroll = null;
    private int vendorID = 0;
    private bool autoUpload = false;
    private bool autoBill = false;
    private bool suBilled = false;
    private string import_hr = null;
    private string import_gp = null;
    private string import_ec = null;
    private string import_io = null;
    private string import_ic = null;
    private string import_pay_mod = null;
    private Guid resourceId = new Guid();          
    private string dbaName = null;
    private bool irsEnabled = false;

    private string iei = null;
    private string iec = null;
    private string ftpei = null;
    private string ftpec = null;
    private string ipi = null;
    private string ipc = null;
    private string ftppi = null;
    private string ftppc = null;
    private string importProcess = null;

	public employer(int _id, string _name, string _address, string _city, int _stateID, string _zip, string _logo, 
        string _baddress, string _bcity, int _bstateID, string _bzip, int _empTypeID, string _ein, int _initID, 
        string _impDemo, string _impPayroll, string _iei, string _iec, string _ftpei, string _ftpec, string _ipi, 
        string _ipc, string _ftppi, string _ftppc, string _importProcess, int _vendorID, bool _autoUpload, bool _autoBill, 
        bool _suBilled, string _importGP, string _importHR, string _importEC, string _importIO, string _importIC,
        string _importPayMod, Guid _resourceId, string _dbaName, bool _irsEnabled)
	{
        this.id = _id;
        this.name = _name;
        this.address = _address;
        this.city = _city;
        this.state_id = _stateID;
        this.zip = _zip;
        this.logo = _logo;
        this.b_address = _baddress;
        this.b_city = _bcity;
        this.b_state_id = _bstateID;
        this.b_zip = _bzip;
        this.emp_type_id = _empTypeID;
        this.ein = _ein;
        this.initID = _initID;
        this.import_demo = _impDemo;
        this.import_payroll = _impPayroll;
        this.iei = _iei;
        this.iec = _iec;
        this.ftpei = _ftpei;
        this.ftpec = _ftpec;
        this.ipi = _ipi;
        this.ipc = _ipc;
        this.ftppi = _ftppi;
        this.ftppc = _ftppc;
        this.importProcess = _importProcess;
        this.vendorID = _vendorID;
        this.autoUpload = _autoUpload;
        this.autoBill = _autoBill;
        this.suBilled = _suBilled;
        this.import_gp = _importGP;
        this.import_hr = _importHR;
        this.import_ec = _importEC;
        this.import_io = _importIO;
        this.import_ic = _importIC;
        this.import_pay_mod = _importPayMod;
        this.resourceId = _resourceId;
        this.dbaName = _dbaName;
        this.irsEnabled = _irsEnabled;
	}

    /// <summary>
    /// Gets the DBA name if one exists, or uses the Employer name if DBA name doesn't exist or is blank
    /// </summary>
    public string DisplayName
    {
        get
        {
            if (false == this.DBAName.IsNullOrEmpty())
            {
                return this.DBAName;
            }

            return this.EMPLOYER_NAME;
        }
            
    }


    /// <summary>
    /// This is the District ID created by the database. 
    /// </summary>
    public int EMPLOYER_ID
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
    /// This is the Name of the EMPLOYER. 
    /// </summary>
    public string EMPLOYER_NAME
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
    /// This is the Name of the EMPLOYER and Ein. 
    /// </summary>
    public string EMPLOYER_NAME_And_EIN
    {
        get
        {
            return this.EMPLOYER_NAME +"-"+this.EMPLOYER_EIN;
        }
        set
        {
            this.name = value;
        }
    }


    /// <summary>
    /// This is the Address of the district. 
    /// </summary>
    public string EMPLOYER_ADDRESS
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

    /// <summary>
    /// This is the Name of the district. 
    /// </summary>
    public string EMPLOYER_CITY
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

    /// <summary>
    /// This is the State ID of the district. 
    /// </summary>
    public int EMPLOYER_STATE_ID
    {
        get
        {
            return this.state_id;
        }
        set
        {
            this.state_id = value;
        }
    }

    /// <summary>
    /// This is the Zip of the district. 
    /// </summary>
    public string EMPLOYER_ZIP
    {
        get
        {
            return this.zip.ZeroPadZip();
        }
        set
        {
            this.zip = value;
        }
    }

    /// <summary>
    /// This is the image path of the district logo. 
    /// </summary>
    public string EMPLOYER_LOGO
    {
        get
        {
            return this.logo;
        }
        set
        {
            this.logo = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string BILLING_ADDRESS
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

    /// <summary>
    /// 
    /// </summary>
    public string BILLING_CITY
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

    /// <summary>
    /// 
    /// </summary>
    public int BILLING_STATE
    {
        get
        {
            return this.b_state_id;
        }
        set
        {
            this.b_state_id = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string BILLING_ZIP
    {
        get
        {
            return this.b_zip.ZeroPadZip();
        }
        set
        {
            this.b_zip = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int EMPLOYER_TYPE_ID
    {
        get
        {
            return this.emp_type_id;
        }
        set
        {
            this.emp_type_id = value;
        }
    }

    /// <summary>
    /// This is the Address of the district. 
    /// </summary>
    public string EMPLOYER_EIN
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

    public int EMPLOYER_INITIAL_MEAS_ID
    {
        get
        {
            return this.initID;
        }
        set
        {
            this.initID = value;
        }
    }

    public string EMPLOYER_IMPORT_EMPLOYEE
    {
        get
        {
            return this.import_demo;
        }
        set
        {
            this.import_demo = value;
        }
    }

    public string EMPLOYER_IMPORT_PAYROLL
    {
        get
        {
            return this.import_payroll;
        }
        set
        {
            this.import_payroll = value;
        }
    }

    public string EMPLOYER_IEI
    {
        get
        {
            return this.iei;
        }
        set
        {
            this.iei = value;
        }
    }

    public string EMPLOYER_IEC
    {
        get
        {
            return this.iec;
        }
        set
        {
            this.iec = value;
        }
    }

    public string EMPLOYER_FTPEI
    {
        get
        {
            return this.ftpei;
        }
        set
        {
            this.ftpei = value;
        }
    }

    public string EMPLOYER_FTPEC
    {
        get
        {
            return this.ftpec;
        }
        set
        {
            this.ftpec = value;
        }
    }

    public string EMPLOYER_IPI
    {
        get
        {
            return this.ipi;
        }
        set
        {
            this.ipi = value;
        }
    }

    public string EMPLOYER_IPC
    {
        get
        {
            return this.ipc;
        }
        set
        {
            this.ipc = value;
        }
    }

    public string EMPLOYER_FTPPI
    {
        get
        {
            return this.ftppi;
        }
        set
        {
            this.ftppi = value;
        }
    }

    public string EMPLOYER_FTPPC
    {
        get
        {
            return this.ftppc;
        }
        set
        {
            this.ftppc = value;
        }
    }

    public string EMPLOYER_IMPORT
    {
        get
        {
            return this.importProcess;
        }
        set
        {
            this.importProcess = value;
        }
    }

    public string EMPLOYER_IMPORT_GP
    {
        get
        {
            return this.import_gp;
        }
        set
        {
            this.import_gp = value;
        }
    }

    public string EMPLOYER_IMPORT_HR
    {
        get
        {
            return this.import_hr;
        }
        set
        {
            this.import_hr = value;
        }
    }

    public string EMPLOYER_IMPORT_EC
    {
        get
        {
            return this.import_ec;
        }
        set
        {
            this.import_ec = value;
        }
    }

    public string EMPLOYER_IMPORT_IO
    {
        get
        {
            return this.import_io;
        }
        set
        {
            this.import_io = value;
        }
    }

    public string EMPLOYER_IMPORT_IC
    {
        get
        {
            return this.import_ic;
        }
        set
        {
            this.import_ic = value;
        }
    }

    public string EMPLOYER_IMPORT_PAY_MOD
    {
        get
        {
            return this.import_pay_mod;
        }
        set
        {
            this.import_pay_mod = value;
        }
    }

    public int EMPLOYER_VENDOR_ID
    {
        get
        {
            return this.vendorID;
        }
        set
        {
            this.vendorID = value;
        }
    }

    public bool EMPLOYER_AUTO_BILL
    {
        get
        {
            return this.autoBill;
        }
        set
        {
            this.autoBill = value;
        }
    }

    public bool EMPLOYER_AUTO_UPLOAD
    {
        get
        {
            return this.autoUpload;
        }
        set
        {
            this.autoUpload = value;
        }
    }

    public bool EMPLOYER_SU_BILLED
    {
        get
        {
            return this.suBilled;
        }
        set
        {
            this.suBilled = value;
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

    /// <summary>
    /// Doing Business As Name, as opposed to the IRS Name.
    /// </summary>
    public string DBAName 
    {
        get 
        {
            if (dbaName == null) 
            {
                return EMPLOYER_NAME;
            }
            return dbaName;
        }
        set
        {
            if (value == EMPLOYER_NAME)
            {
                dbaName = null;
            }
            else
            {
                dbaName = value;
            }
        }
    }

    /// <summary>
    /// True if IRS reporting is enabled, false otherwise
    /// </summary>
    public bool IrsEnabled
    {
        get
        {
            return this.irsEnabled;
        }
        set
        {
            this.irsEnabled = value;
        }
    }
}