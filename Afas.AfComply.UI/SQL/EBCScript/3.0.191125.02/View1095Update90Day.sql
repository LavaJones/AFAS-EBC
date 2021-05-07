USE [aca]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[View_air_replacement_EmployeeYearlyDetails]
AS
SELECT 
       Distinct

                    dbo.employee.employee_id as EmployeeId, --no change-- remove this comment
                    dbo.plan_year.plan_year_id, 
                    dbo.measurement.measurement_id as MeasurementId, 
             dbo.EmployeeMeasurementAverageHours.WeeklyAverageHours, 
             dbo.EmployeeMeasurementAverageHours.MonthlyAverageHours, 
                    DATEPART(month, dbo.plan_year.startDate) AS StartMonth, 
                    dbo.tax_year.tax_year AS StartYear, -- These are here because we don't want to change the View Definition
                    DATEPART(month, dbo.plan_year.endDate) AS EndMonth, 
                    dbo.tax_year.tax_year AS EndYear,  -- These are here because we don't want to change the View Definition
                    dbo.plan_year.startDate as stability_start, 
                    dbo.plan_year.endDate as stability_end, 
             dbo.EmployeeMeasurementAverageHours.IsNewHire, 
                    dbo.employee.employee_type_id, 
                    dbo.employee.terminationDate, 
                   dbo.employee.initialMeasurmentEnd, 
                    dbo.employee.classification_id, 
                    dbo.employee.aca_status_id, 
                    dbo.employer.employer_id, -- Isn't this what we're looking for?
                dbo.employee_classification.ash_code, 
             dbo.employee_classification.WaitingPeriodID, 
                    dbo.employee_classification.Ooc, 
                    dbo.employee.hireDate,
                    dbo.initial_measurement.months AS InitialStabilityPeriodMonths, 
                    Case 
                           WHEN --A New hire's IMP can be lengthened to the start of the next Stability Period
                                 dbo.EmployeeMeasurementAverageHours.IsNewHire = 1 
                                        AND dbo.initial_measurement.months < 11 
                                        AND DATEADD(month, (dbo.initial_measurement.months + 2), dbo.employee.initialMeasurmentEnd) < dbo.plan_year.startDate
                                 THEN
                                        dbo.plan_year.startDate - 1

                           WHEN --A New hire's IMP can be lengthened to the start of the next Stability Period
                                 dbo.EmployeeMeasurementAverageHours.IsNewHire = 1 
                                        AND DATEADD(month, 13, dbo.employee.initialMeasurmentEnd) < dbo.plan_year.startDate
                                 THEN
                                        dbo.plan_year.startDate - 1
                           -- If not the NH special case, then check for the < 11 month case, otherwise use 13 months
                           When 
                                        dbo.initial_measurement.months < 11 
                                 Then 
                                        DATEADD(month, (dbo.initial_measurement.months + 2), dbo.employee.initialMeasurmentEnd) 

                           Else DATEADD(month, 13, dbo.employee.initialMeasurmentEnd)
                    End AS InitialStabilityPeriodEndDate,
                    dbo.tax_year.tax_year

FROM 

dbo.employer          
INNER JOIN
[dbo].[initial_measurement]
ON dbo.initial_measurement.initial_measurement_id = dbo.employer.initial_measurement_id

INNER JOIN [dbo].[tax_year] ON 1=1

INNER JOIN
dbo.plan_year 
ON dbo.plan_year.employer_id = dbo.employer.employer_id
AND (DATEPART(year, dbo.plan_year.startDate) = [dbo].[tax_year].tax_year 
OR DATEPART(year, dbo.plan_year.endDate) = [dbo].[tax_year].tax_year )

INNER JOIN 
dbo.employee_classification 
ON dbo.employee_classification.employer_id =  dbo.employer.employer_id

INNER JOIN
dbo.employee
ON dbo.employee.employer_id = dbo.employer.employer_id 
AND dbo.employee.classification_id = dbo.employee_classification.classification_id

LEFT JOIN
dbo.measurement 
ON dbo.measurement.employer_id = dbo.employer.employer_id
AND dbo.measurement.employee_type_id = dbo.employee.employee_type_id
AND dbo.measurement.plan_year_id = dbo.plan_year.plan_year_id

LEFT JOIN 
dbo.EmployeeMeasurementAverageHours                                
ON dbo.EmployeeMeasurementAverageHours.EmployeeId = dbo.employee.employee_id
AND dbo.EmployeeMeasurementAverageHours.MeasurementId = dbo.measurement.measurement_id
AND dbo.EmployeeMeasurementAverageHours.EntityStatusId = 1

GO
