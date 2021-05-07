using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Afas.AfComply.Domain;

/// <summary>
/// Summary description for insurance_coverage_template
/// </summary>
public class insurance_coverage_template
{
    private int templateID = 0;
    private int carrierID = 0;
    private int columns = 0;
    private int employeeDependentLink = 0;
    private int fname = 0;
    private int mname = 0;
    private int lname = 0;
    private int ssn = 0;
    private int dob = 0;
    private int all12 = 0;
    private int jan = 0;
    private int feb = 0;
    private int mar = 0;
    private int apr = 0;
    private int may = 0;
    private int jun = 0;
    private int jul = 0;
    private int aug = 0;
    private int sep = 0;
    private int oct = 0;
    private int nov = 0;
    private int dec = 0;
    private int subscriber = 0;
    private string trueFormat = null;
    private string nameFormat = null;
    private string all12trueFormat = null;
    private string subscriberFormat = null;
    private int address = 0;
    private int city = 0;
    private int state = 0;
    private int zip = 0;

	public insurance_coverage_template(int _templateID, int _carrierID, int _columns, int _link, int _fname, int _mname, int _lname, int _ssn, int _dob, int _all12, int _jan, int _feb, int _mar, int _apr, int _may, int _jun, int _jul, int _aug, int _sep, int _oct, int _nov, int _dec, string _tFormat, string _nFormat, string _all12Format, int _subscriber, string _subscriberFormat, int _address, int _city, int _state, int _zip)
	{
        this.templateID = _templateID;
        this.carrierID = _carrierID;
        this.columns = _columns;
        this.employeeDependentLink = _link;
        this.fname = _fname;
        this.mname = _mname;
        this.lname = _lname;
        this.ssn = _ssn;
        this.dob = _dob;
        this.all12 = _all12;
        this.jan = _jan;
        this.feb = _feb;
        this.mar = _mar;
        this.apr = _apr;
        this.may = _may;
        this.jun = _jun;
        this.jul = _jul;
        this.aug = _aug;
        this.sep = _sep;
        this.oct = _oct;
        this.nov = _nov;
        this.dec = _dec;
        this.trueFormat = _tFormat;
        this.nameFormat = _nFormat;
        this.all12trueFormat = _all12Format;
        this.subscriber = _subscriber;
        this.subscriberFormat = _subscriberFormat;
        this.address = _address;
        this.city = _city;
        this.state = _state;
        this.zip = _zip;
	}

    public int ICT_TEMPLATE_ID
    {
        get
        {
            return this.templateID;
        }
        set
        {
            this.templateID = value;
        }
    }

    public int ICT_CARRIER_ID
    {
        get
        {
            return this.carrierID;
        }
        set
        {
            this.carrierID = value;
        }
    }

    public int ICT_COLUMNS
    {
        get
        {
            return this.columns;
        }
        set
        {
            this.columns = value;
        }
    }

    public int ICT_EMPLOYEE_DEPENDENT_LINK
    {
        get
        {
            return this.employeeDependentLink;
        }
        set
        {
            this.employeeDependentLink = value;
        }
    }

    public int ICT_FIRST_NAME
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

    public int ICT_MIDDLE_NAME
    {
        get
        {
            return this.mname;
        }
        set
        {
            this.mname = value;
        }
    }

    public int ICT_LAST_NAME
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

    public int ICT_SSN
    {
        get
        {
            return this.ssn;
        }
        set
        {
            this.ssn = value;
        }
    }

    public int ICT_DOB
    {
        get
        {
            return this.dob;
        }
        set
        {
            this.dob = value;
        }
    }

    public int ICT_ALL_12
    {
        get
        {
            return this.all12;
        }
        set
        {
            this.all12 = value;
        }
    }

    public int ICT_JAN
    {
        get
        {
            return this.jan;
        }
        set
        {
            this.jan = value;
        }
    }

    public int ICT_FEB
    {
        get
        {
            return this.feb;
        }
        set
        {
            this.feb = value;
        }
    }

    public int ICT_MAR
    {
        get
        {
            return this.mar;
        }
        set
        {
            this.mar = value;
        }
    }

    public int ICT_APR
    {
        get
        {
            return this.apr;
        }
        set
        {
            this.apr = value;
        }
    }

    public int ICT_MAY
    {
        get
        {
            return this.may;
        }
        set
        {
            this.may = value;
        }
    }

    public int ICT_JUN
    {
        get
        {
            return this.jun;
        }
        set
        {
            this.jun = value;
        }
    }

    public int ICT_JUL
    {
        get
        {
            return this.jul;
        }
        set
        {
            this.jul = value;
        }
    }

    public int ICT_AUG
    {
        get
        {
            return this.aug;
        }
        set
        {
            this.aug = value;
        }
    }

    public int ICT_SEP
    {
        get
        {
            return this.sep;
        }
        set
        {
            this.sep = value;
        }
    }

    public int ICT_OCT
    {
        get
        {
            return this.oct;
        }
        set
        {
            this.oct = value;
        }
    }

    public int ICT_NOV
    {
        get
        {
            return this.nov;
        }
        set
        {
            this.nov = value;
        }
    }

    public int ICT_DEC
    {
        get
        {
            return this.dec;
        }
        set
        {
            this.dec = value;
        }
    }

    public string ICT_TRUE_FORMAT
    {
        get
        {
            return this.trueFormat;
        }
        set
        {
            this.trueFormat = value;
        }
    }

    public string ICT_NAME_FORMAT
    {
        get
        {
            return this.nameFormat;
        }
        set
        {
            this.nameFormat = value;
        }
    }

    public string ICT_ALL_12_TRUE_FORMAT
    {
        get
        {
            return this.all12trueFormat;
        }
        set
        {
            this.all12trueFormat = value;
        }
    }

    public int ICT_SUBSCRIBER
    {
        get
        {
            return this.subscriber;
        }
        set
        {
            this.subscriber = value;
        }
    }

    public string ICT_SUBSCRIBER_FORMAT
    {
        get
        {
            return this.subscriberFormat;
        }
        set
        {
            this.subscriberFormat = value;
        }
    }

    public int ICT_ADDRESS
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

    public int ICT_CITY
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

    public int ICT_STATE
    {
        get
        {
            return this.state;
        }
        set
        {
            this.state = value;
        }
    }

    public int ICT_ZIP
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
}