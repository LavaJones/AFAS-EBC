using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Extension methods for List<Employee_E> lists.
/// </summary>
public static class ListOfEmployeeEExtensionMethods
{

    /// <summary>
    /// Make the best guess on who is an active Employee_E in the list based on the current tax year date range.
    /// </summary>
    public static IList<Employee_E> FilterForActiveEmployees(this List<Employee_E> employees)
    {
        
        List<Employee_E> terminatedEmployeesInPlanYear = (
                                                          from Employee_E employeee in employees.FilterForEmployeesWithATerminationDate()
                                                          where employeee.EMPLOYEE_TERM_DATE.Value >= CURRENT_TAX_YEAR_START_DATE
                                                            &&
                                                          employeee.EMPLOYEE_TERM_DATE.Value <= CURRENT_TAX_YEAR_END_DATE
                                                          select employeee
                                                         ).ToList();

        List<Employee_E> activeEmployees = employees.FilterForEmployeesWithoutTerminationDate();

        activeEmployees.AddRange(terminatedEmployeesInPlanYear);

        return activeEmployees;

    }

    /// <summary>
    /// Return employees that have a termination date set.
    /// </summary>
    public static List<Employee_E> FilterForEmployeesWithATerminationDate(this List<Employee_E> employees)
    {

        return (
                from Employee_E employeee in employees.AsEnumerable()
                where employeee.EMPLOYEE_TERM_DATE.HasValue == true
                select employeee
               ).ToList();

    }

    /// <summary>
    /// Return employees that do not have a termination date set.
    /// </summary>
    public static List<Employee_E> FilterForEmployeesWithoutTerminationDate(this List<Employee_E> employees)
    {

        return (
                from Employee_E employeee in employees.AsEnumerable()
                where employeee.EMPLOYEE_TERM_DATE.HasValue == false
                select employeee
               ).ToList();

    }

    private static DateTime CURRENT_TAX_YEAR_END_DATE = DateTime.Parse("2017-01-01T00:00:00");
    private static DateTime CURRENT_TAX_YEAR_START_DATE = DateTime.Parse("2016-01-01T00:00:00");

}
