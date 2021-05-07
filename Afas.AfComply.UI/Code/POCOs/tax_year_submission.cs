using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Afas.AfComply.Domain;

/// <summary>
/// Summary description for tax_year_submission
/// </summary>
public class tax_year_submission
{
    int id = 0;
    int employerID = 0;
    int taxYear = 0;

    bool? dge = null;
    string dgeName = null;
    string dgeEIN = null;
    string dgeAddress = null;
    string dgeCity = null;
    int dgeStateID = 0;
    string dgeZip = null;
    string dgeFirstName = null;
    string dgeLastName = null;
    string dgePhone = null;

    bool? aale = null;
    List<ale> aleMembers = new List<ale>();

    bool? tr_question1 = null;
    bool? tr_question2 = null;
    bool? tr_question3 = null;
    bool? tr_question4 = null;
    bool? tr_question5 = null;
    bool tr = false;

    bool? tobacco = null;

    bool? unpaidLeave = null;

    bool? ash = null;


    string completedBy = null;
    DateTime? completedOn = null;
    bool ebcApproved = false;
    string ebcApprovedBy = null;
    DateTime? ebcApprovedOn = null;
    bool allowEditing = false;
    
	public tax_year_submission()
	{
	}

    public tax_year_submission(int _id, int _employerID, int _taxYear, bool? _dge, string _dgeEIN, string _dgeName, string _dgeAddress, string _dgeCity, int _stateID, string _zip, string _firstName, string _lastName, string _phone, bool? _ale, List<ale> _aleMembers, bool? _tr1, bool? _tr2, bool? _tr3, bool? _tr4, bool? _tr5, bool _tr, bool? _ash, bool? _tobacco, bool? _unpaid, string _completedBy, DateTime? _completedOn, bool _ebcApproved, string _ebcApprovedBy, DateTime? _ebcApprovedOn, bool _allowEditing)
    {
        this.id = _id;
        this.employerID = _employerID;
        this.taxYear = _taxYear;
        this.dge = _dge;
        this.dgeName = _dgeName;
        this.dgeAddress = _dgeAddress;
        this.dgeEIN = _dgeEIN;
        this.dgeCity = _dgeCity;
        this.dgeStateID = _stateID;
        this.dgeZip = _zip;
        this.dgeFirstName = _firstName;
        this.dgeLastName = _lastName;
        this.dgePhone = _phone;
        this.aale = _ale;
        this.aleMembers = _aleMembers;
        this.tr_question1 = _tr1;
        this.tr_question2 = _tr2;
        this.tr_question3 = _tr3;
        this.tr_question4 = _tr4;
        this.tr_question5 = _tr5;
        this.tr = _tr;
        this.ash = _ash;
        this.tobacco = _tobacco;
        this.unpaidLeave = _unpaid;

        this.completedOn = _completedOn;
        this.completedBy = _completedBy;
        this.ebcApproved = _ebcApproved;
        this.ebcApprovedBy = _ebcApprovedBy;
        this.ebcApprovedOn = _ebcApprovedOn;
        this.allowEditing = _allowEditing;
    }

    public int IRS_ROW_ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    public int IRS_EMPLOYER_ID
    {
        get { return this.employerID; }
        set { this.employerID = value; }
    }

    public int IRS_TAX_YEAR
    {
        get { return this.taxYear; }
        set { this.taxYear = value; }
    }

    public bool? IRS_DGE
    {
        get { return this.dge; }
        set { this.dge = value; }
    }

    public string IRS_DGE_NAME
    {
        get { return this.dgeName; }
        set { this.dgeName = value; }
    }

    public string IRS_DGE_EIN
    {
        get { return this.dgeEIN; }
        set { this.dgeEIN = value; }
    }

    public string IRS_DGE_ADDRESS
    {
        get { return this.dgeAddress; }
        set { this.dgeAddress = value; }
    }

    public string IRS_DGE_CITY
    {
        get { return this.dgeCity; }
        set { this.dgeCity = value; }
    }

    public int IRS_DGE_STATE_ID
    {
        get { return this.dgeStateID; }
        set { this.dgeStateID = value; }
    }

    public string IRS_DGE_ZIP
    {
        get { return this.dgeZip.ZeroPadZip(); }
        set { this.dgeZip = value; }
    }

    public string IRS_DGE_CONTACT_FNAME
    {
        get { return this.dgeFirstName; }
        set { this.dgeFirstName = value; }
    }

    public string IRS_DGE_CONTACT_LNAME
    {
        get { return this.dgeLastName; }
        set { this.dgeLastName = value; }
    }

    public string IRS_DGE_PHONE
    {
        get { return this.dgePhone; }
        set { this.dgePhone = value; }
    }

    public bool? IRS_ALE
    {
        get { return this.aale; }
        set { this.aale = value; }
    }

    public List<ale> IRS_ALE_MEMBERS
    {
        get { return this.aleMembers; }
        set { this.aleMembers = value; }
    }

    public bool? IRS_TR_Q1
    {
        get { return this.tr_question1; }
        set { this.tr_question1 = value; }
    }

    public bool? IRS_TR_Q2
    {
        get { return this.tr_question2; }
        set { this.tr_question2 = value; }
    }

    public bool? IRS_TR_Q3
    {
        get { return this.tr_question3; }
        set { this.tr_question3 = value; }
    }

    public bool? IRS_TR_Q4
    {
        get { return this.tr_question4; }
        set { this.tr_question4 = value; }
    }

    public bool? IRS_TR_Q5
    {
        get { return this.tr_question5; }
        set { this.tr_question5 = value; }
    }

    public bool IRS_TR
    {
        get { return this.tr; }
        set { this.tr = value; }
    }

    public bool? IRS_ASH
    {
        get { return this.ash; }
        set { this.ash = value; }
    }

    public bool? IRS_TOBACCO
    {
        get { return this.tobacco; }
        set { this.tobacco = value; }
    }

    public bool? IRS_UNPAID_LEAVE
    {
        get { return this.unpaidLeave; }
        set { this.unpaidLeave = value; }
    }

    public string IRS_COMPLETED_BY
    {
        get { return this.completedBy; }
        set { this.completedBy = value; }
    }

    public DateTime? IRS_COMPLETED_ON
    {
        get { return this.completedOn; }
        set { this.completedOn = value; }
    }

    public string IRS_EBC_APPROVED_BY
    {
        get { return this.ebcApprovedBy; }
        set { this.ebcApprovedBy = value; }
    }

    public bool IRS_EBC_APPROVED
    {
        get { return this.ebcApproved; }
        set { this.ebcApproved = value; }
    }

    public DateTime? IRS_EBC_APPROVED_ON
    {
        get { return this.ebcApprovedOn; }
        set { this.ebcApprovedOn = value; }
    }

    public bool IRS_EDIT
    {
        get { return this.allowEditing; }
        set { this.allowEditing = value; }
    }
}