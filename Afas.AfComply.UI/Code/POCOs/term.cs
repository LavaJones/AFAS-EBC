using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// This object is for ACA Terms. Each term will have an ID, Name and Definition. 
/// </summary>
public class term
{
    private int id = 0;
    private string name = null;
    private string def = null;

    /// <summary>
    /// Constructor for creating new objects. 
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_name"></param>
    /// <param name="_def"></param>
	public term(int _id,string _name, string _def)
	{
        this.id = _id;
        this.name = _name;
        this.def = _def;
	}

    /// <summary>
    /// Get/Set for id.
    /// </summary>
    public int TERM_ID
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
    /// Get/Set for name.
    /// </summary>
    public string TERM_NAME
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
    /// Get/Set for definition.
    /// </summary>
    public string TERM_DEF
    {
        get
        {
            return this.def;
        }
        set
        {
            this.def = value;
        }
    }
}