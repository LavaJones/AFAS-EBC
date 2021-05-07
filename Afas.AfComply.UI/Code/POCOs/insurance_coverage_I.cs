using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Afas.AfComply.Domain;

/// <summary>
/// Summary description for insurance_coverage_I
/// </summary>
public class insurance_coverage_I : insurance_coverage
{
    protected string jan_i = null;
    protected string feb_i = null;
    protected string mar_i = null;
    protected string apr_i = null;
    protected string may_i = null;
    protected string jun_i = null;
    protected string jul_i = null;
    protected string aug_i = null;
    protected string sep_i = null;
    protected string oct_i = null;
    protected string nov_i = null;
    protected string dec_i = null;
    protected string all12_i = null;
    protected string subscriber_i = null;
    protected string address_i = null;
    protected string city_i = null;
    protected string state_i = null;
    protected int state_id = 0;
    protected string zip_i = null;
    protected int zip = 0;

    public insurance_coverage_I(int _rowID, int _batchID, int _employerID, int _employeeID, int _taxYear, string _empDepLink, int? _dependentID, string _fname, string _mname, string _lname, string _ssn, DateTime? _dob, bool _all12, bool _jan, bool _feb, bool _march, bool _april, bool _may, bool _june, bool _july, bool _august, bool _sep, bool _oct, bool _nov, bool _dec, string _all12_i, bool _subscriber, string _jan_i, string _feb_i, string _march_i, string _april_i, string _may_i, string _june_i, string _july_i, string _august_i, string _sep_i, string _oct_i, string _nov_i, string _dec_i, string _subscriber_i, string _address_i, string _city_i, string _state_i, int _stateID, string _zip_i, int _zip, int _carrierID)
    {
        this.rowID = _rowID;
        this.employerID = _employerID;
        this.employeeID = _employeeID;
        this.taxYear = _taxYear;
        this.empDependentLink = _empDepLink;
        this.dependentID = _dependentID;
        this.fname = _fname;
        this.mname = _mname;
        this.lname = _lname;
        this.ssn = _ssn;
        this.dob = _dob;
        this.all12 = _all12;
        this.january = _jan;
        this.february = _feb;
        this.march = _march;
        this.april = _april;
        this.may = _may;
        this.june = _june;
        this.july = _july;
        this.august = _august;
        this.september = _sep;
        this.october = _oct;
        this.november = _nov;
        this.december = _dec;
        this.subscriber = _subscriber;
        this.carrier_id = _carrierID;
        this.batchID = _batchID;

        this.all12_i = _all12_i;
        this.jan_i = _jan_i;
        this.feb_i = _feb_i;
        this.mar_i = _march_i;
        this.apr_i = _april_i;
        this.may_i = _may_i;
        this.jun_i = _june_i;
        this.jul_i = _july_i;
        this.aug_i = _august_i;
        this.sep_i = _sep_i;
        this.oct_i = _oct_i;
        this.nov_i = _nov_i;
        this.dec_i = _dec_i;
        this.subscriber_i = _subscriber_i;
        this.address_i = _address_i;
        this.city_i = _city_i;
        this.state_i = _state_i;
        this.state_id = _stateID;
        this.zip_i = _zip_i;
        this.zip = _zip;

    }

    public string IC_ALL_12_I
    {
        get
        {
            return this.all12_i;
        }
        set
        {
            this.all12_i = value;
        }
    }

    public string IC_JAN_I
    {
        get
        {
            return this.jan_i;
        }
        set
        {
            this.jan_i = value;
        }
    }

    public string IC_FEB_I
    {
        get
        {
            return this.feb_i;
        }
        set
        {
            this.feb_i = value;
        }
    }

    public string IC_MAR_I
    {
        get
        {
            return this.mar_i;
        }
        set
        {
            this.mar_i = value;
        }
    }

    public string IC_APR_I
    {
        get
        {
            return this.apr_i;
        }
        set
        {
            this.apr_i = value;
        }
    }

    public string IC_MAY_I
    {
        get
        {
            return this.may_i;
        }
        set
        {
            this.may_i = value;
        }
    }

    public string IC_JUN_I
    {
        get
        {
            return this.jun_i;
        }
        set
        {
            this.jun_i = value;
        }
    }

    public string IC_JUL_I
    {
        get
        {
            return this.jul_i;
        }
        set
        {
            this.jul_i = value;
        }
    }

    public string IC_AUG_I
    {
        get
        {
            return this.aug_i;
        }
        set
        {
            this.aug_i = value;
        }
    }

    public string IC_SEP_I
    {
        get
        {
            return this.sep_i;
        }
        set
        {
            this.sep_i = value;
        }
    }

    public string IC_OCT_I
    {
        get
        {
            return this.oct_i;
        }
        set
        {
            this.oct_i = value;
        }
    }

    public string IC_NOV_I
    {
        get
        {
            return this.nov_i;
        }
        set
        {
            this.nov_i = value;
        }
    }


    public string IC_DEC_I
    {
        get
        {
            return this.dec_i;
        }
        set
        {
            this.dec_i = value;
        }
    }

    public string IC_SUBSCRIBER_I
    {
        get
        {
            return this.subscriber_i;
        }
        set
        {
            this.subscriber_i = value;
        }
    }

    public string IC_ADDRESS_I
    {
        get
        {
            return this.address_i;
        }
        set
        {
            this.address_i = value;
        }
    }

    public string IC_CITY_I
    {
        get
        {
            return this.city_i;
        }
        set
        {
            this.city_i = value;
        }
    }

    public string IC_STATE_I
    {
        get
        {
            return this.state_i;
        }
        set
        {
            this.state_i = value;
        }
    }

    public int IC_STATE_ID
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

    public string IC_ZIP_I
    {
        get
        {
            return this.zip_i.ZeroPadZip();
        }
        set
        {
            this.zip_i = value;
        }
    }

    public int IC_ZIP
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

    public string DependentLinkSSN_UnMasked 
    {
        get
        {
            if (this.empDependentLink != null)
            {
                return AesEncryption.Decrypt(this.empDependentLink);
            }
            else 
            { 
                return string.Empty; 
            }
        }
    }

}