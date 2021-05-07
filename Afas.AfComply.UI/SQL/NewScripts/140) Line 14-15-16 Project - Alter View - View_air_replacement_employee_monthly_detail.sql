USE [aca]
GO

/****** Object:  View [dbo].[View_air_replacement_employee_monthly_detail]    Script Date: 12/13/2017 11:28:49 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[View_air_replacement_employee_monthly_detail]
AS
SELECT EmployeeMotnhlyDetailID = ROW_NUMBER() OVER (ORDER BY employer_id), employer_id, EmployeeId AS employee_id, terminationDate, aca_status_id, classification_id, 
month_id, MonthlyAverageHours, Ooc AS DefaultOfferOfCoverage, Mec, ash_code AS DefaultSafeHarborCode, amount AS EmployeesMonthlyCost, 
offered AS EmployeeOfferedCoverage, accepted AS EmployeeEnrolledInCoverage, insurance_type_id AS InsuranceType, offSpouse AS OfferedToSpouse, 
SpouseConditional AS OfferedToSpouseConditional, offDependent AS OfferedToDependent, 0 AS Mainland, effectiveDate AS CoverageEffectiveDate, fullyPlusSelfInsured, minValue, 
WaitingPeriodID, 2017 AS TaxYear, plan_year_id, hireDate
FROM     dbo.View_air_replacement_EmployeeMonthlyDetailsPreFiltered AS v
WHERE  (IsNewHire = 1) AND (initialMeasurmentEnd > CONVERT(varchar(2), month_id) + '/1/2017') AND (stability_start <= CONVERT(varchar(2), month_id) + '/1/2017') AND 
                  (CONVERT(varchar(2), month_id) + '/1/2017' < stability_end) OR
                  (IsNewHire = 0) AND (stability_start <= CONVERT(varchar(2), month_id) + '/1/2017') AND (CONVERT(varchar(2), month_id) + '/1/2017' < stability_end)

GO


