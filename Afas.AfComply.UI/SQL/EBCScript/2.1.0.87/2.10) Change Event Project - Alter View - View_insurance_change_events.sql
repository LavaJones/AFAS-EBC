USE [aca]
GO

/****** Object:  View [dbo].[View_insurance_change_events]    Script Date: 11/17/2017 2:53:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [dbo].[View_insurance_change_events]
AS
SELECT [rowid], [employee_id], [plan_year_id], [employer_id], [insurance_id], [ins_cont_id], [offered], [accepted], [effectiveDate], [hra_flex_contribution], [ResourceId]
FROM     dbo.employee_insurance_offer
UNION
SELECT [rowid], [employee_id], [plan_year_id], [employer_id], [insurance_id], [ins_cont_id], [offered], [accepted], [effectiveDate], [hra_flex_contribution], [ResourceId]
FROM     dbo.employee_insurance_offer_archive


GO


