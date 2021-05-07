using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for carrier
/// </summary>
public class carrier
{
    int id = 0;
    string name = null;
    bool approved = false;

	public carrier(int _id, string _name, bool _approved)
	{
        this.id = _id;
        this.name = _name;
        this.approved = _approved;
	}


    public int CARRIER_ID
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

    public string CARRIER_NAME
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

    public bool CARRIER_APPROVED
    {
        get
        {
            return this.approved;
        }
        set
        {
            this.approved = value;
        }
    }

}