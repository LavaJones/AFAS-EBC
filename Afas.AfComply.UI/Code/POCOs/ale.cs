using Afas.AfComply.Domain.POCO;
using Afas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ale
/// </summary>
public class ale : BaseAfasModel{
    private int id = 0;
    private int employerID = 0;
    private string name = null;
    private string ein = null;

	public ale(int _id, int _employerID, string _name, string _ein)
	{
        this.id = _id;
        this.employerID = _employerID;
        this.name = _name;
        this.ein = _ein;
	}
    
    public ale() 
    {

    }

    public int ALE_ID
    {
        get {return this.id;}
        set { this.id = value; }
    }

    public int ALE_EMPLOYER_ID
    {
        get { return this.employerID; }
        set { this.employerID = value; }
    }

    public string ALE_NAME
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public string ALE_EIN
    {
        get { return this.ein; }
        set { this.ein = value; }
    }






}