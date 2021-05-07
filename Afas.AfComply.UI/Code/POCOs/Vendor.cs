using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Vendor
/// </summary>
public class Vendor
{
    int id = 0;
    string name = null;
    bool autoUpload = false;

	public Vendor(int _id, string _name, bool _autoUpload)
	{
        this.id = _id;
        this.name = _name;
        this.autoUpload = _autoUpload;
	}

    public int VENDOR_ID
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

    public string VENDOR_NAME
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

    public bool VENDOR_AUTO_UPLOAD
    {
        get
        {
            return this.autoUpload;
        }
        set
        {
            this.autoUpload = value;
        }
    }
}