USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_EmployeeCount]    Script Date: 12/8/2016 8:47:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_EmployeeCount' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_EmployeeCount] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[SELECT_EmployeeCount]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT COUNT(ee.[employee_id]) AS CountOfEmployees, er.[name] 
   FROM [aca].[dbo].[employee] AS ee 
	INNER JOIN [aca].[dbo].[employer] AS er 
		ON ee.[employer_id] = er.[employer_id] 
   WHERE ee.[terminationDate] is null
   GROUP BY er.[name]
END
