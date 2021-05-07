using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EmployeeType
/// </summary>
public class EmployeeType
{
    private int id = 0;
    private int employerID = 0;
    private string name = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_employerID"></param>
    /// <param name="_name"></param>
	public EmployeeType(int _id, int _employerID, string _name)
	{
        this.id = _id;
        this.employerID = _employerID;
        this.name = _name;
	}

    /// <summary>
    /// 
    /// </summary>
    public int EMPLOYEE_TYPE_ID
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
    public int EMPLOYEE_TYPE_EMPLOYER_ID
    {
        get
        {
            return this.employerID;
        }
        set
        {
            this.employerID = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_TYPE_NAME
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