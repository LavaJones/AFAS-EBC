USE [aca]
GO

/****** Object:  View [dbo].[View_insurance_change_events_details]    Script Date: 11/17/2017 2:53:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[View_insurance_change_events_details]
AS
SELECT UniqueRowID = ROW_NUMBER() OVER (ORDER BY employee_id), dbo.View_insurance_change_events.rowid, dbo.View_insurance_change_events.employee_id, dbo.View_insurance_change_events.plan_year_id, 
                  dbo.View_insurance_change_events.employer_id, dbo.View_insurance_change_events.insurance_id, dbo.View_insurance_change_events.ins_cont_id, 
                  dbo.View_insurance_change_events.offered, dbo.View_insurance_change_events.accepted, dbo.View_insurance_change_events.effectiveDate, 
                  dbo.View_insurance_change_events.hra_flex_contribution, dbo.insurance_contribution.amount AS EmployerContribution, 
                  dbo.insurance_contribution.contribution_id AS EmployerContributionType, dbo.insurance.monthlycost, dbo.insurance.minValue, dbo.insurance.offSpouse, 
                  dbo.insurance.offDependent, dbo.insurance.insurance_type_id, dbo.insurance.SpouseConditional, dbo.insurance.Mec, dbo.insurance.fullyPlusSelfInsured
FROM     dbo.View_insurance_change_events LEFT OUTER JOIN
                  dbo.insurance_contribution ON dbo.View_insurance_change_events.ins_cont_id = dbo.insurance_contribution.ins_cont_id LEFT OUTER JOIN
                  dbo.insurance ON dbo.View_insurance_change_events.insurance_id = dbo.insurance.insurance_id

GO
