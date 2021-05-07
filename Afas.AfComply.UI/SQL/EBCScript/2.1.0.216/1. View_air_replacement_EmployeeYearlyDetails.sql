USE [aca]
GO

/****** Object:  View [dbo].[View_air_replacement_EmployeeYearlyDetails]    Script Date: 1/10/2018 6:59:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_air_replacement_EmployeeYearlyDetails]
AS
SELECT DISTINCT
              dbo.employee.employee_id as EmployeeId, 
              dbo.measurement.plan_year_id, 
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
DATEADD(month, dbo.initial_measurement.months, dbo.employee.initialMeasurmentEnd) AS InitialStabilityPeriodEndDate,
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
AND dbo.EmployeeMeasurementAverageHours.MeasurementId = dbo.measurement.measurement_id AND dbo.EmployeeMeasurementAverageHours.EntityStatusId = 1

LEFT JOIN 
dbo.insurance_coverage
on dbo.insurance_coverage.employee_id = dbo.employee.employee_id
AND dbo.tax_year.tax_year = dbo.insurance_coverage.tax_year
AND dbo.insurance_coverage.dependent_id is NULL

LEFT JOIN 
dbo.employee_insurance_offer
on dbo.employee_insurance_offer.employee_id = dbo.employee.employee_id
AND dbo.employee_insurance_offer.plan_year_id = dbo.plan_year.plan_year_id



--WHERE dbo.EmployeeMeasurementAverageHours.EntityStatusId = 1 or dbo.EmployeeMeasurementAverageHours.EntityStatusId is null
--tax_year.tax_year = 2017 AND AND dbo.employer.employer_id = 65
GO

ALTER VIEW [dbo].[View_air_replacement_EmployeeMonthlyDetailsPreFiltered]
AS
SELECT v.EmployeeId, v.plan_year_id, v.MeasurementId, v.WeeklyAverageHours, v.MonthlyAverageHours, v.StartMonth, v.StartYear, v.EndMonth, v.EndYear, v.stability_start, 
                  v.stability_end, v.IsNewHire, v.employee_type_id, v.terminationDate, v.initialMeasurmentEnd, v.classification_id, v.aca_status_id, 

                               0 as insurance_id, 0 as ins_cont_id, cast(0 as bit) as offered, NULL as offeredOn, cast(0 as bit) as accepted, NULL as acceptedOn, NULL as effectiveDate, 0.0 as hra_flex_contribution, NULL as contribution_id, 
                  0.0 as amount, 0.0 as monthlycost, cast(0 as bit) as minValue, cast(0 as bit) as offSpouse, cast(0 as bit) as offDependent, 0 as insurance_type_id, cast(0 as bit) as Mec, cast(0 as bit) as SpouseConditional, cast(0 as bit) as fullyPlusSelfInsured, 

                               tym.month_id, tym.name, v.employer_id, v.ash_code, v.WaitingPeriodID, v.Ooc, v.hireDate, 
                  v.InitialStabilityPeriodMonths, v.InitialStabilityPeriodEndDate
FROM     
(
SELECT 
              dbo.employee.employee_id as EmployeeId, 
              dbo.measurement.plan_year_id, 
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
DATEADD(month, dbo.initial_measurement.months, dbo.employee.initialMeasurmentEnd) AS InitialStabilityPeriodEndDate

FROM 

dbo.employer          
INNER JOIN
[dbo].[initial_measurement]
ON dbo.initial_measurement.initial_measurement_id = dbo.employer.initial_measurement_id
INNER JOIN
dbo.employee
ON dbo.employee.employer_id = dbo.employer.employer_id 
INNER JOIN 
dbo.employee_classification 
ON dbo.employee_classification.classification_id = dbo.employee.classification_id 

RIGHT OUTER JOIN
(
       dbo.insurance_coverage
       FULL OUTER JOIN
       (
              (
                      dbo.employee_insurance_offer 
                      FULL OUTER JOIN
                      (
                             dbo.EmployeeMeasurementAverageHours                                
                             INNER JOIN
                             dbo.measurement 
                             ON dbo.EmployeeMeasurementAverageHours.MeasurementId = dbo.measurement.measurement_id
                      )
                      ON dbo.measurement.plan_year_id = dbo.employee_insurance_offer.plan_year_id AND
                      dbo.EmployeeMeasurementAverageHours.EmployeeId = dbo.employee_insurance_offer.employee_id
              )
              INNER JOIN
              dbo.plan_year 
              ON (dbo.plan_year.plan_year_id = dbo.measurement.plan_year_id OR dbo.plan_year.plan_year_id = dbo.employee_insurance_offer.plan_year_id) 
       )
       ON (dbo.insurance_coverage.employee_id = dbo.EmployeeMeasurementAverageHours.EmployeeId OR dbo.insurance_coverage.employee_id = dbo.employee_insurance_offer.employee_id)
       AND (DATEPART(year, dbo.plan_year.startDate) = dbo.insurance_coverage.tax_year OR DATEPART(year, dbo.plan_year.endDate) = dbo.insurance_coverage.tax_year)
) 
ON (dbo.EmployeeMeasurementAverageHours.EmployeeId = dbo.employee.employee_id OR dbo.employee_insurance_offer.employee_id = dbo.employee.employee_id OR dbo.insurance_coverage.employee_id = dbo.employee.employee_id)


WHERE  dbo.EmployeeMeasurementAverageHours.EntityStatusId = 1


) AS v 
CROSS JOIN                  
dbo.tax_year_month AS tym



GO
GRANT SELECT ON [dbo].[View_air_replacement_EmployeeYearlyDetails] TO [aca-user]
GO
GRANT SELECT ON [dbo].[View_air_replacement_EmployeeMonthlyDetailsPreFiltered] TO [aca-user]

