USE [aca]
GO

/****** Object:  View [dbo].[View_air_replacement_MonthlyAverageHours_with_InsuranceOffer_and_Contribution]    Script Date: 12/15/2017 1:16:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[View_air_replacement_MonthlyAverageHours_with_InsuranceOffer_and_Contribution]
AS
SELECT dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.EmployeeId, dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.plan_year_id, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.MeasurementId, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.WeeklyAverageHours, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.MonthlyAverageHours, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.StartMonth, dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.StartYear, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.EndMonth, dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.EndYear, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.stability_start, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.stability_end, dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.IsNewHire, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.employee_type_id, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.terminationDate, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.initialMeasurmentEnd, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.classification_id, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.aca_status_id, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.insurance_id, dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.ins_cont_id, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.offered, dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.offeredOn, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.accepted, dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.acceptedOn, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.effectiveDate, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.hra_flex_contribution, dbo.insurance_contribution.contribution_id, 
                  dbo.insurance_contribution.amount, dbo.insurance.monthlycost, dbo.insurance.minValue, dbo.insurance.offSpouse, dbo.insurance.offDependent, 
                  dbo.insurance.insurance_type_id, dbo.insurance.Mec, dbo.insurance.SpouseConditional, dbo.insurance.fullyPlusSelfInsured, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.employer_id, dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.ash_code, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.WaitingPeriodID, dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.Ooc, 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.hireDate, dbo.initial_measurement.months AS InitialStabilityPeriodMonths, DATEADD(month, 
                  dbo.initial_measurement.months, dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.initialMeasurmentEnd) AS InitialStabilityPeriodEndDate
FROM     dbo.initial_measurement RIGHT OUTER JOIN
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer ON 
                  dbo.initial_measurement.initial_measurement_id = dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.initial_measurement_id LEFT OUTER JOIN
                  dbo.insurance INNER JOIN
                  dbo.insurance_contribution ON dbo.insurance.insurance_id = dbo.insurance_contribution.insurance_id ON 
                  dbo.View_air_replacement_MonthlyAverageHours_with_InsuranceOffer.ins_cont_id = dbo.insurance_contribution.ins_cont_id
