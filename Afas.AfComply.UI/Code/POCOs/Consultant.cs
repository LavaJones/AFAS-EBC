using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Afas.AfComply.Domain;

public class Consultant
{
    public string name = null;
    public string title = null;
    public int phoneNumber = 0;
    private int EmployerID = 0;
    public string employerStatus = null;



    public Consultant(string Name, string Title, int PhoneNumber, string EmployerStatus)
    {
        this.name = Name;
        this.title = Title;
        this.phoneNumber = PhoneNumber;
        this.employerStatus = EmployerStatus;
    }
    public string CONSULTANT_NAME
    {
        get
        {
            return name;
        }
        set
        {
            this.name = value;
        }
    }
    public string CONSULTANT_TITLE
    {
        get
        {
            return this.title;
        }
        set
        {
            this.title = value;
        }
    }
    public int PHONE_NUMBER
    {
        get
        {
            return this.phoneNumber;
        }
        set
        {
            this.phoneNumber = value;
        }
    }
    public int EMPLOYER_ID
    {
        get
        {
            return this.EmployerID;
        }
        set
        {
            this.EmployerID = value;
        }
    }

    public string EMPLOYER_STATUS
    {
        get
        {
            return this.employerStatus;
        }
        set
        {
            this.employerStatus = value;
        }
    }
}
