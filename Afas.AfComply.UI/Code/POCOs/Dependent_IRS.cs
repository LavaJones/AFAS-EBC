using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Dependent_IRS
{
    public Dependent_IRS()
    {
    }

    public int dependent_id { get; set; }

    public int employee_id { get; set; }

    public string Fname { get; set; }

    public string Mi { get; set; }

    public string Lname { get; set; }

    public string ssn { get; set; }

    public DateTime? dob { get; set; }

    public string Name { get { return string.Format("{0} {1} {2}", Fname, Mi, Lname); } }

    public bool Jan { get; set; }

    public bool Feb { get; set; }

    public bool Mar { get; set; }

    public bool Apr { get; set; }

    public bool May { get; set; }

    public bool Jun { get; set; }

    public bool Jul { get; set; }

    public bool Aug { get; set; }

    public bool Sep { get; set; }

    public bool Oct { get; set; }

    public bool Nov { get; set; }

    public bool Dec { get; set; }

    public bool All_Months { get; set; }

}