USE [aca]
GO

/****** Object:  View [dbo].[View_air_replacement_EmployeeYearlyDetails]    Script Date: 1/10/2018 6:59:38 PM ******/
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
			coalesce(DATEPART(year, dbo.plan_year.startDate), dbo.insurance_coverage.tax_year) AS StartYear, 
			DATEPART(month, dbo.plan_year.endDate) AS EndMonth, 
			coalesce(DATEPART(year, dbo.plan_year.endDate), dbo.insurance_coverage.tax_year) AS EndYear, 
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
				When dbo.initial_measurement.months <= 11 Then DATEADD(month, (dbo.initial_measurement.months + 2), dbo.employee.initialMeasurmentEnd) 
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

LEFT JOIN 
dbo.insurance_coverage
on dbo.insurance_coverage.employee_id = dbo.employee.employee_id
AND dbo.tax_year.tax_year = dbo.insurance_coverage.tax_year
AND dbo.insurance_coverage.dependent_id is NULL
AND dbo.insurance_coverage.EntityStatusID = 1

LEFT JOIN 
dbo.employee_insurance_offer
on dbo.employee_insurance_offer.employee_id = dbo.employee.employee_id
AND dbo.employee_insurance_offer.plan_year_id = dbo.plan_year.plan_year_id



