USE [aca]
GO

/****** Object:  View [dbo].[View_air_replacement_EmployeeMonthlyDetailsPreFiltered]    Script Date: 12/15/2017 1:14:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[View_air_replacement_EmployeeMonthlyDetailsPreFiltered]
AS
SELECT v.EmployeeId, v.plan_year_id, v.MeasurementId, v.WeeklyAverageHours, v.MonthlyAverageHours, v.StartMonth, v.StartYear, v.EndMonth, v.EndYear, v.stability_start, 
                  v.stability_end, v.IsNewHire, v.employee_type_id, v.terminationDate, v.initialMeasurmentEnd, v.classification_id, v.aca_status_id, v.insurance_id, v.ins_cont_id, v.offered, 
                  v.offeredOn, v.accepted, v.acceptedOn, v.effectiveDate, v.hra_flex_contribution, v.contribution_id, v.amount, v.monthlycost, v.minValue, v.offSpouse, v.offDependent, 
                  v.insurance_type_id, v.Mec, v.SpouseConditional, v.fullyPlusSelfInsured, tym.month_id, tym.name, v.employer_id, v.ash_code, v.WaitingPeriodID, v.Ooc, v.hireDate, 
                  v.InitialStabilityPeriodMonths, v.InitialStabilityPeriodEndDate
FROM     dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer_and_Contribution AS v CROSS JOIN
                  dbo.tax_year_month AS tym

GO
GRANT SELECT ON dbo.View_air_replacement_EmployeeMonthlyDetailsPreFiltered TO [aca-user]
