USE [aca]
GO

/****** Object:  View [dbo].[View_air_replacement_MonthlyAverageHours_with_InsuranceOffer]    Script Date: 12/15/2017 1:15:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[View_air_replacement_MonthlyAverageHours_with_InsuranceOffer]
AS
SELECT dbo.View_air_replacement_MonthlyAverageHours.EmployeeId, dbo.View_air_replacement_MonthlyAverageHours.plan_year_id, 
                  dbo.View_air_replacement_MonthlyAverageHours.MeasurementId, dbo.View_air_replacement_MonthlyAverageHours.WeeklyAverageHours, 
                  dbo.View_air_replacement_MonthlyAverageHours.MonthlyAverageHours, dbo.View_air_replacement_MonthlyAverageHours.StartMonth, 
                  dbo.View_air_replacement_MonthlyAverageHours.StartYear, dbo.View_air_replacement_MonthlyAverageHours.EndMonth, 
                  dbo.View_air_replacement_MonthlyAverageHours.EndYear, dbo.View_air_replacement_MonthlyAverageHours.stability_start, 
                  dbo.View_air_replacement_MonthlyAverageHours.stability_end, dbo.View_air_replacement_MonthlyAverageHours.IsNewHire, 
                  dbo.View_air_replacement_MonthlyAverageHours.employee_type_id, dbo.View_air_replacement_MonthlyAverageHours.terminationDate, 
                  dbo.View_air_replacement_MonthlyAverageHours.initialMeasurmentEnd, dbo.View_air_replacement_MonthlyAverageHours.classification_id, 
                  dbo.View_air_replacement_MonthlyAverageHours.aca_status_id, dbo.employee_insurance_offer.insurance_id, dbo.employee_insurance_offer.ins_cont_id, 
                  dbo.employee_insurance_offer.offered, dbo.employee_insurance_offer.offeredOn, dbo.employee_insurance_offer.accepted, dbo.employee_insurance_offer.acceptedOn, 
                  dbo.employee_insurance_offer.effectiveDate, dbo.employee_insurance_offer.hra_flex_contribution, dbo.View_air_replacement_MonthlyAverageHours.employer_id, 
                  dbo.View_air_replacement_MonthlyAverageHours.ash_code, dbo.View_air_replacement_MonthlyAverageHours.WaitingPeriodID, 
                  dbo.View_air_replacement_MonthlyAverageHours.Ooc, dbo.View_air_replacement_MonthlyAverageHours.hireDate, dbo.employer.initial_measurement_id
FROM     dbo.View_air_replacement_MonthlyAverageHours LEFT OUTER JOIN
                  dbo.employer ON dbo.View_air_replacement_MonthlyAverageHours.employer_id = dbo.employer.employer_id LEFT OUTER JOIN
                  dbo.employee_insurance_offer ON dbo.View_air_replacement_MonthlyAverageHours.plan_year_id = dbo.employee_insurance_offer.plan_year_id AND 
                  dbo.View_air_replacement_MonthlyAverageHours.EmployeeId = dbo.employee_insurance_offer.employee_id

GO
