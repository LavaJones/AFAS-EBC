using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EmployeeTypeController
/// </summary>
public class EmployeeTypeController
{
    public static List<EmployeeType> getEmployeeTypes(int _employerID)
    {
        EmployeeTypeFactory etf = new EmployeeTypeFactory();
        return etf.manufactureEmployeeType(_employerID);
    }

    public static bool insertEmployeeType(string _name, int _employerID)
    {
        EmployeeTypeFactory etf = new EmployeeTypeFactory();
        return etf.NewEmployeeType(_name, _employerID);
    }

    public static bool updateEmployeeType(string _name, int _typeId)
    {
        EmployeeTypeFactory etf = new EmployeeTypeFactory();
        return etf.UpdateEmployeeType(_name, _typeId);
    }
    
    public static bool deleteEmployeeType(int _typeId)
    {
        EmployeeTypeFactory etf = new EmployeeTypeFactory();
        return etf.DeleteEmployeeType(_typeId);
    }
}