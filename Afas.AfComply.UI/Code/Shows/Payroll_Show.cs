using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Payroll_Show
/// </summary>
public class Payroll_Show
{
    public Payroll_I getSinglePayroll(int _rowID, List<Payroll_I> _tempList)
    {
        Payroll_I tempPayrollI = null;

        foreach (Payroll_I pi in _tempList)
        {
            if (pi.ROW_ID == _rowID)
            {
                tempPayrollI = pi;
                break;
            }
        }

        return tempPayrollI;
    }

    public Payroll_E getSinglePayroll(int _rowID, List<Payroll_E> _tempList)
    {
        Payroll_E tempPayrollE = null;

        foreach (Payroll_E pi in _tempList)
        {
            if (pi.ROW_ID == _rowID)
            {
                tempPayrollE = pi;
                break;
            }
        }

        return tempPayrollE;
    }

    public Payroll getSinglePayroll(int _rowID, List<Payroll> _tempList)
    {
        Payroll tempPayroll = null;

        foreach (Payroll pi in _tempList)
        {
            if (pi.ROW_ID == _rowID)
            {
                tempPayroll = pi;
                break;
            }
        }

        return tempPayroll;
    }
}