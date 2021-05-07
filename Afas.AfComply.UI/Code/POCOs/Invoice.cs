using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Invoice
/// </summary>
public class Invoice
{
    public int INVOICE_ID {get; set;}
    public int INVOICE_EMPLOYERID { get; set; }
    public int INVOICE_COUNT { get; set; }
    public double INVOICE_BASE_FEE { get; set; }
    public double INVOICE_PART_FEE { get; set; }
    public double INVOICE_SU_FEE { get; set; }
    public double INVOICE_TOTAL { get; set; }
    public DateTime INVOICE_MODON { get; set; }
    public string INVOICE_MODBY { get; set; }
    public string INVOICE_MESSAGE { get; set; }
    public int INVOICE_MONTH { get; set; }
    public int INVOICE_YEAR { get; set; }
    public bool INVOICE_PAID { get; set; }
    public string INVOICE_HISTORY { get; set; }

	public Invoice(int _id, int _employerID, int _count, double _bfee, double _pfee, double _sufee, double _total, DateTime _modOn, string _modBy, string _message, int _month, int _year, bool _paid, string _history)
	{
        this.INVOICE_ID = _id;
        this.INVOICE_EMPLOYERID = _employerID;
        this.INVOICE_COUNT = _count;
        this.INVOICE_BASE_FEE = _bfee;
        this.INVOICE_PART_FEE = _pfee;
        this.INVOICE_SU_FEE = _sufee;
        this.INVOICE_TOTAL = _total;
        this.INVOICE_MODON = _modOn;
        this.INVOICE_MODBY = _modBy;
        this.INVOICE_MESSAGE = _message;
        this.INVOICE_YEAR = _year;
        this.INVOICE_MONTH = _month;
        this.INVOICE_PAID = _paid;
        this.INVOICE_HISTORY = _history;
	}
}