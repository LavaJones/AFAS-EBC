using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ash
/// </summary>
public class ash
{
    private string id = null;
    private string description = null;
    private string createdBy = null;
    private DateTime? createdDate = null;
    private string modifiedBy = null;
    private DateTime? modifiedDate = null;
    private int entityStatusID = 0;
    private Guid resourceID;

	public ash(string _id, string _desc)
	{
        this.id = _id;
        this.description = _desc;
	}

    public ash(string _id, string _desc, string _createdBy, DateTime _createdOn, string _modifiedBy, DateTime _modifiedOn, int _entityStatusID, Guid _resourceID)
    {
        this.id = _id;
        this.description = _desc;
        this.createdBy = _createdBy;
        this.createdDate = _createdOn;
        this.modifiedBy = _modifiedBy;
        this.modifiedDate = _modifiedOn;
        this.entityStatusID = _entityStatusID;
        this.resourceID = _resourceID;
    }

    public string ASH_ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    public string ASH_DESCRIPTION
    {
        get { return this.description; }
        set { this.description = value; }
    }

    public DateTime? ASH_CREATED_ON
    {
        get { return this.createdDate; }
        set { this.createdDate = value; }
    }


    public string ASH_CREATED_BY
    {
        get { return this.createdBy; }
        set { this.createdBy = value; }
    }

    public DateTime? ASH_MOD_ON
    {
        get { return this.modifiedDate; }
        set { this.modifiedDate = value; }
    }


    public string ASH_MOD_BY
    {
        get { return this.modifiedBy; }
        set { this.modifiedBy = value; }
    }

    public int ASH_ENTITY_STATUS
    {
        get { return this.entityStatusID; }
        set { this.entityStatusID = value; }
    }

    public Guid ASH_GUID
    {
        get { return this.resourceID; }
        set { this.resourceID = value; }
    }

    public string ASH_ID_DESCRIPTION
    {
        get { return this.id + "-" + this.description; }
    }
}