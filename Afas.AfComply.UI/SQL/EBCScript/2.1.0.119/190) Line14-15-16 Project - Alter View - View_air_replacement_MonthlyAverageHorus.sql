USE [aca]
GO

/****** Object:  View [dbo].[View_air_replacement_MonthlyAverageHours]    Script Date: 12/18/2017 11:07:10 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[View_air_replacement_MonthlyAverageHours]
AS
SELECT dbo.EmployeeMeasurementAverageHours.EmployeeId, dbo.measurement.plan_year_id, dbo.EmployeeMeasurementAverageHours.MeasurementId, 
                  dbo.EmployeeMeasurementAverageHours.WeeklyAverageHours, dbo.EmployeeMeasurementAverageHours.MonthlyAverageHours, DATEPART(month, 
                  dbo.measurement.stability_start) AS StartMonth, DATEPART(year, dbo.measurement.stability_start) AS StartYear, DATEPART(month, dbo.measurement.stability_end) 
                  AS EndMonth, DATEPART(year, dbo.measurement.stability_end) AS EndYear, dbo.measurement.stability_start, dbo.measurement.stability_end, 
                  dbo.EmployeeMeasurementAverageHours.IsNewHire, dbo.employee.employee_type_id, dbo.employee.terminationDate, dbo.employee.initialMeasurmentEnd, 
                  dbo.employee.classification_id, dbo.employee.aca_status_id, dbo.employee.employer_id, dbo.employee_classification.ash_code, 
                  dbo.employee_classification.WaitingPeriodID, dbo.employee_classification.Ooc, dbo.employee.hireDate
FROM     dbo.employee_classification INNER JOIN
                  dbo.employee ON dbo.employee_classification.classification_id = dbo.employee.classification_id RIGHT OUTER JOIN
                  dbo.EmployeeMeasurementAverageHours ON dbo.employee.employee_id = dbo.EmployeeMeasurementAverageHours.EmployeeId LEFT OUTER JOIN
                  dbo.measurement ON dbo.EmployeeMeasurementAverageHours.MeasurementId = dbo.measurement.measurement_id
WHERE  (dbo.EmployeeMeasurementAverageHours.EntityStatusId = 1)

GO


