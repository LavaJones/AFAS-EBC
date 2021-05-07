USE [aca]
GO

/****** Object:  View [dbo].[View_PlanYear_Insurance]    Script Date: 10/12/2017 9:48:59 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[View_PlanYear_Insurance]
AS
SELECT planYear.plan_year_id, planYear.employer_id, planYear.description, planYear.startDate, planYear.endDate, planYear.notes, planYear.history, planYear.modOn, 
                  planYear.modBy, dbo.insurance.insurance_id, dbo.insurance.description AS Expr1, dbo.insurance.monthlycost, dbo.insurance.minValue, dbo.insurance.offSpouse, 
                  dbo.insurance.offDependent, dbo.insurance.ModifiedDate, dbo.insurance.ModifiedBy, dbo.insurance.history AS Expr4, dbo.insurance.insurance_type_id
FROM     dbo.plan_year AS planYear RIGHT OUTER JOIN
                  dbo.insurance ON planYear.plan_year_id = dbo.insurance.plan_year_id

GO
