using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Extension methods for IList<Employee> lists.
/// </summary>
public static class IListOfEmployeeExtensionMethods
{

    public static IQueryable<Measurement> FilterByPlanYearId(this IList<Measurement> measurements, int planYearId)
    {

        return
            (
                from measurement in measurements
                where
                    measurement.MEASUREMENT_PLAN_ID == planYearId
                select
                    measurement
            ).AsQueryable();

    }

    public static IQueryable<insuranceContribution> FilterForEmployeeClassByEmployeeClassId(this IList<insuranceContribution> insuranceContributions, int employeeClassId)
    {

        return
            (
                from insuranceContribution_ in insuranceContributions
                where
                    insuranceContribution_.INS_CONT_CLASSID == employeeClassId
                select
                    insuranceContribution_
            ).AsQueryable();

    }

    public static IQueryable<alert_insurance> FilterForEmployeeByEmployeeId(this IList<alert_insurance> insuranceAlerts, int employeeId)
    {

        return
            (
                from insuranceAlert in insuranceAlerts
                where
                    insuranceAlert.IALERT_EMPLOYEEID == employeeId
                select
                    insuranceAlert
            ).AsQueryable();

    }

    public static IQueryable<alert_insurance> FilterForPlanYearByPlanYearId(this IList<alert_insurance> insuranceAlerts, int planYearId)
    {

        return
            (
                from insuranceAlert in insuranceAlerts
                where
                    insuranceAlert.IALERT_PLANYEARID == planYearId
                select
                    insuranceAlert
            ).AsQueryable();

    }

    /// <summary>
    /// Filters a list of employee's with the same database primary key.
    /// </summary>
    public static IQueryable<Employee> FilterForEmployeeId(this IList<Employee> employees, int employeeId)
    {

        return
            (
                from employee in employees
                where
                    employee.EMPLOYEE_ID == employeeId
                select
                    employee
            ).AsQueryable();

    }

    /// <summary>
    /// Filters a list of employee's with the same ssn.
    /// </summary>
    public static IQueryable<Employee> FilterForSocialSecurityNumber(this IList<Employee> employees, String socialSecurityNumber)
    {

        return
            (
                from employee in employees
                where
                    employee.Employee_SSN_Visible.Equals(socialSecurityNumber)
                select
                    employee
            ).AsQueryable();

    }

}
