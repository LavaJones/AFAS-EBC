USE [aca]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/31/2016
-- Description: Select a Single Row of [PlanYearGroup]
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_PlanYearGroup' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_PlanYearGroup] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[SELECT_PlanYearGroup]
      @PlanYearGroupId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[PlanYearGroup] 
      WHERE [PlanYearGroupId] = @PlanYearGroupId 
		AND [EntityStatusId] = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/31/2016
-- Description: Select all Rows of [PlanYearGroup] belonging to a certain employer
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_PlanYearGroup_ForEmployer' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_PlanYearGroup_ForEmployer] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[SELECT_PlanYearGroup_ForEmployer]
      @EmployerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[PlanYearGroup] 
      WHERE [Employer_Id] = @EmployerId
		AND [EntityStatusId] = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO