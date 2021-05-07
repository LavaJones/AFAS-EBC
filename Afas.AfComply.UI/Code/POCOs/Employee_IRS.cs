using Afas.AfComply.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Employee_IRS
{
    public Employee_IRS()
    {
        Employee_Coverage = new List<Coverage>();
        Dependents = new List<Dependent_IRS>();
    }
    public string FileName { get { return string.Format("{0}_{1}_{2}_{3}_{4}",Lname, Fname, AesEncryption.Decrypt(SSN.Substring(SSN.Length-4)), EIN, EmployeeResourceId ); } }

    public Guid EmployerResourceId { get; set; }

    public string BusinessNameLine1 { get; set; }

    public string BusinessAddressLine1Txt { get; set; }

    public string BusinessCityNm { get; set; }

    public string BusinessUSStateCd { get; set; }

    public string BusinessUSZipCd { get; set; }

    public string EIN { get; set; }

    public string ContactPhoneNum { get; set; }

    public int employee_id { get; set; }

    public Guid EmployeeResourceId { get; set; }

    public string Fname { get; set; }

    public string Mi { get; set; }

    public string Lname { get; set; }

    public string Suffix { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string ZIP { get; set; }

    public string SSN { get; set; }

    public bool Is_Self_Insured { get; set; }

    public DateTime? PersonBirthDt { get; set; }

    public List<Coverage> Employee_Coverage { get; set; }

    public List<Dependent_IRS> Dependents { get; set; }

}