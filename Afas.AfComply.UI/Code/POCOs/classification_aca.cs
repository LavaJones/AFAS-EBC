using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for classification_aca
/// </summary>
public class classification_aca
{
    int id = 0;
    string name = null;

	public classification_aca(int _id, string _name)
	{
        this.id = _id;
        this.name = _name;
	}

    public int ACA_STATUS_ID
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

    public string ACA_STATUS_NAME
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
}