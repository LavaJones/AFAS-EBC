USE [aca]
GO

/****** Object:  View [dbo].[View_1094Part3]    Script Date: 3/7/2018 3:49:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[View_1094Part3]
AS


   
  SELECT count(*) as NoOfEmployees,
             f.employerID as EmployerId,
             Receiving1095C ,
            f.TaxYear,
             Offered as OfferedInsurance,
             MonthId
  FROM [aca].[dbo].[Approved1095FinalPart2] p2
  join [aca].[dbo].[Approved1095Final] f on p2.Approved1095Final_ID=f.Approved1095FinalId  
  INNER JOIN (SELECT 
employer_id,
employee_id,
DATEPART(month,PeriodStartDate) as PeriodStartDate,
DATEPART(month,PeriodEndDate) as PeriodEndDate,
TypeOfPeriod
FROM (
select distinct
dbo.employer.employer_id,
dbo.employee.employee_id,
dbo.employee.hireDate AS PeriodStartDate,
Case 
    When dbo.initial_measurement.months <= 11 Then DATEADD(month, 2, dbo.employee.initialMeasurmentEnd) 
    Else DATEADD(month, 1, dbo.employee.initialMeasurmentEnd)
End AS PeriodEndDate,
'IMP' as TypeOfPeriod

From

dbo.employer          
INNER JOIN
[dbo].[initial_measurement]
ON dbo.initial_measurement.initial_measurement_id = dbo.employer.initial_measurement_id

INNER JOIN
dbo.employee
ON dbo.employee.employer_id = dbo.employer.employer_id 

Where aca_status_id = 5
UNION
select distinct
dbo.employer.employer_id,
dbo.employee.employee_id,
Case 
    When dbo.initial_measurement.months <= 11 Then DATEADD(month, 2, dbo.employee.initialMeasurmentEnd) 
    Else DATEADD(month, 1, dbo.employee.initialMeasurmentEnd)
End AS InitialStabilityPeriodStartDate, 
Case 
    When dbo.initial_measurement.months <= 11 Then DATEADD(month, (dbo.initial_measurement.months + 2), dbo.employee.initialMeasurmentEnd) 
    Else DATEADD(month, 13, dbo.employee.initialMeasurmentEnd)
End AS InitialStabilityPeriodEndDate,
'ISP' as TypeOfPeriod

From

dbo.employer          
INNER JOIN
[dbo].[initial_measurement]
ON dbo.initial_measurement.initial_measurement_id = dbo.employer.initial_measurement_id

INNER JOIN
dbo.employee
ON dbo.employee.employer_id = dbo.employer.employer_id 

INNER JOIN
[dbo].[EmployeeMeasurementAverageHours] 
ON dbo.EmployeeMeasurementAverageHours.EmployeeId = dbo.employee.employee_id
And
dbo.EmployeeMeasurementAverageHours.IsNewHire = 1 
And 
dbo.EmployeeMeasurementAverageHours.MonthlyAverageHours >= 130.0

WHERE dbo.EmployeeMeasurementAverageHours.EntityStatusId = 1) as a
) as mp ON mp.employee_id = p2.employeeID
where (p2.Line14<>'1H' and p2.Line16<>'2d') and 
p2.EntityStatusId=1 and 
f.EntityStatusId=1
group by MonthId, Offered,f.TaxYear,f.employerID, Receiving1095C 
 









GO


