USE [aca]
GO

DROP VIEW [dbo].[View_employer_alerts]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_employer_alerts]
AS
SELECT
	[alert].[alert_id],
	[alert].[alert_type_id],
	[alert].[employer_id],
	[alert_type].[name],
	[alert_type].[image_url],
	[alert_type].[table_name]
FROM [dbo].[alert] as [alert] LEFT OUTER JOIN
	[dbo].[alert_type] ON [alert].[alert_type_id] = [alert_type].[alert_type_id]
GO

DROP VIEW [dbo].[View_PlanYear_Insurance]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_PlanYear_Insurance]
AS
SELECT
	[planYear].[plan_year_id],
	[planYear].[employer_id],
	[planYear].[description],
	[planYear].[startDate],
	[planYear].[endDate],
	[planYear].[notes],
	[planYear].[history],
	[planYear].[modOn],
	[planYear].[modBy],
	[dbo].[insurance].[insurance_id],
	[dbo].[insurance].[description] AS Expr1,
	[dbo].[insurance].[monthlycost],
	[dbo].[insurance].[minValue],
	[dbo].[insurance].[offSpouse],
	[dbo].[insurance].[offDependent],
    [dbo].[insurance].[modOn] AS Expr2,
    [dbo].[insurance].[modBy] AS Expr3,
    [dbo].[insurance].[history] AS Expr4,
    [dbo].[insurance].[insurance_type_id]
FROM [dbo].[plan_year] as [planYear] RIGHT OUTER JOIN
	[dbo].[insurance] ON [planYear].[plan_year_id] = [dbo].[insurance].[plan_year_id]
GO

GRANT SELECT ON [dbo].[View_PlanYear_Insurance] TO [aca-user] AS [dbo]
GO

DROP VIEW [dbo].[View_payroll_summer_avg]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_payroll_summer_avg]
AS
SELECT
	[payrollSummerAvg].[row_id],
	[payrollSummerAvg].[employer_id],
	[payrollSummerAvg].[plan_year_id],
	[payrollSummerAvg].[batch_id],
	[payrollSummerAvg].[employee_id],
	[payrollSummerAvg].[gp_id],
	[payrollSummerAvg].[act_hours],
	[payrollSummerAvg].[sdate],
	[payrollSummerAvg].[edate],
	[payrollSummerAvg].[cdate],
	[payrollSummerAvg].[modBy],
	[payrollSummerAvg].[modOn],
	[payrollSummerAvg].[history],
	[dbo].[gross_pay_type].[description]
FROM [dbo].[payroll_summer_averages] as [payrollSummerAvg] LEFT OUTER JOIN
	[dbo].[gross_pay_type] ON [payrollSummerAvg].[gp_id] = [dbo].[gross_pay_type].[gross_pay_id]
GO

GRANT SELECT ON [dbo].[View_payroll_summer_avg] TO [aca-user] AS [dbo]
GO

DROP VIEW[dbo].[View_Insurance_Contributions]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Insurance_Contributions]
AS
SELECT
	[dbo].[insurance_contribution].[ins_cont_id],
	[dbo].[insurance_contribution].[insurance_id],
	[dbo].[insurance_contribution].[contribution_id],
	[dbo].[insurance_contribution].[classification_id],
    [dbo].[insurance_contribution].[amount],
    [dbo].[insurance_contribution].[modBy],
    [dbo].[insurance_contribution].[modOn],
    [dbo].[insurance_contribution].[history],
    [dbo].[employee_classification].[description]
FROM [dbo].[employee_classification] LEFT OUTER JOIN
	[dbo].[insurance_contribution] ON [dbo].[employee_classification].[classification_id] = [dbo].[insurance_contribution].[classification_id]
GO

GRANT SELECT ON [dbo].[View_Insurance_Contributions] TO [aca-user] AS [dbo]
GO
