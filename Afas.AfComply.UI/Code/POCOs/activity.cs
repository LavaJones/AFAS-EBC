using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for activity
/// </summary>
public class activity
{

    private int id = 0;
    private string name = null;
    private int position_id = 0;

	public activity(int _id, string _name, int _posID)
	{
        this.id = _id;
        this.name = _name;
        this.position_id = _posID;
	}

    public int ACTIVITY_ID
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

    public string ACTIVITY_NAME
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

    public int ACTIVITY_POSITION_ID
    {
        get
        {
            return this.position_id;
        }
        set
        {
            this.position_id = value;
        }
    }

}