using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for fee
/// </summary>
public class fee
{
    private int id = 0;
    private double suFee = 0;
    private double baseFee = 0;
    private double employeeFee = 0;
    private string history = null;
    private DateTime modOn;
    private string modBy = null;

	public fee(int _id, double _suFee, double _baseFee, double _empFee, string _history, DateTime _modOn, string _modBy)
	{
        this.id = _id;
        this.suFee = _suFee;
        this.baseFee = _baseFee;
        this.employeeFee = _empFee;
        this.history = _history;
        this.modOn = _modOn;
        this.modBy = _modBy;
	}

    public int FEE_ID
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

    public double FEE_SU
    {
        get
        {
            return this.suFee;
        }
        set
        {
            this.suFee = value;
        }
    }

    public double FEE_EMPLOYEE
    {
        get
        {
            return this.employeeFee;
        }
        set
        {
            this.employeeFee = value;
        }
    }

    public double FEE_BASE
    {
        get
        {
            return this.baseFee;
        }
        set
        {
            this.baseFee = value;
        }
    }

    public string FEE_HISTORY
    {
        get
        {
            return this.history;
        }
        set
        {
            this.history = value;
        }
    }

    public DateTime FEE_MODON
    {
        get
        {
            return this.modOn;
        }
        set
        {
            this.modOn = value;
        }
    }

    public string FEE_MODBY
    {
        get
        {
            return this.modBy;
        }
        set
        {
            this.modBy = value;
        }
    }
}