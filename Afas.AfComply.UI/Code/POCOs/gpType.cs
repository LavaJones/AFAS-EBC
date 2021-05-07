using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for grosspayType
/// </summary>
public class gpType
{
    private int gp_id = 0;
    private int employer_id = 0;
    private string external_id = null;
    private string description = null;
    private bool active = false;

	public gpType(int _gpID, int _employerID, string _externalID, string _description, bool _active)
	{
        this.gp_id = _gpID;
        this.employer_id = _employerID;
        this.external_id = _externalID;
        this.description = _description;
        this.active = _active;
	}

    /// <summary>
    /// 
    /// </summary>
    public int GROSS_PAY_ID
    {
        get
        {
            return this.gp_id;
        }
        set
        {
            this.gp_id = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int GROSS_PAY_EMPLOYER_ID
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

    /// <summary>
    /// 
    /// </summary>
    public string GROSS_PAY_EXTERNAL_ID
    {
        get
        {
            return this.external_id;
        }
        set
        {
            this.external_id = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string GROSS_PAY_DESCRIPTION
    {
        get
        {
            return this.description;
        }
        set
        {
            this.description = value;
        }
    }

    public string GROSS_PAY_DESCRIPTION_EXTERNAL_ID
    {
        get
        {
            return this.description + " - " + external_id;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool GROSS_PAY_ACTIVE
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

}