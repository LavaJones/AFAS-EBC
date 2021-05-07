USE [aca-demo]
GO
--Start Migration script 21 - 22
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
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'spGetFieldForIRS' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[spGetFieldForIRS] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[spGetFieldForIRS]
	 @employerID int,
	 @taxYear int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	WITH empA AS
	(
	SELECT employee_id,a.time_frame_id, employer_id, offer_of_coverage_code, monthly_status_id, monthly_hours
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
	WHERE employer_id = @employerID AND b.year_id = @taxYear
	),
	empB AS
	(
	SELECT a.employee_id, first_name, middle_name, last_name, name_suffix, a.[address], a.city, state_code, zipcode,
		c.dob
	FROM air.emp.employee a INNER JOIN empA b ON  a.employee_id = b.employee_id INNER JOIN aca.dbo.employee c
	ON a.employee_id = c.employee_id
	)
	SELECT DISTINCT d.name as [Employer Name], d.ein, d.[address] as [Employer Address], 
		d.city as [Employer City], d.state_code AS [Employer Code], d.zipcode AS [Employer Zip], 
		d.contact_telephone AS [Employer Telephone], e.name as months, f.year_id as years, 
		a.offer_of_coverage_code, a.monthly_hours, B.*, C.status_description, b.dob
	FROM empA a INNER JOIN empB B  ON a.employee_id = b.employee_id INNER JOIN air.emp.monthly_status c ON a.monthly_status_id = c.monthly_status_id
		INNER JOIN air.ale.employer d ON d.employer_id = a.employer_id INNER JOIN air.gen.[month] e ON a.monthly_status_id = e.month_id
		INNER JOIN air.gen.time_frame f ON a.time_frame_id = f.time_frame_id
		ORDER BY b.first_name
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
--End Migration script 21 - 22