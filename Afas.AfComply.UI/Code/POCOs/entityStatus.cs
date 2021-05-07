using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class entityStatus
{
    private int id = 0;
    private string name = null;
    private string createdBy = null;
    private DateTime? createdDate = null;
    private string modifiedBy = null;
    private DateTime? modifiedDate = null;

    public entityStatus(int _id, string _name, string _createdBy, DateTime _createdDate, string _modifiedBy, DateTime _modifiedDate)
    {
        this.id = _id;
        this.name = _name;
        this.createdBy = _createdBy;
        this.modifiedBy = _modifiedBy;
        this.modifiedDate = _modifiedDate;
        this.createdDate = _createdDate;
    }

    public int ES_ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    public string ES_NAME
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public DateTime? ES_CREATED_ON
    {
        get { return this.createdDate; }
        set { this.createdDate = value; }
    }


    public string ES_CREATED_BY
    {
        get { return this.createdBy; }
        set { this.createdBy = value; }
    }

    public DateTime? ES_MOD_ON
    {
        get { return this.modifiedDate; }
        set { this.modifiedDate = value; }
    }

    public string ES_MOD_BY
    {
        get { return this.modifiedBy; }
        set { this.modifiedBy = value; }
    }

}