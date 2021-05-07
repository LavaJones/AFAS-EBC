using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

/// <summary>
/// Summary description for EmployeeShow
/// </summary>
public class EmployeeShow
{
    private ILog Log = LogManager.GetLogger(typeof(EmployeeShow));

    public Employee_I findEmployee(List<Employee_I> _empList, int _rowID)
    {
        Employee_I tempEmp = null;

        if (_empList != null)
            foreach (Employee_I emp in _empList)
            {
                if (emp.ROW_ID == _rowID)
                {
                    tempEmp = emp;
                    break;
                }
            }

        return tempEmp;
    }

    public Employee findEmployee(List<Employee> _empList, int _employeeID)
    {
        Employee tempEmp = null;

        if (_empList != null)
            foreach (Employee emp in _empList)
            {
                if (emp.EMPLOYEE_ID == _employeeID)
                {
                    tempEmp = emp;
                    break;
                }
            }

        return tempEmp;
    }

    public Employee validateExistingEmployee(List<Employee> _emp, string _ssn)
    {
        Employee tempEmp = null;

        if (_emp != null)
            foreach (Employee emp in _emp)
            {
                if (emp.Employee_SSN_Visible == _ssn)
                {
                    tempEmp = emp;
                    break;
                }
            }

        return tempEmp;
    }

    /// <summary>
    /// Calculate the Percent complete of the employees Transition Measurement Cycle. 
    /// </summary>
    /// <param name="_emp"></param>
    /// <param name="_m"></param>
    /// <returns></returns>
    public int getDateTimeComplete_Percent(DateTime _sdate, DateTime _edate)
    {
        DateTime today = System.DateTime.Now;

        decimal days = 0;
        decimal pastDays = 0;
        decimal percent = 0;

        days = (_edate - _sdate).Days;

        if (today > _edate)
        {
            pastDays = (_edate - _sdate).Days;
        }
        else
        {
            pastDays = (today - _sdate).Days;
        }

        if (days != 0)
        {
            percent = (pastDays / days) * 100;
        }

        return (int)percent;
    }

    public bool calculateIMP(int _employerID, int _employeeTypeID, int _planYearID, DateTime _hdate, int _measTypeID)
    {
        bool imp = false;
        Measurement currMeas = null;

        try
        {
            currMeas = measurementController.getPlanYearMeasurement(_employerID, _planYearID, _employeeTypeID);

            if (_hdate > currMeas.MEASUREMENT_START)
            {
                imp = true;
            }
        }
        catch (NullReferenceException exception)
        {

            this.Log.Warn(
                    String.Format(
                            "Failed To load Measurement Period for _employerID: [{0}], _planYearID: [{1}], _employeeTypeID: [{2}]",
                            _employerID,
                            _planYearID,
                            _employeeTypeID
                        ), 
                    exception
                );
            
            imp = false;

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);
            
            imp = false;
        
        }

        return imp;

    }

    public bool calculateTMP(int _employerID, int _employeeTypeID, int _planYearID, DateTime _hdate, int _measTypeID)
    {
        Measurement meas = null;
        bool tmp = false;
        List<Measurement> measList = new List<Measurement>();

        try
        {
            measList = measurementController.manufactureMeasurementList(_employerID);

            foreach (Measurement m in measList)
            {
                if (_employerID == m.MEASUREMENT_EMPLOYER_ID && _planYearID == m.MEASUREMENT_PLAN_ID && _employeeTypeID == m.MEASUREMENT_EMPLOYEE_TYPE_ID && _measTypeID == m.MEASUREMENT_TYPE_ID)
                {
                    meas = m;
                    break;
                }
            }

            if (_hdate <= meas.MEASUREMENT_START)
            {
                tmp = true;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tmp = false;
        }

        return tmp;
    }

    /// <summary>
    /// 1-7-2014
    /// Needs to be changed to work with new Business Rules. 
    /// </summary>
    /// <param name="tempPayroll"></param>
    /// <returns></returns>
    public static double calculateTotalHoursWorked(List<Payroll> tempPayroll, DateTime _sDate, DateTime _eDate, bool newHire = false)
    {
        double totalHours = 0;

        foreach (Payroll p in tempPayroll)
        {
            int payroll_days = 0;
            int mp_days = 0;
            double periodAvgHoursPerDay = 0;

            payroll_days = (int)Math.Abs(((DateTime)p.PAY_EDATE - (DateTime)p.PAY_SDATE).TotalDays);
            periodAvgHoursPerDay = (double)p.PAY_HOURS / payroll_days;

            if (p.PAY_SDATE >= _sDate && p.PAY_EDATE <= _eDate)
            {
                totalHours += (double)p.PAY_HOURS;
                continue;
            }

            else if (p.PAY_SDATE < _sDate && p.PAY_EDATE >= _sDate && p.PAY_EDATE <= _eDate)
            {
                if (newHire)
                {
                    totalHours += (double)p.PAY_HOURS;
                }
                else
                {
                    mp_days = (int)Math.Abs(((DateTime)p.PAY_EDATE - _sDate).TotalDays);
                    totalHours += mp_days * periodAvgHoursPerDay;
                }
                continue;
            }

            else if (p.PAY_SDATE >= _sDate && p.PAY_SDATE <= _eDate && p.PAY_EDATE > _eDate)
            {
                mp_days = (int)Math.Abs(((DateTime)p.PAY_SDATE - _eDate).TotalDays);
                totalHours += mp_days * periodAvgHoursPerDay;
                continue;
            }

            else if (p.PAY_SDATE < _sDate && p.PAY_EDATE > _eDate)
            {
                mp_days = (int)Math.Abs(((DateTime)p.PAY_SDATE - (DateTime)p.PAY_EDATE).TotalDays);
                totalHours += mp_days * periodAvgHoursPerDay;
                continue;
            }
        }

        return totalHours;
    }

    public string calculateBarColor(double _hours)
    {
        string imageName = null;

        if (_hours < 100)
        {
            imageName = "bg_nav_red.png";
        }
        else if (_hours >= 100 && _hours < 130)
        {
            imageName = "bg_nav_orange.png";
        }
        else
        {
            imageName = "bg_nav_green.png";
        }

        return imageName;
    }

    /// <summary>
    /// Search a list of Dependent objects for a single dependent. 
    /// </summary>
    /// <param name="dependentList"></param>
    /// <param name="_dependentID"></param>
    /// <returns></returns>
    public Dependent findSingleDependent(List<Dependent> dependentList, int _dependentID)
    {
        Dependent tempDependent = null;

        foreach (Dependent d in dependentList)
        {
            if (d.DEPENDENT_ID == _dependentID)
            {
                tempDependent = d;
                break;
            }
        }

        return tempDependent;
    }
    
}